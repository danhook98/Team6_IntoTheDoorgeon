using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

namespace DoorGame
{
    public class test : MonoBehaviour
    {
        [SerializeField] private Tilemap _tilemap;

        [SerializeField] private TileBase[] _backgroundTiles;

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Generate();
            }
        }

        private void Generate()
        {
            for (int x = -8; x < 8; x++)
            {
                for (int y = -8; y < 8; y++)
                {
                    _tilemap.SetTile(new Vector3Int(x, y, 0), _backgroundTiles[Random.Range(0, _backgroundTiles.Length)]);
                }
            }
        }
    }
}
