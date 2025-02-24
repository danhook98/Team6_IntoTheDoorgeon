using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DoorGame
{
    public class MainMenuController : MonoBehaviour
    {
        [SerializeField] private GameObject optionsMenuContainer;
        [SerializeField] private GameObject mainMenuContainer;
        public void PlayGame()
        {
            SceneManager.LoadSceneAsync("Main Game");
        }

        public void QuitGame()
        {
            Application.Quit();
        }

        public void OpenOptionsMenu()
        {
            optionsMenuContainer.SetActive(true);
            mainMenuContainer.SetActive(false);
        }

        public void CloseOptionsMenu()
        {
            optionsMenuContainer.SetActive(false);
            mainMenuContainer.SetActive(true);
        }
    }
}
