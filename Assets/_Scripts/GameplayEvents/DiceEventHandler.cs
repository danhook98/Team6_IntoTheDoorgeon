using System.Collections.Generic;
using UnityEngine;
using DoorGame.EventSystem;
using System.Collections;
using DoorGame.Audio;
using TMPro;

namespace DoorGame
{
    public class DiceEventHandler : MonoBehaviour
    {
        [Header("Available spawn positions")]
        [SerializeField] private List<Vector2> playerDiceSpawns;
        [SerializeField] private List<Vector2> enemyDiceSpawns;
        
        [Header("Used Spawn Positions")]
        [SerializeField] private List<Vector2> usedPlayerDiceSpawns;
        [SerializeField] private List<Vector2> usedEnemyDiceSpawns;
        
        [Header("Dice List")]
        [SerializeField] private List<Dice> playerDiceList;
        [SerializeField] private List<Dice> enemyDiceList;
        [SerializeField] private List<Dice> playerSelectedDiceList;
        [SerializeField] private List<Dice> enemySelectedDiceList;
        
        [Header("Game Objects")]
        [SerializeField] private GameObject diceContainer;
        [SerializeField] private GameObject dicePrefab;
        [SerializeField] private GameObject introCard;
        [SerializeField] private GameObject endCard;
        [SerializeField] private TextMeshProUGUI scoreToBetText;
        [SerializeField] private TextMeshProUGUI resultText;

        [Header("Events")] 
        [SerializeField] private AudioClipSOEvent onPlaySfxEvent;
        [SerializeField] private IntEvent onScoreChangedEvent;

        [Header("Score")] 
        [SerializeField] private IntValue scoreValue;
        
        [Header("Audio Clip SOs")]
        [SerializeField] private AudioClipSO rollDiceSfx;

        private int _playerSelectedDiceAmount = 0;
        private int _enemySelectedDiceAmount = 0;
        private int _playerTotalScore = 0;
        private int _enemyTotalScore = 0;
        
        private bool _playerHasAOne = false;
        private bool _enemyHasAOne = false;

        private static int _amountOfDice = 5;

        private int _amountToBet;
        private int _minAmountToBet;
        private bool _tie;
        private bool _playerWon;

        private void Awake()
        {
            _minAmountToBet = scoreValue.Value/10;
            _playerWon = false;
            introCard.SetActive(true);
            endCard.SetActive(false);
            scoreToBetText.text = "Betting: 10% of total score";
            _amountToBet = 10;
        }

        private void Update()
        {
            // TODO: remove after testing.
            if (Input.GetKeyDown(KeyCode.Space))
            {
                StartCoroutine(RollDice());
            }
            
            if (Input.GetKeyDown(KeyCode.S))
            {
                SpawnDice();
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                ResetEvent();
            }
        }

        /// <summary>
        /// Spawns 5 enemy dice and 5 player dice.
        /// </summary>
        public void SpawnDice()
        {
            int autoId = 0;
            introCard.SetActive(false);
            
            // Spawn dice
            for (int i = 0; i < _amountOfDice; i++)
            {
                // Player Dice
                int randomSpawnPoint = Random.Range(0, playerDiceSpawns.Count);
                Vector2 spawnPoint = playerDiceSpawns[randomSpawnPoint];
                usedPlayerDiceSpawns.Add(spawnPoint);
                GameObject dice = Instantiate(dicePrefab, spawnPoint, Quaternion.identity);
                dice.transform.SetParent(diceContainer.transform, false);
                playerDiceSpawns.RemoveAt(randomSpawnPoint);
                playerDiceList.Add(dice.gameObject.GetComponent<Dice>());
                dice.gameObject.GetComponent<Dice>().DiceID = autoId;
                autoId++;
                
                // Enemy Dice
                int randomSpawnPoint2 = Random.Range(0, enemyDiceSpawns.Count);
                Vector2 spawnPoint2 = enemyDiceSpawns[randomSpawnPoint2];
                usedEnemyDiceSpawns.Add(spawnPoint2);
                GameObject dice2 = Instantiate(dicePrefab, spawnPoint2, Quaternion.identity);
                dice2.transform.SetParent(diceContainer.transform, false);
                enemyDiceSpawns.RemoveAt(randomSpawnPoint2);
                enemyDiceList.Add(dice2.gameObject.GetComponent<Dice>());
                dice2.gameObject.GetComponent<Dice>().DiceID = autoId;
                dice2.tag = "EnemyDie";
                dice2.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
                autoId++;
            }
        }

        /// <summary>
        /// Resets lists, resets variables, and destroys dice.
        /// </summary>
        public void ResetEvent()
        {
            // Reset scores.
            _amountToBet = 10;
            _enemyTotalScore = 0;
            _playerTotalScore = 0;
            _playerHasAOne = false;
            _enemyHasAOne = false;
            endCard.SetActive(false);
            
            // Move dice spawn positions to original lists.
            for (int i = 0; i < _amountOfDice; i++)
            {
                Vector2 playerSpawnPoint = usedPlayerDiceSpawns[i];
                playerDiceSpawns.Add(playerSpawnPoint);
                Vector2 enemySpawnPoint = usedEnemyDiceSpawns[i];
                enemyDiceSpawns.Add(enemySpawnPoint);
            }

            for (int i = 0; i < _enemySelectedDiceAmount; i++)
            {
                enemyDiceList.Add(enemySelectedDiceList[i]);
            }

            for (int i = 0; i < _playerSelectedDiceAmount; i++)
            {
                playerDiceList.Add(playerSelectedDiceList[i]);
            }
            
            _playerSelectedDiceAmount = 0;
            _enemySelectedDiceAmount = 0;

            // Destroy dice.
            for (int i = 0; i < 5; i++)
            {
                playerDiceList[i].DestroySelf();
                enemyDiceList[i].DestroySelf();
            }
            
            // Clear lists.
            playerDiceList.Clear();
            enemyDiceList.Clear();
            usedEnemyDiceSpawns.Clear();
            usedPlayerDiceSpawns.Clear();
            playerSelectedDiceList.Clear();
            enemySelectedDiceList.Clear();

            if(!_tie) introCard.SetActive(true);
            if (_tie) _tie = false;
        }

        /// <summary>
        /// Checks which die has been selected, ignores enemy dice.
        /// Adds selected die to relevant lists and tracks player selected amount.
        /// </summary>
        /// <param name="instanceId"></param>
        public void DieHasBeenSelected(int instanceId)
        {
            Dice selectedDie = playerDiceList.Find(c => c.GetInstanceID() == instanceId);
            
            if (selectedDie.CompareTag("EnemyDie")) return;
            
            playerSelectedDiceList.Add(selectedDie);
            playerDiceList.Remove(selectedDie);
            
            _playerSelectedDiceAmount++;
        }

        public void DeselectAllDice()
        {
            for (int i = 0; i < playerSelectedDiceList.Count; i++)
            {
                playerDiceList.Add(playerSelectedDiceList[i]);
            }
            
            playerSelectedDiceList.Clear();
            _playerSelectedDiceAmount = 0;
        }

        public void TriggerRollDice()
        {
            StartCoroutine(RollDice());
        }

        /// <summary>
        /// Coroutine for rolling dice, rolls all dice selected
        /// by both the player and enemy. It then triggers
        /// reading the dice results after enough time for the dice to land
        /// and finally compares the results after an appropriate amount of time.
        /// </summary>
        /// <returns></returns>
        public IEnumerator RollDice()
        {
            if (_playerSelectedDiceAmount < 1) yield break;
            
            onPlaySfxEvent.Invoke(rollDiceSfx);
            
            for (int i = 0; i < playerSelectedDiceList.Count; i++)
            {
                var selectedDie = playerSelectedDiceList[i];
                StartCoroutine(selectedDie.RollDie(15));
            }

            int enemyDiceToRoll = Random.Range(1, 6);
            Debug.Log("Rolling " + enemyDiceToRoll + " enemy dice!");
            
            for (int i = 0; i < enemyDiceToRoll; i++)
            {
                _enemySelectedDiceAmount++;
                enemySelectedDiceList.Add(enemyDiceList[i]);
                var selectedDie2 = enemySelectedDiceList[i];
                StartCoroutine(selectedDie2.RollDie(15));
            }

            yield return new WaitForSeconds(3f);
            ReadPlayerDiceResults();
            ReadEnemyDiceResults();
            yield return new WaitForSeconds(0.5f);
            CompareResults();
        }

        /// <summary>
        /// Adds player results to a local list, adds up results
        /// and checks if the player has gotten a one.
        /// </summary>
        public void ReadPlayerDiceResults()
        {
            List<int> playerDiceResults = new List<int>();
            
            for (int i = 0; i < _playerSelectedDiceAmount; i++)
            {
                playerDiceResults.Add(playerSelectedDiceList[i].playerDiceResult);
            }

            for (int i = 0; i < playerDiceResults.Count; i++)
            {
                _playerTotalScore += playerDiceResults[i];
                if(playerDiceResults[i] == 1) _playerHasAOne = true;
            }
        }

        /// <summary>
        /// Adds enemy results in a local list, adds up results
        /// and checks if the enemy has gotten a one.
        /// </summary>
        public void ReadEnemyDiceResults()
        {
            List<int> enemyDiceResults = new List<int>();
            
            for (int i = 0; i < _enemySelectedDiceAmount; i++)
            {
                enemyDiceResults.Add(enemySelectedDiceList[i].enemyDiceResult);
            }

            for (int i = 0; i < enemyDiceResults.Count; i++)
            {
                _enemyTotalScore += enemyDiceResults[i];
                if(enemyDiceResults[i] == 1) _enemyHasAOne = true;
            }
        }

        /// <summary>
        /// It first checks if the player or enemy have gotten a one.
        /// If not, it then compares their total scores and
        /// triggers the relevant function.
        /// </summary>
        public void CompareResults()
        {
            if (_playerHasAOne && _enemyHasAOne) Tie();
            else if (_playerHasAOne) StartCoroutine(EnemyWins());
            else if(_enemyHasAOne) StartCoroutine(PlayerWins());
            else if (_enemyTotalScore > _playerTotalScore && !_playerHasAOne && !_enemyHasAOne) StartCoroutine(EnemyWins());
            else if(_playerTotalScore > _enemyTotalScore && !_playerHasAOne && !_enemyHasAOne) StartCoroutine(PlayerWins());
            else if(_enemyTotalScore == _playerTotalScore && !_playerHasAOne && !_enemyHasAOne) Tie();
        }

        /// <summary>
        /// Resets event & spawns dice so that the player can bet again.
        /// </summary>
        public void Tie()
        {
            Debug.Log("It's a tie!");
            _tie = true;
            ResetEvent();
            SpawnDice();
        }

        public IEnumerator EnemyWins()
        {
            Debug.Log("Enemy wins!");
            int _scoreLost = scoreValue.Value;
            _playerWon = false;
            yield return new WaitForSeconds(1f);
            UpdateScore();
            endCard.SetActive(true);
            // Calculate score.
            // Send through event.
            // doorsValue.Value to access doors opened value.
            
        }

        public IEnumerator PlayerWins()
        {
            Debug.Log("Player wins!");
            _playerWon = true;
            yield return new WaitForSeconds(1f);
            UpdateScore();
            endCard.SetActive(true);
        }

        public void IncreaseBetAmount()
        {
            _amountToBet += 10;
            
            if(_amountToBet >= 100) _amountToBet = 100;
            scoreToBetText.text = "Betting: " + _amountToBet + "% of total score";
        }

        public void DecreaseBetAmount()
        {
            _amountToBet -= 10;
            
            if(_amountToBet <= 10) _amountToBet = 10;
            scoreToBetText.text = "Betting: " + _amountToBet + "% of total score";
        }

        public void UpdateScore()
        {
            int newScore;
            
            if (_playerWon)
            {
                newScore = scoreValue.Value + (scoreValue.Value * (_amountToBet/100));
                resultText.text = "You won!\n " + "Score: " + newScore.ToString() + "\n Player dice results: " + _playerTotalScore + "\n Enemy dice results: " + _enemyTotalScore;
            }
            else
            {
                newScore = scoreValue.Value - (scoreValue.Value * (1 - (_amountToBet/100)));
                resultText.text = "You lost!\n " + "Score: " + newScore.ToString() + "\n Player dice results: " + _playerTotalScore + "\n Enemy dice results: " + _enemyTotalScore;
            }
            
            onScoreChangedEvent.Invoke(newScore);
        }
    }
}