namespace FindPath
{
    public class AlwaysFindMode : FindPathMode
    {
        public override bool TryFind(Seeker seeker)
        {
            return true;
        }
    }
}