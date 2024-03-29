using FindPath;

public class PathParams 
{
    public Tile.Surface StartSurface { get; set; }
    public Tile.Surface TargetSurface { get; set; }

    public PathParams(Tile.Surface startSurface, Tile.Surface targetSurface)
    {
        StartSurface = startSurface;
        TargetSurface = targetSurface;
    }
}
