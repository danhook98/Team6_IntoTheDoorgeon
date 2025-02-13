using System;
using UnityEngine;
using DoorGame.Events;
using DoorGame.Door;
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

        [Header("Events")] 
        [SerializeField] private IntEvent scoreChangedEvent;
        [SerializeField] private IntEvent validDoorsOpenedEvent;
        
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

        public void LeaveGame()
        {
            SaveHighScore();
        }

        public void WaveWon()
        {
            _wavesCompleted++;
        }

        public void WaveLost()
        {
            ScoreLost();
            Debug.Log("Game Over!");
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