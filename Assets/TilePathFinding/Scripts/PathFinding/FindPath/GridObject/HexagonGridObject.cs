using UnityEngine;

namespace FindPath
{
    public class HexagonGridObject : GridObject
    {
        private FindPathProject _findPathProject;
        private int _tileSize;

        public override void SetSurface(FindPathProject pathFinding)
        {
            _findPathProject = FindPathProject.Instance;
            
            int tileSize = _findPathProject.TileSize;
            foreach (var surface in _surfaces)
            {
                AddSurface(surface.direction, pathFinding, tileSize);
            }
        }
        
        public override void AddSurface(Vector3Int direction, FindPathProject pathFinding, int tileSize)
        {
            Directions.DirectionArrayPair directions = new();

            if (pathFinding.Directions.dirDictionary.TryGetValue(direction, out Directions.DirectionArrayPair _directions))
            {
                directions = _directions;
            }

            bool isObstacle = pathFinding.GridObjects.TryGetValue(Position + direction, out GridObject gridObject);
            Surface s = new Surface(direction, directions, this, tileSize, isObstacle, isObstacle);
            Surfaces.Add(direction, s);
        }
    }
}