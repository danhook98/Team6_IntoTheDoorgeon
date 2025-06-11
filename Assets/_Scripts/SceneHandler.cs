using UnityEngine;
using UnityEngine.SceneManagement;

namespace DoorGame
{
    public class SceneHandler : MonoBehaviour
    {
        public void PauseGame(bool state)
        {
            Debug.Log("Game paused");
            
            Time.timeScale = state ? 0 : 1;
        }

        public void RestartGame() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        public void LoadMainMenuScene() => SceneManager.LoadScene("Main Menu");
        
        public void QuitGame()
        {
            Application.Quit();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }
    }
}
