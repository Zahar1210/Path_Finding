using System;
using UnityEngine;

namespace FindPath
{
    public class AAAgent : MonoBehaviour
    {
        [SerializeField] private LayerMask layerMask;
        public FindMode findMode;
        private Camera _camera;
        private FindPathProject _findPathProject;
        
        private void Start()
        {
            _findPathProject = FindPathProject.Instance;
            _camera = Camera.main;
        }

        void Update()
        {
            if (Input.GetMouseButtonUp(0))
            {
                if (_findPathProject.Path != null)
                {
                    Array.Clear(_findPathProject.Path, 0, _findPathProject.Path.Length);
                }
        
                Surface targetSurface = GetTargetSurface();
        
                Surface currentSurface = GetCurrentSurface();
        
                if (targetSurface == null)
                {
                    return;
                }
        
                _findPathProject.Path = PathFinding.GetPath(currentSurface, targetSurface, _findPathProject, findMode);
            }
        }

        private Surface GetCurrentSurface()
        {
            if (_findPathProject.GridObjects.TryGetValue(Vector3Int.RoundToInt(transform.position) + Vector3Int.down, out var tile))
            {
                if (tile.Surfaces.TryGetValue(Vector3Int.up, out var surface))
                {
                    return surface;
                }
            }

            return null;
        }

        private Surface GetTargetSurface()
        {
            if (Physics.Raycast(_camera.ScreenPointToRay(Input.mousePosition), out RaycastHit hit))
            {
                if (hit.collider.TryGetComponent(out CubeGridObject tile))
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