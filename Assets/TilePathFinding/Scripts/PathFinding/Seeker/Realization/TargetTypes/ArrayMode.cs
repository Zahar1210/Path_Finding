using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FindPath
{
    public class ArrayMode : FindTargetType
    {
        public override Transform GetTargetObject(Seeker seeker)
        {
            Dictionary<Transform, float> distanceToTargets = new();
            
            foreach (var target in seeker.ArrayTargets)
            {
                float distance = seeker.SelectTargetMode.SelectTarget(seeker, target);
                distanceToTargets.Add(target, distance);
            }
            
            return distanceToTargets.OrderBy(kv => kv.Value).FirstOrDefault().Key;
        }
    }
}
