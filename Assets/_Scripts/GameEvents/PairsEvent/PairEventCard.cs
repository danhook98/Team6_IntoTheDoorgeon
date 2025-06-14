using UnityEngine;
using UnityEngine.U2D.Animation;
using DoorGame.EventSystem;
using System.Collections;

namespace DoorGame.GameEvents.PairsEvent
{
    public class PairEventCard : MonoBehaviour
    {
        [SerializeField] private IntEvent onFlipCardEvent;
        
        private SpriteResolver _spriteResolver;
        private RectTransform _rectTransform;

        private float _defaultScale;
        private bool _canBeFlipped;
        
        public int PairID { set; get; }

        private void Awake()
        {
            _spriteResolver = GetComponent<SpriteResolver>();
            _rectTransform = GetComponent<RectTransform>();
        }

        private void Start()
        {
            _canBeFlipped = true;
            SetCardSpriteToBack();
            _defaultScale = _rectTransform.localScale.x;
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
        /// If card can be flipped, multiply size by 1.2 when hovering over it.
        /// </summary>
        private void OnMouseEnter()
        {
            if (!_canBeFlipped) return;
            _rectTransform.localScale = new Vector3(_defaultScale * 1.2f, _defaultScale * 1.2f, 1);
        }

        /// <summary>
        /// Decrease size of card back to default when not hovering over it.
        /// </summary>
        private void OnMouseExit()
        {
            _rectTransform.localScale = new Vector3(_defaultScale, _defaultScale, 1);
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
        public void SetCanBeFlipped(bool canBeFlipped)
        {
            _canBeFlipped = canBeFlipped;
        }
    }
}