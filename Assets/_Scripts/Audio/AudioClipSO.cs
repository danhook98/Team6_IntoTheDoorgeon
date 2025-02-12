using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "AudioClip", menuName = "DoorGame/Audio Clip")]
public class AudioClipSO : ScriptableObject
{
    public AudioClip clip;
}
