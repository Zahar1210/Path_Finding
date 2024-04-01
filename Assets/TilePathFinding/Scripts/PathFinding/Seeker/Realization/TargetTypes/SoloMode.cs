using UnityEngine;

namespace FindPath
{
    public class SoloMode : FindTargetType
    {
        public override Transform GetTargetObject(Seeker seeker)
        {
            return seeker.SoloTarget;
        }
    }
}