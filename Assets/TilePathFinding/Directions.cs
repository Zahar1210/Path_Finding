using System;
using System.Collections.Generic;
using FindPath;
using UnityEngine;

[CreateAssetMenu(menuName = "NewInfo/ DirectionInfo", fileName = "Directions")]
public class Directions : ScriptableObject
{
    public Dir dir { get; set; }
    public DirGroup dirGroup { get; set; }
    
    public Dictionary<Vector3Int, DirectionArrayPair> DirDictionary = new();
    private static int TileSize => FindPathProject.Instance.TileSize;

    private void OnEnable()
    { 
        DirDictionary.Add(Vector3Int.right, new DirectionArrayPair()
        {
            DirectionArray = dirGroup.dirHorizontal,
            Directions = dir.dirRight
        } );
        
        DirDictionary.Add(Vector3Int.left,new DirectionArrayPair()
        {
            DirectionArray = dirGroup.dirHorizontal,
            Directions = dir.dirLeft
        } );
        
        DirDictionary.Add(Vector3Int.up, new DirectionArrayPair()
        {
            DirectionArray = dirGroup.dirVertical,
            Directions = dir.dirUp
        } );
        
        DirDictionary.Add(Vector3Int.down,new DirectionArrayPair()
        {
            DirectionArray = dirGroup.dirVertical,
            Directions = dir.dirDown
        } );
        
        DirDictionary.Add(Vector3Int.forward,new DirectionArrayPair()
        {
            DirectionArray = dirGroup.dirDepth,
            Directions = dir.dirFront
        } );
        
        DirDictionary.Add(Vector3Int.back,new DirectionArrayPair()
        {
            DirectionArray = dirGroup.dirDepth,
            Directions = dir.dirBack
        } );
    }

    [Serializable]
    public class Dir
    {
        public Vector3Int[] dirRight => new Vector3Int[]
        {
            new Vector3Int(TileSize, TileSize, 0), new Vector3Int(TileSize, 0, TileSize),
            new Vector3Int(TileSize, -TileSize, 0), new Vector3Int(TileSize, 0, -TileSize)
        };
        
        public Vector3Int[] dirLeft => new Vector3Int[]
        {
            new Vector3Int(-TileSize, TileSize, 0), new Vector3Int(-TileSize, -TileSize, 0),
            new Vector3Int(-TileSize, 0, TileSize), new Vector3Int(-TileSize, 0, -TileSize)
        };
        
        public Vector3Int[] dirUp => new Vector3Int[] 
        {
            new Vector3Int(TileSize, TileSize, 0), new Vector3Int(-TileSize, TileSize, 0),
            new Vector3Int(0, TileSize, TileSize), new Vector3Int(0, TileSize, -TileSize)
        };
        
        public Vector3Int[] dirDown => new Vector3Int[] 
        {
            new Vector3Int(TileSize, -TileSize, 0), new Vector3Int(-TileSize, -TileSize, 0),
            new Vector3Int(0, -TileSize, TileSize), new Vector3Int(0, -TileSize, -TileSize)
        };
        
        public Vector3Int[] dirFront => new Vector3Int[] 
        {
            new Vector3Int(TileSize, 0, TileSize), new Vector3Int(-TileSize, 0, TileSize),
            new Vector3Int(0, TileSize, TileSize), new Vector3Int(0, -TileSize, TileSize)
        };
        
        public Vector3Int[] dirBack => new Vector3Int[] 
        {
            new Vector3Int(TileSize, 0, -TileSize), new Vector3Int(-TileSize, 0, -TileSize),
            new Vector3Int(0, TileSize, -TileSize), new Vector3Int(0, -TileSize, -TileSize)
        };
    }
    
    [Serializable]
    public class DirGroup
    {
        public Vector3Int[] dirHorizontal => new Vector3Int[]
        {
            new Vector3Int(0, 0, TileSize), new Vector3Int(0, 0, -TileSize),
            new Vector3Int(0, TileSize, 0), new Vector3Int(0, -TileSize, 0),
        };
        
        public Vector3Int[] dirVertical => new Vector3Int[] 
        {
            new Vector3Int(0, 0, -TileSize), new Vector3Int(0, 0, TileSize),
            new Vector3Int(TileSize, 0, 0), new Vector3Int(-TileSize, 0, 0),
        };
        
        public Vector3Int[] dirDepth => new Vector3Int[] 
        {
            new Vector3Int(TileSize, 0, 0), new Vector3Int(-TileSize, 0, 0),
            new Vector3Int(0, TileSize, 0), new Vector3Int(0, -TileSize, 0),
        };
   
        public Vector3Int[] directions => new Vector3Int[]
        {
            new Vector3Int(TileSize, 0, 0), new Vector3Int(-TileSize, 0, 0),
            new Vector3Int(0, TileSize, 0), new Vector3Int(0, -TileSize, 0),
            new Vector3Int(0, 0, TileSize), new Vector3Int(0, 0, -TileSize)
        };
    }
    
    public struct DirectionArrayPair
    {
        public Vector3Int[] DirectionArray;
        public Vector3Int[] Directions;
    }
}
