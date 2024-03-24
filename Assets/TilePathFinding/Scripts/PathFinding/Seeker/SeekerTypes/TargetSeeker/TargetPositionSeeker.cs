using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FindPath
{
    public class TargetPositionSeeker : SeekerAbstract
    {
        private Tile.Surface _pastTargetPosition;
        private readonly FindPathProject _findPathProject;
        private bool _isFindPath;
        private float _pastCheckTime;
        private readonly List<Tile> _tiles = new();
        private readonly List<Tile.Surface> _surfaces = new();

        public TargetPositionSeeker(FindPathProject findPathProject, float pastCheckTime)
        {
            _pastCheckTime = pastCheckTime;
            _findPathProject = findPathProject;
        }

        public override bool CheckEvent(Seeker seeker)
        {
            if ((Time.timeSinceLevelLoad - _pastCheckTime) > seeker.targetInterval)
            {
                _pastCheckTime = Time.time;
                return true;
            }

            return false;
        }

        public override PathParams GetPathParams(Seeker seeker)
        {
            Tile.Surface startSurface = seeker.CurrentSurface;
            Tile.Surface targetSurface;

            Seeker targetSeeker = seeker.target.GetComponent<Seeker>();
            if (targetSeeker != null)
            {
                targetSurface = targetSeeker.CurrentSurface;
            }
            else
            {
                targetSurface = GetTargetCurrentSurface(seeker.targetCollider, seeker);
            }
            
            return new PathParams(startSurface, targetSurface);
        }

        private Tile.Surface GetTargetCurrentSurface(Collider collider, Seeker seeker)
        {
            _tiles.Clear();
            _surfaces.Clear();

            SetTilesForCheck(collider);
            List<Tile.Surface> surfaces = CalculateSurfaces(seeker);
            if (seeker.targetDirection == TargetDirection.Around)
            {
                return GetSurface(seeker, surfaces);
            }
            
            return seeker.DirectionTargetSeeker.GetSurface(seeker, surfaces);
        }

        private void SetTilesForCheck(Collider collider)
        {
            Bounds bounds = collider.bounds;
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

            for (int x = Mathf.Min(minPos.x, maxPos.x); x <= Mathf.Max(minPos.x, maxPos.x); x++)
            {
                for (int y = Mathf.Min(minPos.y, maxPos.y); y <= Mathf.Max(minPos.y, maxPos.y); y++)
                {
                    for (int z = Mathf.Min(minPos.z, maxPos.z); z <= Mathf.Max(minPos.z, maxPos.z); z++)
                    {
                        if (_findPathProject.Tiles.TryGetValue(new Vector3Int(x, y, z), out var tile) &&
                            !_tiles.Contains(tile))
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


        private List<Tile.Surface> CalculateSurfaces(Seeker seeker)
        {
            List<Tile.Surface> selectSurfaces = new();
            foreach (var surface in _surfaces)
            {
                Vector3 pos = surface.Tile.position + (surface.direction);
                Collider[] colliders = Physics.OverlapSphere(pos, _findPathProject.TileSize / 2f);

                if (colliders != null && colliders.Length > 0)
                {
                    foreach (var coll in colliders)
                    {
                        if (coll == seeker.targetCollider)
                        {
                            selectSurfaces.Add(surface);
                        }
                    }
                }
            }

            return selectSurfaces;
        }

        private Tile.Surface GetSurface(Seeker seeker, List<Tile.Surface> surfaces)
        {
            Dictionary<float, Tile.Surface> distances = new();
            foreach (var surface in surfaces)
            {
                Vector3 pos = surface.Tile.position + surface.direction;
                distances.Add(Vector3.Distance(seeker.target.transform.position, pos), surface);
            }

            float minDis = distances.Keys.Min();
            if (distances.TryGetValue(minDis, out var _surface))
            {
                return _surface;
            }

            return null;
        }
    }
}