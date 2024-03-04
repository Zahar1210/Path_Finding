using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace FindPath
{
    public class FindPathProject : MonoBehaviour
    {
        public static FindPathProject Instance { get; set; }
        public Directions Directions => directions;
        public Dictionary<Vector3Int, Tile> Tiles = new();
        public int TileSize => tileSize;
        public Tile.Surface[] Path { get; set; }
        
        [BigHeader("Path Finding Parameters")] 
        [SerializeField] [Range(1f, 3f)] private int tileSize;
        
        [BigHeader("Path Find Mode")] 
        public FindMode findMode;

        [BigHeader("Gizmos")]
        [SerializeField] private PathGizmos pathGizmos;
        [SerializeField] private Directions directions;


        private static void GetSize()
        {
        }
        
        private void Awake()
        {
            // Directions.TileSize = TileSize;

            if (Instance == null)
            {
                Instance = this;
                return;
            }
        
            Destroy(gameObject);
        }

        private void Start()
        {
            Debug.Log(Directions.TileSize);
            Debug.Log(directions.dir.dirDown[1]);
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
    }

    public enum FindMode
    {
        BreadthFirstSearch, 
        AStar
    }
    
    
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
    public class BigHeaderAttribute : PropertyAttribute
    {
        public string Text => _mText;
        private string _mText = String.Empty;

        public BigHeaderAttribute(string text)
        {
            _mText = text;
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