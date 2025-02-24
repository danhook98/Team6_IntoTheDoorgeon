using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

namespace DoorGame
{
    public class BackgroundGenerator : MonoBehaviour
    {
        [Header("Tilemap")]
        [SerializeField] private Tilemap _tilemap;
        [SerializeField] private TileBase[] _backgroundTiles;
        
        [Header("Grid Size")]
        [SerializeField][Range(1, 15)] private int gridSize = 8;

        private void Awake() => Generate();

        public void Generate()
        {
            for (int x = -gridSize; x < gridSize; x++)
            {
                for (int y = -gridSize; y < gridSize; y++)
                {
                    _tilemap.SetTile(new Vector3Int(x, y, 0), _backgroundTiles[Random.Range(0, _backgroundTiles.Length)]);
                }
            }
        }
    }
}
