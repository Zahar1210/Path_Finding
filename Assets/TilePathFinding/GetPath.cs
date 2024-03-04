using System;
using UnityEditor;
using UnityEngine;

namespace FindPath
{
    public class GetPath : MonoBehaviour
    {
        [SerializeField] private FindPathProject findPathProject;
        
        private Camera _camera;
        private GUIStyle helperTextStyle;
        
        private void Start()
        {
            helperTextStyle = new GUIStyle()
            {
                fontSize = 8,
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.MiddleCenter
            };
            
            helperTextStyle.normal.textColor = Color.blue;

            _camera = Camera.main;
        }

        void Update()
        {
            if (Input.GetMouseButtonUp(0))
            {
                if (findPathProject.Path != null)
                {
                    Array.Clear(findPathProject.Path, 0, findPathProject.Path.Length);
                }
                
                Tile.Surface targetSurface = GetTargetSurface();
                Tile.Surface currentSurface = GetCurrentSurface();
                
                if (targetSurface == null)
                    return;
                
                findPathProject.Path = PathFinding.GetPath(currentSurface,
                    targetSurface, findPathProject, findPathProject.findMode);
            }
        }

        private Tile.Surface GetCurrentSurface()
        {
            if (findPathProject.Tiles.TryGetValue(Vector3Int.RoundToInt(transform.position) + Vector3Int.down, out var tile))
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
        
#if UNITY_EDITOR
        
        private void OnDrawGizmos()
        {
            if (findPathProject.Path == null || findPathProject.Path.Length <= 1) 
                return;
            
            foreach (var s in findPathProject.Path)
            {
                if (s != null)
                {
                    Vector3 drawPos = s.Tile.Position + s.Direction;
                    Handles.Label(drawPos, Array.IndexOf(findPathProject.Path, s).ToString(), helperTextStyle);
                }
            }
        }
#endif
    }
}