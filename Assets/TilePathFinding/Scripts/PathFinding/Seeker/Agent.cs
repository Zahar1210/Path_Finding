namespace FindPath
{
    public class Agent : Seeker
    {
        public override void Initialize()
        {
            SeekerData.Initialization();//точка входа 

            FindPathMode = SeekerData.GetFindPathMode(PathMode);
            FindPathTrigger = SeekerData.GetFindPathTrigger(PathTrigger);
            DynamicPath = SeekerData.GetFindPathModeDynamicPath(PathDynamic);
        }

        private void Update()
        {
            TargetSurface = FindPathTrigger.GetTargetSurface(this);
            
            if (TargetSurface != null)
            {
                FindPath(CurrentSurface, TargetSurface);
            }
        }

        public override void FindPath(Tile.Surface startSurface, Tile.Surface targetSurface)
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