namespace FindPath
{
    public abstract class FindPathMode
    {
        //это типо навые условия для поиска пути 
        public abstract bool TryFind(Seeker seeker);
    }
}
