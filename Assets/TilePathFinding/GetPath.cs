using System;
using UnityEngine;

public class GetPath : MonoBehaviour
{
    [SerializeField] private PathFinding pathFinding;
    private Camera _camera;


    private void Start()
    {
        _camera = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            Tile.Surface targetPosition = GetTargetSurface();
            Tile.Surface currentSurface = GetCurrentSurface();
            if (targetPosition == null) { return; }
            Tile.Surface[] path = pathFinding.GetPath(currentSurface, targetPosition);
            foreach (var surface in path)
            {
                // surface.InPath = true;
            }
        }
    }

    private Tile.Surface GetCurrentSurface()
    {
        if (pathFinding._tiles.TryGetValue(Vector3Int.RoundToInt(transform.position) + Vector3Int.down, out var tile))
        {
            if (tile._surfaces.TryGetValue(Vector3Int.up, out var surface))
            {
                surface.InPath = true;
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
                    surface.InPath = true;
                    return surface;
                }
            }
        }
        return null;
    }
}
