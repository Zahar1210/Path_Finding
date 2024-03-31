namespace FindPath
{
    public class Agent : Seeker
    {
        public override void Initialize()
        {
            SeekerData.Initialization(this);

            FindPathMode = SeekerData.GetFindPathMode(PathMode);
            FindPathTrigger = SeekerData.GetFindPathTrigger(PathTrigger);
            DynamicPath = SeekerData.GetFindPathModeDynamicPath(PathDynamic);
        }

        private void Update()
        {
            TargetSurface = FindPathTrigger.CheckEvent();
            FindPath(CurrentSurface, TargetSurface);
        }

        private void FindPath(Tile.Surface startSurface, Tile.Surface targetSurface)
        {
            if ((targetSurface != null && startSurface != null) && FindPathMode.CheckFind(this))
            {
                StartSurface = startSurface;
                CurrentSurface = startSurface;
                TargetSurface = targetSurface;
                
                FindPathTrigger.GetPathParams();
            }
        }

        public override void CheckPath(PathDynamic pathDynamic)
        {
            if (PathDynamic == pathDynamic && Path?.Length > 0)
            {
                if (DynamicPath.VerificationPath(this))
                {
                    FindPath(CurrentSurface, TargetSurface);
                }
            }
        }
    }
}