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
        
        [Serializable]
        public class Surface 
        {
            //variable for Path Finding 
            public Tile Tile { get; set; }
            public bool obstacleLock;
            public bool isObstacle;
            public Vector3Int direction;

            public Directions.DirectionArrayPair Directions { get; }

            //variable for Gizmos 
            public Vector3 Size { get; }
            
            //this constructor is to automatically create surfaces on a tile.
            public Surface(Vector3Int direction)
            {
                this.direction = direction;
            }
            
            //this constructor initializes the surface to find the path
            public Surface(Vector3Int direction ,Directions.DirectionArrayPair directions, Tile tile, int tileSize, bool obstacleLock, bool isObstacle)
            {
                Tile = tile;
                Directions = directions;
                this.direction = direction;
                this.obstacleLock = obstacleLock;
                this.isObstacle = isObstacle;
                
                //for Gizmos 
                if (direction == Vector3Int.up || direction == Vector3Int.down)
                    Size = new Vector3(tileSize - 0.1f, 0.05f, tileSize - 0.1f);
                else if (direction == Vector3Int.left || direction == Vector3Int.right)
                    Size = new Vector3(0.05f, tileSize - 0.1f, tileSize - 0.1f);
                else
                    Size = new Vector3(tileSize - 0.1f, tileSize - 0.1f, 0.05f);
            }
        }
    }
}