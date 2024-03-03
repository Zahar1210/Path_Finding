using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FindPath
{
    public class FindPathProject : MonoBehaviour
    {
        [Header("PathFinding Parameters")] 
        [SerializeField] private int tileSize;
        
        [Header("Path Find Mode")] 
        public FindMode findMode;

        [Header("Gizmos")]
        [SerializeField] private PathGizmos pathGizmos;
        public Directions Directions => directions;
        public Dictionary<Vector3Int, Tile> _tiles = new();
        [SerializeField] private Directions directions;


        private void Start()
        {
            AddTiles();
            pathGizmos.SetTiles(_tiles.Values.ToList());
        }
        
        private void AddTiles()
        {
            Tile[] tiles = FindObjectsOfType<Tile>();

            foreach (var tile in tiles)
            {
                Vector3Int tilePosition = Vector3Int.RoundToInt(tile.transform.position);
                if (!_tiles.ContainsKey(tilePosition))
                {
                    tile.Position = tilePosition;
                    _tiles.Add(tilePosition, tile);
                }
            }

            foreach (var tile in tiles)
            {
                tile.SetSurface(this);
            }
        }
    }

    public enum FindMode
    {
        BreadthFirstSearch, 
        AStar
    }
}