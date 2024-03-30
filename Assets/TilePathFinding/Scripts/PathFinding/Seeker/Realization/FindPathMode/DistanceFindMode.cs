using FindPath;
using UnityEngine;


namespace FindPath
{
    public class DistanceFindMode : FindPathMode
    {
        private float _maxDistance;
    
        public override bool CheckFind(Seeker seeker)
        {
            Vector3 start = GetSurfacePosition(seeker.StartSurface);
            Vector3 target = GetSurfacePosition(seeker.TargetSurface);
        
            float distance = Vector3.Distance(start , target);

            return distance < _maxDistance;
        }

        private Vector3 GetSurfacePosition(Tile.Surface surface)
        {
            return 
                new Vector3
                (
                    surface.Tile.position.x + surface.direction.x,
                    surface.Tile.position.y + surface.direction.y,
                    surface.Tile.position.z + surface.direction.z 
                );
        }
    }
}
