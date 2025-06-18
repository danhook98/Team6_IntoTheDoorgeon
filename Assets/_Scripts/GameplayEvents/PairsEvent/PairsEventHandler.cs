using System.Collections.Generic;
using DoorGame.Audio;
using DoorGame.EventSystem;
using UnityEngine;
using Random = UnityEngine.Random;
using System.Collections;

namespace DoorGame.GameplayEvents.PairsEvent
{
    public class PairsEventHandler : MonoBehaviour
    {
        [Header("Track cards")]
        [SerializeField] private List<PairEventCard> availableCards;
        [SerializeField] private List<PairEventCard> usedCards;

        [Header("Spawn positions")]
        [SerializeField] private List<Vector2> spawnPositionsAvailable;
        [SerializeField] private List<Vector2> spawnPositionsUsed;

        [Header("Attempts")]
        [SerializeField] private int attempts;

        [Header("Events")] 
        [SerializeField] private VoidEvent onCardsMatchEvent;
        [SerializeField] private VoidEvent onCardsDoNotMatchEvent;
        [SerializeField] private VoidEvent onGameEndedEvent;
        [SerializeField] private AudioClipSOEvent onPlaySfxEvent;
        
        [Header("Audio Clip SOs")]
        [SerializeField] private AudioClipSO spawnCardsSfx;
        [SerializeField] private AudioClipSO flipCardSfx;
        [SerializeField] private AudioClipSO pairMatchesSfx;
        [SerializeField] private AudioClipSO pairDoesNotMatchSfx;

        [Header("Game Objects")]
        [SerializeField] private GameObject introCard;
        [SerializeField] private GameObject outroCard;
        
        [Space] 
        [SerializeField] private float timeToFlipCard;

        private int _completedPairs;
        private const int TotalPairs = 8;
        private int _numberOfFlippedCards;

        private PairEventCard _firstCard;
        private PairEventCard _secondCard;

        private void Start()
        {
            introCard.SetActive(true);
            outroCard.SetActive(false);
            attempts = 8;
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
            List<PairEventCard> tempCardPool = new List<PairEventCard>(availableCards);

            int autoId = 0;

            for (int i = 0; i < TotalPairs; i++)
            {
                int prefabIndex = Random.Range(0, tempCardPool.Count);
                PairEventCard cardPrefab = tempCardPool[prefabIndex];
                tempCardPool.RemoveAt(prefabIndex);

                // First copy
                int index1 = Random.Range(0, spawnPositionsAvailable.Count);
                Vector2 pos1 = spawnPositionsAvailable[index1];
                spawnPositionsAvailable.RemoveAt(index1);
                spawnPositionsUsed.Add(pos1);

                PairEventCard card1 = Instantiate(cardPrefab, transform);
                card1.PairID = autoId; 
                card1.GetComponent<RectTransform>().anchoredPosition = pos1;
                usedCards.Add(card1);

                // Second copy
                int index2 = Random.Range(0, spawnPositionsAvailable.Count);
                Vector2 pos2 = spawnPositionsAvailable[index2];
                spawnPositionsAvailable.RemoveAt(index2);
                spawnPositionsUsed.Add(pos2);

                PairEventCard card2 = Instantiate(cardPrefab, transform);
                card2.PairID = autoId;
                card2.GetComponent<RectTransform>().anchoredPosition = pos2;
                usedCards.Add(card2);
                
                autoId++;
            }
            onPlaySfxEvent.Invoke(spawnCardsSfx);
        }

        /// <summary>
        /// Resets lists and variables, destroys card game objects.
        /// </summary>
        private IEnumerator GameOver()
        {
            spawnPositionsAvailable.AddRange(spawnPositionsUsed);
            spawnPositionsUsed.Clear();

            foreach (var card in usedCards)
            {
                Destroy(card.gameObject);
            }
            
            onGameEndedEvent.Invoke(new Empty());
            yield return new WaitForSeconds(1.5f);
            outroCard.SetActive(true);
        }
        
        /// <summary>
        /// Tracks number of cards flipped. If 1 card has been flipped, it flips the card
        /// and waits for a second card. Once that happens, it checks if the IDs match
        /// and triggers an event based on that.
        /// </summary>
        /// <param name="instanceId"></param>
        public void CardHasBeenFlipped(int instanceId)
        {
            PairEventCard clickedCard = usedCards.Find(c => c.GetInstanceID() == instanceId);
            
            if (!clickedCard) return;
            
            if (_numberOfFlippedCards == 0)
            {
                onPlaySfxEvent.Invoke(flipCardSfx);
                _firstCard = clickedCard;
                clickedCard.SetCardSpriteToFront();
                _numberOfFlippedCards++; 
            }
            else if (_numberOfFlippedCards == 1)
            {
                _secondCard = clickedCard;

                if (_firstCard == _secondCard) return; 
                
                onPlaySfxEvent.Invoke(flipCardSfx);

                // Cards match
                if (DoCardsMatch(_firstCard, _secondCard))
                {
                    _secondCard.SetCardSpriteToFront();
                    
                    //Prevent cards from being flipped
                    _firstCard.SetCanBeFlipped(false);
                    _secondCard.SetCanBeFlipped(false);
                    
                    onCardsMatchEvent.Invoke(new Empty());
                    _completedPairs++;
                    onPlaySfxEvent.Invoke(pairMatchesSfx);
                }
                
                // Cards do not match
                else
                {
                    _firstCard.StartCoroutine(_firstCard.ShowCard(timeToFlipCard));
                    _secondCard.StartCoroutine(_secondCard.ShowCard(timeToFlipCard));
                    onCardsDoNotMatchEvent.Invoke(new Empty());
                    onPlaySfxEvent.Invoke(pairDoesNotMatchSfx);
                }

                _numberOfFlippedCards = 0;
                attempts--;
                if (attempts == 0)
                {
                    StartCoroutine(GameOver());
                }
            }
        }

        /// <summary>
        /// Checks if card IDs match.
        /// </summary>
        /// <param name="cardA"></param>
        /// <param name="cardB"></param>
        /// <returns></returns>
        private bool DoCardsMatch(PairEventCard cardA, PairEventCard cardB)
        {
            return cardA.PairID == cardB.PairID;
        }

        public void ResetEvent()
        {
            if (usedCards.Count != 0)
            {
                for (int i = 0; i < usedCards.Count; i++)
                {
                    usedCards[i].DeleteSelf();
                    usedCards.RemoveAt(i);
                }
            }
            
            for (int i = 0; i < spawnPositionsUsed.Count; i++)
            {
                var pos = spawnPositionsUsed[i];
                spawnPositionsAvailable.Add(pos);
            }
            
            usedCards.Clear();
            spawnPositionsUsed.Clear();
            attempts = 8;
            _numberOfFlippedCards = 0;
            _completedPairs = 0;
            introCard.SetActive(false);
            outroCard.SetActive(false);
        }
    }
}