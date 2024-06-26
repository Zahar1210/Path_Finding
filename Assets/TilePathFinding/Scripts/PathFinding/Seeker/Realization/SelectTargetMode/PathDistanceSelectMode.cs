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
            Surface[] path = seeker.FindMode.GetPath(startSurface, targetSurface, FindPathProject.Instance);
            
            return path.Length;
        }
    }
}
