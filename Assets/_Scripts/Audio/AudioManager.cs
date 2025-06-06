using UnityEngine;
using UnityEngine.Audio;

namespace DoorGame.Audio
{
    public class AudioManager : MonoBehaviour
    {
        [Header("Audio Sources")]
        [SerializeField] private AudioSource audioSourceSfx;
        [SerializeField] private AudioSource audioSourceMusic;
        [Space]
        [SerializeField] private AudioMixer audioMixer;
        
        private readonly string[] _mixerParameters = { "MasterVolume", "SfxVolume", "MusicVolume" };
        
        private void Awake()
        {
            if (!audioSourceSfx)
            {
                Debug.LogWarning("<color=red>Audio Manager</color>: No audio source set/found for SFX. Creating " +
                                 "one, expect weird behaviour.");
                audioSourceSfx = gameObject.AddComponent<AudioSource>();
            }

            if (!audioSourceMusic)
            {
                Debug.LogWarning("<color=red>Audio Manager</color>: No audio source set/found for Music. Creating " +
                                 "one, expect weird behaviour.");
                audioSourceMusic = gameObject.AddComponent<AudioSource>();
            }
        }

        private void Start() => LoadVolume();

        /// <summary>
        /// Plays a one shot sound from the given audio data.
        /// </summary>
        /// <param name="clipData">AudioClipSO data.</param>
        public void PlaySfx(AudioClipSO clipData)
        {
            if (!clipData.clip)
            {
                Debug.LogWarning($"<color=red>Audio Manager</color>: Attempted to play audio one shot for " +
                                 $"{clipData.name}, but clip data is null.");
                return; 
            }
            
            audioSourceSfx.PlayOneShot(clipData.clip, clipData.volume);
        }

        /// <summary>
        /// Plays a continuous sound from the given audio data.
        /// </summary>
        /// <param name="clipData">AudioClipSO data.</param>
        public void PlayMusic(AudioClipSO clipData)
        {
            if (!clipData.clip)
            {
                Debug.LogWarning($"<color=red>Audio Manager</color>: Attempted to play audio music for " +
                                 $"{clipData.name}, but clip data is null.");
                return; 
            }
            
            audioSourceMusic.clip = clipData.clip;
            audioSourceMusic.volume = clipData.volume;
            audioSourceMusic.Play();
        }
        
        /// <summary>
        /// Stops audio playing from the music audio source.
        /// </summary>
        public void StopMusic() => audioSourceMusic.Stop();

        /// <summary>
        /// Sets the master volume. 
        /// </summary>
        /// <param name="volume">Volume value, between 0 and 1.</param>
        public void SetMasterVolume(float volume) => SetVolume("MasterVolume", volume);

        /// <summary>
        /// Sets the sound effects volume. 
        /// </summary>
        /// <param name="volume">Volume value, between 0 and 1.</param>
        public void SetSfxVolume(float volume) => SetVolume("SfxVolume", volume);

        /// <summary>
        /// Sets the music volume. 
        /// </summary>
        /// <param name="volume">Volume value, between 0 and 1.</param>
        public void SetMusicVolume(float volume) => SetVolume("MusicVolume", volume);

        /// <summary>
        /// Sets the volume of the given mixer parameter to the given volume. Values outside of 0-1 and ignored.
        /// </summary>
        /// <param name="mixerParameter">Mixer group parameter.</param>
        /// <param name="volume">Volume level.</param>
        private void SetVolume(string mixerParameter, float volume)
        {
            // Check that the passed volume value is invalid. 
            if (volume is < 0 or > 1)
            {
                Debug.LogWarning($"<color=red>Audio Manager</color>: Attempting to set volume for {mixerParameter}, " +
                                 $"but the volume value ({volume}) was outside the range [0, 1].");
                return; 
            }
            
            // Convert the 0-1 float volume value into a base 10 logarithmic curve. This ensures that the volume change
            // in the mixer sounds correct to our ears, as anything below -20 dB is essentially silent to us. 
            float mixerVolume = Mathf.Log10(volume) * 20;
            
            // Set the volume of the mixer group, and update the volume in PlayerPrefs for loading. 
            audioMixer.SetFloat(mixerParameter, mixerVolume);
            PlayerPrefs.SetFloat(mixerParameter, mixerVolume);
        }
        
        /// <summary>
        /// Loads all of the saved mixer group volume levels. Non-existent values in PlayerPrefs are set to 0.
        /// </summary>
        private void LoadVolume()
        {
            foreach (string parameter in _mixerParameters)
            {
                float volume = PlayerPrefs.GetFloat(parameter, 0f);
                audioMixer.SetFloat(parameter, volume);
            }
        }
    }
}