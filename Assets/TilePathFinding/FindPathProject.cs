using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace FindPath
{
    public class FindPathProject : MonoBehaviour
    {
        #region Variables

        public Dictionary<Vector3Int, Tile> Tiles = new();
        public Tile.Surface[] Path { get; set; }
        public static FindPathProject Instance { get; set; }
        public int TileSize => tileSize;
        public Directions Directions => directions;
        
        [BigHeader("Path Finding Parameters")] 
        [SerializeField] [Range(1f, 3f)] private int tileSize;
        [SerializeField] private Directions directions;
        
        [BigHeader("Path Find Mode")] 
        public FindMode findMode;

        [BigHeader("Gizmos")]
        [SerializeField] private PathGizmos pathGizmos;
        
        #endregion
        
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                return;
            }
        
            Destroy(gameObject);
        }
        
        private void Start()
        {
            AddTiles();
            pathGizmos.SetTiles(Tiles.Values.ToList());
        }
        
        private void AddTiles()
        {
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
                
                    Vector3Int roundedPosition = new Vector3Int(
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
        public static void SetTileSurfaces()
        { 
            Tile[] tiles = FindObjectsOfType<Tile>();
            if (tiles.Length > 0)
            {
                int size = FindObjectOfType<FindPathProject>().tileSize;

                foreach (var tile in tiles)
                {
                    if (tile.surfacesArray.Length == 0)
                    {
                        HashSet<Vector3Int> surfaces = new HashSet<Vector3Int>();
                    
                        surfaces.Add(new Vector3Int(size, 0, 0));
                        surfaces.Add(new Vector3Int(-size, 0, 0));
                    
                        surfaces.Add(new Vector3Int(0, size, 0));
                        surfaces.Add(new Vector3Int(0, -size, 0));
                    
                        surfaces.Add(new Vector3Int(0, 0, size));
                        surfaces.Add(new Vector3Int(0, 0, -size));
            
                        tile.surfacesArray = surfaces.ToArray();
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

    public enum FindMode
    {
        BreadthFirstSearch, 
        AStar
    }
    
    
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
    public class BigHeaderAttribute : PropertyAttribute
    {
        public string Text { get; set; }
        
        public BigHeaderAttribute(string text)
        {
            Text = text;
        }
    }
    
    
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
}