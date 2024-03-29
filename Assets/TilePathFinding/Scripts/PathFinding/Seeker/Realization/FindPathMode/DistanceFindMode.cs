using FindPath;
using UnityEngine;

public class DistanceFindMode : FindPathMode
{
    private float _maxDistance;
    
    public override bool CheckFind(Seeker2 seeker)
    {
        Vector3 start = GetSurfacePosition(seeker, seeker.CurrentSurface);
        Vector3 target = GetSurfacePosition(seeker, );
        
        float distance = Vector3.Distance(seeker.CurrentSurface , seeker.TargetPosition);

        return distance < _maxDistance;
    }

    private Vector3 GetSurfacePosition(Seeker2 seeker, Tile.Surface surface)
    {
        return 
            new Vector3(
            seeker.CurrentSurface.Tile.position.x + seeker.CurrentSurface.direction.x,
            seeker.CurrentSurface.Tile.position.y + seeker.CurrentSurface.direction.y,
            seeker.CurrentSurface.Tile.position.z + seeker.CurrentSurface.direction.z );
    }
}
