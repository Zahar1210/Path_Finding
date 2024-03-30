using UnityEngine;

namespace FindPath
{
    public abstract class Seeker : MonoBehaviour
    {
        #region Mandatory Variables

        #region Path

        public FindPathProject FindPathProject => findPathProject;
        [SerializeField] private FindPathProject findPathProject;

        public Tile.Surface[] Path { get; set; } // путь

        public Tile.Surface CurrentSurface { get; set; } // текущая поерхность
        public Tile.Surface StartSurface { get; set; } // стартовая поерхность
        public Tile.Surface TargetSurface { get; set; } // целевая поерхность

        #endregion

        #region Parameters

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

        #endregion

        #endregion

        #region Mode-related Variables

        #region Find Trigger Variables

        public int MouseSide => mouseSide;
        [SerializeField] [Range(0, 1)] private int mouseSide; // mouse input 

        public LayerMask LayerMask => layerMask;
        [SerializeField] private LayerMask layerMask; //mouse position и для // mouse input 

        public Transform SeekerTarget => target;
        [SerializeField] private Transform target; // trigger target
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
        [SerializeField] private float maxDistance;
        public float MaxDistance => maxDistance;

        // radius
        [SerializeField] private float differenceX;
        public float DifferenceX => differenceX;

        [SerializeField] private float differenceY;
        public float DifferenceY => differenceY;

        [SerializeField] private float differenceZ;
        public float DifferenceZ => differenceZ;

        // monitoring

        #endregion

        #endregion

        public abstract void Initialize();
    }
}