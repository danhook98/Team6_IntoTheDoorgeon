using UnityEngine;
using DoorGame.Events;
using DoorGame.Door;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

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
        }

        private void Update()
        {
            _mouseScreenPos = Input.mousePosition;
        }

        public void LeaveGame()
        {
            SaveHighScore();
            //Make Win Screen visible
        }

        public void WaveWon()
        {
            _wavesCompleted++;
        }

        public void OpenedBadDoor()
        {
            Debug.Log("Game Over!", this);
            
            gameOverEvent.Invoke(new Empty());
            
            _score = 0;
            scoreChangedEvent.Invoke(_score);
            
            _totalDoorsOpened = 0;
            validDoorsOpenedEvent.Invoke(_totalDoorsOpened);
        }

        public void OpenedGoodDoor()
        {
            Debug.Log("Opened Good Door!", this);
            
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