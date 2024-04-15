using System;
using System.Collections.Generic;
using UnityEngine;

namespace FindPath
{
    public class Tile : GridObject
    { 
        private FindPathProject _findPathProject;
        private int _tileSize;

        public override void SetSurface(FindPathProject pathFinding)
        {
            _findPathProject = FindPathProject.Instance;
            
            int tileSize = _findPathProject.TileSize;
            foreach (var surface in _surfaces)
            {
                AddSurface(surface.direction, pathFinding, tileSize);
            }
        }
        
        public override void AddSurface(Vector3Int direction, FindPathProject pathFinding, int tileSize)
        {
            Directions.DirectionArrayPair directions = new();

            if (pathFinding.Directions.DirDictionary.TryGetValue(direction, out Directions.DirectionArrayPair _directions))
            {
                directions = _directions;
            }

            bool isObstacle = pathFinding.Tiles.TryGetValue(Position + direction, out Tile tile);
            Surface s = new Surface(direction, directions, this, tileSize, isObstacle, isObstacle);
            Surfaces.Add(direction, s);
        }
    }
}