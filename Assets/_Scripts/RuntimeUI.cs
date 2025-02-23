using UnityEngine;
using UnityEngine.SceneManagement;
using DoorGame.Events;
using TMPro;

namespace DoorGame
{
    public class RuntimeUI : MonoBehaviour
    {
        [Header("Events")]
        [SerializeField] private BoolEvent onPauseGameEvent;
        
        public void PauseGame()
        {
            Debug.Log("Game paused");
            onPauseGameEvent.Invoke(true);
        }

        public void RestartGame() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        public void LoadMainMenuScene() => SceneManager.LoadScene("Main Menu");
    }
}
