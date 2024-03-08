using System;
using UnityEngine;

namespace FindPath
{
    public class GetPath : MonoBehaviour
    {
        [SerializeField] private FindPathProject findPathProject;
        [SerializeField] private LayerMask layerMask;
        private Camera _camera;

        private void Start()
        {
            _camera = Camera.main;
        }

        void Update()
        {
            if (Input.GetMouseButtonUp(0))
            {
                Debug.Log("попытка");
                if (findPathProject.Path != null)
                {
                    Array.Clear(findPathProject.Path, 0, findPathProject.Path.Length);
                }

                Tile.Surface targetSurface = GetTargetSurface();
                Debug.Log("попытка");

                Tile.Surface currentSurface = GetCurrentSurface();

                if (targetSurface == null)
                {
                    Debug.Log("не повезло");
                    return;
                }

                findPathProject.Path = PathFinding.GetPath(currentSurface, targetSurface, findPathProject, findPathProject.findMode);
            }
        }

        private Tile.Surface GetCurrentSurface()
        {
            if (findPathProject.Tiles.TryGetValue(Vector3Int.RoundToInt(transform.position) + Vector3Int.down, out var tile))
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