using UnityEngine;

namespace FindPath
{
    public class PathDistanceSelectMode : SelectTargetMode
    {
        public override float SelectTarget(Seeker seeker, Transform target)
        {
            Surface startSurface = seeker.CurrentSurface;
            Surface targetSurface =
                SurfaceFinder.GetSurface(target.transform.position, seeker.TargetDirection,
                seeker.Count, FindPathProject.Instance, target);
            
            //TODO тут ошибка при вызове метода 
           Surface[] path = PathFinding.GetPath(startSurface, targetSurface, FindPathProject.Instance, PathFindMode.BreadthFirstSearch);

           return path.Length;
        }
    }
}
