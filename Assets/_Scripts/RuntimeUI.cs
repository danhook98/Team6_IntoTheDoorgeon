using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DoorGame.Events;
using UnityEngine.Serialization;

namespace DoorGame
{
    public class RuntimeUI : MonoBehaviour
    {
        [Header("Events")]
        [SerializeField] private BoolEvent onPauseGameEvent;
        [SerializeField] private BoolEvent onResumeGameEvent;
        [SerializeField] private BoolEvent onLoadMainGameEvent;
        [SerializeField] private BoolEvent onRestartCurrentSceneEvent;

        void Update()
        {
            Debug.Log("Test");
        }
        
        public void PauseGame()
        {
            Debug.Log("Game paused");
            onPauseGameEvent.Invoke(true);
        }

        public void ResumeGame()
        {
            Debug.Log("Game resumed");
            onResumeGameEvent.Invoke(true);
        }

        public void LoadMainGameScene()
        {
            //SceneManager.LoadScene("Game"); //TODO: Uncomment this so it works.
        }

        public void RestartGame()
        {
            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            //onRestartCurrentSceneEvent.Invoke();
        }

        public void LoadMainMenuScene()
        {
            //SceneManager.LoadScene("MainMenu"); //TODO: Uncomment this so it works.
        }
    }
}
