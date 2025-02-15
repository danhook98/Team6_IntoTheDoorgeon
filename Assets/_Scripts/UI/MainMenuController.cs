using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DoorGame
{
    public class MainMenuController : MonoBehaviour
    {
        public void PlayGame()
        {
            SceneManager.LoadSceneAsync("Main Game");
        }

        public void QuitGame()
        {
            Application.Quit();
        }
    }
}
