namespace FindPath
{
    public class AlwaysFindReasonReasonMode : FindPathReasonMode
    {
        public override bool TryFind(Seeker seeker)
        {
            return true;
        }
    }
}