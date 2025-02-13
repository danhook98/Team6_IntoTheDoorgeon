using UnityEngine;
using Random = UnityEngine.Random;

namespace DoorGame
{
    public class GameController : MonoBehaviour
    {
        [Header("Score Variables")] 
        [SerializeField] private int minimumScoreToAdd = 15;
        [SerializeField] private int maxScoreToAdd = 45;
        [Space]
        [SerializeField] private DoorController doorController;
        
        // Game state variables.
        private int _wavesCompleted;
        
        // Score variables.
        private int _score;
        private int _highScore;
        private int _validDoorsSelected;

        private void Awake()
        {
            _highScore = PlayerPrefs.GetInt("HighScore", 0);
        }

        private void Start()
        {
            doorController.GenerateDoors();
        }

        private void LeaveGame()
        {
            SaveHighScore();
        }

        private void WaveWon()
        {
            _wavesCompleted++;
        }

        private void WaveLost()
        {
            ScoreLost();
        }

        private void StartNextWave()
        {
            _validDoorsSelected = 0;
            
            doorController.GenerateDoors();
        }
        
        // Score
        public void AddScore()
        {
            _validDoorsSelected++;
            _score += Random.Range(minimumScoreToAdd, maxScoreToAdd + 1) * _validDoorsSelected;
        }

        private void ScoreLost()
        {
            _score = 0;
        }

        private void SaveHighScore()
        {
            if (_score > _highScore)
            {
                PlayerPrefs.SetInt("HighScore", _score);
            }
        }
    }
}
