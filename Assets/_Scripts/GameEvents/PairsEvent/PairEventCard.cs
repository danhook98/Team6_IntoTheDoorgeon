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

        private void Awake()
        {
            _spriteResolver = GetComponent<SpriteResolver>();
        }

        private void Start()
        {
            _canBeFlipped = true;
            SetCardSpriteToBack();
        }

        public void SetCardSpriteToBack()
        {
            if (!_canBeFlipped) return;
            _spriteResolver.SetCategoryAndLabel("Back", "Entry");
        }

        public void SetCardSpriteToFront()
        {
            if (!_canBeFlipped) return;
            _spriteResolver.SetCategoryAndLabel("Front", "Entry");
        }

        private void OnMouseDown()
        {
            if (!_canBeFlipped) return;
            onFlipCardEvent.Invoke(this.gameObject.GetInstanceID());
        }

        public IEnumerator ShowCard()
        {
            SetCardSpriteToFront();
            yield return new WaitForSeconds(timeToFlipCard);
            SetCardSpriteToBack();
        }

        public void SetBool(bool canBeFlipped)
        {
            _canBeFlipped = canBeFlipped;
        }
    }
}