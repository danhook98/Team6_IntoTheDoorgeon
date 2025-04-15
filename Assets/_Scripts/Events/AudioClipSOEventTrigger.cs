using UnityEngine;

namespace DoorGame.Events
{
    public class AudioClipSOEventTrigger : AbstractEventTrigger<AudioClipSO>
    {
        [SerializeField] private AudioClipSO audioClip;
        public void Trigger() => eventToTrigger.Invoke(audioClip);
    }
}