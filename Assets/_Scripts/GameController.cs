using UnityEngine;
using Random = UnityEngine.Random;

namespace DoorGame
{
    public class GameController : MonoBehaviour
    {
        [Header("Score Variables")] 
        [SerializeField] private int minimumScoreToAdd = 15;
        [SerializeField] private int maxScoreToAdd = 45;
        
        // reference to the door manager
        private int _score;
        private int _highScore;

        private void Awake()
        {
            _score = 0; 
            _highScore = PlayerPrefs.GetInt("HighScore", 0);
        }

        private void Start()
        {
            // tell the door manager to generate doors
        }
        
        // Score
        public void AddScore()
        {
            _score += Random.Range(minimumScoreToAdd, maxScoreToAdd + 1);
        }
    }
}
