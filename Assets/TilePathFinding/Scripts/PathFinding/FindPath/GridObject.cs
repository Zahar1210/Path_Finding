using System.Collections.Generic;
using UnityEngine;

namespace FindPath
{
    public abstract class GridObject : MonoBehaviour
    {
        public Vector3Int Position { get; set; }
        public Surface[] _surfaces;

        public readonly Dictionary<Vector3Int, Surface> Surfaces = new();

        public abstract void SetSurface(FindPathProject pathFinding);
        public abstract void AddSurface(Vector3Int direction, FindPathProject pathFinding, int tileSize);
    }
}