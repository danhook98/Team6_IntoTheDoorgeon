using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DoorGame.GameplayEvents
{
    public class WheelOfFortune : MonoBehaviour
    {
        [Header("Wheel Variables")]
        [SerializeField] private float initialTorquePower = 1000f;
        
        private Rigidbody2D _rigidbody2D;
        private WaitForSeconds _spinCheckDelay;

        private void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _spinCheckDelay = new WaitForSeconds(0.1f);
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
            float power = initialTorquePower + Random.Range(-(initialTorquePower * 0.25f), initialTorquePower * 0.25f);
            _rigidbody2D.AddTorque(power);

            yield return _spinCheckDelay;

            while (_rigidbody2D.angularVelocity > 0f)
            {
                Debug.Log("Reducing angular torque");
                _rigidbody2D.angularVelocity -= 200 * Time.deltaTime;
                yield return null; 
            }
            
            _rigidbody2D.angularVelocity = 0f;
            
            // Get results.
        }
    }
}
