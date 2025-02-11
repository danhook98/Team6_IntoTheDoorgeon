using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioEventChannelSO audioEventChannel;
    
    [Header("Audio Sources")]
    [SerializeField] private AudioSource sfxAudioSource;
    [SerializeField] private AudioSource musicAudioSource;

    private void Awake()
    {
        // Create audio sources if the references are broken or missing. 
        if (!sfxAudioSource)
        {
            Debug.LogWarning("<color=red>AudioManager</color>: SFX AudioSource is missing or the reference is " +
                             "missing. Creating a new AudioSource.");
            sfxAudioSource = gameObject.AddComponent<AudioSource>();
        }

        if (!musicAudioSource)
        {
            Debug.LogWarning("<color=red>AudioManager</color>: Music AudioSource is missing or the reference is " +
                             "missing. Creating a new AudioSource.");
            musicAudioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    /// <summary>
    /// Plays an audio one shot of the given AudioClipSO's clip.
    /// </summary>
    /// <param name="audioClip">AudioClipSO data file.</param>
    public void PlayAudioOneShot(AudioClipSO audioClip)
    {
        sfxAudioSource.PlayOneShot(audioClip.clip);
    }

    /// <summary>
    /// Plays an audio clip of the given AudioClipSO's clip.
    /// </summary>
    /// <param name="audioClip">AudioClipSO data file.</param>
    public void PlayMusic(AudioClipSO audioClip)
    {
        musicAudioSource.clip = audioClip.clip;
        musicAudioSource.Play();
    }
}
