namespace FindPath
{
    public class StaticObstacleObject : ObstacleType
    {
        public override void Initialize(Obstacle obstacle)
        {
            Obstacle = obstacle;
            StartChecking();
        }

        public override void StartChecking()
        {
            Check();
        }

        public override void Check()
        {
            TileObstacleChecker.GetTilesForCheck(Obstacle);
            TileObstacleChecker.CalculateSurfaces(Obstacle);
        }
    }
}