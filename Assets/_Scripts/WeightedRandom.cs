using System.Collections.Generic;
using UnityEngine;

namespace DoorGame
{
    public class WeightedRandom
    {
        private List<WeightedValue> _values;
        private int _totalWeight;
        
        public WeightedRandom(List<WeightedValue> values)
        {
            _values = values;
            
            // Calculate the total weight. 
            foreach (WeightedValue kvp in values)
                _totalWeight += kvp.Weight;
        }

        private int GetRandom(bool asValue)
        {
            int randomValue = Random.Range(0, _totalWeight + 1);
            
            int cumulativeWeight = 0;

            for (int i = 0; i < _values.Count; i++)
            {
                cumulativeWeight += _values[i].Weight;

                if (cumulativeWeight > randomValue) 
                    return asValue ? _values[i].Value : i;
            }

            // Failed to get the index from the weighted values.
            return 0;
        }

        public int GetRandomAsValue() => GetRandom(true);
        public int GetRandomAsIndex() => GetRandom(false);
        
        public int GetValueAtIndex(int index) => _values[index].Value;
        public int GetWeightAtIndex(int index) => _values[index].Weight;
        
        public void SetValues(List<WeightedValue> values) => _values = values;
    }
    
    [System.Serializable]
    public struct WeightedValue
    {
        public int Value; 
        public int Weight;
        
        public WeightedValue(int value, int weight) { Value = value; Weight = weight; }
    }
}
