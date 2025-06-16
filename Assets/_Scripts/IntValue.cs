using UnityEngine;

namespace DoorGame
{
    [CreateAssetMenu(fileName = "IntValue", menuName = "DoorGame/Values/IntValue")]
    public class IntValue : ScriptableObject
    { 
        public int Value; 
        
        private void OnEnable() => Value = 0;
    }
}
