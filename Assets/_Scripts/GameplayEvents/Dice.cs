using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DoorGame.EventSystem;

namespace DoorGame
{
    public class Dice : MonoBehaviour
    {
        [Header("Events")]
        [SerializeField] private IntEvent onSelectDieEvent;

        [Header("Sprites")] 
        [SerializeField] private List<Sprite> diceSprites;
        
        private RectTransform _rectTransform;
        private SpriteRenderer _spriteRenderer;
        
        public int DiceID { set; get; } 
        
        private float _defaultScale;
        
        private bool _isSelected;
        
        public int playerDiceResult;
        public int enemyDiceResult;
        
        private void Start()
        {
            _isSelected = false;
            _rectTransform = GetComponent<RectTransform>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _defaultScale = _rectTransform.localScale.x;
        }
        
        public void DestroySelf()
        {
            Destroy(gameObject);
        }
        
        private void OnMouseDown()
        {
            if (gameObject.CompareTag("EnemyDie") || _isSelected) return;
            
            _isSelected = true;
            onSelectDieEvent.Invoke(GetInstanceID());
        }
        
        private void OnMouseEnter()
        {
            if (gameObject.CompareTag("EnemyDie") || _isSelected) return;
            _rectTransform.localScale = new Vector3(_defaultScale * 1.2f, _defaultScale * 1.2f, 1);
        }
        
        private void OnMouseExit()
        {
            if (gameObject.CompareTag("EnemyDie") || _isSelected) return;
            _rectTransform.localScale = new Vector3(_defaultScale, _defaultScale, 1);
        }

        public IEnumerator RollDie(int spritesShown)
        {
            for (int i = 0; i < spritesShown; i++)
            {
                _spriteRenderer.sprite = diceSprites[Random.Range(0, diceSprites.Count)];
                yield return new WaitForSeconds(0.1f);
            }

            // If it's the AI's dice.
            if (gameObject.CompareTag("EnemyDie"))
            {
                int spriteIndex1 = Random.Range(0, diceSprites.Count);
                enemyDiceResult = spriteIndex1 + 1;
                _spriteRenderer.sprite = diceSprites[spriteIndex1];
                Debug.Log("Enemy die result: " + enemyDiceResult);
            }
            // If it's the player's dice.
            else
            {
                int spriteIndex2 = Random.Range(0, diceSprites.Count);
                playerDiceResult = spriteIndex2 + 1;
                _spriteRenderer.sprite = diceSprites[spriteIndex2];
                Debug.Log("Player die result: " + playerDiceResult);
            }
        }
    }
}
