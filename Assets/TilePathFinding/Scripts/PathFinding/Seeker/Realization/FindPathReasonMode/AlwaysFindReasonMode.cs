namespace FindPath
{
    public class AlwaysFindReasonMode : FindPathReasonMode
    {
        public override bool TryFind(Seeker seeker)
        {
            return true;
        }
    }
}