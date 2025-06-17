using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using DoorGame.EventSystem;

namespace DoorGame.GameplayEvents
{
    public class WheelOfFortune : MonoBehaviour
    {
        [Header("Objects")] 
        [SerializeField] private GameObject introCard;
        [SerializeField] private GameObject wheel;
        [SerializeField] private GameObject endCard;
        
        [Header("Value Objects")]
        [SerializeField] private IntValue scoreValue;
        [SerializeField] private IntValue dungeonsEnteredValue;

        [Header("Events")] 
        [SerializeField] private IntEvent onScoreChangedEvent;
        [SerializeField] private AudioClipSOEvent onPlaySfxEvent; 
        
        [Header("End Card")]
        [SerializeField] private TextMeshProUGUI endCardText;
        
        [Header("Wheel")] 
        [SerializeField] private Transform wheelTransform;
        [SerializeField] private Image wheelImage;
        [SerializeField] private float wheelSelectionChangeDelay = 0.25f;
        
        [Header("Wheel Images")]
        [SerializeField] private Sprite goodWheelImage;
        [SerializeField] private Sprite badWheelImage;

        [Header("Wheel Spin")] 
        [SerializeField] private int minimumFullRotations = 4;
        [SerializeField] private int maximumFullRotations = 8;
        [SerializeField] private float baseSpinDuration = 4f;

        [Header("Results and Random Weights")]
        [SerializeField] private List<WeightedValue> goodResultsWeights = new List<WeightedValue>
        {
            new(10, 40),
            new(25, 30),
            new(50, 20),
            new(100, 9),
            new(200, 1)
        };

        [SerializeField] private List<WeightedValue> badResultsWeights = new List<WeightedValue>
        {
            new(10, 40),
            new(25, 30),
            new(50, 20),
            new(66, 9),
            new(75, 1)
        };
        
        [Header("Wheel Text Objects (clockwise from 12)")]
        [SerializeField] private TextMeshProUGUI[] wheelTexts;

        private Canvas _canvas;
        
        private WeightedRandom _weightedRandom;
        
        private int _totalWeight; 
        private float _anglePerSegment;

        private WaitForSeconds _wheelSelectionInterval;
        private WaitForSeconds _wheelFinishDelay; 
        private bool _isWheelSpinning;
        private bool _isWheelGood;
        private bool _canSpin;

        private void Awake()
        {
            _canvas = GetComponent<Canvas>();
            
            // Create a new instance of the WeightedRandom class, and pass the bad results as we assume that's the
            // default. 
            _weightedRandom = new WeightedRandom(badResultsWeights);
            
            _anglePerSegment = 360f / badResultsWeights.Count;
            
            _wheelSelectionInterval = new WaitForSeconds(wheelSelectionChangeDelay);
            _wheelFinishDelay = new WaitForSeconds(2f);
        }
        
        public void StartEvent()
        {
            introCard.SetActive(false);
            wheel.SetActive(true);
            
            // Ensure the wheel is not rotated.
            wheelTransform.localEulerAngles = Vector3.zero;
            
            // Determine which results will be used for the wheel.
            DetermineWheelResults();
            
            //SetWheelText();
            StartCoroutine(SelectWheel());
        }

        public void EndEvent()
        {
            // send score changed event
            // hide the object 

            _canvas.enabled = false; 
            
            introCard.SetActive(true);
            wheel.SetActive(false);
            endCard.SetActive(false);
        }

        public void SpinWheel() => StartCoroutine(Spin());
        
        private void DetermineWheelResults()
        {
            int baseGoodChance = Mathf.Clamp(60 - dungeonsEnteredValue.Value, 40, 60); 

            int randomNumber = Random.Range(0, 101);

            _isWheelGood = randomNumber <= baseGoodChance;
            
            //Debug.Log($"Wheel will be good? {_isWheelGood}");
            
            _weightedRandom.SetValues(_isWheelGood ? goodResultsWeights : badResultsWeights);
        }

        private IEnumerator SelectWheel()
        {
            _canSpin = false;
            
            int max = 2 * Random.Range(4, 7) + (_isWheelGood ? 0 : 1);
            
            for (int i = 1; i <= max; i++)
            {
                wheelImage.sprite = i % 2 == 0 ? goodWheelImage : badWheelImage;
                
                SetWheelText(i % 2 == 0);
                
                yield return _wheelSelectionInterval;
            }
            
            _canSpin = true;
        }

        private void SetWheelText(bool isGood)
        {
            for (int i = 0; i < wheelTexts.Length; i++)
            {
                wheelTexts[i].text = (isGood ? "+" : "-") + (isGood ? goodResultsWeights[i].Value : badResultsWeights[i].Value) + "%"; 
            }
        }

        private float GetCurrentWheelAngle()
        {
            float currentAngle = wheelTransform.eulerAngles.z;
            
            while (currentAngle > 360f) currentAngle -= 360f;
            while (currentAngle < 0f) currentAngle += 360f;
            
            return currentAngle;
        }

        private IEnumerator Spin()
        {
            if (_isWheelSpinning || !_canSpin) yield break; 
            
            _isWheelSpinning = true;
            
            int segmentIndex = _weightedRandom.GetRandomAsIndex(); //GetRandomIndex();
            
            float currentAngle = GetCurrentWheelAngle();
            
            float segmentAngle = segmentIndex * _anglePerSegment;
            
            int numberOfRotations = Random.Range(minimumFullRotations, maximumFullRotations);
            float targetAngle = segmentAngle + 360f * numberOfRotations; 
            
            // Debug.Log($"Will spin {numberOfRotations} times before ending at index {segmentIndex} with an angle of {segmentAngle}", this);
            // Debug.Log($"The odds for this were {wheelResults[segmentIndex].Weight / (float)wheelResults.Sum(p => p.Weight):P}!");

            float spinTime = numberOfRotations + baseSpinDuration;
            float elapsedTime = 0f;
            
            while (elapsedTime < spinTime)
            {
                float lerpFactor = Mathf.SmoothStep(0, 1, (Mathf.SmoothStep(0, 1, elapsedTime / spinTime)));
                
                wheelTransform.localEulerAngles = new Vector3(0.0f, 0.0f, Mathf.Lerp(currentAngle, targetAngle, lerpFactor));
                
                elapsedTime += Time.deltaTime;
                
                yield return null; 
            }
            
            wheelTransform.localEulerAngles = new Vector3(0.0f, 0.0f, targetAngle);
            _isWheelSpinning = false;
            
            // Get results.
            int resultModifier = _weightedRandom.GetValueAtIndex(segmentIndex);
            // Debug.Log($"Score will be modified by {resultModifier}%!");
            
            yield return _wheelFinishDelay;
            
            int newScore = GetNewScore(resultModifier);
            onScoreChangedEvent.Invoke(newScore);
            // Debug.Log($"New score is {newScore}");
            
            SetEndCardText(resultModifier, newScore);
            ShowEndCard();
        }

        private int GetNewScore(int baseModifier)
        {
            float modifier = _isWheelGood ? 1 + (baseModifier / 100f) : 1 - (baseModifier / 100f);
            // Debug.Log($"Score will be modified by {modifier}");
            return (int)(Mathf.Ceil(scoreValue.Value * modifier));
        }

        private void SetEndCardText(int modifier, int score)
        {
            endCardText.text = (_isWheelGood ? "+" : "-") + modifier + "%\n\nYour score is now\n" + score;
        }

        private void ShowEndCard()
        {
            wheel.SetActive(false);
            endCard.SetActive(true);
        }
    }
}
