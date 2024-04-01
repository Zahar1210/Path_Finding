using UnityEngine;

namespace FindPath
{
    public class FieldOfViewAngleFindMode : FindPathMode
    {
        public override bool CheckFind(Seeker seeker)
        {
            Collider[] colliders = Physics.OverlapSphere(seeker.transform.position, seeker.Radius, seeker.TargetLayerMask);
            
            if (colliders.Length > 0)
            {
                foreach (var collider in colliders)
                {
                    Transform target = collider.transform;
                    Vector3 targetDirection = (target.position - seeker.transform.position).normalized;

                    if (Vector3.Angle(seeker.transform.forward, targetDirection) < seeker.Angle / 2)
                    {
                        float distanceToTarget = Vector3.Distance(seeker.transform.position, seeker.SeekerTarget.transform.position);
                        
                        if (Physics.Raycast(seeker.transform.position, targetDirection, distanceToTarget, seeker.ObstacleLayerMask))
                        {
                            return false;
                        }
                        
                        return true;
                    }
                }
            }
            
            return false;
        }
    }

}