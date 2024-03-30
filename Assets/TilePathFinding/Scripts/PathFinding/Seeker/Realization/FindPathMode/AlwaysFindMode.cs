namespace FindPath
{
    public class AlwaysFindMode : FindPathMode
    {
        public override bool CheckFind(Seeker seeker)
        {
            return true;
        }
    }
}