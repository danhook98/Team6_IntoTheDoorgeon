using UnityEngine;
using UnityEngine.SceneManagement;
using DoorGame.Events;
using TMPro;

namespace DoorGame
{
    public class RuntimeUI : MonoBehaviour
    {
        [Header("Score")] 
        [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private string scorePrefixText = "Score: ";
        
        [Header("Doors Opened")]
        [SerializeField] private TextMeshProUGUI doorsOpenedText;
        [SerializeField] private string doorsOpenedPrefixText = "Doors Opened: ";
        
        [Header("Events")]
        [SerializeField] private BoolEvent onPauseGameEvent;
        [SerializeField] private VoidEvent onLoadMainGameEvent;
        [SerializeField] private VoidEvent onLoadMainMenuEvent;
        [SerializeField] private BoolEvent onRestartCurrentSceneEvent;
        [SerializeField] private BoolEvent onHowToPlayEvent;
        [SerializeField] private VoidEvent onBackToWinMenuEvent;
        
        [Header("Canvases")]
        [SerializeField] private Canvas winMenuCanvas;
        [SerializeField] private Canvas loseMenuCanvas;
        [SerializeField] private Canvas howToPlayCanvas;

        private bool isHowToPlayOn = false;
        
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
            onLoadMainGameEvent.Invoke(new Empty());
        }

        public void LoadMainMenuScene()
        {
            SceneManager.LoadScene("Main Menu"); 
            onLoadMainMenuEvent.Invoke(new Empty());
        }

        public void OnScoreChanged(int score)
        {
            scoreText.text = scorePrefixText + score;
        }

        public void OnValidDoorsOpenedChanged(int numDoors)
        {
            doorsOpenedText.text = doorsOpenedPrefixText + numDoors;
        }
        
        public void WinScreenOn()
        {
            winMenuCanvas.enabled = true;
        }

        public void LoseScreenOn()
        {
            loseMenuCanvas.enabled = true;
        }

        public void DisplayHowToPlay()
        {
            howToPlayCanvas.enabled = true;
            winMenuCanvas.enabled = false;
        }
        
        public void BackToWinScreen()
        {
            howToPlayCanvas.enabled = false;
            winMenuCanvas.enabled = true;
        }
        public void HowToPlayScreen() => onHowToPlayEvent.Invoke(true);
        public void OffHowToPlayScreen() => onBackToWinMenuEvent.Invoke(new Empty());
    }
}
