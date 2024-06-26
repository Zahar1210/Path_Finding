namespace FindPath
{
    public class ObstacleObject : Obstacle
    {
        public override void Initialize()
        {
            GridObjects = new();
            Surfaces = new();
            
            FindPathProjectInstance = FindPathProject.Instance;
            StartObjectObstacleType = ObstacleObjectType;
            
            ObstacleData.Initialize();
            ObstacleType = ObstacleData.GetObstacleType(ObstacleObjectType);
            
            ObstacleType.Initialize(this);
        }
    }
}
