using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DoorGame.Events;

namespace DoorGame
{
    public class RuntimeUI : MonoBehaviour
    {
        [Header("Events")]
        [SerializeField] private BoolEvent onPauseGameEvent;
        // [SerializeField] private BoolEvent onResumeGameEvent; - not needed, the pause game event can pass false
        [SerializeField] private BoolEvent onLoadMainGameEvent;
        [SerializeField] private BoolEvent onLoadMainMenuEvent;
        [SerializeField] private BoolEvent onRestartCurrentSceneEvent;
        
        public void PauseGame()
        {
            Debug.Log("Game paused");
            onPauseGameEvent.Invoke(true);
        }

        public void ResumeGame()
        {
            Debug.Log("Game resumed");
            onPauseGameEvent.Invoke(false);
        }

        public void RestartGame()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            onRestartCurrentSceneEvent.Invoke(true);
        }
        
        public void LoadMainGameScene()
        {
            SceneManager.LoadScene("Main Game"); 
            onLoadMainGameEvent.Invoke(true);
        }

        public void LoadMainMenuScene()
        {
            SceneManager.LoadScene("Main Menu"); 
            onLoadMainMenuEvent.Invoke(true);
        }
    }
}
