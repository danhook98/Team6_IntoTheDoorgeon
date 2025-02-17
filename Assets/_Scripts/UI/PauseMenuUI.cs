using UnityEngine;
using UnityEngine.SceneManagement;
using DoorGame.Events;

namespace DoorGame.UI
{
    public class PauseMenuUI : MonoBehaviour
    {
        [SerializeField] private Canvas pauseMenuCanvas;
        [SerializeField] private GameObject pauseMenuContainer;
        [SerializeField] private GameObject optionsMenuContainer;
        [SerializeField] private GameObject audioManager;
        
        [Header("Events")]
        [SerializeField] private BoolEvent onPauseGameEvent;
        [SerializeField] private FloatEvent OnMusicChange;
        [SerializeField] private FloatEvent OnSFXChange;

        public void DisplayPauseMenu(bool isPaused)
        {
            Debug.Log(isPaused);
            pauseMenuCanvas.enabled = isPaused;
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
        
        //Audio Settings
        public void AdjustMusicVolume()
        {
            OnMusicChange.Invoke(PlayerPrefs.GetFloat("musicVolume"));
        }
        
        public void AdjustSFXVolume()
        {
            OnSFXChange.Invoke(PlayerPrefs.GetFloat("sfxVolume"));
        }
    }
}
