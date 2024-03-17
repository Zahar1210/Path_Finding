using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace FindPath
{
    public class NewObstacle : MonoBehaviour
    {
        public bool isCheck = true;
        
        [SerializeField] private ObstacleObjectType obstacleObjectType;
        [SerializeField] private Collider[] colliders;
        [SerializeField] [Range(0.1f, 1f)] private float checkTime;
        [SerializeField] private LayerMask layerMask;
        
        private readonly List<Tile> _tiles = new();
        private readonly List<Tile.Surface> _surfaces = new();
        private FindPathProject _findPathProject;

        private Vector3 _pos1;
        private Vector3 _pos2;
        
        public void Initialize()
        {
            _findPathProject = FindPathProject.Instance;
            StartChecking();
        }

        private void StartChecking()
        {
            if (obstacleObjectType == ObstacleObjectType.Dynamic)
            {
                StartCoroutine(Timer());
            }
            else
            {
                Check();
            }
        }

        private IEnumerator Timer()
        {
            while (isCheck)
            {
                yield return new WaitForSeconds(checkTime);
                Check();
            }
        }

        private void Check()
        {
            _tiles.Clear();
            _surfaces.Clear();
            
            GetTilesForCheck();
            CalculateSurfaces();
        }

        private void GetTilesForCheck()
        {
            if (_findPathProject == null) { return; }

            foreach (var coll in colliders)
            {
                Bounds bounds = coll.bounds;
                int tileRadius = _findPathProject.TileSize;

                Vector3Int minPos = new Vector3Int
                (
                    Mathf.FloorToInt(bounds.min.x) - tileRadius,
                    Mathf.FloorToInt(bounds.min.y) - tileRadius,
                    Mathf.FloorToInt(bounds.min.z) - tileRadius
                );
                
                Vector3Int maxPos = new Vector3Int
                (
                    Mathf.CeilToInt(bounds.max.x) + tileRadius,
                    Mathf.CeilToInt(bounds.max.y) + tileRadius,
                    Mathf.CeilToInt(bounds.max.z) + tileRadius
                );
                
                _pos1 = minPos;
                _pos2 = maxPos;
                
                GetTiles(minPos, maxPos);
            }

            void GetTiles(Vector3Int minPos, Vector3Int maxPos)
            {
                for (int x = Mathf.Min(minPos.x, maxPos.x); x <= Mathf.Max(minPos.x, maxPos.x); x ++)
                {
                    for (int y = Mathf.Min(minPos.y, maxPos.y); y <= Mathf.Max(minPos.y, maxPos.y); y ++)
                    {
                        for (int z = Mathf.Min(minPos.z, maxPos.z); z <= Mathf.Max(minPos.z, maxPos.z); z ++)
                        {
                            if (_findPathProject.Tiles.TryGetValue(new Vector3Int(x, y, z), out var tile) && !_tiles.Contains(tile))
                            {
                                _tiles.Add(tile);
                                foreach (var surface in tile.Surfaces.Values)
                                {
                                    if (!surface.obstacleLock)
                                    {
                                        _surfaces.Add(surface);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        
        private void CalculateSurfaces()
        {
            foreach (var surface in _surfaces)
            {
                Vector3 pos = surface.Tile.position + surface.direction;
                Collider[] colliders = Physics.OverlapSphere(pos, _findPathProject.TileSize / 2f, layerMask);
                foreach (var c in colliders)
                {
                    Debug.Log( c.transform.name);
                }
                if (!surface.Tile)
                {
                    surface.isObstacle = (colliders.Length > 0);
                }
            }
        }

        private void OnDrawGizmos()
        {
            // if (_tiles != null && _tiles.Count > 0)
            // {
            //     foreach (var tile in _tiles)
            //     {
            //         Gizmos.color = Color.red;
            //         Gizmos.DrawCube(tile.transform.position, new Vector3(1,1,1));
            //     }
            // }
            if (_pos1 != Vector3.zero)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawCube(_pos1, new Vector3(0.1f,0.1f,0.1f));
            } 
            if (_pos2 != Vector3.zero) 
            {
                Gizmos.color = Color.yellow; 
                Gizmos.DrawCube(_pos2, new Vector3(0.1f,0.1f,0.1f));
            }
        }
    }


    public enum ObstacleObjectType
    {
        Static, 
        Dynamic
    }
}