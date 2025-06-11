using UnityEngine;
using UnityEngine.U2D.Animation;
using UnityEngine.EventSystems;

namespace DoorGame
{
    public class PairEventCard : MonoBehaviour
    {
        private SpriteResolver _spriteResolver;
        private Sprite _currentSprite;

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
        
        public void OnMouseDown()
        {
            FlipCard();
        }

        private void FlipCard()
        {
            _spriteResolver.SetCategoryAndLabel("Front", "Entry");
        }
    }
}
