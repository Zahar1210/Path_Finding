using UnityEditor;

namespace FindPath
{
    [CustomEditor(typeof(ObstacleObjects))]
    public class ObstacleObjectEditor : Editor
    {
        #region SerializeFiedProperites
    
        private SerializedProperty _obstacleType;
        private SerializedProperty _checkTime;
        private SerializedProperty _colliders;
        private SerializedProperty _layerMask;
        private SerializedProperty _isCheck;
        private SerializedProperty _checkSize;
    
        #endregion
    
        private void OnEnable()
        {
            _obstacleType = serializedObject.FindProperty("obstacleObjectType");
            _checkTime = serializedObject.FindProperty("checkTime");
            _colliders = serializedObject.FindProperty("colliders");
            _layerMask = serializedObject.FindProperty("layerMask");
            _isCheck = serializedObject.FindProperty("isCheck");
            _checkSize = serializedObject.FindProperty("checkRadius");
        }
        
        public override void OnInspectorGUI()
        {
            EditorGUILayout.Space(9);
            ObstacleObjects obstacleObjects = (ObstacleObjects)target;
            
            serializedObject.Update();
            
            EditorGUILayout.PropertyField(_layerMask);
            EditorGUILayout.PropertyField(_colliders);
            EditorGUILayout.PropertyField(_obstacleType);
            EditorGUILayout.PropertyField(_checkSize);
            
            EditorGUILayout.Space(3);
    
            if (obstacleObjects.obstacleObjectType == ObstacleObjectType.Dynamic)
            {
                EditorGUILayout.PropertyField(_checkTime);
                EditorGUILayout.PropertyField(_isCheck);
            }
            
            serializedObject.ApplyModifiedProperties();
        }
    }
}
