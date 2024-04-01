using UnityEngine;

namespace FindPath
{
    public abstract class SelectTargetMode
    {
        public abstract float SelectTarget(Seeker seeker, Transform target);
    }
}
