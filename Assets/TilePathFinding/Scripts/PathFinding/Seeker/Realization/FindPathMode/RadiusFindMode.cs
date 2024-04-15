using UnityEngine;

namespace FindPath
{
    public class RadiusFindMode : FindPathMode
    {
        public override bool TryFind(Seeker seeker) //TODO
        {
            Vector3Int startPosition = seeker.StartSurface.Tile.position;
            Vector3Int targetPosition = seeker.TargetSurface.Tile.position;

            Vector3Int difference = targetPosition - startPosition;

            return difference.x <= seeker.DifferenceX && difference.y <= seeker.DifferenceY && difference.z <= seeker.DifferenceZ;
        }
    }
}
