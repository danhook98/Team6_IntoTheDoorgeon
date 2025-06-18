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
        [SerializeField] private IntEvent dungeonsEnteredChangedEvent;
        [SerializeField] private VoidEvent generateDoorsEvent; 
        [SerializeField] private VoidEvent onLeaveDungeonEvent;
        [SerializeField] private BoolEvent showEnterDungeonButtonEvent;
        
        [Header("Gameplay Event Trigger Events")]
        [SerializeField] private VoidEvent[] gameplayEventTriggers;
        
        [Header("Score Variables")] 
        [SerializeField] private int minimumScoreToAdd = 15;
        [SerializeField] private int maxScoreToAdd = 45;
        
        [Header("Value Objects")]
        [SerializeField] private IntValue scoreValue;
        [SerializeField] private IntValue doorsOpenedValue;
        [SerializeField] private IntValue dungeonsEnteredValue;

        [Header("SFX Sounds")] 
        [SerializeField] private AudioClipSOEvent playSfxAudioChannel; 
        [SerializeField] private AudioClipSO gameStartSound;
        [SerializeField] private AudioClipSO leaveDungeonSound;
        [SerializeField] private AudioClipSO scoreAddedSound;
        [SerializeField] private AudioClipSO goodDoorSound;
        [SerializeField] private AudioClipSO badDoorSound;
        [SerializeField] private AudioClipSO[] eventCardStartSounds; 
        
        [Header("Music")]
        [SerializeField] private AudioClipSOEvent playMusicAudioChannel;
        [SerializeField] private AudioClipSO[] gameMusic;
        
        // Game variables.
        private int _dungeonsEntered = 0;
        private byte _numberOfDoors = 3;
        
        // Score variables.
        private int _score = 0;
        private int _highScore = 0;
        private float _scoreMultiplier = 1f;
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

        public void OpenedBadDoor()
        {
            //Debug.Log("Game Over!", this);
            
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
            //Debug.Log("Opened Good Door!", this);
            
            playSfxAudioChannel.Invoke(goodDoorSound);
            
            AddScore();
            
            if (_validDoorsOpened == 1)
            {
                showEnterDungeonButtonEvent.Invoke(true);
            }
        }

        private void DisplayGameplayEvent()
        {
            if (_score == 0 || gameplayEventTriggers.Length == 0) return;
            
            int randomChance = Random.Range(1, 101);

            if (randomChance < 20)
            {
                // Trigger one of the random events. 
                gameplayEventTriggers[Random.Range(0, gameplayEventTriggers.Length)].Invoke(new Empty());
                playSfxAudioChannel.Invoke(eventCardStartSounds[Random.Range(0, eventCardStartSounds.Length)]);
                Debug.Log("Triggering a gameplay event", this);
            }
        }

        public void EnterNextDungeon()
        {
            //Debug.Log("Starting next wave", this);
            
            showEnterDungeonButtonEvent.Invoke(false);
            _validDoorsOpened = 0;
            
            _dungeonsEntered++;
            dungeonsEnteredValue.Value = _dungeonsEntered;
            dungeonsEnteredChangedEvent.Invoke(_dungeonsEntered);
            
            generateDoorsEvent.Invoke(new Empty());

            DisplayGameplayEvent();

            _scoreMultiplier = 1f;
        }

        public void SetScore(int score)
        {
            _score = score;
            scoreValue.Value = score;
        }
        
        public void SetMultiplier(int multiplier) => _scoreMultiplier = multiplier;

        // Score
        private void AddScore()
        {
            _validDoorsOpened++;
            
            float score = Random.Range(minimumScoreToAdd, maxScoreToAdd + 1) * _validDoorsOpened * (1 + _dungeonsEntered) * (1 + _dungeonsEntered) * _scoreMultiplier; 
            _score += Mathf.RoundToInt(score);
            
            scoreValue.Value = _score;
            
            // Trigger the OnScoreChanged and OnValidDoorsOpenedChanged events.
            scoreChangedEvent.Invoke(_score);
            playSfxAudioChannel.Invoke(scoreAddedSound);
            // Debug.Log(_wavesCompleted);
            
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