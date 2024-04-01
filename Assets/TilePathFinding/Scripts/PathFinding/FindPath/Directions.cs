using System;
using System.Collections.Generic;
using UnityEngine;

namespace FindPath
{
    [CreateAssetMenu(menuName = "NewInfo/ DirectionInfo", fileName = "Directions")]
    public class Directions : ScriptableObject
    {
        public Direction direction;
        public DirectionGroup directionGroup;
        public Dictionary<Vector3Int, DirectionArrayPair> DirDictionary = new();
        
        #region Initialize
        
        //This function is called at the beginning to ensure everything functions correctly in the future.
        public void Initialize()//точка входа
        {
            //Here we determine the size of the tile so that the directions can be adjusted accordingly
            int tileSize = FindPathProject.Instance.TileSize;
            
            //Initialization
            direction.SetVector(tileSize);
            directionGroup.SetVector(tileSize);
            SetDirDictionary();
        }

        private void SetDirDictionary()
        {
            /*
             Create a dictionary for each type of surface.
             This is important so that the tile knows which direction to check for the presence of another tile. 
            */
            DirDictionary.Add(Vector3Int.right, new DirectionArrayPair
            {
                DirectionArray = directionGroup.directionHorizontal,
                Directions = direction.directionRight
            });

            DirDictionary.Add(Vector3Int.left, new DirectionArrayPair
            {
                DirectionArray = directionGroup.directionHorizontal,
                Directions = direction.directionLeft
            });

            DirDictionary.Add(Vector3Int.up, new DirectionArrayPair
            {
                DirectionArray = directionGroup.directionVertical,
                Directions = direction.directionUp
            });

            DirDictionary.Add(Vector3Int.down, new DirectionArrayPair
            {
                DirectionArray = directionGroup.directionVertical,
                Directions = direction.directionDown
            });

            DirDictionary.Add(Vector3Int.forward, new DirectionArrayPair
            {
                DirectionArray = directionGroup.directionDepth,
                Directions = direction.directionFront
            });

            DirDictionary.Add(Vector3Int.back, new DirectionArrayPair
            {
                DirectionArray = directionGroup.directionDepth,
                Directions = direction.directionBack
            });
        }

        [Serializable]
        public class Direction
        {
            //Initializing each array while considering the tile size.
            public Vector3Int[] directionRight;
            public Vector3Int[] directionLeft;
            public Vector3Int[] directionUp;
            public Vector3Int[] directionDown;
            public Vector3Int[] directionFront;
            public Vector3Int[] directionBack;

            public void SetVector(int tileSize)
            {
                directionRight = new[]
                {
                    new Vector3Int(tileSize, tileSize, 0), new Vector3Int(tileSize, 0, tileSize),
                    new Vector3Int(tileSize, -tileSize, 0), new Vector3Int(tileSize, 0, -tileSize)
                };

                directionLeft = new[]
                {
                    new Vector3Int(-tileSize, tileSize, 0), new Vector3Int(-tileSize, -tileSize, 0),
                    new Vector3Int(-tileSize, 0, tileSize), new Vector3Int(-tileSize, 0, -tileSize)
                };

                directionUp = new[]
                {
                    new Vector3Int(tileSize, tileSize, 0), new Vector3Int(-tileSize, tileSize, 0),
                    new Vector3Int(0, tileSize, tileSize), new Vector3Int(0, tileSize, -tileSize)
                };

                directionDown = new[]
                {
                    new Vector3Int(tileSize, -tileSize, 0), new Vector3Int(-tileSize, -tileSize, 0),
                    new Vector3Int(0, -tileSize, tileSize), new Vector3Int(0, -tileSize, -tileSize)
                };

                directionFront = new[]
                {
                    new Vector3Int(tileSize, 0, tileSize), new Vector3Int(-tileSize, 0, tileSize),
                    new Vector3Int(0, tileSize, tileSize), new Vector3Int(0, -tileSize, tileSize)
                };

                directionBack = new[]
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
            public Vector3Int[] directionHorizontal;
            public Vector3Int[] directionVertical;
            public Vector3Int[] directionDepth;
            public Vector3Int[] directions;

            public void SetVector(int tileSize)
            {
                directionHorizontal = new[]
                {
                    new Vector3Int(0, 0, tileSize), new Vector3Int(0, 0, -tileSize),
                    new Vector3Int(0, tileSize, 0), new Vector3Int(0, -tileSize, 0),
                };
                
                directionVertical = new[]
                {
                    new Vector3Int(0, 0, -tileSize), new Vector3Int(0, 0, tileSize),
                    new Vector3Int(tileSize, 0, 0), new Vector3Int(-tileSize, 0, 0),
                };
                
                directionDepth = new[]
                {
                    new Vector3Int(tileSize, 0, 0), new Vector3Int(-tileSize, 0, 0),
                    new Vector3Int(0, tileSize, 0), new Vector3Int(0, -tileSize, 0),
                };
                
                directions = new[]
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
            public Vector3Int[] DirectionArray;
            public Vector3Int[] Directions;
        }
    }
}