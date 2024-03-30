using FindPath;
using UnityEngine;

namespace FindPath
{
    public class MouseInputTrigger : FindPathTrigger
    {
        private readonly int _mouseSide;
        private Tile.Surface _currentTargetSurface;
        private readonly LayerMask _layerMask;
        private readonly Camera _camera;

        public MouseInputTrigger(int mouseSide, LayerMask layerMask, Camera camera, Tile.Surface currentTargetSurface)
        {
            _currentTargetSurface = currentTargetSurface;
            _layerMask = layerMask;
            _camera = camera;
            _mouseSide = mouseSide;
        }

        public override Tile.Surface CheckEvent()
        {
            if (Input.GetMouseButtonUp(_mouseSide))
            {
                if (Physics.Raycast(_camera.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, _layerMask))
                {
                    if (hit.collider.TryGetComponent(out Tile tile))
                    {
                        Vector3 hitOffset = hit.point - tile.transform.position;
                        Vector3Int direction = Vector3Int.RoundToInt(hitOffset.normalized);

                        if ((tile.Surfaces.TryGetValue(direction, out var surface) && _currentTargetSurface != surface) 
                            || _currentTargetSurface == null)
                        {
                            _currentTargetSurface = surface;
                            return surface;
                        }
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