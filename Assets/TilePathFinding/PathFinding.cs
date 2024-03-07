using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace FindPath
{
    public static class PathFinding 
    {
        public static Tile.Surface[] GetPath(Tile.Surface startSurface,
            Tile.Surface targetSurface, FindPathProject findPathProject, FindMode mode)
        {
            if (mode == FindMode.BreadthFirstSearch)
                return BreadthFirstSearch(startSurface, targetSurface, findPathProject);
            
            if (mode == FindMode.AStar)
                return AStar(startSurface, targetSurface, findPathProject);
            
            return null;
        }

        
        #region AStar
        
        private static Tile.Surface[] AStar(Tile.Surface startSurface, Tile.Surface targetSurface, FindPathProject findPathProject)
        {
            List<Tile.Surface> path = new();
            List<Tile> selectTiles = new();
            List<Tile> selectTilesCopy = new();
            Tile.Surface currentSurface = startSurface;
            Dictionary<Vector3Int, TileAStar> visitedTiles = new();
            
            path.Add(currentSurface);
            
            for (int i = 0; i < 45; i++)
            {
                selectTiles = SelectTiles(findPathProject, currentSurface, visitedTiles);
                if (selectTiles.Count != 0)
                {
                    foreach (var tile in selectTiles)
                    {
                        AddTileInfo(tile);
                    }
                }
                
                selectTilesCopy = SelectTilesCopy(findPathProject, currentSurface, visitedTiles);
                if (selectTilesCopy.Count != 0)
                {
                    foreach (var tile in selectTilesCopy)
                    {
                        AddTileInfo(tile);
                    }
                }
                
                AddTileInfo(currentSurface.Tile);

                
                List<Tile.Surface> selectSurface = 
                    SelectTileSurfaces(selectTiles, currentSurface, selectTilesCopy, findPathProject);
                
                Tile.Surface surfacesWithMinCost = selectSurface
                    .OrderBy(s => visitedTiles[s.Tile.Position].FCost) 
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

            void AddTileInfo(Tile tile)
            {
                float weight = GetStep(tile);
                
                if (!visitedTiles.ContainsKey(tile.Position))
                {
                    visitedTiles.Add(tile.Position, new TileAStar(weight));
                }
            }

            float GetStep(Tile tile)
            {
                float target = Vector3.Distance(targetSurface.Tile.Position, tile.Position);
                // float pos = Vector3.Distance(tile.Position ,currentSurface.Tile.Position);
                return target;
            }
            
            return path.ToArray();
        }

        private static List<Tile> SelectTiles(FindPathProject findPathProject, 
            Tile.Surface currentSurface, Dictionary<Vector3Int, TileAStar> visitedTiles)
        {
            List<Tile> selectTiles = new();
            
            foreach (var direction in currentSurface.Directions.DirectionArray)
            {
                Vector3Int tilePos = currentSurface.Tile.Position + direction;
                AddTile(findPathProject, selectTiles, tilePos, visitedTiles);
            } 
            
            return selectTiles;
        }

        private static List<Tile> SelectTilesCopy(FindPathProject findPathProject, 
            Tile.Surface currentSurface, Dictionary<Vector3Int, TileAStar> visitedTiles)
        {
            List<Tile> selectTiles = new();
            foreach (var direction in currentSurface.Directions.Directions)
            {
                Vector3Int tilePos = currentSurface.Tile.Position + direction;
                AddTile(findPathProject, selectTiles, tilePos, visitedTiles);
            } 
            
            return selectTiles;
        }
        
        private static void AddTile(FindPathProject findPathProject, 
            List<Tile> selectTiles, Vector3Int tilePos, Dictionary<Vector3Int, TileAStar> visitedTiles )
        {
            if (findPathProject.Tiles.TryGetValue(tilePos, out var tile) && !visitedTiles.ContainsKey(tilePos))
            {
                selectTiles.Add(tile);
            }
        }
        
        #endregion
        
        #region BreadthFirstSearch
        
        private static Tile.Surface[] BreadthFirstSearch(Tile.Surface startPosition, Tile.Surface targetPosition, FindPathProject findPathProject)
        {
            int step = 0;
            List<Vector3Int> queue = new();
            Dictionary<Vector3Int, TileBreadthFirstSearch> visitedTiles = new();

            queue.Add(startPosition.Tile.Position);

            while (true)
            {
                if (queue.Count == 0)
                {
                    break;
                }

                GetQueue(queue, visitedTiles, step, findPathProject);
                step++;

                if (visitedTiles.ContainsKey(targetPosition.Tile.Position))
                {
                    break;
                }
            }

            Tile.Surface[] path = GetFinalPath(visitedTiles, startPosition, targetPosition, findPathProject);
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
                foreach (var direction in findPathProject.Directions.dirGroup.directions)
                {
                    if (findPathProject.Tiles.TryGetValue(t + direction, out var tile))
                    {
                        if (!visitedTiles.ContainsKey(tile.Position) && !queueTiles.Contains(tile.Position))
                        {
                            queueTiles.Add(tile.Position);
                        }
                    }
                }
            }
        }

        private static Tile.Surface[] GetFinalPath(Dictionary<Vector3Int, TileBreadthFirstSearch> visited, Tile.Surface startSurface,
            Tile.Surface targetSurface, FindPathProject findPathProject)
        {
            List<Tile.Surface> path = new();
            List<Tile> selectTiles = new();
            List<Tile> selectTilesCopy = new();
            Tile.Surface currentSurface = targetSurface;

            while(!path.Contains(startSurface))
            {
                selectTiles.Clear();
                selectTilesCopy.Clear();
                path.Add(currentSurface);
                
                foreach (var direction in currentSurface.Directions.DirectionArray)
                {
                    Vector3Int tilePos = currentSurface.Tile.Position + direction;

                    if (visited.TryGetValue(tilePos, out var tileSurface) &&
                        tileSurface.Step == visited[currentSurface.Tile.Position].Step - 1)
                    {
                        if (findPathProject.Tiles.TryGetValue(tilePos, out Tile tile) &&
                            tile._surfaces.TryGetValue(currentSurface.Direction, out var surface))
                        {
                            if (!path.Contains(surface) && !surface.IsObstacle)
                            {
                                selectTiles.Add(tile);
                            }
                        }
                    }
                }

                foreach (var dir in currentSurface.Directions.Directions)
                {
                    Vector3Int tilePosition = currentSurface.Tile.Position + dir;

                    if (visited.TryGetValue(tilePosition, out var tileSurface) &&
                        tileSurface.Step <= visited[currentSurface.Tile.Position].Step)
                    {
                        if (findPathProject.Tiles.TryGetValue(tilePosition, out Tile tile) && !selectTilesCopy.Contains(tile))
                        {
                            selectTilesCopy.Add(tile);
                        }
                    }
                }

                List<Tile.Surface> selectSurfaces = SelectTileSurfaces(selectTiles, currentSurface, selectTilesCopy, findPathProject);
                
                int minStep = selectSurfaces.Min(s => visited[s.Tile.Position].Step);
                List<Tile.Surface> surfacesWithMinStep = selectSurfaces.Where(s => visited[s.Tile.Position].Step == minStep).ToList();
                
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

        private static List<Tile.Surface> SelectTileSurfaces(List<Tile> tiles, Tile.Surface currentSurface,
            List<Tile> selectedTilesCopy, FindPathProject findPathProject)
        {
            List<Tile.Surface> selectedSurfaces = new();
            foreach (Tile tile in tiles)
            {
                if (tile._surfaces.TryGetValue(currentSurface.Direction, out var surface) && !surface.IsObstacle)
                {
                    selectedSurfaces.Add(surface);
                }
            }

            foreach (KeyValuePair<Vector3Int, Tile.Surface> s in currentSurface.Tile._surfaces)
            {
                if (s.Value.Directions.DirectionArray != currentSurface.Directions.DirectionArray && !s.Value.IsObstacle)
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

        private static Tile.Surface SelectSurfacesTile(Tile tile, Tile.Surface currentSurface, FindPathProject findPathProject)
        {
            Vector3Int vector = currentSurface.Tile.Position - tile.Position;

            if (currentSurface.Directions.Directions == findPathProject.Directions.dir.dirBack ||
                currentSurface.Directions.Directions == findPathProject.Directions.dir.dirFront)
                vector.z = 0;
            else if (currentSurface.Directions.Directions == findPathProject.Directions.dir.dirDown ||
                     currentSurface.Directions.Directions == findPathProject.Directions.dir.dirUp)
                vector.y = 0;
            else if (currentSurface.Directions.Directions == findPathProject.Directions.dir.dirRight ||
                     currentSurface.Directions.Directions == findPathProject.Directions.dir.dirLeft)
                vector.x = 0;

            if (tile._surfaces.TryGetValue(vector, out Tile.Surface sur) && !sur.IsObstacle)
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