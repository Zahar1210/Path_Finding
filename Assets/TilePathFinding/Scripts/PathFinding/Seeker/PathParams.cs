namespace FindPath
{
    [System.Serializable]
    public class PathParams 
    {
        public Surface StartSurface { get; set; }
        public Surface TargetSurface { get; set; }

        public PathParams(Surface startSurface, Surface targetSurface)
        {
            StartSurface = startSurface;
            TargetSurface = targetSurface;
        }
    }
}
