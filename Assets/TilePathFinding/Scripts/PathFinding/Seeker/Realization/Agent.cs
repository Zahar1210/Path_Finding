namespace FindPath
{
    public class Agent : Seeker
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
            TargetSurface = FindPathTrigger.CheckEvent();

            if (TargetSurface != null)
            {
                if (FindPathMode.CheckFind(this))
                {
                    FindPathTrigger.GetPathParams();
                }
            }
        }
    }
}