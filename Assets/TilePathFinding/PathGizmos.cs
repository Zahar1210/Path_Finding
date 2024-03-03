using System.Collections.Generic;
using UnityEngine;


namespace FindPath
{
    public class PathGizmos : MonoBehaviour
    {
        private List<Tile.Surface> _surfaces = new();

        public void SetTiles(List<Tile> tiles)
        {
            foreach (var t in tiles)
            {
                foreach (KeyValuePair<Vector3Int, Tile.Surface> surface in t._surfaces)
                {
                    _surfaces.Add(surface.Value);
                }
            }
        }

#if UNITY_EDITOR

        

#endif
    }
}