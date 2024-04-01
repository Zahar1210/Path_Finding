using UnityEngine;

namespace FindPath
{
    public class TargetPositionTrigger : FindPathTrigger
    {
        private Tile.Surface _currentSurfaceTarget;
        private readonly FindPathProject _findPathProject;
        private Vector3Int _pastTargetPosition;

        public TargetPositionTrigger(Seeker seeker)
        {
            _findPathProject = FindPathProject.Instance;
        }

        public override Tile.Surface GetTargetSurface(Seeker seeker)
        {
            seeker.FindTargetType.GetTargetObject(seeker);
            
            Vector3Int targetPos = Vector3Int.RoundToInt(seeker.SeekerTarget.transform.position);

            if (VectorsAreDifferent(targetPos, _pastTargetPosition)) //если прошлая позиция не равна текущей 
            {
                _pastTargetPosition = targetPos;
                TargetDirection targetDirection = seeker.TargetDirection;

                return SurfaceFinder.GetSurface(_pastTargetPosition, targetDirection, seeker.Count, _findPathProject,
                    seeker.SeekerTarget);
            }

            return null;
        }

        private bool VectorsAreDifferent(Vector3Int currentPosition, Vector3Int pastPosition)
        {
            return currentPosition.x != pastPosition.x || currentPosition.y != pastPosition.y || currentPosition.z != pastPosition.z;
        }

        public override PathParams GetPathParams()
        {
            throw new System.NotImplementedException();
        }
    }
}