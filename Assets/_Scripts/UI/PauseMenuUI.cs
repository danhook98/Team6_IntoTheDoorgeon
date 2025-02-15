using DoorGame.Events;
using UnityEngine;

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

        public void DisplayPauseMenu(bool isPaused)
        {
            Debug.Log(isPaused);
            pauseMenuCanvas.enabled = isPaused;
        }
    }
}
