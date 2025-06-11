using UnityEngine;
using UnityEngine.U2D.Animation;

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
            _spriteResolver.SetCategoryAndLabel("Card Back", "Back");
        }

        private void OnPointerClick()
        {
            FlipCard();
        }

        private void FlipCard()
        {
            _spriteResolver.SetCategoryAndLabel("Card Back", "Back");
        }
    }
}
