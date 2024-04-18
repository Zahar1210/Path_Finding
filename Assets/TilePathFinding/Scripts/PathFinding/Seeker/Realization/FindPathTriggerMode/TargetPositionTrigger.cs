using UnityEngine;

namespace FindPath
{
    public class TargetPositionTrigger : FindPathTrigger
    {
        private readonly FindPathProject _findPathProject;
        private Surface _currentTargetSurface;
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
                return null;

            seeker.SeekerTarget = seeker.FindTargetType.GetTargetObject(seeker);

            return SurfaceFinder.GetSurface(seeker.SeekerTarget.transform.position,
                seeker.TargetDirection, seeker.Count, _findPathProject, seeker.SeekerTarget);
        }

        public override Surface TryGetCurrentSurface(Seeker seeker)
        {
            throw new System.NotImplementedException();
        }
    }
}