using UnityEngine;
using UnityEngine.U2D.Animation;
using DoorGame.EventSystem;
using System.Collections;

namespace DoorGame.GameEvents.PairsEvent
{
    public class PairEventCard : MonoBehaviour
    {
        [SerializeField] private IntEvent onFlipCardEvent;
        [SerializeField] private float timeToFlipCard;
        
        private SpriteResolver _spriteResolver;

        private bool _canBeFlipped;
        
        public int PairID { set; get; }

        private void Awake()
        {
            _spriteResolver = GetComponent<SpriteResolver>();
        }

        private void Start()
        {
            _canBeFlipped = true;
            SetCardSpriteToBack();
        }

        /// <summary>
        /// Flips card to front side.
        /// Sets card sprite to the category "Back"
        /// Labelled "Entry" through the sprite resolver.
        /// </summary>
        public void SetCardSpriteToBack()
        {
            if (!_canBeFlipped) return;
            _spriteResolver.SetCategoryAndLabel("Back", "Entry");
        }

        /// <summary>
        /// Flips card to back side.
        /// Sets card sprite to the category "Front"
        /// Labelled "Entry" through the sprite resolver.
        /// </summary>
        public void SetCardSpriteToFront()
        {
            if (!_canBeFlipped) return;
            _spriteResolver.SetCategoryAndLabel("Front", "Entry");
        }

        /// <summary>
        /// Checks if card can be flipped.
        /// If yes, triggers onFlipCard event.
        /// </summary>
        private void OnMouseDown()
        {
            if (!_canBeFlipped) return;
            onFlipCardEvent.Invoke(GetInstanceID());
        }

        /// <summary>
        /// Coroutine which shows the front of the card,
        /// waits the time to flip and then shows the back of the card.
        /// </summary>
        /// <returns></returns>
        public IEnumerator ShowCard(float timeToFlipCard)
        {
            SetCardSpriteToFront();
            yield return new WaitForSeconds(timeToFlipCard);
            SetCardSpriteToBack();
        }

        /// <summary>
        /// Set boolean for whether a card can be flipped or not.
        /// </summary>
        /// <param name="canBeFlipped"></param>
        public void SetBool(bool canBeFlipped)
        {
            _canBeFlipped = canBeFlipped;
        }
    }
}