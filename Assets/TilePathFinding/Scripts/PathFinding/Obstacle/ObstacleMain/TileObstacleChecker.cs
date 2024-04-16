using UnityEngine;

namespace FindPath
{
    public static class TileObstacleChecker
    {
        public static void GetTilesForCheck(Obstacle obstacle)
        {
            if (obstacle.FindPathProjectInstance == null || obstacle.Colliders.Length <= 0)
            {
                return;
            }

            foreach (var coll in obstacle.Colliders)
            {
                Bounds bounds = coll.bounds;
                int tileRadius = obstacle.FindPathProjectInstance.TileSize;

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
                            if (obstacle.FindPathProjectInstance.GridObjects.TryGetValue(new Vector3Int(x, y, z), out var tile) &&
                                !obstacle.GridObjects.Contains(tile))
                            {
                                obstacle.GridObjects.Add(tile);
                                foreach (var surface in tile.Surfaces.Values)
                                {
                                    if (!surface.obstacleLock)
                                    {
                                        obstacle.Surfaces.Add(surface);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        public static void CalculateSurfaces(Obstacle obstacle)
        {
            foreach (var surface in obstacle.Surfaces)
            {
                Vector3 pos = surface.GridObject.Position + surface.direction;
                Collider[] colls = Physics.OverlapSphere(pos, obstacle.CheckRadius, obstacle.LayerMask);

                surface.isObstacle = (colls.Length > 0);
                
                //Todo вохможно где то здесь будет вызов метода проверки пути чтобы находить путь но новой  
            }
        }
    }
}