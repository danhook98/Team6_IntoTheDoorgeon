using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DoorGame.GameplayEvents
{
    public class WheelOfFortune : MonoBehaviour
    {
        [Header("Wheel Variables")] 
        [SerializeField] private int numberOfSegments = 5;
        [SerializeField] private Transform wheelTransform;

        [Header("Wheel Spin Variables")] 
        [SerializeField] private int minimumFullRotations = 4;
        [SerializeField] private int maximumFullRotations = 8;
        [SerializeField] private float baseSpinDuration = 3f;

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

        private int _totalWeight; 
        private float _anglePerSegment; 

        private void Awake()
        {
            _anglePerSegment = 360f / numberOfSegments;
            
            // Calculate the total weight. 
            foreach (WeightedValue kvp in goodResultsWeights)
            {
                _totalWeight += kvp.Weight;
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                StartCoroutine(Spin());
            }
        }

        private int GetRandomIndex()
        {
            // Get a random point on the results 'line'. 
            int randomPoint = Random.Range(0, _totalWeight);
            
            // Get the value and random segment the wheel will land on. 
            int cumulativeWeight = 0;

            for (int i = 0; i < goodResultsWeights.Count; i++)
            {
                cumulativeWeight += goodResultsWeights[i].Weight;

                if (cumulativeWeight < randomPoint) continue;

                return i; 
            }
        }

        private IEnumerator Spin()
        {
            int segmentIndex = GetRandomIndex();
            
            float segmentAngle = segmentIndex * _anglePerSegment;
            float currentAngle = wheelTransform.eulerAngles.z;
            
            while (currentAngle > 360f) currentAngle -= 360f;
            while (currentAngle < 0f) currentAngle += 360f;
            
            int numberOfRotations = Random.Range(minimumFullRotations, maximumFullRotations);
            float targetAngle = -(segmentAngle + 360f * numberOfRotations); 
            
            Debug.Log($"Will spin {numberOfRotations} times before ending at index {segmentIndex} with an angle of {segmentAngle}", this);
            Debug.Log($"The odds for this were {goodResultsWeights[segmentIndex].Weight / (float)goodResultsWeights.Sum(p => p.Weight):P}!");
            
            float spinTime = baseSpinDuration;
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
