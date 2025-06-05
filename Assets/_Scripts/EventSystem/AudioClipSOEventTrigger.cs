using UnityEngine;
using DoorGame.Audio; 

namespace DoorGame.EventSystem
{
    public class AudioClipSOEventTrigger : AbstractEventTrigger<AudioClipSO>
    {
        [SerializeField] private AudioClipSO audioClip;
        public void Trigger() => eventToTrigger.Invoke(audioClip);
    }
}