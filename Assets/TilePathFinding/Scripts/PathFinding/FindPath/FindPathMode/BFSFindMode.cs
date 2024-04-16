using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FindPath
{
    public class BFSFindMode : FindMode
    {
        public override Surface[] GetPath(Surface startSurface, Surface targetSurface, FindPathProject findPathProject)
        {
            return BreadthFirstSearch(startSurface, targetSurface, findPathProject);
        }
        
        private Surface[] BreadthFirstSearch(Surface startSurface, Surface targetSurface, FindPathProject findPathProject)
        {
            int step = 0;
            List<Vector3Int> queue = new();
            Dictionary<Vector3Int, TileBreadthFirstSearch> visitedTiles = new();

            queue.Add(startSurface.GridObject.Position);

            while (true)
            {
                if (queue.Count == 0)
                {
                    break;
                }

                GetQueue(queue, visitedTiles, step, findPathProject);
                step++;

                if (visitedTiles.ContainsKey(targetSurface.GridObject.Position))
                {
                    break;
                }
            }

            Surface[] path = GetFinalPath(visitedTiles, startSurface, targetSurface, findPathProject);
            return path;
        }

        private void GetQueue(List<Vector3Int> queueTiles, Dictionary<Vector3Int, TileBreadthFirstSearch> visitedTiles, 
            int step, FindPathProject findPathProject)
        {
            Vector3Int[] tiles = queueTiles.ToArray();

            foreach (var t in tiles)
            {
                queueTiles.Remove(t);
                visitedTiles.Add(t, new TileBreadthFirstSearch(step));
                foreach (var direction in findPathProject.Directions.directionGroup._directions)
                {
                    if (findPathProject.GridObjects.TryGetValue(t + direction, out var tile))
                    {
                        if (!visitedTiles.ContainsKey(tile.Position) && !queueTiles.Contains(tile.Position))
                        {
                            queueTiles.Add(tile.Position);
                        }
                    }
                }
            }
        }

        private Surface[] GetFinalPath(Dictionary<Vector3Int, TileBreadthFirstSearch> visited, Surface startSurface,
            Surface targetSurface, FindPathProject findPathProject)
        {
            List<Surface> path = new();
            List<GridObject> selectTiles = new();
            List<GridObject> selectTilesCopy = new();
            Surface currentSurface = targetSurface;

            while(!path.Contains(startSurface))
            {
                selectTiles.Clear();
                selectTilesCopy.Clear();
                path.Add(currentSurface);
                
                foreach (var direction in currentSurface.Directions._directionArray)
                {
                    Vector3Int tilePos = currentSurface.GridObject.Position + direction;

                    if (visited.TryGetValue(tilePos, out var tileSurface) &&
                        tileSurface.Step == visited[currentSurface.GridObject.Position].Step - 1)
                    {
                        if (findPathProject.GridObjects.TryGetValue(tilePos, out GridObject tile) &&
                            tile.Surfaces.TryGetValue(currentSurface.direction, out var surface))
                        {
                            if (!path.Contains(surface) && !surface.isObstacle)
                            {
                                selectTiles.Add(tile);
                            }
                        }
                    }
                }

                foreach (var dir in currentSurface.Directions._directions)
                {
                    Vector3Int tilePosition = currentSurface.GridObject.Position + dir;

                    if (visited.TryGetValue(tilePosition, out var tileSurface) &&
                        tileSurface.Step <= visited[currentSurface.GridObject.Position].Step)
                    {
                        if (findPathProject.GridObjects.TryGetValue(tilePosition, out GridObject tile) && !selectTilesCopy.Contains(tile))
                        {
                            selectTilesCopy.Add(tile);
                        }
                    }
                }

                List<Surface> selectSurfaces = SelectTileSurfaces(selectTiles, currentSurface, selectTilesCopy, findPathProject);
                
                int minStep = selectSurfaces.Min(s => visited[s.GridObject.Position].Step);
                List<Surface> surfacesWithMinStep = selectSurfaces.Where(s => visited[s.GridObject.Position].Step == minStep).ToList();
                
                currentSurface = surfacesWithMinStep[Random.Range(0, surfacesWithMinStep.Count)];

                if (currentSurface == startSurface)
                {
                    path.Add(currentSurface);
                    return path.ToArray();
                }
            }
            return path.ToArray();
        }
        

        private List<Surface> SelectTileSurfaces(List<GridObject> tiles, Surface currentSurface,
            List<GridObject> selectedTilesCopy, FindPathProject findPathProject)
        {
            List<Surface> selectedSurfaces = new();
            foreach (CubeGridObject tile in tiles)
            {
                if (tile.Surfaces.TryGetValue(currentSurface.direction, out var surface) && !surface.isObstacle)
                {
                    selectedSurfaces.Add(surface);
                }
            }

            foreach (KeyValuePair<Vector3Int, Surface> s in currentSurface.GridObject.Surfaces)
            {
                if (s.Value.Directions._directionArray != currentSurface.Directions._directionArray && !s.Value.isObstacle)
                {
                    selectedSurfaces.Add(s.Value);
                }
            }

            foreach (var tile in selectedTilesCopy)
            {
                selectedSurfaces.Add(SelectSurfacesTile(tile, currentSurface, findPathProject));
            }

            return selectedSurfaces;
        }

        private Surface SelectSurfacesTile(GridObject tile, Surface currentSurface, FindPathProject findPathProject)
        {
            Vector3Int vector = currentSurface.GridObject.Position - tile.Position;

            if (currentSurface.Directions._directions == findPathProject.Directions.direction._directionBack ||
                currentSurface.Directions._directions == findPathProject.Directions.direction._directionFront)
                vector.z = 0;
            else if (currentSurface.Directions._directions == findPathProject.Directions.direction._directionDown ||
                     currentSurface.Directions._directions == findPathProject.Directions.direction._directionUp)
                vector.y = 0;
            else if (currentSurface.Directions._directions == findPathProject.Directions.direction._directionRight ||
                     currentSurface.Directions._directions == findPathProject.Directions.direction._directionLeft)
                vector.x = 0;

            if (tile.Surfaces.TryGetValue(vector, out Surface sur) && !sur.isObstacle)
                return sur;

            return null;
        }
        
        private class TileBreadthFirstSearch
        {
            public int Step { get; }

            public TileBreadthFirstSearch(int step)
            {
                Step = step;
            }
        }
    }
}
