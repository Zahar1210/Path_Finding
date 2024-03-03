using System;
using System.Linq;
using UnityEngine;

namespace FindPath
{
    public class GetPath : MonoBehaviour
    {
        [SerializeField] private FindPathProject findPathProject;
        private Camera _camera;

        private void Start()
        {
            _camera = Camera.main;
        }

        void Update()
        {
            if (Input.GetMouseButtonUp(0))
            {
                if (findPathProject._path != null)
                {
                    Array.Clear(findPathProject._path, 0, findPathProject._path.Length);
                }
                
                Tile.Surface targetSurface = GetTargetSurface();
                Tile.Surface currentSurface = GetCurrentSurface();
                if (targetSurface == null)
                    return;
                
                findPathProject._path = 
                    PathFinding.GetPath(currentSurface, targetSurface, findPathProject, findPathProject.findMode);
            }
        }

        private Tile.Surface GetCurrentSurface()
        {
            if (findPathProject._tiles.TryGetValue(Vector3Int.RoundToInt(transform.position) + Vector3Int.down, out var tile))
            {
                if (tile._surfaces.TryGetValue(Vector3Int.up, out var surface))
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
                    if (tile._surfaces.TryGetValue(direction, out var surface))
                    {
                        return surface;
                    }
                }
            }

            return null;
        }
    }
}