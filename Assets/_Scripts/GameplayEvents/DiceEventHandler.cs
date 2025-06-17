using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DoorGame
{
    public class DiceEventHandler : MonoBehaviour
    {
        [Header("Dice prefab")]
        [SerializeField] private GameObject dicePrefab;
        
        [Header("Spawn positions")]
        [SerializeField] private List<Vector2> playerDiceSpawns;
        [SerializeField] private List<Vector2> enemyDiceSpawns;

        private void SpawnDice()
        {
            
        }
    }
}
