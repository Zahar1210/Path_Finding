using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FindPath
{
    public class ObstacleObjects : MonoBehaviour
    {
        public ObstacleObjectType obstacleObjectType;
        [SerializeField] [Range(0.1f, 1f)] private float checkTime = 0.3f;
        [SerializeField] [Range(0.3f, 2)] private float checkRadius = 0.5f;
        [SerializeField] private bool isCheck = true;
        [SerializeField] private Collider[] colliders;
        [SerializeField] private LayerMask layerMask;

        private readonly List<Tile> _tiles = new();
        private readonly List<Tile.Surface> _surfaces = new();
        private FindPathProject _findPathProjectInstance;
        private ObstacleObjectType _startObjectObstacleType;

        public void Initialize()
        {
            _findPathProjectInstance = FindPathProject.Instance;
            _startObjectObstacleType = obstacleObjectType;
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
            if (obstacleObjectType != _startObjectObstacleType)
            {
                StartChecking();
            }
            
            _tiles.Clear();
            _surfaces.Clear();

            GetTilesForCheck();
            CalculateSurfaces();
        }

        private void GetTilesForCheck()
        {
            if (_findPathProjectInstance == null) { return; }

            foreach (var coll in colliders)
            {
                Bounds bounds = coll.bounds;
                int tileRadius = _findPathProjectInstance.TileSize;

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

                for (int x = Mathf.Min(minPos.x, maxPos.x); x <= Mathf.Max(minPos.x, maxPos.x); x++)
                {
                    for (int y = Mathf.Min(minPos.y, maxPos.y); y <= Mathf.Max(minPos.y, maxPos.y); y++)
                    {
                        for (int z = Mathf.Min(minPos.z, maxPos.z); z <= Mathf.Max(minPos.z, maxPos.z); z++)
                        {
                            if (_findPathProjectInstance.Tiles.TryGetValue(new Vector3Int(x, y, z), out var tile) && !_tiles.Contains(tile))
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
                Vector3 pos = surface.Tile.position + (surface.direction);
                Collider[] colls = Physics.OverlapSphere(pos, checkRadius, layerMask);

                surface.isObstacle = (colls.Length > 0);
            }
        }
    }
}
