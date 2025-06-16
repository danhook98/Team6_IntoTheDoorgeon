using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

        private List<WeightedValue> wheelResults;
        private int _totalWeight; 
        private float _anglePerSegment; 

        private void Awake()
        {
            // Assume that the wheel results will always be bad until the determine method is called.
            wheelResults = badResultsWeights;
            
            _anglePerSegment = 360f / wheelResults.Count;
            
            // Calculate the total weight. 
            foreach (WeightedValue kvp in goodResultsWeights)
            {
                _totalWeight += kvp.Weight;
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                StartCoroutine(Spin());
            }
        }

        private void DetermineWheelResults()
        {
            // No point calculating the chances if somehow the player has opened more than 60 doors.
            if (doorsOpenedValue.Value >= 60)
            {
                wheelResults = badResultsWeights;
                return; 
            }
                
            int baseGoodChance = 60 - doorsOpenedValue.Value; 

            int randomNumber = Random.Range(0, 101);
            
            wheelResults = randomNumber <= baseGoodChance ? goodResultsWeights : badResultsWeights;
        }

        public void StartEvent()
        {
            // Determine which results will be used for the wheel.
            DetermineWheelResults();
                
            StartCoroutine(SelectWheel());
            //StartCoroutine(Spin());
        }

        private IEnumerator SelectWheel()
        {
            yield return new WaitForSeconds(0.25f);
            
            for (int i = 1; i <= 10; i++)
            {
                if (i % 2 == 0)
                {
                    wheelImage.sprite = goodWheelImage;
                }
                else
                {
                    wheelImage.sprite = badWheelImage;
                }
                
                yield return new WaitForSeconds(0.3f);
            }
        }

        private int GetRandomIndex()
        {
            // Get a random point on the results 'line'. 
            int randomPoint = Random.Range(0, _totalWeight);
            
            // Get the value and random segment the wheel will land on. 
            int cumulativeWeight = 0;

            for (int i = 0; i < wheelResults.Count; i++)
            {
                cumulativeWeight += wheelResults[i].Weight;

                if (cumulativeWeight < randomPoint) continue;

                return i; 
            }

            // Failed to get the index from the weighted values.
            return 0;
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
            int segmentIndex = GetRandomIndex();
            
            float currentAngle = GetCurrentWheelAngle();
            
            float segmentAngle = segmentIndex * _anglePerSegment;
            
            int numberOfRotations = Random.Range(minimumFullRotations, maximumFullRotations);
            float targetAngle = -(segmentAngle + 360f * numberOfRotations); 
            
            Debug.Log($"Will spin {numberOfRotations} times before ending at index {segmentIndex} with an angle of {segmentAngle}", this);
            Debug.Log($"The odds for this were {wheelResults[segmentIndex].Weight / (float)wheelResults.Sum(p => p.Weight):P}!");

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
        }
    }

    [System.Serializable]
    public struct WeightedValue
    {
        public int Value; 
        public int Weight;
        
        public WeightedValue(int value, int weight) { Value = value; Weight = weight; }
    }
}
