using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PathFinding : MonoBehaviour
{
    public bool drawGizmos;
    public Dictionary<Vector3Int, Tile> _tiles = new();
    public Directions Directions => directions;
    [SerializeField] private Directions directions;

    private void Start()
    {
        AddTiles();
    }

    private void AddTiles()
    {
        Tile[] tiles = FindObjectsOfType<Tile>();
        
        foreach (var tile in tiles)
        {
            Vector3Int tilePosition = Vector3Int.RoundToInt(tile.transform.position);
            if (!_tiles.ContainsKey(tilePosition))
            {
                tile.Position = tilePosition;
                _tiles.Add(tilePosition, tile);
            }
        }

        foreach (var tile in tiles)
        {
            tile.SetSurface(this);
        }
    }
    
    public Tile.Surface[] GetPath(Tile.Surface startPosition, Tile.Surface targetPosition)
    {
        List<Vector3Int> queue = new();
        Dictionary<Vector3Int, TileInfo> visitedTiles = new();
        List<Tile.Surface> visitedSurfaces = new();
        
        int step = 0;
        
        queue.Add(startPosition.Tile.Position);
        SetDistance(startPosition.Tile._surfaces, startPosition, visitedSurfaces);
        
        while (true)
        {
            if (queue.Count == 0)
            {
                break;
            }
            GetQueue(queue, visitedTiles, step, startPosition, visitedSurfaces);
            step++;
            if (visitedTiles.ContainsKey(targetPosition.Tile.Position))
            {
                break;
            }
        }

        Tile.Surface[] path = GetFinalPath(visitedTiles, startPosition, targetPosition);
        return path;
    }

    private void GetQueue(List<Vector3Int> queueTiles, Dictionary<Vector3Int, TileInfo> visitedTiles,
        int step, Tile.Surface startSurface, List<Tile.Surface> visitedSurfaces)
    {
        Vector3Int[] tiles = queueTiles.ToArray();
        
        foreach (var t in tiles)
        {
            queueTiles.Remove(t);
            visitedTiles.Add(t, new TileInfo(step));
            foreach (var direction in directions.dirGroup.directions)
            {
                if (_tiles.TryGetValue(t + direction, out var tile))
                {
                    if (!visitedTiles.ContainsKey(tile.Position) && !queueTiles.Contains(tile.Position))
                    {
                        queueTiles.Add(tile.Position);
                        SetDistance(tile._surfaces, startSurface, visitedSurfaces);
                    }
                }
            }
        }
    }

    private Tile.Surface[] GetFinalPath(Dictionary<Vector3Int, TileInfo> visited, Tile.Surface startSurface, Tile.Surface targetSurface)
    {
        List<Tile.Surface> path = new();
        List<Tile> selectTiles = new();
        List<Tile> selectTilesCopy = new();
        Tile.Surface currentSurface = targetSurface;
        
        for (int i = 0; i < 55; i++) //итераций столько  сколько длина пути а вообще должен быть while()
        {
            selectTiles.Clear();
            selectTilesCopy.Clear();
            path.Add(currentSurface);
            
            foreach (var direction in currentSurface.Directions.DirectionArray) 
            {
                Vector3Int tilePos = currentSurface.Tile.Position + direction;
                
                if (visited.TryGetValue(tilePos, out var tileSurface) && tileSurface.Step == visited[currentSurface.Tile.Position].Step - 1)
                {
                    if (_tiles.TryGetValue(tilePos, out Tile tile) && tile._surfaces.TryGetValue(currentSurface.Direction, out var surface))
                    {
                        if (!path.Contains(surface) && surface.IsObstacle) 
                        {
                            selectTiles.Add(tile);
                            
                            if (surface == startSurface) 
                            {
                                path.Add(surface);
                                return path.ToArray(); 
                            }
                        }
                    }
                }
            }
            foreach (var dir in currentSurface.Directions.Directions) 
            {
                Vector3Int tilePosition = currentSurface.Tile.Position + dir;
                if (_tiles.TryGetValue(tilePosition, out Tile tile))
                    selectTilesCopy.Add(tile);
            }
            
            currentSurface = SelectTileSurfaces(selectTiles, currentSurface, selectTilesCopy, visited).OrderBy(s => s.Distance).FirstOrDefault();
            
            if (currentSurface == startSurface) 
            {
                path.Add(currentSurface);
                return path.ToArray();
            }
        }
        return path.ToArray();
    }
    
    private List<Tile.Surface> SelectTileSurfaces(List<Tile> tiles, Tile.Surface currentSurface, List<Tile> selectedTilesCopy,Dictionary<Vector3Int, TileInfo> visited)
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
            if (s.Value.Directions.Directions != currentSurface.Directions.Directions && !s.Value.IsObstacle)
            {
                selectedSurfaces.Add(s.Value);
            }
        }
        
        foreach (var tile in selectedTilesCopy) 
        {
            if (visited.TryGetValue(tile.Position, out TileInfo tileInfo)) 
            {
                selectedSurfaces.Add(SelectSurfacesTile(tile,currentSurface));
            }
        }
        return selectedSurfaces;
    }
    
    private Tile.Surface SelectSurfacesTile(Tile tile, Tile.Surface currentSurface)
    {
        Vector3Int direction = new();
        if (currentSurface.Directions.DirectionArray == directions.dirGroup.dirHorizontal)
        {
            if (tile.Position.z == currentSurface.Tile.Position.z)
                direction = currentSurface.Tile.Position.y < tile.Position.y ? Vector3Int.down : Vector3Int.up;
            if (tile.Position.y == currentSurface.Tile.Position.y)
                direction= currentSurface.Tile.Position.z < tile.Position.z ? Vector3Int.back : Vector3Int.forward;
        } 
        else if(currentSurface.Directions.DirectionArray == directions.dirGroup.dirVertical) 
        {
            if (tile.Position.x == currentSurface.Tile.Position.x)
                direction = currentSurface.Tile.Position.z < tile.Position.z ?Vector3Int.back : Vector3Int.forward;
            if (tile.Position.z == currentSurface.Tile.Position.z)
                direction = currentSurface.Tile.Position.x < tile.Position.x ? Vector3Int.left : Vector3Int.right;
        }
        else if (currentSurface.Directions.DirectionArray == directions.dirGroup.dirDepth)
        {
            if (tile.Position.y == currentSurface.Tile.Position.y)
                direction = currentSurface.Tile.Position.x < tile.Position.x ? Vector3Int.left : Vector3Int.right;
            if (tile.Position.x == currentSurface.Tile.Position.x)
                direction = currentSurface.Tile.Position.y < tile.Position.y ? Vector3Int.down : Vector3Int.up;
        }

        if (tile._surfaces.TryGetValue(direction, out Tile.Surface sur) && !sur.IsObstacle) 
            return sur;
        return null;
    }
    
    private void SetDistance(Dictionary<Vector3Int, Tile.Surface> surfaces, Tile.Surface startSurface, List<Tile.Surface> visitedSurfaces)
    {
        foreach (KeyValuePair<Vector3Int, Tile.Surface> surface in surfaces)
        {
            Tile.Surface s = surface.Value;
            
            if (!s.IsObstacle)
            {
                s.Distance =
                    Vector3.Distance(
                    s.Tile.transform.position + s.Direction,
                    startSurface.Tile.transform.position + startSurface.Direction);
                
                visitedSurfaces.Add(s);
            }
        }
    }
    
    
    private class TileInfo
    {
        public int Step { get; set; }

        public TileInfo(int step)
        {
            Step = step;
        }
    }
}