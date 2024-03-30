using System.Collections.Generic;
using UnityEngine;

namespace FindPath
{
    public abstract class Obstacle : MonoBehaviour
    {
        #region Data
        
        public ObstacleType ObstacleType { get; set; }
        
        public bool IsCheck { get; set; } = true;
        public List<Tile> Tiles { get; set; }
        public List<Tile.Surface> Surfaces { get; set; }
        public FindPathProject FindPathProjectInstance { get; set; }
        public ObstacleObjectType StartObjectObstacleType { get; set; }
        
        public ObstacleObjectType ObstacleObjectType => obstacleObjectType;
        [SerializeField] private ObstacleObjectType obstacleObjectType;
        
        public float CheckRadius => checkRadius;
        [SerializeField] [Range(0.3f, 2)] private float checkRadius = 0.5f;

        public Collider[] Colliders => colliders;
        [SerializeField] private Collider[] colliders;
        
        public LayerMask LayerMask => layerMask;
        [SerializeField] private LayerMask layerMask;
        
        
        //для динамического 
        public float CheckInterval => checkInterval;
        [SerializeField] private float checkInterval = 0.3f;
        
        #endregion

        public abstract void Initialize();
    }
}

