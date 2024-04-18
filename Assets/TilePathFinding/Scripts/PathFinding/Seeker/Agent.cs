
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
            CheckFind();
        }

        private void CheckFind()
        {
            TargetSurface = FindPathTrigger.TryGetTargetSurface(this);

            if (TargetSurface != null) 
            {
                if (CurrentSurface == null)
                {
                    CurrentSurface = FindPathTrigger.TryGetCurrentSurface(this);
                }
                
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
                
                Path = FindMode.GetPath(startSurface, targetSurface, FindPathProject);
            }
        }

        public override void CheckPath(PathDynamic pathDynamic) // какаха ненужная )) 
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