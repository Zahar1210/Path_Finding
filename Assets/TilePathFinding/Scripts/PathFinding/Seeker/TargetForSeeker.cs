using UnityEngine;

namespace FindPath
{
    public class TargetForSeeker : MonoBehaviour
    {
        private FindPathProject _findPathProject;
        
        private void Initialize()
        {
            _findPathProject = FindPathProject.Instance;
        }

        public void GetCurrentSurface(Seeker seeker)
        {
            Vector3Int roundedPosition = Vector3Int.RoundToInt(transform.position);

            // if (_findPathProject.Tiles.TryGetValue())
            // {
            //     
            // }
        }
    }
}