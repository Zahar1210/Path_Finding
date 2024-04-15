using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FindPath
{
    [System.Serializable]
    public class Surface 
    {
        //variable for Path Finding 
        public Tile Tile { get; set; }
        public bool obstacleLock;
        public bool isObstacle;
        public Vector3Int direction;

        public Directions.DirectionArrayPair Directions { get; }

        //variable for Gizmos 
        public Vector3 Size { get; }
            
        //this constructor is to automatically create surfaces on a tile.
        public Surface(Vector3Int direction)
        {
            this.direction = direction;
        }
            
        //this constructor initializes the surface to find the path
        public Surface(Vector3Int direction ,Directions.DirectionArrayPair directions, Tile tile, int tileSize, bool obstacleLock, bool isObstacle)
        {
            Tile = tile;
            Directions = directions;
            this.direction = direction;
            this.obstacleLock = obstacleLock;
            this.isObstacle = isObstacle;
                
            //for Gizmos 
            if (direction == Vector3Int.up || direction == Vector3Int.down)
                Size = new Vector3(tileSize - 0.1f, 0.05f, tileSize - 0.1f);
            else if (direction == Vector3Int.left || direction == Vector3Int.right)
                Size = new Vector3(0.05f, tileSize - 0.1f, tileSize - 0.1f);
            else
                Size = new Vector3(tileSize - 0.1f, tileSize - 0.1f, 0.05f);
        }
    }
}