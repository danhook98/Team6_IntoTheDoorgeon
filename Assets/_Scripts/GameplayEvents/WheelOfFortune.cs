using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace DoorGame.GameplayEvents
{
    public class WheelOfFortune : MonoBehaviour
    {
        [Header("Value Objects")]
        [SerializeField] private IntValue scoreValue;
        [SerializeField] private IntValue doorsOpenedValue;
        
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
        
        [SerializeField] private TextMeshProUGUI[] wheelTexts;

        private WeightedRandom _weightedRandom;
        
        private int _totalWeight; 
        private float _anglePerSegment;

        private WaitForSeconds _wheelSelectionInterval; 
        private bool _isWheelSpinning;
        private bool _isWheelGood; 

        private void Awake()
        {
            // Assume that the wheel results will always be bad until the determine method is called.
            // wheelResults = badResultsWeights;
            
            _weightedRandom = new WeightedRandom(badResultsWeights);
            
            _anglePerSegment = 360f / badResultsWeights.Count;
            
            _wheelSelectionInterval = new WaitForSeconds(wheelSelectionChangeDelay);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                StartCoroutine(Spin());
            }
        }
        
        public void StartEvent()
        {
            // Determine which results will be used for the wheel.
            DetermineWheelResults();
            
            //SetWheelText();
            StartCoroutine(SelectWheel());
        }
        
        public void SpinWheel() => StartCoroutine(Spin());

        private void DetermineWheelResults()
        {
            // No point calculating the chances if somehow the player has opened more than 60 doors.
            // if (doorsOpenedValue.Value >= 60)
            // {
            //     //wheelResults = badResultsWeights;
            //     _weightedRandom.SetValues(badResultsWeights);
            //     _isWheelGood = false;
            //     return; 
            // }
                
            int baseGoodChance = Mathf.Clamp(60 - doorsOpenedValue.Value, 0, 60); 

            int randomNumber = Random.Range(0, 101);

            _isWheelGood = randomNumber <= baseGoodChance;
            
            //wheelResults = randomNumber <= baseGoodChance ? goodResultsWeights : badResultsWeights;
            _weightedRandom.SetValues(_isWheelGood ? goodResultsWeights : badResultsWeights);
        }

        private IEnumerator SelectWheel()
        {
            yield return new WaitForSeconds(0.25f);
            
            int max = 2 * Random.Range(4, 7) + (_isWheelGood ? 0 : 1);
            
            for (int i = 1; i <= max; i++)
            {
                if (i % 2 == 0)
                {
                    wheelImage.sprite = goodWheelImage;
                }
                else
                {
                    wheelImage.sprite = badWheelImage;
                }
                
                _isWheelGood = i % 2 == 0;
                
                SetWheelText();
                
                yield return _wheelSelectionInterval;
            }
        }

        private void SetWheelText()
        {
            for (int i = 0; i < wheelTexts.Length; i++)
            {
                wheelTexts[i].text = (_isWheelGood ? "+" : "-") + (_isWheelGood ? goodResultsWeights[i].Value : badResultsWeights[i].Value) + "%"; 
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
            if (_isWheelSpinning) yield break; 
            
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
            
            // Get results.
            
            _isWheelSpinning = false;
        }
    }
}
