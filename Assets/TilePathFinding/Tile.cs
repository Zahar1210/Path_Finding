using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FindPath
{
    public class Tile : MonoBehaviour
    {
        public Vector3Int Position { get; set; }
        public Surface[] _surfaces;
        public Dictionary<Vector3Int, Surface> Surfaces = new();

        private FindPathProject _findPathProject;
        private int _tileSize;

        public void SetSurface(FindPathProject pathFinding)
        {
            _findPathProject = FindPathProject.Instance;
            int tileSize = _findPathProject.TileSize;
            
            foreach (var surface in _surfaces)
            {
                AddSurface2(surface.direction, pathFinding, tileSize);
            }
        }
        
        private void AddSurface2(Vector3Int direction, FindPathProject pathFinding, int tileSize)
        {
            Directions.DirectionArrayPair directions = new();

            if (pathFinding.Directions.DirDictionary.TryGetValue(direction, out Directions.DirectionArrayPair _directions))
            {
                directions = _directions;
            }
            
            Surface s = new Surface(direction, directions, this, tileSize);
            
            s.isObstacle = pathFinding.Tiles.TryGetValue(s.Tile.Position + direction, out Tile tile);
            Surfaces.Add(direction, s);
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            foreach (KeyValuePair<Vector3Int, Surface> surface in Surfaces)
            {
                Surface s = surface.Value;
                Vector3 dir = s.direction;
                
                if (_findPathProject.Path != null && _findPathProject.Path.Contains(surface.Value))
                    Gizmos.color = new Color(0.1f, 1f, 0f, 1f);
                else if (!s.isObstacle)
                    Gizmos.color = new Color(0.1f, 1f, 0f, 0.1f);
                else
                    Gizmos.color = new Color(1f, 0f, 0f, 0.1f);
                
                Gizmos.DrawCube(transform.position + dir * 0.5f, s.Size);
            }
        }
#endif


        [Serializable]
        public class Surface
        {
            //variable for Path Finding 
            public Tile Tile { get; set; }
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
            public Surface(Vector3Int direction ,Directions.DirectionArrayPair directions, Tile tile, int tileSize)
            {
                Tile = tile;
                Directions = directions;
                this.direction = direction;
                
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