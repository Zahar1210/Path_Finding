using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FindPath
{
    public class Tile : MonoBehaviour
    {
        public Vector3Int[] surfacesArray;
        public Vector3Int Position;
        public Dictionary<Vector3Int, Surface> _surfaces = new();
        
        private FindPathProject _findPathProject;
        private int TileSize;

        public void SetSurface(FindPathProject pathFinding)
        {
            _findPathProject = FindPathProject.Instance;
            TileSize = _findPathProject.TileSize;
            foreach (var surface in surfacesArray)
            {
                AddSurface(surface, pathFinding);
            }
        }

        private void AddSurface(Vector3Int direction, FindPathProject pathFinding)
        {
            Directions.DirectionArrayPair dirs = new();

            if (pathFinding.Directions.DirDictionary.TryGetValue(direction, out Directions.DirectionArrayPair _dirs))
            {
                dirs = _dirs;
            }
            Surface s = new Surface(direction, dirs, this, TileSize);
            
            s.IsObstacle = pathFinding.Tiles.TryGetValue(s.Tile.Position + direction, out Tile tile);
            _surfaces.Add(direction, s);
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            foreach (KeyValuePair<Vector3Int, Surface> surface in _surfaces)
            {
                Surface s = surface.Value;
                Vector3 dir = s.Direction;
                if (_findPathProject.Path != null && _findPathProject.Path.Contains(surface.Value))
                    Gizmos.color = new Color(0.1f, 1f, 0f, 1f);
                else if (!s.IsObstacle)
                    Gizmos.color = new Color(0.1f, 1f, 0f, 0.15f);
                else
                    Gizmos.color = new Color(1f, 0f, 0f, 0.15f);
                Gizmos.DrawCube(transform.position + dir * 0.5f, s.Size);
            }
        }
#endif


        public class Surface
        {
            public Tile Tile { get; set; }
            public bool IsObstacle { get; set; }
            public Vector3Int Direction { get; }

            public Directions.DirectionArrayPair Directions { get; }

            //These variable are needed for Gizmos 
            public Vector3 Size { get; }

            public Surface(Vector3Int direction, Directions.DirectionArrayPair directions, Tile tile, int tileSize)
            {
                Tile = tile;
                Direction = direction;
                Directions = directions;

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