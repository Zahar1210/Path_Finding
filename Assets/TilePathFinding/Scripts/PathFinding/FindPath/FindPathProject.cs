using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace FindPath
{
    public class FindPathProject : MonoBehaviour
    {
        public static FindPathProject Instance { get; private set; } //singleton
        public int TileSize => _tileSize; //property for use in other classes
        public Directions Directions => _directions; //property for use in other classes
        
        public readonly Dictionary<Vector3Int, GridObject> GridObjects = new();//this is where all the tiles are stored 

        [SerializeField] [Range(1f, 3f)] private int _tileSize;
        [SerializeField] private Directions _directions;
        
        private FindPathGizmos _findPathGizmos;
        public Color ObstacleColor;
        public Color NoObstacleColor;
        public Color PathColor;
        

        #region Initialize

        public void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                Directions.Initialize();//точка входа  
                return;
            }

            Destroy(gameObject);
        }

        private void Start()
        {
            _findPathGizmos = FindPathGizmos.Instance;
            
            AddTiles();
            FindObstacleObjects();
            SeekerInitialize();
        }

        private void SeekerInitialize()
        {
            Seeker[] seekers = FindObjectsOfType<Seeker>();
            if (seekers.Length > 0)
            {
                foreach (var seeker in seekers)
                {
                    seeker.Initialize();
                }
            }
        }

        private void FindObstacleObjects()
        {
            foreach (var obstacle in FindObjectsOfType<ObstacleObject>())
            {
                obstacle.Initialize();
            }
        }
        
        private void AddTiles()
        {
            //here the "Tiles" dictionary is filled in and the found tiles are initialized
            GridObject[] gridObjects = FindObjectsOfType<CubeGridObject>();
            foreach (var gridObject in gridObjects)
            {
                Vector3Int tilePosition = Vector3Int.RoundToInt(gridObject.transform.position);
                if (!GridObjects.ContainsKey(tilePosition))
                {
                    gridObject.Position = tilePosition;
                    GridObjects.Add(tilePosition, gridObject);
                }
            }

            foreach (var tile in gridObjects)
            {
                tile.SetSurface(this);
            }
            
            _findPathGizmos.Initialize(gridObjects.ToList());
        }

        #endregion

        #region MenuFunctions
        
        [MenuItem("PathFinding/Utility Functions/Set Position All")]
        
        
        public static void ArrangeAll()
        {
            CubeGridObject[] tiles = FindObjectsOfType<CubeGridObject>();
            if (tiles.Length > 0)
            {
                foreach (var tile in tiles)
                {
                    Vector3 tilePos = tile.transform.position;

                    Vector3Int roundedPosition = new Vector3Int
                    (
                        Mathf.RoundToInt(tilePos.x),
                        Mathf.RoundToInt(tilePos.y),
                        Mathf.RoundToInt(tilePos.z)
                    );

                    tile.transform.position = roundedPosition;
                }
            }
            else
            {
                throw new ArgumentException($"Tiles are not found");
            }
        }

        [MenuItem("PathFinding/Utility Functions/Set Component All")]
        public static void GetComponentAll()
        {
            foreach (var item in FindObjectsOfType<GameObject>())
            {
                CubeGridObject cubeGridObject = item.gameObject.GetComponent<CubeGridObject>();

                if (item.gameObject.layer == 3 && !cubeGridObject)
                {
                    item.gameObject.AddComponent<CubeGridObject>();
                }
            }
        }

        [MenuItem("PathFinding/Utility Functions/Set Tile Surfaces All")]
        public static void SetTileSurfacesAll()
        {
            CubeGridObject[] tiles = FindObjectsOfType<CubeGridObject>();
            if (tiles.Length > 0)
            {
                int size = FindObjectOfType<FindPathProject>()._tileSize;
                
                foreach (var tile in tiles)
                {
                    if (tile._surfaces == null || tile._surfaces.Length == 0)
                    {
                        HashSet<Surface> surfaces = new();
                    
                        surfaces.Add(new Surface(new Vector3Int(size, 0, 0)));
                        surfaces.Add(new Surface(new Vector3Int(-size, 0, 0)));
                        surfaces.Add(new Surface(new Vector3Int(0, size, 0)));
                        surfaces.Add(new Surface(new Vector3Int(0, -size, 0)));
                        surfaces.Add(new Surface(new Vector3Int(0, 0, size)));
                        surfaces.Add(new Surface(new Vector3Int(0, 0, -size)));
                
                        tile._surfaces = surfaces.ToArray();
                    }
                }
            }
            else
            {
                throw new ArgumentException($"Tiles without surfaces are not found");
            }
        }

        [MenuItem("PathFinding/Open Documentation")]
        public static void OpenDocumentation()
        {
            throw new ArgumentException($"No documentation yet ");
        }

        #endregion//это в Editor
    }

    // #region Inspector
    //
    // [CustomPropertyDrawer(typeof(BigHeaderAttribute))]
    // public class BigHeaderAttributeDrawer : DecoratorDrawer
    // {
    //     public override void OnGUI(Rect position)
    //     {
    //         BigHeaderAttribute attributeHandle = (BigHeaderAttribute)attribute;
    //         position.yMin += EditorGUIUtility.singleLineHeight * 0.5f;
    //         position = EditorGUI.IndentedRect(position);
    //
    //         GUIStyle headerTextStyle = new GUIStyle()
    //         {
    //             fontSize = 15,
    //             fontStyle = FontStyle.Normal,
    //             alignment = TextAnchor.MiddleLeft
    //         };
    //
    //         headerTextStyle.normal.textColor = Color.cyan;
    //         GUI.Label(position, attributeHandle.Text, headerTextStyle);
    //     }
    //
    //     public override float GetHeight()
    //     {
    //         return EditorGUIUtility.singleLineHeight * 1.8f;
    //     }
    // }
    //
    // [AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
    // public class BigHeaderAttribute : PropertyAttribute
    // {
    //     public string Text { get; }
    //
    //     public BigHeaderAttribute(string text)
    //     {
    //         Text = text;
    //     }
    // }
    //
    // #endregion
}