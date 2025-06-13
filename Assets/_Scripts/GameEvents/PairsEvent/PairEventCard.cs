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

        private void Awake()
        {
            _spriteResolver = GetComponent<SpriteResolver>();
        }

        private void Start()
        {
            SetCardSpriteToBack();
        }

        public void SetCardSpriteToBack()
        {
            _spriteResolver.SetCategoryAndLabel("Back", "Entry");
        }

        public void SetCardSpriteToFront()
        {
            _spriteResolver.SetCategoryAndLabel("Front", "Entry");
        }

        private void OnMouseDown()
        {
            //SetCardSpriteToFront();
            onFlipCardEvent.Invoke(this.gameObject.GetInstanceID());
        }

        public IEnumerator ShowCard()
        {
            SetCardSpriteToFront();
            yield return new WaitForSeconds(2f);
            SetCardSpriteToBack();
        }
    }
}