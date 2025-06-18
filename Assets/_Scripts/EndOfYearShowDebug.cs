using UnityEngine;

namespace DoorGame
{
    public class EndOfYearShowDebug : MonoBehaviour
    {
        private AudioListener _audioListener;

        private void Awake()
        {
            _audioListener = GetComponent<AudioListener>();
        }
        
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.M))
            {
                _audioListener.enabled = !_audioListener.enabled;
            }
        }
    }
}
