using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace FindPath
{
    public class FindPathProject : MonoBehaviour
    {
        public Tile.Surface[] Path { get; set; } //this is where all the paths that have been traveled
        public static FindPathProject Instance { get; set; }//singleton
        public int TileSize => tileSize; //property for use in other classes
        public Directions Directions => directions; //property for use in other classes
        
        public Dictionary<Vector3Int, Tile> Tiles = new();//this is where all the tiles are stored 

        [BigHeader("Path Finding Parameters")]
        [SerializeField] [Range(1f, 3f)] private int tileSize;
        [SerializeField] private Directions directions;

        [BigHeader("Path Find Mode")] 
        public FindMode findMode;

        [BigHeader("Gizmos")]
        [SerializeField] private PathGizmos pathGizmos;


        #region Initialize

        public void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                Directions.Initialize();
                return;
            }

            Destroy(gameObject);
        }

        private void Start()
        {
            AddTiles();
        }

        private void AddTiles()
        {
            //here the "Tiles" dictionary is filled in and the found tiles are initialized
            
            Tile[] tiles = FindObjectsOfType<Tile>();
            foreach (var tile in tiles)
            {
                Vector3Int tilePosition = Vector3Int.RoundToInt(tile.transform.position);
                if (!Tiles.ContainsKey(tilePosition))
                {
                    tile.Position = tilePosition;
                    Tiles.Add(tilePosition, tile);
                }
            }

            foreach (var tile in tiles)
            {
                tile.SetSurface(this);
            }
        }

        #endregion

        #region MenuFunctions
        
        [MenuItem("PathFinding/Utility Functions/Set Position All")]
        
        
        public static void ArrangeAll()
        {
            Tile[] tiles = FindObjectsOfType<Tile>();
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
                Tile tile = item.gameObject.GetComponent<Tile>();

                if (item.gameObject.layer == 3 && !tile)
                {
                    item.gameObject.AddComponent<Tile>();
                }
            }
        }

        [MenuItem("PathFinding/Utility Functions/Set Tile Surfaces All")]
        public static void SetTileSurfacesAll()
        {
            Tile[] tiles = FindObjectsOfType<Tile>();
            if (tiles.Length > 0)
            {
                int size = FindObjectOfType<FindPathProject>().tileSize;
                
                foreach (var tile in tiles)
                {
                    if (tile._surfaces == null || tile._surfaces.Length == 0)
                    {
                        HashSet<Tile.Surface> surfaces = new();
                    
                        surfaces.Add(new Tile.Surface(new Vector3Int(size, 0, 0)));
                        surfaces.Add(new Tile.Surface(new Vector3Int(-size, 0, 0)));
                        surfaces.Add(new Tile.Surface(new Vector3Int(0, size, 0)));
                        surfaces.Add(new Tile.Surface(new Vector3Int(0, -size, 0)));
                        surfaces.Add(new Tile.Surface(new Vector3Int(0, 0, size)));
                        surfaces.Add(new Tile.Surface(new Vector3Int(0, 0, -size)));
                
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

        #endregion
    }

    #region Mode

    public enum FindMode
    {
        BreadthFirstSearch,
        AStar
    }

    #endregion

    #region Inspector

    [CustomPropertyDrawer(typeof(BigHeaderAttribute))]
    public class BigHeaderAttributeDrawer : DecoratorDrawer
    {
        public override void OnGUI(Rect position)
        {
            BigHeaderAttribute attributeHandle = (BigHeaderAttribute)attribute;
            position.yMin += EditorGUIUtility.singleLineHeight * 0.5f;
            position = EditorGUI.IndentedRect(position);

            GUIStyle headerTextStyle = new GUIStyle()
            {
                fontSize = 15,
                fontStyle = FontStyle.Normal,
                alignment = TextAnchor.MiddleLeft
            };

            headerTextStyle.normal.textColor = Color.cyan;
            GUI.Label(position, attributeHandle.Text, headerTextStyle);
        }

        public override float GetHeight()
        {
            return EditorGUIUtility.singleLineHeight * 1.8f;
        }
    }

    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
    public class BigHeaderAttribute : PropertyAttribute
    {
        public string Text { get; }

        public BigHeaderAttribute(string text)
        {
            Text = text;
        }
    }

    #endregion
}