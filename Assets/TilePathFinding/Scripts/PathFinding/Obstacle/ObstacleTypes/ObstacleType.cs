namespace FindPath
{
    public abstract class ObstacleType
    {
        public Obstacle Obstacle { get; set; }
    
        public abstract void Initialize(Obstacle obstacle);
        
        public abstract void StartChecking();

        public abstract void Check();
    }
}
