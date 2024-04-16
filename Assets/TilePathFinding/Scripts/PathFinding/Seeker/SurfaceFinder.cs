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

        public static Surface GetSurface(Vector3 pos, TargetDirection targetDirection, int count, FindPathProject findPathProject, Transform target)
        {
            List<Vector3Int> queueGridObjects = new();
            List<GridObject> visitedGridObjects = new();
            Dictionary<Surface, float> distanceGridObjects = new();

            Vector3Int tilePos = Vector3Int.RoundToInt(pos);
            if (findPathProject.GridObjects.TryGetValue(tilePos, out GridObject tile))
            {
                queueGridObjects.Add(tilePos);
            }

            for (int i = 0; i < count; i++)
            {
                if (queueGridObjects.Count <= 0)
                    break;

                GetQueueTiles(visitedGridObjects, queueGridObjects, findPathProject.Directions._directionGroup._directions,
                    findPathProject);
            }

            Vector3Int targetVector = GetTargetVector(targetDirection);
            GridObject[] selectedTiles = SelectTiles(visitedGridObjects, targetVector, target);

            distanceGridObjects = GetSelectedTiles(target, selectedTiles, targetVector, distanceGridObjects);

            float valueToFind = distanceGridObjects.Values.Min();
            Surface key = distanceGridObjects.FirstOrDefault(x => x.Value == valueToFind).Key;

            return key;
        }

        private static Dictionary<Surface, float> GetSelectedTiles(Transform target, GridObject[] selectedTiles,
            Vector3Int targetVector, Dictionary<Surface, float> distanceTiles)
        {
            Dictionary<Surface, float> selectedSurface = new();
            foreach (var selectedTile in selectedTiles)
            {
                float dis = Vector3.Distance(selectedTile.Position, target.transform.position);
                Surface surface = GetSurface(selectedTile, targetVector);
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

        private static GridObject[] SelectTiles(List<GridObject> visitedGridObjects, Vector3Int targetVector, Transform target)
        {
            List<GridObject> selectedGridObjects = new();
            
            foreach (var tile in visitedGridObjects)
            {
                Vector3 hitOffset = tile.Position - target.transform.position;
                Vector3Int direction = Vector3Int.RoundToInt(hitOffset.normalized);

                if (direction == targetVector)
                {
                    selectedGridObjects.Add(tile);
                }
            }

            return selectedGridObjects.ToArray();
        }

        private static Surface GetSurface(GridObject gridObject, Vector3Int direction)
        {
            if (gridObject.Surfaces.TryGetValue(direction, out Surface surface))
            {
                if (!surface.isObstacle)
                {
                    return surface;
                }
            }

            return null;
        }

        #region GetNearTiles

        private static void GetQueueTiles(List<GridObject> visitedGridObj, List<Vector3Int> queueGridObj, Vector3Int[] directions,
            FindPathProject findPathProject)
        {
            List<Vector3Int> gridObjectPositions = queueGridObj;

            foreach (var objPos in gridObjectPositions)
            {
                SetVisitedTile(visitedGridObj, queueGridObj, findPathProject, objPos);
                SetQueueTile(visitedGridObj, queueGridObj, directions, findPathProject, objPos);
            }
        }

        private static void SetQueueTile(List<GridObject> visitedGridObjects, List<Vector3Int> queueGridObjects, Vector3Int[] directions,
            FindPathProject findPathProject, Vector3Int gridObjectPos)
        {
            foreach (var direction in directions)
            {
                Vector3Int pos = gridObjectPos + direction;

                if (findPathProject.GridObjects.TryGetValue(pos, out GridObject tile))
                {
                    if (!visitedGridObjects.Contains(tile) && !queueGridObjects.Contains(pos))
                    {
                        queueGridObjects.Add(tile.Position);
                    }
                }
            }
        }

        private static void SetVisitedTile(List<GridObject> visitedGridObjects, List<Vector3Int> queueGridObjects,
            FindPathProject findPathProject, Vector3Int gridObjPos)
        {
            queueGridObjects.Remove(gridObjPos);

            GridObject _tile = GetTile(findPathProject, gridObjPos);
            if (_tile != null)
            {
                visitedGridObjects.Add(_tile);
            }
        }

        private static GridObject GetTile(FindPathProject findPathProject, Vector3Int objectPos)
        {
            return (findPathProject.GridObjects.TryGetValue(objectPos, out GridObject gridObject)) ? gridObject : null;
        }

        #endregion
    }
}