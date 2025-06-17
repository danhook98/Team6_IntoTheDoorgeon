using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;
using DoorGame.Audio;
using DoorGame.EventSystem;

namespace DoorGame
{
    public class GameController : MonoBehaviour
    {
        [Header("Events")] 
        [SerializeField] private VoidEvent gameOverEvent;
        [SerializeField] private VoidEvent showGameOverScreenEvent;
        [SerializeField] private IntEvent scoreChangedEvent;
        [SerializeField] private IntEvent highScoreChangedEvent;
        [SerializeField] private IntEvent validDoorsOpenedEvent;
        [SerializeField] private VoidEvent generateDoorsEvent; 
        [SerializeField] private VoidEvent onLeaveDungeonEvent;
        [SerializeField] private BoolEvent showEnterDungeonButtonEvent;
        
        [Header("Gameplay Event Trigger Events")]
        [SerializeField] private VoidEvent onStartWheelOfFortuneEvent;
        [SerializeField] private VoidEvent onStartPairsEvent;
        [SerializeField] private VoidEvent onStartMysteriousDoorsEvent;
        [SerializeField] private VoidEvent onStartDiceEvent;
        
        [Header("Score Variables")] 
        [SerializeField] private int minimumScoreToAdd = 15;
        [SerializeField] private int maxScoreToAdd = 45;
        
        [Header("Value Objects")]
        [SerializeField] private IntValue scoreValue;
        [SerializeField] private IntValue doorsOpenedValue;

        [Header("SFX Sounds")] 
        [SerializeField] private AudioClipSOEvent playSfxAudioChannel; 
        [SerializeField] private AudioClipSO gameStartSound;
        [SerializeField] private AudioClipSO leaveDungeonSound;
        [SerializeField] private AudioClipSO scoreAddedSound;
        [SerializeField] private AudioClipSO goodDoorSound;
        [SerializeField] private AudioClipSO badDoorSound;
        
        [Header("Music")]
        [SerializeField] private AudioClipSOEvent playMusicAudioChannel;
        [SerializeField] private AudioClipSO[] gameMusic;
        
        // Game variables.
        private int _wavesCompleted = 0;
        private byte _numberOfDoors = 3;
        
        // Score variables.
        private int _score = 0;
        private int _highScore = 0;
        private int _scoreMultiplier = 1;
        private int _validDoorsOpened = 0;
        private int _totalDoorsOpened = 0;
        
        private void Awake()
        {
            _highScore = PlayerPrefs.GetInt("HighScore", 0);
        }

        private void Start()
        {
            generateDoorsEvent.Invoke(new Empty());
            
            // Play the start game sound. 
            playSfxAudioChannel.Invoke(gameStartSound);
            
            // Choose a random piece of background music (temporary).
            AudioClipSO music = gameMusic[Random.Range(0, gameMusic.Length)];
            playMusicAudioChannel.Invoke(music);
        }

        public void LeaveGame()
        {
            SaveHighScore();
            playSfxAudioChannel.Invoke(leaveDungeonSound);
        }

        public void WaveWon()
        {
            _wavesCompleted++;
        }

        public void OpenedBadDoor()
        {
            Debug.Log("Game Over!", this);
            
            playSfxAudioChannel.Invoke(badDoorSound);
            
            gameOverEvent.Invoke(new Empty());
            StartCoroutine(ShowGameOverScreen());
            
            _score = 0;
            scoreValue.Value = 0; 
            scoreChangedEvent.Invoke(_score);
            
            _totalDoorsOpened = 0;
            validDoorsOpenedEvent.Invoke(_totalDoorsOpened);
        }

        private IEnumerator ShowGameOverScreen()
        {
            yield return new WaitForSeconds(3f);
            showGameOverScreenEvent.Invoke(new Empty());
        }

        public void OpenedGoodDoor()
        {
            Debug.Log("Opened Good Door!", this);
            
            playSfxAudioChannel.Invoke(goodDoorSound);
            
            AddScore();
            
            if (_validDoorsOpened == 1)
            {
                showEnterDungeonButtonEvent.Invoke(true);
            }
        }

        public void StartNextWave()
        {
            Debug.Log("Starting next wave", this);
            
            showEnterDungeonButtonEvent.Invoke(false);
            _validDoorsOpened = 0;
            _wavesCompleted++;
            _scoreMultiplier++;
            generateDoorsEvent.Invoke(new Empty());
        }

        public void SetScore(int score) => _score = score;

        // Score
        private void AddScore()
        {
            _validDoorsOpened++;
            _score += Random.Range(minimumScoreToAdd, maxScoreToAdd + 1) * _validDoorsOpened * (1 + _wavesCompleted) * _scoreMultiplier;
            scoreValue.Value = _score;
            
            // Trigger the OnScoreChanged and OnValidDoorsOpenedChanged events.
            scoreChangedEvent.Invoke(_score);
            playSfxAudioChannel.Invoke(scoreAddedSound);
            Debug.Log(_wavesCompleted);
            
            _totalDoorsOpened++;
            doorsOpenedValue.Value = _totalDoorsOpened;
            
            validDoorsOpenedEvent.Invoke(_totalDoorsOpened);
        }

        private void SaveHighScore()
        {
            if (_score > _highScore)
            {
                PlayerPrefs.SetInt("HighScore", _score);
                highScoreChangedEvent.Invoke(_score);
            }
            else
            {
                highScoreChangedEvent.Invoke(PlayerPrefs.GetInt("HighScore", 0));
            }
        }
    }
}