using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public Dictionary<Vector3Int, Surface> _surfaces = new();
    public Vector3Int Position { get; set; }
    public void SetSurface(PathFinding pathFinding)
    {
        AddSurface(Vector3Int.left, pathFinding);
        AddSurface(Vector3Int.right, pathFinding);
        AddSurface(Vector3Int.up, pathFinding);
        AddSurface(Vector3Int.down, pathFinding);
        AddSurface(Vector3Int.forward, pathFinding);
        AddSurface(Vector3Int.back, pathFinding);
    }

    private void AddSurface(Vector3Int direction, PathFinding pathFinding)
    {
        Directions.DirectionArrayPair dirs = new();
        
        if (pathFinding.Directions.dirDictionary.TryGetValue(direction, out Directions.DirectionArrayPair _dirs))
            dirs = _dirs;
        
        Surface s = new Surface(direction, dirs, this);
        s.IsObstacle = pathFinding._tiles.TryGetValue(s.Tile.Position + direction, out Tile tile);
        
        _surfaces.Add(direction, s);
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        // if (Application.isPlaying == false) return;
        
        foreach (KeyValuePair<Vector3Int, Surface> surface in _surfaces)
        {
            Surface s = surface.Value;
            Vector3 dir = s.Direction;
            if (s.InPath)
                Gizmos.color = new Color(0.1f, 1f, 0f, 1f);
            else if (!s.IsObstacle)
                Gizmos.color = new Color(0.1f, 1f, 0f, 0.1f);
            else
                Gizmos.color = new Color(1f, 0f, 0f, 0.1f);
            
            Gizmos.DrawCube(transform.position + dir * 0.5f, s.Size);
        }
    }
#endif


    public class Surface
    {
        public Tile Tile { get; set; }
        public bool IsObstacle { get; set; }
        public float Distance { get; set; }
        public Vector3Int Direction { get; }
        public bool InPath { get; set; }

        public Directions.DirectionArrayPair  Directions { get; }
        
        //These variable are needed for Gizmos 
        public Vector3 Size { get; }

        public Surface(Vector3Int direction, Directions.DirectionArrayPair directions, Tile tile)
        { 
            Direction = direction;
            Directions = directions;
            Tile = tile;

            if (direction == Vector3Int.up || direction == Vector3Int.down)
                Size = new Vector3(0.9f, 0.05f, 0.9f);
            else if (direction == Vector3Int.left || direction == Vector3Int.right)
                Size = new Vector3(0.05f, 0.9f, 0.9f);
            else
                Size = new Vector3(0.9f, 0.9f, 0.05f);
        }
    }
}