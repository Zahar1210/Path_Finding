using System;
using UnityEngine;

namespace FindPath
{
    public class ObstacleObject : MonoBehaviour
    {
        [SerializeField] private Dimensions dimensions;
        [SerializeField] private float checkDistance;
        [SerializeField] private Tile tile;
        private FindPathProject _findPathProject;
        private Transform _transform;
        private Bounds _cubeBounds;
        private Bounds _objectBounds;
        private Vector3 Gtile;
        private Vector3 size;

        private void Start()
        {
            Renderer cubeRenderer = transform.GetComponent<Renderer>();
            if (cubeRenderer != null)
            {
                _cubeBounds = cubeRenderer.bounds;
                _objectBounds = _cubeBounds;
            }
            _transform = transform;
            _findPathProject = FindPathProject.Instance;
        }
        
        private Bounds CalculateBoundsWithRotation()
        {
            MeshFilter meshFilter = transform.GetComponent<MeshFilter>();
            if (meshFilter == null || meshFilter.sharedMesh == null)
            {
                Debug.LogError("Mesh filter or mesh not found.");
                return new Bounds();
            }

            Mesh mesh = meshFilter.sharedMesh;
            Vector3[] vertices = mesh.vertices;

            // Получаем матрицу преобразования объекта
            Matrix4x4 localToWorldMatrix = transform.localToWorldMatrix;

            // Инициализируем границы с первой вершиной
            Vector3 firstVertex = localToWorldMatrix.MultiplyPoint3x4(vertices[0]);
            Bounds bounds = new Bounds(firstVertex, Vector3.zero);

            // Обходим все вершины и обновляем границы
            foreach (Vector3 vertex in vertices)
            {
                Vector3 transformedVertex = localToWorldMatrix.MultiplyPoint3x4(vertex);
                bounds.Encapsulate(transformedVertex);
            }

            return bounds;
        }

        private void FixedUpdate()
        {
            Bounds bounds = CalculateBoundsWithRotation();

            size.x = bounds.size.x * dimensions.ValueX;
            size.y =  bounds.size.y * dimensions.ValueY;
            size.z =  bounds.size.z * dimensions.ValueZ;
            
            bounds.Expand(size);
            if (bounds.Contains(tile.transform.position))
            {
                Gtile = tile.transform.position;
            }
            else
            {
                Gtile = Vector3.zero;
            }
        }

        private void OnDrawGizmos()
        {
            if (size != null)
            {
                Gizmos.color = new Color(0.1f, 1f, 0f, 0.3f);
                Gizmos.DrawCube(transform.position, size);
            }
            
            if (Gtile != Vector3.zero)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawCube(Gtile, new Vector3(1,1,1));
            }
        }
    }

    [Serializable]
    public class Dimensions
    {
        [SerializeField] [Range(1f, 2f)] private float valueX;
        [SerializeField] [Range(1f, 2f)] private float valueY;
        [SerializeField] [Range(1f, 2f)] private float valueZ;
        public float ValueX => valueX;
        public float ValueY => valueY;
        public float ValueZ => valueZ;
    }
}
