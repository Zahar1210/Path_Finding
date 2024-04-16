using UnityEngine;

namespace FindPath
{
    public abstract class Seeker : MonoBehaviour
    {
        #region Main Variables

        #region Main Find Path Variables
        public FindPathProject FindPathProject => _findPathProject;  //ссылка на проект)
        [SerializeField] private FindPathProject _findPathProject;
        
        public Surface[] Path { get; set; } // найденный (текущий путь)
        
        
        public FindMode FindMode { get; set; }// проинициализированный режим поиска пути 
        public PathFindMode PathFindMode => _pathFindMode;  //режим поиска пути
        [SerializeField] private PathFindMode _pathFindMode;

        
        public Transform SeekerTarget { get; set; }// проинициализированна только если find path trigger = target (которую мы находим выбераем и тд)

        public Surface CurrentSurface { get; set; } // текущая поверхность при движении
        
        public Surface StartSurface { get; set; } // стартовая поверхность
        
        public Surface TargetSurface { get; set; }  // целевая поверхность

        #endregion

        #region Parameter Mode 
        
        //ДАННЫЕ ПО КОТОРЫМ ИЩЕМ ПУТЬ (РЕЖИМЫ)
        
        public FindPathReasonMode FindPathReasonMode { get; set; } // проинициализированное условие при поиске пути
        public PathReason PathReason => _pathReason;
        [SerializeField] private PathReason _pathReason;
        
      
        public FindPathTrigger FindPathTrigger { get; set; } // проинициализированный триггер пути (событие при котором будет найден путь)
        public PathTrigger PathTrigger => _pathTrigger; 
        [SerializeField] private PathTrigger _pathTrigger;
        
        public DynamicPath DynamicPath { get; set; } //проинициализированный динамичность пути (событие при котором путь будет перенайдн) 
        public PathDynamic PathDynamic => _pathDynamic;
        [SerializeField] private PathDynamic _pathDynamic;
        
        public FindTargetType FindTargetType { get; set; } //проинициализированный режим таргет (если FindPathTrigger = TargetPosition)
        //здесь список режимов выбора и слежки за целевыми обьектами
        public TargetType TargetType => _targetType;
        [SerializeField] private TargetType _targetType;

        #endregion

        #endregion
        
        #region Related Variables

        #region Find Path Trigger 

        #region Main Variables Mouse

        // mouse input 
        public int MouseSide => _mouseSide;
        [SerializeField] [Range(0, 1)] private int _mouseSide;

        // mouse position и для mouse input 
        public LayerMask MouseLayerMask => _mouseLayerMask;
        [SerializeField] private LayerMask _mouseLayerMask;

        #endregion

        #region Main Variables Target 
        
        //обязательные переменные при поиске Surface у Target 
        public TargetDirection TargetDirection => _targetDirection;
        [SerializeField] private TargetDirection _targetDirection;

        public int Count => _count;
        [SerializeField] private int _count;

        public float CheckInterval => _checkInterval;
        [SerializeField] private float _checkInterval;

        
        //MODES
         // если Find PAth Mode == targetPositionMode
        public SelectTargetMode SelectTargetMode { get; set; }
        public TargetSelectMode TargetSelectMode => _targetSelectMode;
        [SerializeField] private TargetSelectMode _targetSelectMode;
        
        
        // Solo mode
        public Transform SoloTarget => _soloTarget;
        [SerializeField] private Transform _soloTarget; 
        
        // Array mode
        public Transform[] ArrayTargets => _arrayTargets;
        [SerializeField] private Transform[] _arrayTargets;
        
        // Select mode
        public LayerMask TargetLayer => _targetLayer;
        [SerializeField] private LayerMask _targetLayer;

        #endregion

        #endregion

        #region Dynamic Variables

        // [SerializeField] private 

        #endregion

        #region Find Mode Variables

        // distance
        public float MaxDistance => _maxDistance;
        [SerializeField] private float _maxDistance;

        // radius
        public float DifferenceX => _differenceX;
        [SerializeField] private float _differenceX;

        public float DifferenceY => _differenceY;
        [SerializeField] private float _differenceY;

        public float DifferenceZ => _differenceZ;
        [SerializeField] private float _differenceZ;

        // field of view 
        public float Angle => _angle;
        [SerializeField] [Range(0, 360)] private float _angle;
        
        public float Radius => _radius;
        [SerializeField] private float _radius;
        
        public LayerMask ObstacleLayerMask => _obstacleLayerMask;
        [SerializeField] private LayerMask _obstacleLayerMask;
        
        public LayerMask TargetLayerMask => _targetLayerMask;
        [SerializeField] private LayerMask _targetLayerMask;
 
        #endregion

        #endregion

        public abstract void Initialize();
        
        public abstract void CheckPath(PathDynamic pathDynamic);
        
        public abstract void TryGetPath(Surface startSurface, Surface targetSurface);
    }
}