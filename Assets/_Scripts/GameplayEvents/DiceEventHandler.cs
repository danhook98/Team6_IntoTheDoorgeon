using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace DoorGame
{
    public class DiceEventHandler : MonoBehaviour
    {
        [Header("Dice prefab")]
        [SerializeField] private GameObject dicePrefab;
        
        [Header("Available spawn positions")]
        [SerializeField] private List<Vector2> playerDiceSpawns;
        [SerializeField] private List<Vector2> enemyDiceSpawns;
        
        [Header("Used spawn positions")]
        [SerializeField] private List<Vector2> usedPlayerDiceSpawns;
        [SerializeField] private List<Vector2> usedEnemyDiceSpawns;

        [Header("Spawned dice")]
        [SerializeField] private List<GameObject> dice;
        
        [Header("Dice Container")]
        [SerializeField] private GameObject diceContainer;

        private int _playerSelectedDiceAmount;
        private int _enemySelectedDiceAmount;

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

        private void SpawnDice()
        {
            // Spawn player dice
            for (int i = 0; i < _amountOfDice; i++)
            {
                int randomSpawnPoint = Random.Range(0, playerDiceSpawns.Count);
                Vector2 spawnPoint = playerDiceSpawns[randomSpawnPoint];
                usedPlayerDiceSpawns.Add(spawnPoint);
                GameObject dice = Instantiate(dicePrefab, spawnPoint, Quaternion.identity);
                dice.transform.SetParent(diceContainer.transform, false);
                playerDiceSpawns.RemoveAt(randomSpawnPoint);
            }
            
            // Spawn enemy dice
            for (int i = 0; i < _amountOfDice; i++)
            {
                int randomSpawnPoint = Random.Range(0, enemyDiceSpawns.Count);
                Vector2 spawnPoint = enemyDiceSpawns[randomSpawnPoint];
                usedEnemyDiceSpawns.Add(spawnPoint);
                GameObject dice = Instantiate(dicePrefab, spawnPoint, Quaternion.identity);
                dice.transform.SetParent(diceContainer.transform, false);
                enemyDiceSpawns.RemoveAt(randomSpawnPoint);
            }
        }

        private void ResetDice()
        {
            
        }
    }
}
