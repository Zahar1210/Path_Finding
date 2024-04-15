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
            TargetSurface = FindPathTrigger.TryGetTargetSurface(this);
            
            if (TargetSurface != null)
            {
                GetPath(CurrentSurface, TargetSurface);
            }
        }

        public override void GetPath(Surface startSurface, Surface targetSurface)
        {
            if ((targetSurface != null && startSurface != null) && FindPathMode.TryFind(this))
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
                    GetPath(CurrentSurface, TargetSurface);
                }
            }
        }
    }
}