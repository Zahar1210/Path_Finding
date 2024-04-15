using UnityEngine;

namespace FindPath
{
    public class DistanceFindMode : FindPathMode
    {
        public override bool TryFind(Seeker seeker)
        {
            Vector3 start = GetSurfacePosition(seeker.StartSurface);
            Vector3 target = GetSurfacePosition(seeker.TargetSurface);
        
            float distance = Vector3.Distance(start , target);

            return distance < seeker.MaxDistance;
        }

        private Vector3 GetSurfacePosition(Surface surface)
        {
            return 
                new Vector3
                (
                    surface.GridObject.Position.x + surface.direction.x,
                    surface.GridObject.Position.y + surface.direction.y,
                    surface.GridObject.Position.z + surface.direction.z 
                );
        }
    }
}
