using UnityEngine;
using UnityEngine.U2D.Animation;
using System.Collections;

namespace DoorGame.GameEvents.PairsEvent
{
    public class PairEventCard : MonoBehaviour
    {
        private SpriteResolver _spriteResolver;

        private void Awake()
        {
            _spriteResolver = GetComponent<SpriteResolver>();
        }

        private void Start()
        {
            SetCardSpriteToBack();
        }

        private void SetCardSpriteToBack()
        {
            _spriteResolver.SetCategoryAndLabel("Back", "Entry");
        }

        private void SetCardSpriteToFront()
        {
            _spriteResolver.SetCategoryAndLabel("Front", "Entry");
        }

        private IEnumerator ShowCard()
        {
            SetCardSpriteToFront();
            yield return new WaitForSeconds(2f);
            SetCardSpriteToBack();
        }
        
        public void OnMouseDown()
        {
            SetCardSpriteToFront();
        }
    }
}
