using UnityEngine;

namespace FindPath
{
    public class PathDistanceSelectMode : SelectTargetMode
    {
        public override float SelectTarget(Seeker seeker, Transform target)
        {
            Tile.Surface startSurface = seeker.CurrentSurface;
            Tile.Surface targetSurface =
                SurfaceFinder.GetSurface(target.transform.position, seeker.TargetDirection,
                seeker.Count, FindPathProject.Instance, target);

           Tile.Surface[] path = PathFinding.GetPath(startSurface, targetSurface, FindPathProject.Instance, FindMode.BreadthFirstSearch);

           return path.Length;
        }
    }
}
