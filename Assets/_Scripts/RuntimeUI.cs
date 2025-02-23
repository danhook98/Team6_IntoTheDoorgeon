using UnityEngine;
using UnityEngine.SceneManagement;
using DoorGame.Events;
using TMPro;

namespace DoorGame
{
    public class RuntimeUI : MonoBehaviour
    {
        [Header("Score")] 
        [SerializeField] private TextMeshProUGUI winScreenScoreText;
        [SerializeField] private TextMeshProUGUI highScoreText;
        [SerializeField] private string winScreenScorePrefixText = "Score: ";
        [SerializeField] private string highScorePrefixText = "High Score: ";
        
        [Header("Doors Opened")]
        [SerializeField] private TextMeshProUGUI winScreenOpenedText;
        [SerializeField] private string winScreenOpenedPrefixText = "Doors Opened: ";
        
        [Header("Events")]
        [SerializeField] private BoolEvent onPauseGameEvent;
        [SerializeField] private VoidEvent onLoadMainGameEvent;
        [SerializeField] private VoidEvent onHowToPlayEvent;
        [SerializeField] private VoidEvent onBackToWinMenuEvent;
        
        [Header("Canvases")]
        [SerializeField] private Canvas winMenuCanvas;
        [SerializeField] private Canvas howToPlayCanvas;
        
        public void PauseGame()
        {
            Debug.Log("Game paused");
            onPauseGameEvent.Invoke(true);
        }

        public void RestartGame()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        
        public void LoadMainGameScene()
        {
            SceneManager.LoadScene("Main Game"); 
            onLoadMainGameEvent.Invoke(new Empty());
        }

        public void LoadMainMenuScene()
        {
            SceneManager.LoadScene("Main Menu"); 
        }

        public void OnHighScoreChanged(int highScore)
        {
            PlayerPrefs.GetInt("High Score", highScore);
            highScoreText.text = highScorePrefixText + highScore;
        }
        
        public void WinScreenOn()
        {
            highScoreText.text = highScorePrefixText + PlayerPrefs.GetInt("High Score");
            winMenuCanvas.enabled = true;
        }

        public void DisplayHowToPlay()
        {
            howToPlayCanvas.enabled = true;
        }
        
        public void BackToWinScreen()
        {
            howToPlayCanvas.enabled = false;
        }
        public void HowToPlayScreen() => onHowToPlayEvent.Invoke(new Empty());
        public void OffHowToPlayScreen() => onBackToWinMenuEvent.Invoke(new Empty());
    }
}
