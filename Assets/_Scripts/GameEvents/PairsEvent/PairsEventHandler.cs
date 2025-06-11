using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DoorGame.GameEvents.PairsEvent
{
    public class PairsEventHandler : MonoBehaviour
    {
        // Add new list with 12 card prefabs. Then select 8 of these and place into available cards.
        [SerializeField] private List<GameObject> cards;
        [SerializeField] private List<GameObject> availableCardPairs;
        [SerializeField] private List<GameObject> usedCards;
        
        [SerializeField] private List<Vector2> spawnPositionsAvailable;
        [SerializeField] private List<Vector2> spawnPositionsUsed;
        
        [SerializeField] private int attempts;
        
        private int _completedPairs;
        private const int TotalPairs = 8;

        private void Start()
        {
            attempts = 5;
            //SpawnCards();
        }

        private void SpawnCards()
        {
            for (int i = 0; i < TotalPairs; i++)
            {
                int randomSpawnPoint = Random.Range(0, spawnPositionsAvailable.Count);
                Vector3 spawnPosition = spawnPositionsAvailable[randomSpawnPoint];
                spawnPositionsUsed.Add(spawnPosition); 
                var cardID = Random.Range(0, availableCardPairs.Count);
                //Instantiate(_availableCards(cardID), spawnPosition, Quaternion.identity);
               // _availableCards.RemoveAt(cardID);
            }
        }
    }
}
