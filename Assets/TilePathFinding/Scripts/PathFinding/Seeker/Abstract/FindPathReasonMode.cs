namespace FindPath
{
    public abstract class FindPathReasonMode
    {
        //это типо навые условия для поиска пути 
        public abstract bool TryFind(Seeker seeker);
    }
}
