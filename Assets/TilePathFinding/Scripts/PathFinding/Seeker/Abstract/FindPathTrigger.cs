namespace FindPath
{
    public abstract class FindPathTrigger
    {
        // это первичные условия для поиска пути
        public abstract Tile.Surface GetTargetSurface(Seeker seeker);

        public abstract PathParams GetPathParams();
    }
}
