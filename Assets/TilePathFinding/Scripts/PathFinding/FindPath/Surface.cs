using UnityEngine;

namespace FindPath
{
    [System.Serializable]
    public class Surface 
    {
        //variable for Path Finding 
        public Seeker InPath { get; set; }
        public GridObject GridObject { get; set; }
        public Directions.DirectionArrayPair Directions { get; }

        public bool ObstacleLock;
        public bool IsObstacle; 
        public Vector3Int Direction;

        //variable for Gizmos 
        public Vector3 Size { get; }
            
        //this constructor is to automatically create surfaces on a tile.
        public Surface(Vector3Int direction)
        {
            this.Direction = direction;
        }
            
        //this constructor initializes the surface to find the path
        public Surface(Vector3Int direction ,Directions.DirectionArrayPair directions, GridObject gridObject, int tileSize, bool obstacleLock, bool isObstacle)
        {
            GridObject = gridObject;
            Directions = directions;
            this.Direction = direction;
            this.ObstacleLock = obstacleLock;
            this.IsObstacle = isObstacle;
                
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