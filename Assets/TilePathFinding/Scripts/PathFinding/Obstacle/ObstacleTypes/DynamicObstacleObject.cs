using System.Collections;
using UnityEngine;

namespace FindPath
{
    public class DynamicObstacleObject : ObstacleType
    {
        
        public override void Initialize(Obstacle obstacle)
        {
            Obstacle = obstacle;
            StartChecking();
        }

        public override void StartChecking()
        {
            Obstacle.StartCoroutine(Timer());
        }

        private IEnumerator Timer()
        {
            while (Obstacle.IsCheck)
            {
                yield return new WaitForSeconds(Obstacle.CheckInterval);
                Check();
            }
        }

        public override void Check()
        {
            if (Obstacle.ObstacleObjectType != Obstacle.StartObjectObstacleType)
            {
                StartChecking();
            }

            Obstacle.Tiles.Clear();
            Obstacle.Surfaces.Clear();

            TileObstacleChecker.GetTilesForCheck(Obstacle);
            TileObstacleChecker.CalculateSurfaces(Obstacle);
        }
    }
}