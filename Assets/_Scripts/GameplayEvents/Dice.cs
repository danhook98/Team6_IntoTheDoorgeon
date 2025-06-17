using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DoorGame.EventSystem;

namespace DoorGame
{
    public class Dice : MonoBehaviour
    {
        [SerializeField] private IntEvent onSelectDieEvent;
        
        private bool _isSelected;
        private RectTransform _rectTransform;
        public int DiceID { set; get; }
        private float _defaultScale;

        private void Start()
        {
            _isSelected = false;
            _rectTransform = GetComponent<RectTransform>();
            _defaultScale = _rectTransform.localScale.x;
        }
        
        public void DestroySelf()
        {
            Destroy(gameObject);
        }
        
        private void OnMouseDown()
        {
            if (gameObject.CompareTag("EnemyDie")) return;
            if (_isSelected) return;
            
            _isSelected = true;
            onSelectDieEvent.Invoke(GetInstanceID());
        }
        
        private void OnMouseEnter()
        {
            _rectTransform.localScale = new Vector3(_defaultScale * 1.2f, _defaultScale * 1.2f, 1);
        }
        
        private void OnMouseExit()
        {
            _rectTransform.localScale = new Vector3(_defaultScale, _defaultScale, 1);
        }
    }
}
