using System.Collections.Generic;
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
        [SerializeField] private VoidEvent onGameEndedEvent;

        [Space] 
        [SerializeField] private float timeToFlipCard;

        private int _completedPairs;
        private const int TotalPairs = 8;
        private int _numberOfFlippedCards;

        private GameObject _firstCard;
        private GameObject _secondCard;

        private PairEventCard _cardScript1;
        private PairEventCard _cardScript2;

        private void Start()
        {
            attempts = 5;
            _numberOfFlippedCards = 0;
            _completedPairs = 0;
        }

        /// <summary>
        /// Spawns 8 pairs of cards at random positions, removes
        /// used position when spawning a card.
        /// </summary>
        public void SpawnCards()
        {
            usedCards.Clear();
            List<GameObject> tempCardPool = new List<GameObject>(availableCards);

            for (int i = 0; i < TotalPairs; i++)
            {
                int prefabIndex = Random.Range(0, tempCardPool.Count);
                GameObject cardPrefab = tempCardPool[prefabIndex];
                tempCardPool.RemoveAt(prefabIndex);

                // First copy
                int index1 = Random.Range(0, spawnPositionsAvailable.Count);
                Vector2 pos1 = spawnPositionsAvailable[index1];
                spawnPositionsAvailable.RemoveAt(index1);
                spawnPositionsUsed.Add(pos1);

                GameObject card1 = Instantiate(cardPrefab, transform);
                card1.GetComponent<RectTransform>().anchoredPosition = pos1;
                usedCards.Add(card1);

                // Second copy
                int index2 = Random.Range(0, spawnPositionsAvailable.Count);
                Vector2 pos2 = spawnPositionsAvailable[index2];
                spawnPositionsAvailable.RemoveAt(index2);
                spawnPositionsUsed.Add(pos2);

                GameObject card2 = Instantiate(cardPrefab, transform);
                card2.GetComponent<RectTransform>().anchoredPosition = pos2;
                usedCards.Add(card2);
            }
        }

        /// <summary>
        /// Resets lists and variables, destroys card game objects.
        /// </summary>
        private void GameOver()
        {
            spawnPositionsAvailable.AddRange(spawnPositionsUsed);
            spawnPositionsUsed.Clear();

            foreach (var card in usedCards)
            {
                Destroy(card);
            }

            usedCards.Clear();
            onGameEndedEvent.Invoke(new Empty());
        }
        
        /// <summary>
        /// Tracks number of cards flipped. If 1 card has been flipped, it flips the card
        /// and waits for a second card. Once that happens, it checks if the names match.
        /// It triggers the appropriate event after checking depending on the result.
        /// </summary>
        /// <param name="instanceId"></param>
        public void CardHasBeenFlipped(int instanceId)
        {
            if (_numberOfFlippedCards == 2) return;

            _numberOfFlippedCards++;

            GameObject clickedCard = usedCards.Find(c => c.GetInstanceID() == instanceId);
            
            if (clickedCard == null)
            {
                _numberOfFlippedCards--;
                return;
            }

            if (_numberOfFlippedCards == 1)
            {
                _firstCard = clickedCard;
                _cardScript1 = clickedCard.GetComponent<PairEventCard>();
                _cardScript1.SetCardSpriteToFront();
            }
            else if (_numberOfFlippedCards == 2)
            {
                _secondCard = clickedCard;
                _cardScript2 = clickedCard.GetComponent<PairEventCard>();

                // Cards match
                if (DoCardsMatch(_firstCard, _secondCard))
                {
                    _cardScript2.SetCardSpriteToFront();
                    
                    //Prevent cards from being flipped
                    _cardScript1.SetBool(false);
                    _cardScript2.SetBool(false);
                    
                    onCardsMatchEvent.Invoke(new Empty());
                    _completedPairs++;
                }
                
                // Cards do not match
                else
                {
                    _cardScript1.StartCoroutine(_cardScript1.ShowCard(timeToFlipCard));
                    _cardScript2.StartCoroutine(_cardScript2.ShowCard(timeToFlipCard));
                    onCardsDoNotMatchEvent.Invoke(new Empty());
                }

                _numberOfFlippedCards = 0;
                attempts--;
                if (attempts == 0)
                {
                    GameOver();
                }
            }
        }

        /// <summary>
        /// Remove "(Clone)" from card name and check if names match.
        /// If the names of the cards match, return true.
        /// </summary>
        /// <param name="cardA"></param>
        /// <param name="cardB"></param>
        /// <returns></returns>
        private bool DoCardsMatch(GameObject cardA, GameObject cardB)
        {
            string nameA = cardA.name.Replace("(Clone)", "").Trim();
            string nameB = cardB.name.Replace("(Clone)", "").Trim();
            return nameA == nameB;
        }
    }
}