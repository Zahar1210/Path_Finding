using UnityEngine;

namespace FindPath
{
    public class PathEntryPoint : MonoBehaviour
    {
        [SerializeField] private Directions directions;
        [SerializeField] private FindPathProject findPathProject;
        private void Awake()
        {
            findPathProject.Initialize();
            directions.Initialize();
        }
    }
}
