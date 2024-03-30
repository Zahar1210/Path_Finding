using UnityEngine;

namespace FindPath
{
    public class TargetPositionTrigger : FindPathTrigger
    {
        private readonly Seeker _seeker;
        private Tile.Surface _currentSurfaceTarget;
        private Vector3Int _pastTargetPosition;
        private FindPathProject _findPathProject;

        public TargetPositionTrigger(Seeker seeker)
        {
            _findPathProject = FindPathProject.Instance;
            _seeker = seeker;
        }

        public override Tile.Surface CheckEvent()
        {
            Vector3Int targetPos = Vector3Int.RoundToInt(_seeker.SeekerTarget.transform.position);

            if (VectorsAreDifferent(targetPos, _pastTargetPosition)) //если прошлая позиция не равна текущей 
            {
                _pastTargetPosition = targetPos;
                TargetDirection targetDirection = _seeker.TargetDirection;

                return SurfaceFinder.GetSurface(_pastTargetPosition, targetDirection, _seeker.Count, _findPathProject,
                    _seeker.SeekerTarget);
            }

            return null;
        }

        private bool VectorsAreDifferent(Vector3Int currentPosition, Vector3Int pastPosition)
        {
            return currentPosition.x != pastPosition.x || currentPosition.y != pastPosition.y ||
                   currentPosition.z != pastPosition.z;
        }

        public override PathParams GetPathParams()
        {
            throw new System.NotImplementedException();
        }
    }
}