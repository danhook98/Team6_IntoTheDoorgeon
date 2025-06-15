using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DoorGame.GameplayEvents
{
    public class WheelOfFortune : MonoBehaviour
    {
        [Header("Wheel Variables")] 
        [SerializeField] private int numberOfSegments = 5;
        [SerializeField] private float initialTorquePower = 1000f;
        [SerializeField] private Transform wheelTransform;
        
        private WaitForSeconds _spinCheckDelay;

        private float _anglePerSegment; 

        private void Awake()
        {
            _spinCheckDelay = new WaitForSeconds(0.1f);
            _anglePerSegment = 360f / numberOfSegments;
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
            float spinTime = 8f;
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
}
