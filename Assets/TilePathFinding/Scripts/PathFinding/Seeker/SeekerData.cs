using System.Collections.Generic;
using UnityEngine;

namespace FindPath
{
    public static class SeekerData
    {
        #region Variables
        
        private static readonly Dictionary<PathMode, FindPathMode> _FindPathMode = new();
        private static readonly Dictionary<PathTrigger, FindPathTrigger> _FindPathTrigger = new();
        private static readonly Dictionary<TargetType, FindTargetType> _FindTargetType = new();
        private static readonly Dictionary<PathDynamic, DynamicPath> _FindPathDynamic = new();
        private static readonly Dictionary<TargetSelectMode, SelectTargetMode> _SelectMode = new();

        #endregion

        #region Initialization

        public static void Initialization()// точка входа 
        {
            //Find Path Trigger
            _FindPathTrigger.Add(PathTrigger.MouseInput, new MouseInputTrigger(Camera.main));
            _FindPathTrigger.Add(PathTrigger.MousePosition, new MousePositionTrigger(Camera.main));
            _FindPathTrigger.Add(PathTrigger.TargetPosition, new TargetPositionTrigger(FindPathProject.Instance));

            //Find Path Mode
            _FindPathMode.Add(PathMode.Always, new AlwaysFindMode());
            _FindPathMode.Add(PathMode.Radius, new RadiusFindMode());
            _FindPathMode.Add(PathMode.Distance, new DistanceFindMode());
            _FindPathMode.Add(PathMode.FieldOfViewOverlap, new FieldOfViewOverlapFindMode());
            _FindPathMode.Add(PathMode.FieldOfViewAngle, new FieldOfViewAngleFindMode());

            //Find Path Dynamic
            _FindPathDynamic.Add(PathDynamic.ChangeObstacle, new ChangeObstacleDynamicPath());
            _FindPathDynamic.Add(PathDynamic.Interval, new IntervalDynamicPath());
            _FindPathDynamic.Add(PathDynamic.CombinedIntervalObstacle, new CombinedIntervalObstacle());
            _FindPathDynamic.Add(PathDynamic.Initial, new InitialFindingPath());
            _FindPathDynamic.Add(PathDynamic.CombinedIntervalTarget, new CombinedIntevalTargetPosition());
            
            //Find Target Type
            _FindTargetType.Add(TargetType.SoloMode, new SoloMode());
            _FindTargetType.Add(TargetType.ArrayMode, new ArrayMode());
            _FindTargetType.Add(TargetType.SelectMode, new SelectMode());
            _FindTargetType.Add(TargetType.RandomMode, new RandomMode());
            
            //Select Mode
            _SelectMode.Add(TargetSelectMode.Distance, new DistanceSelectMode());
            _SelectMode.Add(TargetSelectMode.PathDistance, new PathDistanceSelectMode());
        }

        #endregion

        #region Get Find Path

        public static FindPathMode GetFindPathMode(PathMode pathMode)
        {
            return _FindPathMode.TryGetValue(pathMode, out FindPathMode findPathMode) ? findPathMode : default;
        }

        public static FindPathTrigger GetFindPathTrigger(PathTrigger pathTrigger)
        {
            return _FindPathTrigger.TryGetValue(pathTrigger, out FindPathTrigger findPathTrigger) ? findPathTrigger : default;
        }

        public static DynamicPath GetFindPathModeDynamicPath(PathDynamic pathDynamic)
        {
            return _FindPathDynamic.TryGetValue(pathDynamic, out DynamicPath dynamicPath) ? dynamicPath : default;
        }
        
        public static FindTargetType GetFindPathTargetType(TargetType targetType)
        {
            return _FindTargetType.TryGetValue(targetType, out FindTargetType type) ? type : default;
        }

        public static SelectTargetMode GetTargetSelectMode(TargetSelectMode selectMode)
        {
            return _SelectMode.TryGetValue(selectMode, out SelectTargetMode selectTargetMode) ? selectTargetMode : default;
        }

        #endregion
    }

    #region enumFindPath

    public enum PathMode
    {
        Always,
        Distance,
        Radius,
        FieldOfViewOverlap,
        FieldOfViewAngle
    }

    public enum PathTrigger
    {
        MouseInput,
        MousePosition,
        TargetPosition
    }

    public enum PathDynamic
    {
        ChangeObstacle,
        Interval,
        Initial,
        CombinedIntervalObstacle,
        CombinedIntervalTarget
    }

    public enum TargetDirection
    {
        Left,
        Right,
        Up,
        Down,
        Forward,
        Back,
        Around
    }

    public enum TargetType
    {
        SoloMode, 
        ArrayMode,
        SelectMode,
        RandomMode
    }

    public enum TargetSelectMode
    {
        Distance,
        PathDistance
    }

    #endregion
}