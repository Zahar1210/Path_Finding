using System.Collections.Generic;
using UnityEngine;

namespace FindPath
{
    public static class SeekerData
    {
        private static readonly Dictionary<PathReason, FindPathReasonMode> FindPathReason = new();
        private static readonly Dictionary<PathTrigger, FindPathTrigger> FindPathTrigger = new();
        private static readonly Dictionary<TargetType, FindTargetType> FindTargetType = new();
        private static readonly Dictionary<PathDynamic, DynamicPath> FindPathDynamic = new();
        private static readonly Dictionary<TargetSelectMode, SelectTargetMode> SelectMode = new();
        private static readonly Dictionary<PathFindMode, FindMode> FindPathMode = new();

        #region Initialization

        public static void Initialization()// точка входа 
        {
            //Find Path Trigger
            FindPathTrigger.Add(PathTrigger.MouseInput, new MouseInputTrigger(Camera.main));
            FindPathTrigger.Add(PathTrigger.MousePosition, new MousePositionTrigger(Camera.main));
            FindPathTrigger.Add(PathTrigger.TargetPosition, new TargetPositionTrigger(FindPathProject.Instance));

            //Find Path Mode
            FindPathReason.Add(PathReason.Always, new AlwaysFindReasonMode());
            FindPathReason.Add(PathReason.Radius, new RadiusFindReasonMode());
            FindPathReason.Add(PathReason.Distance, new DistanceFindReasonMode());
            FindPathReason.Add(PathReason.FieldOfViewOverlap, new FieldOfViewOverlapFindReasonMode());
            FindPathReason.Add(PathReason.FieldOfViewAngle, new FieldOfViewAngleFindReasonMode());

            //Find Path Dynamic
            FindPathDynamic.Add(PathDynamic.ChangeObstacle, new ChangeObstacleDynamicPath());
            FindPathDynamic.Add(PathDynamic.Interval, new IntervalDynamicPath());
            FindPathDynamic.Add(PathDynamic.CombinedIntervalObstacle, new CombinedIntervalObstacle());
            FindPathDynamic.Add(PathDynamic.Initial, new InitialFindingPath());
            FindPathDynamic.Add(PathDynamic.CombinedIntervalTarget, new CombinedIntevalTargetPosition());
            
            //Find Target Type
            FindTargetType.Add(TargetType.SoloMode, new SoloMode());
            FindTargetType.Add(TargetType.ArrayMode, new ArrayMode());
            FindTargetType.Add(TargetType.SelectMode, new SelectMode());
            FindTargetType.Add(TargetType.RandomMode, new RandomMode());
            
            //Select Dynamic Mode
            SelectMode.Add(TargetSelectMode.Distance, new DistanceSelectMode());
            SelectMode.Add(TargetSelectMode.PathDistance, new PathDistanceSelectMode());
            
            //
            FindPathMode.Add(PathFindMode.AStar, new AStarFindMode());
            FindPathMode.Add(PathFindMode.BreadthFirstSearch, new BFSFindMode());
        }

        #endregion

        #region Get Find Path

        public static FindPathReasonMode GetFindPathMode(PathReason pathReason)
        {
            return FindPathReason.TryGetValue(pathReason, out FindPathReasonMode findPathMode) ? findPathMode : default;
        }

        public static FindPathTrigger GetFindPathTrigger(PathTrigger pathTrigger)
        {
            return FindPathTrigger.TryGetValue(pathTrigger, out FindPathTrigger findPathTrigger) ? findPathTrigger : default;
        }

        public static DynamicPath GetFindPathModeDynamicPath(PathDynamic pathDynamic)
        {
            return FindPathDynamic.TryGetValue(pathDynamic, out DynamicPath dynamicPath) ? dynamicPath : default;
        }
        
        public static FindTargetType GetFindPathTargetType(TargetType targetType)
        {
            return FindTargetType.TryGetValue(targetType, out FindTargetType type) ? type : default;
        }

        public static SelectTargetMode GetTargetSelectMode(TargetSelectMode selectMode)
        {
            return SelectMode.TryGetValue(selectMode, out SelectTargetMode selectTargetMode) ? selectTargetMode : default;
        }

        public static FindMode GetFindMode(PathFindMode pathFindMode)
        {
            return FindPathMode.TryGetValue(pathFindMode, out FindMode findMode) ? findMode : default;
        }

        #endregion
    }

    #region enumFindPath

    public enum PathReason
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
    
    public enum PathFindMode
    {
        BreadthFirstSearch,
        AStar
    }

    #endregion
}