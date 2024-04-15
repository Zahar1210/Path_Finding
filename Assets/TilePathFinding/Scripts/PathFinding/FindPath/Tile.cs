using System;
using System.Collections.Generic;
using UnityEngine;

namespace FindPath
{
    public class Tile : MonoBehaviour
    { 
        public Vector3Int position;
        public Surface[] surfaces;
        
        public readonly Dictionary<Vector3Int, Surface> Surfaces = new();

        private FindPathProject _findPathProject;
        private int _tileSize;

        public void SetSurface(FindPathProject pathFinding)
        {
            _findPathProject = FindPathProject.Instance;
            
            int tileSize = _findPathProject.TileSize;
            foreach (var surface in surfaces)
            {
                AddSurface(surface.direction, pathFinding, tileSize);
            }
        }
        
        private void AddSurface(Vector3Int direction, FindPathProject pathFinding, int tileSize)
        {
            Directions.DirectionArrayPair directions = new();

            if (pathFinding.Directions.DirDictionary.TryGetValue(direction, out Directions.DirectionArrayPair _directions))
            {
                directions = _directions;
            }

            bool isObstacle = pathFinding.Tiles.TryGetValue(position + direction, out Tile tile);
            Surface s = new Surface(direction, directions, this, tileSize, isObstacle, isObstacle);
            Surfaces.Add(direction, s);
        }
    }
}