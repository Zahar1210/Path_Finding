using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace FindPath
{
    // служит только примером 
    public class AAAgent : MonoBehaviour
    {
        public PathFindMode FindMode;
        private Camera _camera;
        private FindPathProject _findPathProject;
        
        private void Start()
        {
            _findPathProject = FindPathProject.Instance;
            _camera = Camera.main;
        }

        void Update()
        {
            // if (Input.GetMouseButtonUp(0))
            // {
            //     if (_findPathProject.Path != null)
            //     {
            //         Array.Clear(_findPathProject.Path, 0, _findPathProject.Path.Length);
            //     }
            //
            //     Surface targetSurface = GetTargetSurface();
            //
            //     Surface currentSurface = GetCurrentSurface();
            //
            //     if (targetSurface == null)
            //     {
            //         return;
            //     }
            //
            //     //TODO исправить ошибку чтобы метод ниже можно было нормально вызывать 
            //     _findPathProject.Path = PathFinding.GetPath(currentSurface, targetSurface, _findPathProject, FindMode);
            // }
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