using UnityEngine;
using UnityEngine.SceneManagement;
using DoorGame.Events;

namespace DoorGame.UI
{
    public class PauseMenuUI : MonoBehaviour
    {
        [SerializeField] private Canvas pauseMenuCanvas;
        
        [Header("Events")]
        [SerializeField] private BoolEvent onPauseGameEvent;

        public void ResumeGame()
        {
            onPauseGameEvent.Invoke(false); // False = not paused.
        }

        public void RestartGame()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void QuitGame()
        {
            Application.Quit();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }

        public void DisplayPauseMenu(bool isPaused)
        {
            Debug.Log(isPaused);
            pauseMenuCanvas.enabled = isPaused;
        }
    }
}
