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
        public static FindPathProject Instance { get; set; }
        public Directions Directions => directions;
        public int TileSize => tileSize;
        public Tile.Surface[] Path { get; set; }
        
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

        
        [MenuItem("PathFinding/ArrangeAll")]
        public static void ArrangeAll()
        {
            List<Tile> tiles = new();
            foreach (var tile in FindObjectsOfType<Tile>())
            {
                tiles.Add(tile);
            }
            Debug.Log(tiles.Count);
        }
        
        [MenuItem("PathFinding/GetComponentAll")]
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