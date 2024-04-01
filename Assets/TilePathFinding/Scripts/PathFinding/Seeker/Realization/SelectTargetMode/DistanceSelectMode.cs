using UnityEngine;

namespace FindPath
{
    public class DistanceSelectMode : SelectTargetMode
    {
        public override float SelectTarget(Seeker seeker, Transform target)
        {
            return Vector3.Distance(seeker.transform.position, target.transform.position);
        }
    }
}