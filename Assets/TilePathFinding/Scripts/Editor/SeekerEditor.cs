using UnityEditor;

namespace FindPath
{
    [CustomEditor(typeof(FindPathProject))]
    public class SeekerEditor : Editor
    {
        #region Variable
        
        private SerializedProperty _findMode;
        private SerializedProperty _pathReason;
        private SerializedProperty _pathTrigger;
        private SerializedProperty _pathDynamic;
        private SerializedProperty _targetType;
        private SerializedProperty _targetDirection;
        private SerializedProperty _targetSelectMode;
        
        private SerializedProperty _findPathProject;
        private SerializedProperty _soloTarget;
        private SerializedProperty _arrayTargets;
        
        private SerializedProperty _count;
        private SerializedProperty _checkInterval;
        private SerializedProperty _maxDistance;
        private SerializedProperty _differenceX;
        private SerializedProperty _differenceY;
        private SerializedProperty _differenceZ;
        private SerializedProperty _angle;
        private SerializedProperty _radius;
        private SerializedProperty _mouseSide;
        
        private SerializedProperty _targetLayer;
        private SerializedProperty _mouseLayerMask;
        private SerializedProperty _targetLayerMask;
        private SerializedProperty _obstacleLayerMask;
        //еще должны быть переменные для dynamic path changer
        #endregion
        
        private void OnEnable()
        {
            _count = serializedObject.FindProperty("_count");
            _angle = serializedObject.FindProperty("_angle");
            _radius = serializedObject.FindProperty("_radius");
            _findMode = serializedObject.FindProperty("_findMode");
            _mouseSide = serializedObject.FindProperty("_mouseSide");
            _targetType = serializedObject.FindProperty("_targetType");
            _soloTarget = serializedObject.FindProperty("_soloTarget");
            _pathReason = serializedObject.FindProperty("_pathReason");
            _pathTrigger = serializedObject.FindProperty("_pathTrigger");
            _pathDynamic = serializedObject.FindProperty("_pathDynamic");
            _targetLayer = serializedObject.FindProperty("_targetLayer");
            _maxDistance = serializedObject.FindProperty("_maxDistance");
            _differenceX = serializedObject.FindProperty("_differenceX");
            _differenceY = serializedObject.FindProperty("_differenceY");
            _differenceZ = serializedObject.FindProperty("_differenceZ");
            _arrayTargets = serializedObject.FindProperty("_arrayTargets");
            _checkInterval = serializedObject.FindProperty("_checkInterval");
            _mouseLayerMask = serializedObject.FindProperty("_mouseLayerMask");
            _findPathProject = serializedObject.FindProperty("_findPathProject");
            _targetDirection = serializedObject.FindProperty("_targetDirection");
            _targetLayerMask = serializedObject.FindProperty("_targetLayerMask");
            _targetSelectMode = serializedObject.FindProperty("_targetSelectMode");
            _obstacleLayerMask = serializedObject.FindProperty("_obstacleLayerMask");
        }

        public override void OnInspectorGUI()
        {
            DrawFindPathInspector();
        }
        
         private void DrawFindPathInspector()
        {
            serializedObject.Update();

            serializedObject.ApplyModifiedProperties();
        }
    }
}