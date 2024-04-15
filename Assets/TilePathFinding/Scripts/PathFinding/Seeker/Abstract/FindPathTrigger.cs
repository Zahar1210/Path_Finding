namespace FindPath
{
    public abstract class FindPathTrigger
    {
        // это первичные условия для поиска пути
        public abstract Surface TryGetTargetSurface(Seeker seeker);

        public abstract PathParams GetPathParams();
    }
}
