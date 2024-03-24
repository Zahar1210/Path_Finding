using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace FindPath
{
    public static class SeekerSystem
    {
        private static readonly Dictionary<EventType, SeekerAbstract> _SeekerTypes = new();
        
        public static void Initialize()
        {
            FindPathProject findPathProject = FindPathProject.Instance;
            
            _SeekerTypes.Add(
                EventType.MouseInput,
                new MouseInputSeeker(findPathProject, Camera.main));
            _SeekerTypes.Add(
                EventType.Interval,
                new IntervalSeeker(Time.time, findPathProject));
            _SeekerTypes.Add(
                EventType.TargetPosition,
                new TargetPositionSeeker(findPathProject, Time.time));
            _SeekerTypes.Add(
                EventType.MousePosition,
                new MousePositionSeeker(Camera.main, findPathProject));
            _SeekerTypes.Add(
                EventType.Random,
                new RandomSeeker(findPathProject));
        }

        public static SeekerAbstract GetSeekerType(EventType eventType)
        {
            if (_SeekerTypes.TryGetValue(eventType, out var seekerAbstract))
            {
                return seekerAbstract;
            }
            
            return null;
        }
    }
}