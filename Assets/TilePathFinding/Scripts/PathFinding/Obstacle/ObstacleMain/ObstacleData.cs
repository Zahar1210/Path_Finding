using System.Collections.Generic;

namespace FindPath
{
    public static class ObstacleData
    {
        private static Dictionary<ObstacleObjectType, ObstacleType> _obstacles;

        public static void Initialize()
        {
            _obstacles = new();
            
            _obstacles.Add(ObstacleObjectType.Static, new StaticObstacleObject());
            _obstacles.Add(ObstacleObjectType.Dynamic, new DynamicObstacleObject());
            _obstacles.Add(ObstacleObjectType.LaterStatic, new LaterStaticObstacleObject());
        }
        
        public static ObstacleType GetObstacleType(ObstacleObjectType obstacleObjectType)
        {
            return _obstacles.TryGetValue(obstacleObjectType, out ObstacleType obstacle) ? obstacle : null;
        }
    }
}

public enum ObstacleObjectType
{
    Static,
    Dynamic,
    LaterStatic
}

