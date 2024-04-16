using UnityEngine;

namespace FindPath
{
    // в движкние за мышкой 
    public class MousePositionTrigger : FindPathTrigger
    {
        private Surface _currentTargetSurface;
        private readonly Camera _camera;
    
        public MousePositionTrigger(Camera camera)
        {
            _camera = camera;
        }
    
        public override Surface TryGetTargetSurface(Seeker seeker)
        {
            if (Physics.Raycast(_camera.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, seeker.MouseLayerMask))
            {
                if (hit.collider.TryGetComponent(out CubeGridObject tile))
                {
                    Vector3 hitOffset = hit.point - tile.transform.position;
                    Vector3Int direction = Vector3Int.RoundToInt(hitOffset.normalized);
                
                    if ((tile.Surfaces.TryGetValue(direction, out var surface) && _currentTargetSurface != surface) || _currentTargetSurface == null)
                    {
                        _currentTargetSurface = surface;
                        return surface;
                    }
                }
            }
            return null;
        }
    
        public override PathParams GetPathParams()
        {
            throw new System.NotImplementedException();
        }
    }
}
