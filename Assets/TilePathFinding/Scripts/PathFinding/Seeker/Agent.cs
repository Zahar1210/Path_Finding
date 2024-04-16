namespace FindPath
{
    public class Agent : Seeker
    {
        public override void Initialize()
        {
            SeekerData.Initialization(); //точка входа 

            FindPathReasonMode = SeekerData.GetFindPathMode(PathReason);
            FindTargetType = SeekerData.GetFindPathTargetType(TargetType);
            SelectTargetMode = SeekerData.GetTargetSelectMode(TargetSelectMode);
            FindPathTrigger = SeekerData.GetFindPathTrigger(PathTrigger);
            DynamicPath = SeekerData.GetFindPathModeDynamicPath(PathDynamic);
            FindMode = SeekerData.GetFindMode(PathFindMode);
        }

        private void Update()
        {
            TargetSurface = FindPathTrigger.TryGetTargetSurface(this);
            
            if (TargetSurface != null)
            {
                TryGetPath(CurrentSurface, TargetSurface);
            }
        }

        public override void TryGetPath(Surface startSurface, Surface targetSurface)
        {
            if ((targetSurface != null && startSurface != null) && FindPathReasonMode.TryFind(this))
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
                    TryGetPath(CurrentSurface, TargetSurface);
                }
            }
        }
    }
}