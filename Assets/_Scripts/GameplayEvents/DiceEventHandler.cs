using System.Collections.Generic;
using UnityEngine;
using DoorGame.EventSystem;

namespace DoorGame
{
    public class DiceEventHandler : MonoBehaviour
    {
        [Header("Dice Prefab")]
        [SerializeField] private GameObject dicePrefab;
        
        [Header("Available spawn positions")]
        [SerializeField] private List<Vector2> playerDiceSpawns;
        [SerializeField] private List<Vector2> enemyDiceSpawns;
        
        [Header("Used Spawn Positions")]
        [SerializeField] private List<Vector2> usedPlayerDiceSpawns;
        [SerializeField] private List<Vector2> usedEnemyDiceSpawns;

        [Header("Dice List")]
        [SerializeField] private List<Dice> diceList;
        [SerializeField] private List<Dice> playerSelectedDiceList;
        [SerializeField] private List<Dice> enemySelectedDiceList;
        
        [Header("Dice Container")]
        [SerializeField] private GameObject diceContainer;

        [Header("Events")] 
        [SerializeField] private VoidEvent onResetDiceEvent;
        [SerializeField] private AudioClipSOEvent onPlaySfxEvent;

        private int _playerSelectedDiceAmount = 0;
        private int _enemySelectedDiceAmount = 0;
        private int _playerTotalScore = 0;
        private int _enemyTotalScore = 0;
        
        private bool _playerHasAOne;
        private bool _enemyHasAOne;

        private static int _amountOfDice = 5;

        private void Update()
        {
            // TODO: remove after testing.
            if (Input.GetKeyDown(KeyCode.Space))
            {
                RollDice();
            }
            
            if (Input.GetKeyDown(KeyCode.S))
            {
                SpawnDice();
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                ResetDice();
            }

            if (Input.GetKeyDown(KeyCode.T))
            {
                ReadPlayerDiceResults();
            }
        }

        /// <summary>
        /// Spawns 5 enemy dice and 5 player dice.
        /// </summary>
        private void SpawnDice()
        {
            int autoId = 0;
            
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
                diceList.Add(dice.gameObject.GetComponent<Dice>());
                dice.gameObject.GetComponent<Dice>().DiceID = autoId;
                autoId++;
                
                // Enemy Dice
                int randomSpawnPoint2 = Random.Range(0, enemyDiceSpawns.Count);
                Vector2 spawnPoint2 = enemyDiceSpawns[randomSpawnPoint2];
                usedEnemyDiceSpawns.Add(spawnPoint2);
                GameObject dice2 = Instantiate(dicePrefab, spawnPoint2, Quaternion.identity);
                dice2.transform.SetParent(diceContainer.transform, false);
                enemyDiceSpawns.RemoveAt(randomSpawnPoint2);
                diceList.Add(dice2.gameObject.GetComponent<Dice>());
                dice2.gameObject.GetComponent<Dice>().DiceID = autoId;
                dice2.tag = "EnemyDie";
                autoId++;
            }
        }

        /// <summary>
        /// Resets lists and destroys dice.
        /// </summary>
        private void ResetDice()
        {
            // Move dice spawn positions to original lists.
            for (int i = 0; i < _amountOfDice; i++)
            {
                Vector2 playerSpawnPoint = usedPlayerDiceSpawns[i];
                playerDiceSpawns.Add(playerSpawnPoint);
                Vector2 enemySpawnPoint = usedEnemyDiceSpawns[i];
                enemyDiceSpawns.Add(enemySpawnPoint);
            }

            // Destroy dice.
            for (int i = 0; i < 10; i++)
            {
                diceList[i].DestroySelf();
            }
            
            // Clear lists.
            diceList.Clear();
            usedEnemyDiceSpawns.Clear();
            usedPlayerDiceSpawns.Clear();
        }

        public void DieHasBeenSelected(int instanceId)
        {
            Dice selectedDie = diceList.Find(c => c.GetInstanceID() == instanceId);
            
            if (selectedDie.CompareTag("EnemyDie")) return;
            
            playerSelectedDiceList.Add(selectedDie);
            diceList.Remove(selectedDie);
            
            _playerSelectedDiceAmount++;
        }

        public void RollDice()
        {
            for (int i = 0; i < playerSelectedDiceList.Count; i++)
            {
                var selectedDie = playerSelectedDiceList[i];
                StartCoroutine(selectedDie.RollDie(18));
            }
        }

        public void ReadPlayerDiceResults()
        {
            int _firstDieResult = playerSelectedDiceList[0].playerDiceResult;
            int _secondDieResult = 0;
            int _thirdDieResult = 0;
            int _fourthDieResult = 0;
            int _fifthDieResult = 0;
            
            // Replace this with checks for how many dice have been selected. playerselectedicelist[2] does not exist
            // if the player has not selected 3 dice!
            if(_secondDieResult != null) _secondDieResult = playerSelectedDiceList[1].playerDiceResult;
            if(_thirdDieResult != null) _thirdDieResult = playerSelectedDiceList[2].playerDiceResult;
            if(_fourthDieResult != null) _fourthDieResult = playerSelectedDiceList[3].playerDiceResult;
            if(_fifthDieResult != null) _fifthDieResult = playerSelectedDiceList[4].playerDiceResult;
            
            _playerTotalScore = _firstDieResult + _secondDieResult + _thirdDieResult + _fourthDieResult + _fifthDieResult;

            for (int i = 0; i < playerSelectedDiceList.Count; i++)
            {
                if (playerSelectedDiceList[i].playerDiceResult == 1) _playerHasAOne = true;
            }
            Debug.Log("Does player have a one: " + _playerHasAOne);
            Debug.Log("Player total: " + _playerTotalScore);
        }

        public void ReadEnemyDiceResults()
        {
            
        }

        public void CompareResults()
        {
            
        }
    }
}