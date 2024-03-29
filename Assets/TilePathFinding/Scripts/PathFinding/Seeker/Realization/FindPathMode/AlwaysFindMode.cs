
public class AlwaysFindMode : FindPathMode
{
    public override bool CheckFind(Seeker2 seeker)
    {
        return true;
    }
}
