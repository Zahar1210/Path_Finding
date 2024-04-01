using UnityEngine;

namespace FindPath
{
    public abstract class Seeker : MonoBehaviour
    {
        #region Mandatory Variables

        #region Path

        public FindPathProject FindPathProject => findPathProject;
        [SerializeField] private FindPathProject findPathProject;

        public Transform SeekerTarget { get; set; }// проинициализированна только если find path trigger = target
        public Tile.Surface[] Path { get; set; } // путь

        public Tile.Surface CurrentSurface { get; set; } // текущая поерхность
        public Tile.Surface StartSurface { get; set; } // стартовая поерхность
        public Tile.Surface TargetSurface { get; set; } // целевая поерхность

        #endregion

        #region Parameters

        public FindTargetType FindTargetType { get; set; } //проинициализированный режим таргет 
        public FindPathMode FindPathMode { get; set; } // проинициализированный режим поиска пути
        public FindPathTrigger FindPathTrigger { get; set; } // проинициализированный триггер пути
        public DynamicPath DynamicPath { get; set; } //проинициализированный динамичность пути

        //данные по ктоорым проходит инициализация 
        public PathMode PathMode => pathMode;
        [SerializeField] private PathMode pathMode;
        
        public PathTrigger PathTrigger => pathTrigger;
        [SerializeField] private PathTrigger pathTrigger;
        
        public PathDynamic PathDynamic => pathDynamic;
        [SerializeField] private PathDynamic pathDynamic;
        
        public TargetType TargetType => targetType;
        [SerializeField] private TargetType targetType;

        #endregion

        #endregion

        #region Mode-related Variables

        #region Find Trigger Variables

        public int MouseSide => mouseSide;
        [SerializeField] [Range(0, 1)] private int mouseSide; // mouse input 

        public LayerMask LayerMask => layerMask;
        [SerializeField] private LayerMask layerMask; //mouse position и для // mouse input 

        // trigger target
        
        //solo mode
        [SerializeField] private Transform target; 
        
        // array mode
        [SerializeField] private Transform[] targets;
        
        // select mode
        [SerializeField] private LayerMask targetLayer;
        
        public TargetDirection TargetDirection => targetDirection;
        [SerializeField] private TargetDirection targetDirection;

        public int Count => count;
        [SerializeField] private int count;

        #endregion

        #region Dynamic Variables

        // [SerializeField] private 

        #endregion

        #region Find Mode Variables

        // distance
        public float MaxDistance => maxDistance;
        [SerializeField] private float maxDistance;

        // radius
        public float DifferenceX => differenceX;
        [SerializeField] private float differenceX;

        public float DifferenceY => differenceY;
        [SerializeField] private float differenceY;

        public float DifferenceZ => differenceZ;
        [SerializeField] private float differenceZ;

        // field of view 
        public float Angle => angle;
        [SerializeField] [Range(0, 360)] private float angle;
        
        public float Radius => radius;
        [SerializeField] private float radius;
        
        public LayerMask ObstacleLayerMask => obstacleLayerMask;
        [SerializeField] private LayerMask obstacleLayerMask;
        
        public LayerMask TargetLayerMask => targetLayerMask;
        [SerializeField] private LayerMask targetLayerMask;
 
        #endregion

        #endregion

        public abstract void Initialize();
        
        public abstract void CheckPath(PathDynamic pathDynamic);
        
        public abstract void FindPath(Tile.Surface startSurface, Tile.Surface targetSurface);
    }
}