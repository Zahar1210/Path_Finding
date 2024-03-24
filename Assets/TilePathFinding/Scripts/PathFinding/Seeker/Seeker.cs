using System;
using System.Collections.Generic;
using UnityEngine;

namespace FindPath
{
    public class Seeker : MonoBehaviour
    {
        public Tile.Surface[] Path { get; set; }
        
        [SerializeField] private EventType eventType;
        [SerializeField] private float speed;
        [SerializeField] private FindMode findMode;
        private SeekerAbstract _seekerType;
        private FindPathProject _findPathProject;
        public PathParams PathParams;
        public Tile.Surface CurrentSurface { get; set; }

        //если тип по мышке 
        [Range(0, 1)]  public int mouseSide;

        //если интервал или ранодомное
        public float interval;

        //если за целью
        public Transform target; //будет свой тип для цели
        public float targetInterval;
        public TargetDirection targetDirection;
        public Collider targetCollider;
        public DirectionTargetSeeker DirectionTargetSeeker { get; set; }
        public Dictionary<TargetDirection, DirectionTargetSeeker> DirectionsTargetSeekers = new();
        //если комбинированно то и интервал и позиция цели
        

        public void Initialize()
        {
            _findPathProject = FindPathProject.Instance;
            _seekerType = SeekerSystem.GetSeekerType(eventType);
            
            if (_seekerType is TargetPositionSeeker)
            {
                switch (targetDirection)
                {
                    case TargetDirection.Left:
                        DirectionTargetSeeker = new HorizontalDirections();
                        break;
                    case TargetDirection.Right:
                        DirectionTargetSeeker = new HorizontalDirections();
                        break;
                    case TargetDirection.Up:
                        DirectionTargetSeeker = new VerticalDirections();
                        break;
                    case TargetDirection.Down:
                        DirectionTargetSeeker = new VerticalDirections();
                        break;
                    case TargetDirection.Forward:
                        DirectionTargetSeeker = new DepthDirections();
                        break;
                    case TargetDirection.Back:
                        DirectionTargetSeeker = new DepthDirections();
                        break;
                }
            }
        }

        private void Update()
        {
            if (_seekerType.CheckEvent(this))
            {
                if (Path != null )
                {
                    Array.Clear(Path, 0, Path.Length);
                }

                PathParams = _seekerType.GetPathParams(this);

                if (PathParams.StartSurface == null || PathParams.TargetSurface == null)
                {
                    return;
                }

                Path = PathFinding.GetPath(PathParams.StartSurface, PathParams.TargetSurface, _findPathProject, findMode);
            }
        }
    }

    public class PathParams
    {
        public Tile.Surface StartSurface { get; }
        public Tile.Surface TargetSurface { get; }

        public PathParams(Tile.Surface startSurface = null, Tile.Surface targetSurface = null)
        {
            TargetSurface = targetSurface;
            StartSurface = startSurface;
        }
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
}