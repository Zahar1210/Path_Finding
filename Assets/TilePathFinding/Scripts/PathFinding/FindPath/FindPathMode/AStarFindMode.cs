using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FindPath
{
    public class AStarFindMode : FindMode
    {
        public override Surface[] GetPath(Surface startSurface, Surface targetSurface, FindPathProject findPathProject)
        {
            return AStar(startSurface, targetSurface, findPathProject);
        }

        private Surface[] AStar(Surface startSurface, Surface targetSurface, FindPathProject findPathProject)
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

        private List<GridObject> SelectTiles(FindPathProject findPathProject,
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

        private List<GridObject> SelectTilesCopy(FindPathProject findPathProject,
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

        private void AddTile(FindPathProject findPathProject,
            List<GridObject> selectTiles, Vector3Int tilePos, Dictionary<Vector3Int, TileAStar> visitedTiles)
        {
            if (findPathProject.GridObjects.TryGetValue(tilePos, out var tile) && !visitedTiles.ContainsKey(tilePos))
            {
                selectTiles.Add(tile);
            }
        }

        private List<Surface> SelectTileSurfaces(List<GridObject> tiles, Surface currentSurface,
            List<GridObject> selectedTilesCopy, FindPathProject findPathProject)
        {
            List<Surface> selectedSurfaces = new();
            foreach (GridObject gridObject in tiles)
            {
                if (gridObject.Surfaces.TryGetValue(currentSurface.Direction, out var surface) && !surface.IsObstacle)
                {
                    selectedSurfaces.Add(surface);
                }
            }

            foreach (KeyValuePair<Vector3Int, Surface> s in currentSurface.GridObject.Surfaces)
            {
                if (s.Value.Directions._directionArray != currentSurface.Directions._directionArray &&
                    !s.Value.IsObstacle)
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

            if (tile.Surfaces.TryGetValue(vector, out Surface sur) && !sur.IsObstacle)
                return sur;

            return null;
        }
        
        private class TileAStar
        {
            public float FCost { get; }

            public TileAStar(float gCost)
            {
                FCost = gCost;
            }
        }
    }
}