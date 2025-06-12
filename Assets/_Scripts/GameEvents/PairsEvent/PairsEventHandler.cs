using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Random = UnityEngine.Random;
using DoorGame.EventSystem;

namespace DoorGame.GameEvents.PairsEvent
{
    public class PairsEventHandler : MonoBehaviour
    {
        [Header("Track cards")]
        [SerializeField] private List<GameObject> availableCards;
        [SerializeField] private List<GameObject> usedCards;
        
        [Header("Spawn positions")]
        [SerializeField] private List<Vector2> spawnPositionsAvailable;
        [SerializeField] private List<Vector2> spawnPositionsUsed;
        
        [Header("Attempts")]
        [SerializeField] private int attempts;

        [Header("Events")] 
        [SerializeField] private VoidEvent onCardsMatchEvent;
        [SerializeField] private VoidEvent onCardsDoNotMatchEvent;
        
        
        private int _completedPairs;
        private const int TotalPairs = 8;

        private int _numberOfFlippedCards; // Tracks how many cards have been flipped this turn (Should only be 0, 1 or 2).
        private int _firstCardID;

        private GameObject _firstCard;
        private GameObject _secondCard;
        
        private void Start()
        {
            attempts = 5;
            _numberOfFlippedCards = 0;
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
                int cardID = Random.Range(0, availableCards.Count);
                GameObject lastCardSpawned = Instantiate(availableCards[cardID], spawnPosition, Quaternion.identity);
                lastCardSpawned.transform.SetParent(transform, false);
                
                // Update lists with correct information.
                spawnPositionsUsed.Add(spawnPosition); 
                spawnPositionsAvailable.RemoveAt(randomSpawnPoint);
                
                // 2nd copy of card is instantiated at a different random position.
                randomSpawnPoint = Random.Range(0, spawnPositionsAvailable.Count);
                spawnPosition = spawnPositionsAvailable[randomSpawnPoint];
                lastCardSpawned = Instantiate(availableCards[cardID], spawnPosition, Quaternion.identity);
                lastCardSpawned.transform.SetParent(transform, false);
                
                // Update lists with correct information.
                spawnPositionsUsed.Add(spawnPosition); 
                spawnPositionsAvailable.RemoveAt(randomSpawnPoint);
                usedCards.Add(lastCardSpawned);
                availableCards.RemoveAt(cardID);
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

        public void CardHasBeenFlipped(int instanceId)
        {
            if (_numberOfFlippedCards == 2) return;
            
            _numberOfFlippedCards++;
            if (_numberOfFlippedCards == 1)
            {
                //_firstCardID = instanceId;
                for (int i = 0; i < usedCards.Count; i++)
                {
                    if (instanceId == usedCards[i].GetInstanceID())
                    {
                        Debug.Log("IF statement triggered!");
                        // Save and compare game object instead!
                        _firstCard = usedCards[i];
                        Debug.Log(_firstCard);
                        Debug.Log("First card: " + usedCards[i]);
                        break;
                    }
                }
                Debug.Log("Search completed");
            }
            
            else if (_numberOfFlippedCards == 2)
            {
                for (int i = 0; i < usedCards.Count; i++)
                {
                    if (instanceId == usedCards[i].GetInstanceID())
                    {
                        // Save and compare game object instead!
                        _secondCard = usedCards[i];
                        Debug.Log("2nd card: " + usedCards[i]);
                        Debug.Log(_secondCard);
                        break;
                    }
                    Debug.Log("Search completed 2");
                }

                if (_firstCard == _secondCard)
                {
                    Debug.Log("Cards match");
                    onCardsMatchEvent.Invoke(new Empty());
                    _completedPairs++;
                }
                else
                {
                    Debug.Log("Cards do not match");
                }
                
                    
                /*if(instanceId == usedCards.BinarySearch(usedCards[_firstCardID]))    
                {
                    Debug.Log("Cards match");
                    onCardsMatchEvent.Invoke(new Empty());
                    _completedPairs++;
                }
                else
                {
                    Debug.Log("Cards do not match");
                    onCardsDoNotMatchEvent.Invoke(new Empty());
                }*/
                _numberOfFlippedCards = 0;
                attempts--;
            }
        }
    }
}
