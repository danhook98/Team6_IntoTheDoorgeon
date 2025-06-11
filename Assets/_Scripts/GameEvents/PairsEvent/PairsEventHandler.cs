using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DoorGame
{
    public class PairsEventHandler : MonoBehaviour
    {
        // Add new list with 12 card prefabs. Then select 8 of these and place into available cards.
        [SerializeField] private List<GameObject> _availableCards;
        [SerializeField] private List<GameObject> _usedCards;
        [SerializeField] private int _lives;
        [SerializeField] private List<Vector2> _spawnPositionsAvailable;
        [SerializeField] private List<Vector2> _spawnPositionsUsed;
        
        private int _completedPairs;
        private int _totalPairs;

        private void Start()
        {
            _lives = 3;
            SpawnCards();
        }

        private void SpawnCards()
        {
            _totalPairs = _availableCards.Count;
            for (int i = 0; i < _totalPairs; i++)
            {
                int randomSpawnPoint = Random.Range(0, _spawnPositionsAvailable.Count);
                Vector3 spawnPosition = _spawnPositionsAvailable[randomSpawnPoint];
                _spawnPositionsUsed.Add(spawnPosition); 
                var cardID = Random.Range(0, _availableCards.Count);
                //Instantiate(_availableCards(cardID), spawnPosition, Quaternion.identity);
               // _availableCards.RemoveAt(cardID);
            }
        }
    }
}
