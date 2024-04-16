using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace FindPath
{
    public static class PathFinding 
    {
        public static Surface[] GetPath(Surface startSurface, Surface targetSurface, FindPathProject findPathProject)
        {
            //TODO
            // if (findMode == PathFindMode.BreadthFirstSearch)
            // {
            //     return BreadthFirstSearch(startSurface, targetSurface, findPathProject);
            // }
            //
            // if (findMode == PathFindMode.AStar)
            // {
            //     return AStar(startSurface, targetSurface, findPathProject);
            // }
            //
            // return null;
        }

        
        #region AStar
        
        private static Surface[] AStar(Surface startSurface, Surface targetSurface, FindPathProject findPathProject)
        {
            List<Surface> path = new();
            Surface currentSurface = startSurface;
            Dictionary<Vector3Int, TileAStar> visitedTiles = new();
            
            path.Add(currentSurface);
            
            for (int i = 0; i < 45; i++)
            {
                List<GridObject> selectedTiles = SelectTiles(findPathProject, currentSurface, visitedTiles);
                if (selectedTiles.Count != 0)
                {
                    foreach (var tile in selectedTiles)
                    {
                        AddTileInfo(tile);
                    }
                }
                
                List<GridObject> selectedTilesCopy = SelectTilesCopy(findPathProject, currentSurface, visitedTiles);
                if (selectedTilesCopy.Count != 0)
                {
                    foreach (var tile in selectedTilesCopy)
                    {
                        AddTileInfo(tile);
                    }
                }
                
                AddTileInfo(currentSurface.GridObject);

                
                List<Surface> selectSurface = 
                    SelectTileSurfaces(selectedTiles, currentSurface, selectedTilesCopy, findPathProject);
                
                Surface surfacesWithMinCost = selectSurface
                    .OrderBy(s => visitedTiles[s.GridObject.Position].FCost)
                    .FirstOrDefault(s => !path.Contains(s)); 
                
                currentSurface = surfacesWithMinCost;


                if (currentSurface == null)
                {
                    throw new ArgumentException($"No surface found for current surface");
                }
                
                path.Add(currentSurface);
                
                if (currentSurface == targetSurface) 
                {
                    break;
                }
            }

            void AddTileInfo(GridObject tile)
            {
                float weight = GetStep(tile);
                
                if (!visitedTiles.ContainsKey(tile.Position))
                {
                    visitedTiles.Add(tile.Position, new TileAStar(weight));
                }
            }

            float GetStep(GridObject tile)
            {
                return Vector3.Distance(targetSurface.GridObject.Position, tile.Position);
            }
            
            return path.ToArray();
        }

        private static List<GridObject> SelectTiles(FindPathProject findPathProject, 
            Surface currentSurface, Dictionary<Vector3Int, TileAStar> visitedTiles)
        {
            List<GridObject> selectTiles = new();
            
            foreach (var direction in currentSurface.Directions._directionArray)
            {
                Vector3Int tilePos = currentSurface.GridObject.Position + direction;
                AddTile(findPathProject, selectTiles, tilePos, visitedTiles);
            } 
            
            return selectTiles;
        }

        private static List<GridObject> SelectTilesCopy(FindPathProject findPathProject, 
            Surface currentSurface, Dictionary<Vector3Int, TileAStar> visitedTiles)
        {
            List<GridObject> selectTiles = new();
            foreach (var direction in currentSurface.Directions._directions)
            {
                Vector3Int tilePos = currentSurface.GridObject.Position + direction;
                AddTile(findPathProject, selectTiles, tilePos, visitedTiles);
            } 
            
            return selectTiles;
        }
        
        private static void AddTile(FindPathProject findPathProject, 
            List<GridObject> selectTiles, Vector3Int tilePos, Dictionary<Vector3Int, TileAStar> visitedTiles )
        {
            if (findPathProject.GridObjects.TryGetValue(tilePos, out var tile) && !visitedTiles.ContainsKey(tilePos))
            {
                selectTiles.Add(tile);
            }
        }
        
        #endregion
        
        #region BreadthFirstSearch
        
        private static Surface[] BreadthFirstSearch(Surface startPosition, Surface targetPosition, FindPathProject findPathProject)
        {
            int step = 0;
            List<Vector3Int> queue = new();
            Dictionary<Vector3Int, TileBreadthFirstSearch> visitedTiles = new();

            queue.Add(startPosition.GridObject.Position);

            while (true)
            {
                if (queue.Count == 0)
                {
                    break;
                }

                GetQueue(queue, visitedTiles, step, findPathProject);
                step++;

                if (visitedTiles.ContainsKey(targetPosition.GridObject.Position))
                {
                    break;
                }
            }

            Surface[] path = GetFinalPath(visitedTiles, startPosition, targetPosition, findPathProject);
            return path;
        }

        private static void GetQueue(List<Vector3Int> queueTiles, Dictionary<Vector3Int, TileBreadthFirstSearch> visitedTiles, 
            int step, FindPathProject findPathProject)
        {
            Vector3Int[] tiles = queueTiles.ToArray();

            foreach (var t in tiles)
            {
                queueTiles.Remove(t);
                visitedTiles.Add(t, new TileBreadthFirstSearch(step));
                foreach (var direction in findPathProject.Directions._directionGroup._directions)
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

        private static Surface[] GetFinalPath(Dictionary<Vector3Int, TileBreadthFirstSearch> visited, Surface startSurface,
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
        
        #endregion

        #region SelectSurface

        private static List<Surface> SelectTileSurfaces(List<GridObject> tiles, Surface currentSurface,
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

        private static Surface SelectSurfacesTile(GridObject tile, Surface currentSurface, FindPathProject findPathProject)
        {
            Vector3Int vector = currentSurface.GridObject.Position - tile.Position;

            if (currentSurface.Directions._directions == findPathProject.Directions._direction._directionBack ||
                currentSurface.Directions._directions == findPathProject.Directions._direction._directionFront)
                vector.z = 0;
            else if (currentSurface.Directions._directions == findPathProject.Directions._direction._directionDown ||
                     currentSurface.Directions._directions == findPathProject.Directions._direction._directionUp)
                vector.y = 0;
            else if (currentSurface.Directions._directions == findPathProject.Directions._direction._directionRight ||
                     currentSurface.Directions._directions == findPathProject.Directions._direction._directionLeft)
                vector.x = 0;

            if (tile.Surfaces.TryGetValue(vector, out Surface sur) && !sur.isObstacle)
                return sur;

            return null;
        }
        #endregion

        
        private class TileBreadthFirstSearch
        {
            public int Step { get; }

            public TileBreadthFirstSearch(int step)
            {
                Step = step;
            }
        }

        private class TileAStar
        {
            public float FCost { get; }

            public TileAStar(float gCost)
            {
                FCost =  gCost;
            }
        }
    }
}