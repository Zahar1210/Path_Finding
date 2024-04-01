using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FindPath
{
    public static class SurfaceFinder
    {
        private static Dictionary<TargetDirection, Vector3Int> _vector = new();

        public static void Initialize() //TODO
        {
            _vector.Add(TargetDirection.Around, Vector3Int.zero);
            _vector.Add(TargetDirection.Right, Vector3Int.right);
            _vector.Add(TargetDirection.Left, Vector3Int.left);
            _vector.Add(TargetDirection.Forward, Vector3Int.forward);
            _vector.Add(TargetDirection.Back, Vector3Int.back);
            _vector.Add(TargetDirection.Up, Vector3Int.up);
            _vector.Add(TargetDirection.Down, Vector3Int.down);
        }

        public static Tile.Surface GetSurface(Vector3 pos, TargetDirection targetDirection, int count,
            FindPathProject findPathProject, Transform target)
        {
            List<Vector3Int> queueTiles = new();
            List<Tile> visitedTiles = new();
            Dictionary<Tile.Surface, float> distanceTiles = new();

            Vector3Int tilePos = Vector3Int.RoundToInt(pos);
            if (findPathProject.Tiles.TryGetValue(tilePos, out Tile tile))
            {
                queueTiles.Add(tilePos);
            }

            for (int i = 0; i < count; i++)
            {
                if (queueTiles.Count <= 0)
                    break;

                GetQueueTiles(visitedTiles, queueTiles, findPathProject.Directions.directionGroup.directions,
                    findPathProject);
            }

            Vector3Int targetVector = GetTargetVector(targetDirection);
            Tile[] selectedTiles = SelectTiles(visitedTiles, targetVector, target);

            distanceTiles = GetSelectedTiles(target, selectedTiles, targetVector, distanceTiles);

            float valueToFind = distanceTiles.Values.Min();
            Tile.Surface key = distanceTiles.FirstOrDefault(x => x.Value == valueToFind).Key;

            return key;
        }

        private static Dictionary<Tile.Surface, float> GetSelectedTiles(Transform target, Tile[] selectedTiles,
            Vector3Int targetVector, Dictionary<Tile.Surface, float> distanceTiles)
        {
            Dictionary<Tile.Surface, float> selectedSurface = new();
            foreach (var selectedTile in selectedTiles)
            {
                float dis = Vector3.Distance(selectedTile.position, target.transform.position);
                Tile.Surface surface = GetSurface(selectedTile, targetVector);
                if (surface != null)
                {
                    selectedSurface.Add(surface, dis);
                }
            }

            return selectedSurface;
        }

        private static Vector3Int GetTargetVector(TargetDirection targetDirection)
        {
            return _vector.TryGetValue(targetDirection, out Vector3Int vector) ? vector : Vector3Int.zero;
        }

        private static Tile[] SelectTiles(List<Tile> visitedTiles, Vector3Int targetVector, Transform target)
        {
            List<Tile> selectedTiles = new();

            foreach (var tile in visitedTiles)
            {
                Vector3 hitOffset = tile.position - target.transform.position;
                Vector3Int direction = Vector3Int.RoundToInt(hitOffset.normalized);

                if (direction == targetVector)
                {
                    selectedTiles.Add(tile);
                }
            }

            return selectedTiles.ToArray();
        }

        private static Tile.Surface GetSurface(Tile tile, Vector3Int direction)
        {
            if (tile.Surfaces.TryGetValue(direction, out Tile.Surface surface))
            {
                if (!surface.isObstacle)
                {
                    return surface;
                }
            }

            return null;
        }

        #region GetNearTiles

        private static void GetQueueTiles(List<Tile> visitedTiles, List<Vector3Int> queueTiles, Vector3Int[] directions,
            FindPathProject findPathProject)
        {
            List<Vector3Int> tilePositions = queueTiles;

            foreach (var tilePos in tilePositions)
            {
                SetVisitedTile(visitedTiles, queueTiles, findPathProject, tilePos);
                SetQueueTile(visitedTiles, queueTiles, directions, findPathProject, tilePos);
            }
        }

        private static void SetQueueTile(List<Tile> visitedTiles, List<Vector3Int> queueTiles, Vector3Int[] directions,
            FindPathProject findPathProject, Vector3Int tilePos)
        {
            foreach (var direction in directions)
            {
                Vector3Int pos = tilePos + direction;

                if (findPathProject.Tiles.TryGetValue(pos, out Tile tile))
                {
                    if (!visitedTiles.Contains(tile) && !queueTiles.Contains(pos))
                    {
                        queueTiles.Add(tile.position);
                    }
                }
            }
        }

        private static void SetVisitedTile(List<Tile> visitedTiles, List<Vector3Int> queueTiles,
            FindPathProject findPathProject, Vector3Int tilePos)
        {
            queueTiles.Remove(tilePos);

            Tile _tile = GetTile(findPathProject, tilePos);
            if (_tile != null)
            {
                visitedTiles.Add(_tile);
            }
        }

        private static Tile GetTile(FindPathProject findPathProject, Vector3Int tilePos)
        {
            return (findPathProject.Tiles.TryGetValue(tilePos, out Tile tile)) ? tile : null;
        }

        #endregion
    }
}