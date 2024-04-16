namespace FindPath
{
    public abstract class FindMode
    {
        public abstract Surface[] GetPath(Surface startSurface, Surface targetSurface, FindPathProject findPathProject);
    }
}