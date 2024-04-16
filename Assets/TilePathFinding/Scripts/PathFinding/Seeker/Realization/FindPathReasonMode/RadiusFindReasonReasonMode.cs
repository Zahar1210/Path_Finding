using UnityEngine;

namespace FindPath
{
    public class RadiusFindReasonReasonMode : FindPathReasonMode
    {
        public override bool TryFind(Seeker seeker) //TODO
        {
            Vector3Int startPosition = seeker.StartSurface.GridObject.Position;
            Vector3Int targetPosition = seeker.TargetSurface.GridObject.Position;

            Vector3Int difference = targetPosition - startPosition;

            return difference.x <= seeker.DifferenceX && difference.y <= seeker.DifferenceY && difference.z <= seeker.DifferenceZ;
        }
    }
}
