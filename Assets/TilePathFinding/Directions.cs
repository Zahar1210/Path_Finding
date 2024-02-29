using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "NewInfo/ DirectionInfo", fileName = "Directions")]
public class Directions : ScriptableObject
{
    public Dir dir;
    public DirGroup dirGroup;
    
    public Dictionary<Vector3Int, DirectionArrayPair> dirDictionary = new();

    private void OnEnable()
    {
        dirDictionary.Add(Vector3Int.right, new DirectionArrayPair()
        {
            DirectionArray = dirGroup.dirHorizontal,
            Directions = dir.dirRight
        } );
        
        dirDictionary.Add(Vector3Int.left,new DirectionArrayPair()
        {
            DirectionArray = dirGroup.dirHorizontal,
            Directions = dir.dirLeft
        } );
        
        dirDictionary.Add(Vector3Int.up, new DirectionArrayPair()
        {
            DirectionArray = dirGroup.dirVertical,
            Directions = dir.dirUp
        } );
        
        dirDictionary.Add(Vector3Int.down,new DirectionArrayPair()
        {
            DirectionArray = dirGroup.dirVertical,
            Directions = dir.dirDown
        } );
        
        dirDictionary.Add(Vector3Int.forward,new DirectionArrayPair()
        {
            DirectionArray = dirGroup.dirDepth,
            Directions = dir.dirFront
        } );
        
        dirDictionary.Add(Vector3Int.back,new DirectionArrayPair()
        {
            DirectionArray = dirGroup.dirDepth,
            Directions = dir.dirBack
        } );
    }

    [Serializable]
    public class Dir
    {
        public Vector3Int[] dirRight = 
        {
            new Vector3Int(1, 1, 0), new Vector3Int(1, 0, 1), new Vector3Int(1, -1, 0), new Vector3Int(1, 0, -1),
        };
        
        public Vector3Int[] dirLeft = 
        {
            new Vector3Int(-1, 1, 0), new Vector3Int(-1, -1, 0), new Vector3Int(-1, 0, 1), new Vector3Int(-1, 0, -1),
        };
        
        public Vector3Int[] dirUp = 
        {
            new Vector3Int(1, 1, 0), new Vector3Int(-1, 1, 0), new Vector3Int(0, 1, 1), new Vector3Int(0, 1, -1),
        };
        
        public Vector3Int[] dirDown = 
        {
            new Vector3Int(1, -1, 0), new Vector3Int(-1, -1, 0), new Vector3Int(0, -1, 1), new Vector3Int(0, -1, -1),
        };
        
        public Vector3Int[] dirFront = 
        {
            new Vector3Int(1, 0, 1), new Vector3Int(-1, 0, 1), new Vector3Int(0, 1, 1), new Vector3Int(0, -1, 1),
        };
        
        public Vector3Int[] dirBack = 
        {
            new Vector3Int(1, 0, -1), new Vector3Int(-1, 0, -1), new Vector3Int(0, 1, -1), new Vector3Int(0, -1, -1),
        };
    }
    
    [Serializable]
    public class DirGroup
    {
        public Vector3Int[] dirHorizontal =
        {
            new Vector3Int(0, 0, 1), new Vector3Int(0, 0, -1), new Vector3Int(0, 1, 0), new Vector3Int(0, -1, 0),
        };
        
        public Vector3Int[] dirVertical = 
        {
            new Vector3Int(0, 0, -1), new Vector3Int(0, 0, 1), new Vector3Int(1, 0, 0), new Vector3Int(-1, 0, 0),
        };
        
        public Vector3Int[] dirDepth = 
        {
            new Vector3Int(1, 0, 0), new Vector3Int(-1, 0, 0), new Vector3Int(0, 1, 0), new Vector3Int(0, -1, 0),
        };
   
        public Vector3Int[] directions =
        {
            new Vector3Int(1, 0, 0), new Vector3Int(-1, 0, 0), new Vector3Int(0, 1, 0), new Vector3Int(0, -1, 0),
            new Vector3Int(0, 0, 1), new Vector3Int(0, 0, -1)
        };
    }
    
    public struct DirectionArrayPair
    {
        public Vector3Int[] DirectionArray;
        public Vector3Int[] Directions;
    }
}
