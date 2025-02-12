using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "AudioEventChannel", menuName = "DoorGame/Audio Event Channel")]
public class AudioEventChannelSO : ScriptableObject
{
    public event UnityAction<AudioClipSO> OnPlayAudioSfx;
    public event UnityAction<AudioClipSO> OnStopAudioMusic;
    
    public void PlayAudioSfx(AudioClipSO audioClip) => OnPlayAudioSfx?.Invoke(audioClip);
    public void PlayAudioMusic(AudioClipSO audioClip) => OnStopAudioMusic?.Invoke(audioClip);
}
