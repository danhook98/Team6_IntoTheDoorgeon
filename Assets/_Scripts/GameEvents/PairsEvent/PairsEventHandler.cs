using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DoorGame.GameEvents.PairsEvent
{
    public class PairsEventHandler : MonoBehaviour
    {
        [Header("Track card positions")]
        [SerializeField] private List<GameObject> cards;
        [SerializeField] private List<GameObject> availableCardPairs;
        [SerializeField] private List<GameObject> usedCards;
        
        [Header("Spawn positions")]
        [SerializeField] private List<Vector2> spawnPositionsAvailable;
        [SerializeField] private List<Vector2> spawnPositionsUsed;
        
        [Header("Attempts")]
        [SerializeField] private int attempts;
        
        private int _completedPairs;
        private const int TotalPairs = 8;

        private void Start()
        {
            attempts = 5;
            SpawnCards();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                ResetCards();
            }
        }

        private void SpawnCards()
        {
            for (int i = 0; i < TotalPairs; i++)
            {
                // Random available position is selected.
                int randomSpawnPoint = Random.Range(0, spawnPositionsAvailable.Count);
                Vector3 spawnPosition = spawnPositionsAvailable[randomSpawnPoint];
                
                // Random card is selected & instantiated.
                int cardID = Random.Range(0, availableCardPairs.Count);
                GameObject lastCardSpawned = Instantiate(availableCardPairs[cardID], spawnPosition, Quaternion.identity);
                lastCardSpawned.transform.SetParent(transform, false);
                
                // Update lists with correct information.
                spawnPositionsUsed.Add(spawnPosition); 
                spawnPositionsAvailable.RemoveAt(randomSpawnPoint);
                
                // 2nd copy of card is instantiated at a different random position.
                randomSpawnPoint = Random.Range(0, spawnPositionsAvailable.Count);
                spawnPosition = spawnPositionsAvailable[randomSpawnPoint];
                lastCardSpawned = Instantiate(availableCardPairs[cardID], spawnPosition, Quaternion.identity);
                lastCardSpawned.transform.SetParent(transform, false);
                
                // Update lists with correct information.
                spawnPositionsUsed.Add(spawnPosition); 
                spawnPositionsAvailable.RemoveAt(randomSpawnPoint);
                availableCardPairs.RemoveAt(cardID);
            }
        }

        private void ResetCards()
        {
            Debug.Log("Resetting cards");
            for (int i = 0; i < 12; i++)
            {
                spawnPositionsAvailable.Add(spawnPositionsUsed[i]);
            }
            spawnPositionsUsed.Clear();
        }
    }
}
