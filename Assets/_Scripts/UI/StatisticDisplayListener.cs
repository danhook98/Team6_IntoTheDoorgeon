using UnityEngine;
using TMPro;

namespace DoorGame.UI
{
    public class StatisticDisplayListener : MonoBehaviour
    {
        [SerializeField] private string _prefixText;
        
        private TextMeshProUGUI _text;

        private void Awake() => _text = GetComponent<TextMeshProUGUI>();

        public void SetText(int value)
        {
            _text.text = _prefixText + value;
        }

        public void SetTextFormatted(int value)
        {
            _text.text = _prefixText + $"{value:n0}";
        }
    }
}