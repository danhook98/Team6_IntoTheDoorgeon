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
        
        [Header("Dice Container")]
        [SerializeField] private GameObject diceContainer;

        [Header("Events")] 
        [SerializeField] private VoidEvent onResetDiceEvent;
        [SerializeField] private AudioClipSOEvent onPlaySfxEvent;

        private int _playerSelectedDiceAmount = 0;
        private int _enemySelectedDiceAmount = 0;

        private static int _amountOfDice = 5;

        private void Update()
        {
            // TODO: remove after testing.
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SpawnDice();
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                ResetDice();
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
                Vector2 spawnPoint2 = enemyDiceSpawns[randomSpawnPoint];
                usedEnemyDiceSpawns.Add(spawnPoint);
                GameObject dice2 = Instantiate(dicePrefab, spawnPoint, Quaternion.identity);
                dice2.transform.SetParent(diceContainer.transform, false);
                enemyDiceSpawns.RemoveAt(randomSpawnPoint);
                diceList.Add(dice2.gameObject.GetComponent<Dice>());
                dice2.gameObject.GetComponent<Dice>().DiceID = autoId;
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
    }
}
