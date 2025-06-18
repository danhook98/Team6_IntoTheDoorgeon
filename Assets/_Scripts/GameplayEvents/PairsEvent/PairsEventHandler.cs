using System.Collections.Generic;
using DoorGame.Audio;
using DoorGame.EventSystem;
using UnityEngine;
using Random = UnityEngine.Random;
using System.Collections;
using TMPro;

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

        [Header("Score")] 
        [SerializeField] private IntValue scoreValue;

        [Header("Events")] 
        [SerializeField] private VoidEvent onCardsMatchEvent;
        [SerializeField] private VoidEvent onCardsDoNotMatchEvent;
        [SerializeField] private VoidEvent onGameEndedEvent;
        [SerializeField] private AudioClipSOEvent onPlaySfxEvent;
        [SerializeField] private IntEvent onScoreChangedEvent;
        
        [Header("Audio Clip SOs")]
        [SerializeField] private AudioClipSO spawnCardsSfx;
        [SerializeField] private AudioClipSO flipCardSfx;
        [SerializeField] private AudioClipSO pairMatchesSfx;
        [SerializeField] private AudioClipSO pairDoesNotMatchSfx;

        [Header("Game Objects")]
        [SerializeField] private GameObject introCard;
        [SerializeField] private GameObject outroCard;
        [SerializeField] private Transform cardsContainer;
        [SerializeField] private GameObject attemptsLeftHolder;
        [SerializeField] private TextMeshProUGUI endText;
        [SerializeField] private TextMeshProUGUI attemptsLeftText;
        
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
            attemptsLeftText.text = "Attempts left: " + attempts;
        }

        /// <summary>
        /// Spawns 8 pairs of cards at random positions, removes
        /// used position when spawning a card.
        /// </summary>
        public void SpawnCards()
        {
            attemptsLeftHolder.SetActive(true);
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
                
                card1.transform.SetParent(cardsContainer);

                // Second copy
                int index2 = Random.Range(0, spawnPositionsAvailable.Count);
                Vector2 pos2 = spawnPositionsAvailable[index2];
                spawnPositionsAvailable.RemoveAt(index2);
                spawnPositionsUsed.Add(pos2);

                PairEventCard card2 = Instantiate(cardPrefab, transform);
                card2.PairID = autoId;
                card2.GetComponent<RectTransform>().anchoredPosition = pos2;
                usedCards.Add(card2);
                
                card2.transform.SetParent(cardsContainer);
                
                autoId++;
            }
            
            cardsContainer.gameObject.SetActive(true);
            
            onPlaySfxEvent.Invoke(spawnCardsSfx);
        }

        /// <summary>
        /// Resets lists and variables, destroys card game objects.
        /// </summary>
        private IEnumerator GameOver()
        {
            // Calculate new score
            int scoreMultiplier = -50;
            if (_completedPairs >= 3) scoreMultiplier += 20;
            scoreMultiplier += _completedPairs * 20;
            //int newScore = scoreValue.Value * (scoreMultiplier / 10);
            int newScore;

            if(scoreMultiplier < 0) 
            {
                int localMult = scoreMultiplier * -1; 
                newScore = scoreValue.Value - ((scoreValue.Value * localMult) / 100);
            }
            else
            {
                newScore = scoreValue.Value + ((scoreValue.Value * scoreMultiplier) / 100);
            }
            
            // Send new score
            onScoreChangedEvent.Invoke(newScore);
            
            // Update end text with results.
            //endText.text = newScore + "\nScore modifier: " + scoreMultiplier + "%" + "\nTotal pairs completed: " + _completedPairs;
            endText.text = (scoreMultiplier > 0 ? "+" : "") + scoreMultiplier + "% score!\n\nYou now have \n" + $"{newScore:n0}";
            
            // Reset positions
            spawnPositionsAvailable.AddRange(spawnPositionsUsed);
            spawnPositionsUsed.Clear();
            
            yield return new WaitForSeconds(1.5f);

            foreach (var card in usedCards)
            {
                Destroy(card.gameObject);
            }
            
            cardsContainer.gameObject.SetActive(false);
            onGameEndedEvent.Invoke(new Empty());
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
                attemptsLeftText.text = "Attempts left: " + attempts;
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
            /*if (usedCards.Count != 0)
            {
                for (int i = 0; i < usedCards.Count; i++)
                {
                    usedCards[i].DeleteSelf();
                    usedCards.RemoveAt(i);
                }
            }*/
            
            usedCards.Clear();
            spawnPositionsUsed.Clear();
            attempts = 8;
            _numberOfFlippedCards = 0;
            _completedPairs = 0;
            introCard.SetActive(true);
            outroCard.SetActive(false);
        }
    }
}