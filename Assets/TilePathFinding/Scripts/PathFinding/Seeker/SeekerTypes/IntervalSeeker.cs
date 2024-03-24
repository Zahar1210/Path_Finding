using UnityEngine;

namespace FindPath
{
    public class IntervalSeeker : SeekerAbstract
    {
        private bool _isFindPath;
        private float _pastCheckTime;
        private readonly FindPathProject _findPathProject;

        public IntervalSeeker(float pastCheckTime, FindPathProject findPathProject)
        {
            _pastCheckTime = pastCheckTime;
            _findPathProject = findPathProject;
        }
            
        public override bool CheckEvent(Seeker seeker)
        {
            return Timer(seeker);
        }
        
        private bool Timer(Seeker seeker)
        {
            if ((Time.timeSinceLevelLoad - _pastCheckTime) > seeker.interval)
            {
                _pastCheckTime = Time.time;
                return true;
            }
            return false;
        }
        
        public override PathParams GetPathParams(Seeker seeker)
        {
            Tile.Surface startSurface = GetSurface(seeker.transform);
            Tile.Surface targetSurface = GetSurface(seeker.target.transform);
            
            return new PathParams(startSurface, targetSurface);
        }

        private Tile.Surface GetSurface(Transform @object)//рпи условии что обект размерами (1,1,1)
        {
            if (_findPathProject.Tiles.TryGetValue(Vector3Int.RoundToInt(@object.position) + Vector3Int.down, out var tile))
            {
                if (tile.Surfaces.TryGetValue(Vector3Int.up, out var surface))
                {
                    return surface;
                }
            }
            return null;
        }
    }
}
