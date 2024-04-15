using UnityEngine;

namespace FindPath
{
    public abstract class Seeker : MonoBehaviour
    {
        #region Main Variables

        #region Main Find Path Variables
       
        //ссылка на проект)
        public FindPathProject FindPathProject => findPathProject;
        [SerializeField] private FindPathProject findPathProject;
        
        // найденный (текущий путь)
        public Surface[] Path { get; set; } 
        
        // проинициализированна только если find path trigger = target (которую мы находим выбераем и тд)
        public Transform SeekerTarget { get; set; } 

        // текущая поверхность при движении
        public Surface CurrentSurface { get; set; }
        
        // стартовая поверхность
        public Surface StartSurface { get; set; }
        
        // целевая поверхность
        public Surface TargetSurface { get; set; } 

        #endregion

        #region Parameter Mode 
        
        //ДАННЫЕ ПО КОТОРЫМ ИЩЕМ ПУТЬ (РЕЖИМЫ)
        
        // проинициализированное условие при поиске пути
        public FindPathMode FindPathMode { get; set; } 
        public PathMode PathMode => pathMode;
        [SerializeField] private PathMode pathMode;
        
        // проинициализированный триггер пути (событие при котором будет найден путь)
        public FindPathTrigger FindPathTrigger { get; set; }
        public PathTrigger PathTrigger => pathTrigger;
        [SerializeField] private PathTrigger pathTrigger;
        
        //проинициализированный динамичность пути (событие при котором путь будет перенайдн)
        public DynamicPath DynamicPath { get; set; } 
        public PathDynamic PathDynamic => pathDynamic;
        [SerializeField] private PathDynamic pathDynamic;
        
        //проинициализированный режим таргет (если FindPathTrigger = TargetPosition)
        //здесь список режимов выбора и слежки за целевыми обьектами
        public FindTargetType FindTargetType { get; set; }
        public TargetType TargetType => targetType;
        [SerializeField] private TargetType targetType;

        #endregion

        #endregion
        
        #region Related Variables

        #region Find Path Trigger 

        #region Main Variables Mouse

        // mouse input 
        public int MouseSide => mouseSide;
        [SerializeField] [Range(0, 1)] private int mouseSide;

        // mouse position и для mouse input 
        public LayerMask MouseLayerMask => mouseLayerMask;
        [SerializeField] private LayerMask mouseLayerMask;

        #endregion

        #region Main Variables Target 
        
        //обязательные переменные при поиске Surface у Target 
        public TargetDirection TargetDirection => targetDirection;
        [SerializeField] private TargetDirection targetDirection;

        public int Count => count;
        [SerializeField] private int count;

        public float CheckInterval => checkInterval;
        [SerializeField] private float checkInterval;

        
        //MODES
         // если Find PAth Mode == targetPositionMode
        public SelectTargetMode SelectTargetMode { get; set; }
        public TargetSelectMode TargetSelectMode => targetSelectMode;
        [SerializeField] private TargetSelectMode targetSelectMode;
        
        
        // Solo mode
        public Transform SoloTarget => soloTarget;
        [SerializeField] private Transform soloTarget; 
        
        // Array mode
        public Transform[] ArrayTargets => arrayTargets; 
        [SerializeField] private Transform[] arrayTargets;
        
        // Select mode
        public LayerMask TargetLayer => targetLayer;
        [SerializeField] private LayerMask targetLayer;

        #endregion

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
        
        public abstract void GetPath(Surface startSurface, Surface targetSurface);
    }
}