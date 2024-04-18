using UnityEngine;

namespace FindPath
{
    public class MouseInputTrigger : FindPathTrigger
    {
        private Surface _currentTargetSurface;
        private readonly Camera _camera;

        public MouseInputTrigger(Camera camera)
        {
            _camera = camera;
        }

        public override Surface TryGetTargetSurface(Seeker seeker)
        {
            if (Input.GetMouseButtonUp(seeker.MouseSide))
            {
                if (Physics.Raycast(_camera.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, seeker.MouseLayerMask))
                {
                    if (hit.collider.TryGetComponent(out CubeGridObject tile))
                    {
                        Vector3 hitOffset = hit.point - tile.transform.position;
                        Vector3Int direction = Vector3Int.RoundToInt(hitOffset.normalized);

                        if ((tile.Surfaces.TryGetValue(direction, out var surface) 
                             && _currentTargetSurface != surface) || _currentTargetSurface == null)
                        {
                            _currentTargetSurface = surface;
                            return surface;
                        }
                    }
                }
            }

            return null;
        }

        public override Surface TryGetCurrentSurface(Seeker seeker)
        {
            throw new System.NotImplementedException();
        }
    }
}