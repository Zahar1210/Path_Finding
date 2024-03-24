using UnityEngine;
using Input = UnityEngine.Input;

namespace FindPath
{
    public class MouseInputSeeker : SeekerAbstract
    {
        private readonly FindPathProject _findPathProject;
        private readonly Camera _camera;

        public MouseInputSeeker(FindPathProject findPathProject, Camera camera)
        {
            _camera = camera;
            _findPathProject = findPathProject;
        }
        
        public override bool CheckEvent(Seeker seeker)
        {
            if (Input.GetMouseButtonUp(seeker.mouseSide))
            {
                return true;
            }
            
            return false;
        }
        
        public override PathParams GetPathParams(Seeker seeker)
        {
            Tile.Surface startSurface = GetCurrentSurface(seeker);
            Tile.Surface targetSurface = GetTargetSurface();
            return new PathParams(startSurface, targetSurface);
        }

        private Tile.Surface GetCurrentSurface(Seeker seeker)
        {
            if (_findPathProject.Tiles.TryGetValue(Vector3Int.RoundToInt(seeker.transform.position) + Vector3Int.down, out var tile))
            {
                if (tile.Surfaces.TryGetValue(Vector3Int.up, out var surface))
                {
                    return surface;
                }
            }
            return null;
        }

        private Tile.Surface GetTargetSurface()
        {
            if (Physics.Raycast(_camera.ScreenPointToRay(Input.mousePosition), out RaycastHit hit))
            {
                if (hit.collider.TryGetComponent(out Tile tile))
                {
                    Vector3 hitOffset = hit.point - tile.transform.position;
                    Vector3Int direction = Vector3Int.RoundToInt(hitOffset.normalized);
                    
                    if (tile.Surfaces.TryGetValue(direction, out var surface))
                    {
                        return surface;
                    }
                }
            }

            return null;
        }
    }
}