using UnityEngine;
using UnityEngine.Serialization;

namespace DoorGame.Audio
{
    [CreateAssetMenu(fileName = "AudioClip", menuName = "DoorGame/Audio Clip")]
    public class AudioClipSO : ScriptableObject
    {
        public AudioClip clip;
        [Range(0f, 1f)] public float volume = 1f;
    }
}
