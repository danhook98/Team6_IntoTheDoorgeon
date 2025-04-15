using UnityEngine;
using UnityEngine.Audio;

namespace DoorGame.Audio
{
    public class AudioManager : MonoBehaviour
    {
        [Header("Audio Sources")]
        [SerializeField] private AudioSource sfxSource;
        [SerializeField] private AudioSource musicSource;
        
        [Header("Audio Mixer")]
        [SerializeField] private AudioMixer audioMixer;
        
        private void Awake()
        {
            // If audio sources are missing, send warning messages through the console and add sources.
            if (!sfxSource)
            {
                Debug.LogWarning("<color=red>AudioManager</color>: SFX AudioSource is missing or the reference is " +
                                 "missing. Creating a new AudioSource.");
                sfxSource = gameObject.AddComponent<AudioSource>();
            }

            if (!musicSource)
            {
                Debug.LogWarning("<color=red>AudioManager</color>: Music AudioSource is missing or the reference is " +
                                 "missing. Creating a new AudioSource.");
                musicSource = gameObject.AddComponent<AudioSource>();
            }
        }

        private void Start()
        {
            LoadVolume();
        }

        public void PlayOneShotSFX(AudioClipSO audioClip)
        {
            sfxSource.PlayOneShot(audioClip.clip);
        }

        public void PlayMusic(AudioClipSO audioClip)
        {
            musicSource.clip = audioClip.clip;
            musicSource.Play();
        }

        public void StopMusic()
        {
            musicSource.Stop();
        }
        
        public void SetSfxVolume(float value) => SetVolume(value, "SFXParameter");
        public void SetMusicVolume(float value) => SetVolume(value, "musicParameter");

        public void SetVolume(float volume, string mixerGroup)
        {
            // Ensure the volume value given is valid. 
            if (volume is < 0 or > 1)
            {
                Debug.LogWarning("<color=red>AudioManager</color>: Attempting to set mixer volume, but the given" +
                                 $"value was outside the range [0, 1]: {volume}.");
                return;
            }
            
            float mixerVolume = (Mathf.Log10(volume) * 20);
            audioMixer.SetFloat(mixerGroup, mixerVolume);
            PlayerPrefs.SetFloat(mixerGroup, mixerVolume);
        }

        private void LoadVolume()
        {
            // Load the saved volume if it exists, otherwise it defaults to 1.
            float sfxVolume = PlayerPrefs.GetFloat("SFXParameter", 1f);
            float musicVolume = PlayerPrefs.GetFloat("musicParameter", 1f);
            
            // Set the volume levels of the mixers.
            audioMixer.SetFloat("SFXParameter", sfxVolume);
            audioMixer.SetFloat("musicParameter", musicVolume);
        }
    }
}