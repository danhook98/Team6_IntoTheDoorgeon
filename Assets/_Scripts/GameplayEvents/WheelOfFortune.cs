using System.Collections;
using System.Collections.Generic;
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

        private float _totalWeight; 
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

        private IEnumerator Spin()
        {
            float spinTime = Random.Range(minimumFullRotations, maximumFullRotations) + baseSpinDuration;
            float elapsedTime = 0f;
            
            while (elapsedTime < spinTime)
            {
                float lerpFactor = Mathf.SmoothStep(0, 1, (Mathf.SmoothStep(0, 1, elapsedTime / spinTime)));
                
                wheelTransform.localEulerAngles = new Vector3(0.0f, 0.0f, Mathf.Lerp(0f, (360f * 20) - 90f, lerpFactor));
                
                elapsedTime += Time.deltaTime;
                
                yield return null; 
            }
            
            // Get results.
        }
    }

    [System.Serializable]
    public struct WeightedValue
    {
        public float Value; 
        public float Weight;
        
        public WeightedValue(float value, float weight) { Value = value; Weight = weight; }
    }
}
