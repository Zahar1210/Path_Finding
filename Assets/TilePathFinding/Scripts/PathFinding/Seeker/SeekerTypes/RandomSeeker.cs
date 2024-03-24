using System.Collections.Generic;
using UnityEngine;

namespace FindPath
{
    public class RandomSeeker : SeekerAbstract
    {
        private FindPathProject _findPathProject;
        public RandomSeeker(FindPathProject findPathProject)
        {
            _findPathProject = findPathProject;
        }
        
        public override bool CheckEvent(Seeker seeker)
        {
            if (seeker.PathParams.TargetSurface == seeker.CurrentSurface)
            {
                return true;
            }

            return false;
        }
        
        public override PathParams GetPathParams(Seeker seeker)
        {
            Tile.Surface startSurface = seeker.PathParams.TargetSurface;
            Tile.Surface targetSurface = GetTargetSurface(startSurface);
            return new PathParams(startSurface, targetSurface);
        }

        private Tile.Surface GetTargetSurface(Tile.Surface startSurface)
        {
            List<Vector3Int> queue = new();
            queue.Add(startSurface.Tile.position);
            List<Tile> visitedTiles = new();
            
            for (int i = 0; i < 6; i++)
            {
                if (queue.Count == 0)
                {
                    break;
                }

                GetQueue(queue, visitedTiles);
            }
            
            return GetTargetSurface(visitedTiles);
        }
        
        private void GetQueue(List<Vector3Int> queueTiles, List<Tile> visitedTiles)
        {
            Vector3Int[] tiles = queueTiles.ToArray();

            foreach (var t in tiles)
            {
                queueTiles.Remove(t);
                foreach (var direction in _findPathProject.Directions.directionGroup.directions)
                {
                    if (_findPathProject.Tiles.TryGetValue(t + direction, out var tile))
                    {
                        if (!visitedTiles.Contains(tile) && !queueTiles.Contains(tile.position))
                        {
                            visitedTiles.Add(tile);
                            queueTiles.Add(tile.position);
                        }
                    }
                }
            }
        }

        private Tile.Surface GetTargetSurface(List<Tile> tiles)
        {
            List<Tile.Surface> surfaces = new();
            
            foreach (var tile in tiles)
            {
                foreach (var surface in tile.surfaces)
                {
                    if (!surface.isObstacle)
                    {
                        surfaces.Add(surface);
                    }
                }
            }

            return surfaces[Random.Range(0, surfaces.Count)];
        }
    }
}