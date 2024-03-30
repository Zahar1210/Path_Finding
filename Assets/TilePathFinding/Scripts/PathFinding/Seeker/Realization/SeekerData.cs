using System.Collections.Generic;
using UnityEngine;

namespace FindPath
{
    public static class SeekerData
    {
        #region Variables
        
        private static readonly Dictionary<PathMode, FindPathMode> _FindPathMode = new();
        private static readonly Dictionary<PathTrigger, FindPathTrigger> _FindPathTrigger = new();
        private static readonly Dictionary<PathDynamic, DynamicPath> _FindPathDynamic = new();

        #endregion

        #region Initialization

        public static void Initialization(Seeker seeker)
        {
            //Find Path Trigger
            _FindPathTrigger.Add(PathTrigger.MouseInput,
                new MouseInputTrigger(seeker.MouseSide, seeker.LayerMask, Camera.main, null));
            _FindPathTrigger.Add(PathTrigger.MousePosition,
                new MousePositionTrigger(seeker.LayerMask, Camera.main, null));
            _FindPathTrigger.Add(PathTrigger.TargetPosition, new TargetPositionTrigger(seeker));

            //Find Path Mode
            _FindPathMode.Add(PathMode.Always, new AlwaysFindMode());
            _FindPathMode.Add(PathMode.Radius, new RadiusFindMode());
            _FindPathMode.Add(PathMode.Distance, new DistanceFindMode());
            _FindPathMode.Add(PathMode.Monitoring, new MonitoringFindMode());

            //Find Path Dynamic
            _FindPathDynamic.Add(PathDynamic.ChangeObstacle, new ChangeObstacleDynamicPath());
            _FindPathDynamic.Add(PathDynamic.Interval, new IntervalDynamicPath());
            _FindPathDynamic.Add(PathDynamic.CombinedIntervalObstacle, new CombinedIntervalObstacle());
            _FindPathDynamic.Add(PathDynamic.Initial, new InitialFindingPath());
            _FindPathDynamic.Add(PathDynamic.CombinedIntervalTarget, new CombinedIntevalTargetPosition());
        }

        #endregion

        #region Get Find Path

        public static FindPathMode GetFindPathMode(PathMode pathMode)
        {
            return _FindPathMode.TryGetValue(pathMode, out FindPathMode findPathMode) ? findPathMode : default;
        }

        public static FindPathTrigger GetFindPathTrigger(PathTrigger pathTrigger)
        {
            return _FindPathTrigger.TryGetValue(pathTrigger, out FindPathTrigger findPathTrigger)
                ? findPathTrigger
                : default;
        }

        public static DynamicPath GetFindPathModeDynamicPath(PathDynamic pathDynamic)
        {
            return _FindPathDynamic.TryGetValue(pathDynamic, out DynamicPath dynamicPath) ? dynamicPath : default;
        }

        #endregion
    }

    #region enumFindPath

    public enum PathMode
    {
        Always,
        DistancePath,
        Distance,
        Radius,
        Monitoring
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

    #endregion
}