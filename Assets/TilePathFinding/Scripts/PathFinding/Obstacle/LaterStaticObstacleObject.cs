using System.Collections;
using UnityEngine;

namespace FindPath
{
    public class LaterStaticObstacleObject : ObstacleType
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
            TileObstacleChecker.GetTilesForCheck(Obstacle);
            TileObstacleChecker.CalculateSurfaces(Obstacle);
        }
    }
}