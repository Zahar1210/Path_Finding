using System;
using System.Collections.Generic;
using FindPath;
using UnityEngine;

[CreateAssetMenu(menuName = "NewInfo/ DirectionInfo", fileName = "Directions")]
public class Directions : ScriptableObject
{
    public Dir dir;
    public DirGroup dirGroup;
    public Dictionary<Vector3Int, DirectionArrayPair> DirDictionary = new();

    public void Initialize()
    {
        int tileSize = FindPathProject.Instance.TileSize;
        dir.SetVector(tileSize);
        dirGroup.SetVector(tileSize);
        SetDirDictionary();
    }

    private void SetDirDictionary()
    {
        DirDictionary.Add(Vector3Int.right, new DirectionArrayPair
        {
            DirectionArray = dirGroup.dirHorizontal,
            Directions = dir.dirRight
        });

        DirDictionary.Add(Vector3Int.left, new DirectionArrayPair
        {
            DirectionArray = dirGroup.dirHorizontal,
            Directions = dir.dirLeft
        });

        DirDictionary.Add(Vector3Int.up, new DirectionArrayPair
        {
            DirectionArray = dirGroup.dirVertical,
            Directions = dir.dirUp
        });

        DirDictionary.Add(Vector3Int.down, new DirectionArrayPair
        {
            DirectionArray = dirGroup.dirVertical,
            Directions = dir.dirDown
        });

        DirDictionary.Add(Vector3Int.forward, new DirectionArrayPair
        {
            DirectionArray = dirGroup.dirDepth,
            Directions = dir.dirFront
        });

        DirDictionary.Add(Vector3Int.back, new DirectionArrayPair
        {
            DirectionArray = dirGroup.dirDepth,
            Directions = dir.dirBack
        });
    }

    [Serializable]
    public class Dir
    {
        public Vector3Int[] dirRight;
        public Vector3Int[] dirLeft;
        public Vector3Int[] dirUp;
        public Vector3Int[] dirDown;
        public Vector3Int[] dirFront;
        public Vector3Int[] dirBack;
        public void SetVector(int _tileSize)
        {
            dirRight = new[]
            {
                new Vector3Int(_tileSize, _tileSize, 0), new Vector3Int(_tileSize, 0, _tileSize),
                new Vector3Int(_tileSize, -_tileSize, 0), new Vector3Int(_tileSize, 0, -_tileSize)
            };

            dirLeft = new[]
            {
                new Vector3Int(-_tileSize, _tileSize, 0), new Vector3Int(-_tileSize, -_tileSize, 0),
                new Vector3Int(-_tileSize, 0, _tileSize), new Vector3Int(-_tileSize, 0, -_tileSize)
            };
            
            dirUp = new[]
            {
                new Vector3Int(_tileSize, _tileSize, 0), new Vector3Int(-_tileSize, _tileSize, 0),
                new Vector3Int(0, _tileSize, _tileSize), new Vector3Int(0, _tileSize, -_tileSize)
            };
            
            dirDown = new[]
            {
                new Vector3Int(_tileSize, -_tileSize, 0), new Vector3Int(-_tileSize, -_tileSize, 0),
                new Vector3Int(0, -_tileSize, _tileSize), new Vector3Int(0, -_tileSize, -_tileSize)
            };
            
            dirFront = new[]
            {
                new Vector3Int(_tileSize, 0, _tileSize), new Vector3Int(-_tileSize, 0, _tileSize),
                new Vector3Int(0, _tileSize, _tileSize), new Vector3Int(0, -_tileSize, _tileSize)
            };
            
            dirBack = new[]
            {
                new Vector3Int(_tileSize, 0, -_tileSize), new Vector3Int(-_tileSize, 0, -_tileSize), 
                new Vector3Int(0, _tileSize, -_tileSize), new Vector3Int(0, -_tileSize, -_tileSize)
            };
        }
    }

    [Serializable]
    public class DirGroup
    {
        public Vector3Int[] dirHorizontal;
        public Vector3Int[] dirVertical;
        public Vector3Int[] dirDepth;
        public Vector3Int[] directions;
        public void SetVector(int _tileSize)
        {
            dirHorizontal = new[]
            {
                new Vector3Int(0, 0, _tileSize), new Vector3Int(0, 0, -_tileSize),
                new Vector3Int(0, _tileSize, 0), new Vector3Int(0, -_tileSize, 0),
            };
            dirVertical = new[]
            {
                new Vector3Int(0, 0, -_tileSize), new Vector3Int(0, 0, _tileSize),
                new Vector3Int(_tileSize, 0, 0), new Vector3Int(-_tileSize, 0, 0),
            };
            dirDepth = new[]
            {
                new Vector3Int(_tileSize, 0, 0), new Vector3Int(-_tileSize, 0, 0),
                new Vector3Int(0, _tileSize, 0), new Vector3Int(0, -_tileSize, 0),
            };
            directions = new[]
            {
                new Vector3Int(_tileSize, 0, 0), new Vector3Int(-_tileSize, 0, 0),
                new Vector3Int(0, _tileSize, 0), new Vector3Int(0, -_tileSize, 0),
                new Vector3Int(0, 0, _tileSize), new Vector3Int(0, 0, -_tileSize)
            };
        }
    }

    public struct DirectionArrayPair
    {
        public Vector3Int[] DirectionArray;
        public Vector3Int[] Directions;
    }
}