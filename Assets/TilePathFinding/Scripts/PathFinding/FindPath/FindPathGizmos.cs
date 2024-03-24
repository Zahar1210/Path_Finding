using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FindPath
{
    public class FindPathGizmos : MonoBehaviour
    {
        public static FindPathGizmos Instance { get; private set; }

        public bool drawTileGizmos;
        private List<Tile> _tiles = new();

        private FindPathProject _findPathProject;


        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                return;
            }
            Destroy(gameObject);
        }

        public void Initialize(List<Tile> tiles)
        {
            _findPathProject = FindPathProject.Instance;
            _tiles = tiles;
        }
        
        
        
#if UNITY_EDITOR
        
        private void OnDrawGizmos()
        {
            if (drawTileGizmos)
            {
                foreach (var tile in _tiles)
                {
                    foreach (KeyValuePair<Vector3Int, Tile.Surface> surface in tile.Surfaces)
                    {
                        Tile.Surface s = surface.Value;
                        Vector3 dir = s.direction;

                        if (_findPathProject.Path != null && _findPathProject.Path.Contains(s))//если он часть пути
                            Gizmos.color = _findPathProject.pathColor;
                        else if (!s.isObstacle)//если он совбоден
                            Gizmos.color = _findPathProject.noObstacleColor;
                        else//если не свободен
                            Gizmos.color = _findPathProject.obstacleColor;
                
                        Gizmos.DrawCube(tile.transform.position + dir * 0.5f, s.Size);
                    }
                }
            }
        }

#endif
    }
}