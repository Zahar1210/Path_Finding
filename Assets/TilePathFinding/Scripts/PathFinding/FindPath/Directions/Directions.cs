using System;
using System.Collections.Generic;
using UnityEngine;

namespace FindPath
{
    [CreateAssetMenu(menuName = "NewInfo/ DirectionInfo", fileName = "Directions")]
    public class Directions : ScriptableObject
    {
        public Direction _direction;
        public DirectionGroup _directionGroup;
        public readonly Dictionary<Vector3Int, DirectionArrayPair> dirDictionary = new();
        
        #region Initialize
        
        //This function is called at the beginning to ensure everything functions correctly in the future.
        public void Initialize()
        {
            //Here we determine the size of the tile so that the directions can be adjusted accordingly
            int tileSize = FindPathProject.Instance.TileSize;

            //Initialization
            _direction.SetVector(tileSize);
            _directionGroup.SetVector(tileSize);
            SetDirDictionary();
        }

        public void SetDirDictionary()
        {
            /*
             Create a dictionary for each type of surface.
             This is important so that the tile knows which direction to check for the presence of another tile. 
            */
            dirDictionary.Add(Vector3Int.right, new DirectionArrayPair
            {
                _directionArray = _directionGroup._directionHorizontal,
                _directions = _direction._directionRight
            });

            dirDictionary.Add(Vector3Int.left, new DirectionArrayPair
            {
                _directionArray = _directionGroup._directionHorizontal,
                _directions = _direction._directionLeft
            });

            dirDictionary.Add(Vector3Int.up, new DirectionArrayPair
            {
                _directionArray = _directionGroup._directionVertical,
                _directions = _direction._directionUp
            });

            dirDictionary.Add(Vector3Int.down, new DirectionArrayPair
            {
                _directionArray = _directionGroup._directionVertical,
                _directions = _direction._directionDown
            });

            dirDictionary.Add(Vector3Int.forward, new DirectionArrayPair
            {
                _directionArray = _directionGroup._directionDepth,
                _directions = _direction._directionFront
            });

            dirDictionary.Add(Vector3Int.back, new DirectionArrayPair
            {
                _directionArray = _directionGroup._directionDepth,
                _directions = _direction._directionBack
            });
        }

        [Serializable]
        public class Direction
        {
            //Initializing each array while considering the tile size.
            public Vector3Int[] _directionRight;
            public Vector3Int[] _directionLeft;
            public Vector3Int[] _directionUp;
            public Vector3Int[] _directionDown;
            public Vector3Int[] _directionFront;
            public Vector3Int[] _directionBack;

            
            public void SetVector(int tileSize)
            {
                _directionRight = new[]
                {
                    new Vector3Int(tileSize, tileSize, 0), new Vector3Int(tileSize, 0, tileSize),
                    new Vector3Int(tileSize, -tileSize, 0), new Vector3Int(tileSize, 0, -tileSize)
                };

                _directionLeft = new[]
                {
                    new Vector3Int(-tileSize, tileSize, 0), new Vector3Int(-tileSize, -tileSize, 0),
                    new Vector3Int(-tileSize, 0, tileSize), new Vector3Int(-tileSize, 0, -tileSize)
                };

                _directionUp = new[]
                {
                    new Vector3Int(tileSize, tileSize, 0), new Vector3Int(-tileSize, tileSize, 0),
                    new Vector3Int(0, tileSize, tileSize), new Vector3Int(0, tileSize, -tileSize)
                };

                _directionDown = new[]
                {
                    new Vector3Int(tileSize, -tileSize, 0), new Vector3Int(-tileSize, -tileSize, 0),
                    new Vector3Int(0, -tileSize, tileSize), new Vector3Int(0, -tileSize, -tileSize)
                };

                _directionFront = new[]
                {
                    new Vector3Int(tileSize, 0, tileSize), new Vector3Int(-tileSize, 0, tileSize),
                    new Vector3Int(0, tileSize, tileSize), new Vector3Int(0, -tileSize, tileSize)
                };

                _directionBack = new[]
                {
                    new Vector3Int(tileSize, 0, -tileSize), new Vector3Int(-tileSize, 0, -tileSize),
                    new Vector3Int(0, tileSize, -tileSize), new Vector3Int(0, -tileSize, -tileSize)
                };
            }
        }

        [Serializable]
        public class DirectionGroup
        {
            //Initializing each array while considering the tile size.
            public Vector3Int[] _directionHorizontal;
            public Vector3Int[] _directionVertical;
            public Vector3Int[] _directionDepth; 
            public Vector3Int[] _directions;

            public void SetVector(int tileSize)
            {
                _directionHorizontal = new[]
                {
                    new Vector3Int(0, 0, tileSize), new Vector3Int(0, 0, -tileSize),
                    new Vector3Int(0, tileSize, 0), new Vector3Int(0, -tileSize, 0),
                };
                
                _directionVertical = new[]
                {
                    new Vector3Int(0, 0, -tileSize), new Vector3Int(0, 0, tileSize),
                    new Vector3Int(tileSize, 0, 0), new Vector3Int(-tileSize, 0, 0),
                };
                
                _directionDepth = new[]
                {
                    new Vector3Int(tileSize, 0, 0), new Vector3Int(-tileSize, 0, 0),
                    new Vector3Int(0, tileSize, 0), new Vector3Int(0, -tileSize, 0),
                };
                
                _directions = new[]
                {
                    new Vector3Int(tileSize, 0, 0), new Vector3Int(-tileSize, 0, 0),
                    new Vector3Int(0, tileSize, 0), new Vector3Int(0, -tileSize, 0),
                    new Vector3Int(0, 0, tileSize), new Vector3Int(0, 0, -tileSize)
                };
            }
        }
        
        #endregion

        public struct DirectionArrayPair
        {
            public Vector3Int[] _directionArray;
            public Vector3Int[] _directions;
        }
    }
}