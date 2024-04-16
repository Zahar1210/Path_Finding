using UnityEngine;

namespace FindPath
{
    public class FieldOfViewOverlapFindReasonReasonMode : FindPathReasonMode
    {
        public override bool TryFind(Seeker seeker)
        {
            Collider[] colliders = Physics.OverlapSphere(seeker.transform.position, seeker.Radius, seeker.TargetLayerMask);
            
            if (colliders.Length > 0)
            {
                foreach (var collider in colliders)
                {
                    Transform target = collider.transform;
                    Vector3 targetDirection = (target.position - seeker.transform.position).normalized;
                }
            }

            return false;
        }
    }
}