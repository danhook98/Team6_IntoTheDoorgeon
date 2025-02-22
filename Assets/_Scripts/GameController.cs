using UnityEngine;
using Random = UnityEngine.Random;
using DoorGame.Events;
using DoorGame.Door;
using DoorGame.Audio;

namespace DoorGame
{
    public class GameController : MonoBehaviour
    {
        [Header("Events")] 
        [SerializeField] private VoidEvent gameOverEvent;
        [SerializeField] private IntEvent scoreChangedEvent;
        [SerializeField] private IntEvent validDoorsOpenedEvent;
        [SerializeField] private FloatEvent onPlayerPositionChangeEvent;
        [SerializeField] private VoidEvent onLeaveDungeonEvent;
        
        [Header("Score Variables")] 
        [SerializeField] private int minimumScoreToAdd = 15;
        [SerializeField] private int maxScoreToAdd = 45;
        [Space]
        [SerializeField] private DoorController doorController;

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
        private int _validDoorsOpened = 0;
        private int _totalDoorsOpened = 0;

        // Player
        private GameObject _player;

        private Vector3 _mouseScreenPos;
        private void Awake()
        {
            _highScore = PlayerPrefs.GetInt("HighScore", 0);
            _player = GameObject.FindGameObjectWithTag("Player");
        }

        private void Start()
        {
            doorController.GenerateDoors();
            
            // Play the start game sound. 
            playSfxAudioChannel.Invoke(gameStartSound);
            
            // Choose a random piece of background music (temporary).
            AudioClipSO music = gameMusic[Random.Range(0, gameMusic.Length)];
            playMusicAudioChannel.Invoke(music);
        }

        private void Update()
        {
            _mouseScreenPos = Input.mousePosition;
        }

        public void LeaveGame()
        {
            SaveHighScore();
            playSfxAudioChannel.Invoke(leaveDungeonSound);
            onLeaveDungeonEvent.Invoke(new Empty());
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
            
            _score = 0;
            scoreChangedEvent.Invoke(_score);
            
            _totalDoorsOpened = 0;
            validDoorsOpenedEvent.Invoke(_totalDoorsOpened);
        }

        public void OpenedGoodDoor()
        {
            Debug.Log("Opened Good Door!", this);
            
            playSfxAudioChannel.Invoke(goodDoorSound);
            
            AddScore();

            if (_validDoorsOpened == _numberOfDoors - 1)
            {
                StartNextWave();
            }
        }

        private void StartNextWave()
        {
            Debug.Log("Starting next wave", this);
            
            _validDoorsOpened = 0;
            
            doorController.GenerateDoors();
        }
        
        // Score
        private void AddScore()
        {
            _validDoorsOpened++;
            _score += Random.Range(minimumScoreToAdd, maxScoreToAdd + 1) * _validDoorsOpened;
            
            // Trigger the OnScoreChanged and OnValidDoorsOpenedChanged events.
            scoreChangedEvent.Invoke(_score);
            playSfxAudioChannel.Invoke(scoreAddedSound);
            
            _totalDoorsOpened++;
            validDoorsOpenedEvent.Invoke(_totalDoorsOpened);
        }

        private void SaveHighScore()
        {
            if (_score > _highScore)
            {
                PlayerPrefs.SetInt("HighScore", _score);
            }
        }
        
        public void DetectMouseHover()
        {
            onPlayerPositionChangeEvent.Invoke(_mouseScreenPos.x);
        }

        public void ChangePlayerPosition()
        {
            var vector3 = _player.transform.position;
            vector3.x = _mouseScreenPos.x;
            _player.transform.position = vector3;
        }
    }
}