using System;
using UnityEngine;
using DoorGame.Events;
using DoorGame.Door;
using Random = UnityEngine.Random;

namespace DoorGame
{
    public class GameController : MonoBehaviour
    {
        [Header("Events")] 
        [SerializeField] private VoidEvent gameOverEvent;
        [SerializeField] private IntEvent scoreChangedEvent;
        [SerializeField] private IntEvent validDoorsOpenedEvent;
        
        [Header("Score Variables")] 
        [SerializeField] private int minimumScoreToAdd = 15;
        [SerializeField] private int maxScoreToAdd = 45;
        [Space]
        [SerializeField] private DoorController doorController;
        
        // Game variables.
        private int _wavesCompleted = 0;
        private byte _numberOfDoors = 3;
        
        // Score variables.
        private int _score = 0;
        private int _highScore = 0;
        private int _validDoorsSelected = 0;

        private void Awake()
        {
            _highScore = PlayerPrefs.GetInt("HighScore", 0);
        }

        private void Start()
        {
            doorController.GenerateDoors();
        }

        public void LeaveGame()
        {
            SaveHighScore();
        }

        public void WaveWon()
        {
            _wavesCompleted++;
        }

        public void OpenedBadDoor()
        {
            ScoreLost();
            gameOverEvent.Invoke(new Empty());
            Debug.Log("Game Over!", this);
        }

        public void OpenedGoodDoor()
        {
            Debug.Log("Opened Good Door!", this);
            
            AddScore();

            if (_validDoorsSelected == _numberOfDoors)
            {
                StartNextWave();
            }
        }

        private void StartNextWave()
        {
            Debug.Log("Starting next wave", this);
            
            _validDoorsSelected = 0;
            
            doorController.GenerateDoors();
        }
        
        // Score
        private void AddScore()
        {
            _validDoorsSelected++;
            _score += Random.Range(minimumScoreToAdd, maxScoreToAdd + 1) * _validDoorsSelected;
            
            // Trigger the OnScoreChanged and OnValidDoorsOpenedChanged events.
            scoreChangedEvent.Invoke(_score);
            validDoorsOpenedEvent.Invoke(_validDoorsSelected);
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