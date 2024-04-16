using UnityEngine;

namespace FindPath
{
    public class TargetPositionTrigger : FindPathTrigger
    {
        private Surface _currentSurfaceTarget;
        private readonly FindPathProject _findPathProject;
        private float _pastCheckTime;

        public TargetPositionTrigger(FindPathProject findPathProject)
        {
            _pastCheckTime = Time.time;
            _findPathProject = findPathProject;
        }

        private bool Timer(Seeker seeker)
        {
            if (_pastCheckTime + seeker.CheckInterval < Time.time)
            {
                _pastCheckTime = Time.time;
                return true;
            }

            return false;
        }

        public override Surface TryGetTargetSurface(Seeker seeker)
        {
            if (!Timer(seeker)) 
            {
                return null;
            }
            
            seeker.SeekerTarget = seeker.FindTargetType.GetTargetObject(seeker);
            
            return 
                SurfaceFinder.GetSurface(seeker.SeekerTarget.transform.position, 
                seeker.TargetDirection, seeker.Count, _findPathProject, seeker.SeekerTarget);
        }
        
        public override PathParams GetPathParams()
        {
            throw new System.NotImplementedException();
        }
    }
}