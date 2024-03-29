using FindPath;

public class SeekerR : Seeker2
{
    private void Start()
    {
        Initialize();
    }

    public override void Initialize()
    {
        SeekerData.Initialization(this);
        
        FindPathMode = SeekerData.GetFindPathMode(PathMode);
        FindPathTrigger = SeekerData.GetFindPathTrigger(PathTrigger);
        DynamicPath = SeekerData.GetFindPathModeDynamicPath(PathDynamic);
    }

    private void Update()
    {
        Tile.Surface targetPos = FindPathTrigger.CheckEvent();
        
        if (targetPos != null)
        {
            if (FindPathMode.CheckFind(this))
            {
                FindPathTrigger.GetPathParams();
            }
        }
    }
}
