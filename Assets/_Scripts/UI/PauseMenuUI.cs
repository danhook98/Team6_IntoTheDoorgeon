using UnityEngine;
using UnityEngine.SceneManagement;
using DoorGame.Events;
using DoorGame.Audio; 

namespace DoorGame.UI
{
    public class PauseMenuUI : MonoBehaviour
    {
        [Header("Game Objects")]
        [SerializeField] private Canvas pauseMenuCanvas;
        [SerializeField] private GameObject pauseMenuContainer;
        [SerializeField] private GameObject optionsMenuContainer;
        [SerializeField] private GameObject winMenuContainer;
        [SerializeField] private GameObject loseMenuContainer;
        [SerializeField] private GameObject audioManager;
        
        [Header("Events")]
        [SerializeField] private BoolEvent onPauseGameEvent;
        [SerializeField] private FloatEvent OnMusicChange;
        [SerializeField] private FloatEvent OnSFXChange;

        [Header("Audio")] 
        [SerializeField] private AudioClipSOEvent playSfxAudioChannel;
        [SerializeField] private AudioClipSO pauseSound; 
        [SerializeField] private AudioClipSO unpauseSound;

        public void DisplayPauseMenu(bool isPaused)
        {
            Debug.Log(isPaused);
            pauseMenuCanvas.enabled = isPaused;
            playSfxAudioChannel.Invoke(isPaused ? pauseSound : unpauseSound);
        }
        
        // Pause menu button actions. 
        public void ResumeGame() => onPauseGameEvent.Invoke(false); // False = not paused.

        public void RestartGame() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        public void OpenOptionsMenu()
        {
            pauseMenuContainer.SetActive(false);
            optionsMenuContainer.SetActive(true);
        }
        public void QuitGame()
        {
            Application.Quit();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }

        // Options menu button actions.
        public void BackToPauseMenu()
        {
            optionsMenuContainer.SetActive(false);
            pauseMenuContainer.SetActive(true);
        }

        public void WinScreenOn()
        {
            winMenuContainer.SetActive(true);
        }

        public void LoseScreenOn()
        {
            loseMenuContainer.SetActive(true);
        }
        
        //Audio Settings
        public void AdjustMusicVolume(float musicVolume)
        {
            OnMusicChange.Invoke(musicVolume);
        }

        public void AdjustSFXVolume(float sfxVolume)
        {
            OnSFXChange.Invoke(sfxVolume);
        }
    }
}
