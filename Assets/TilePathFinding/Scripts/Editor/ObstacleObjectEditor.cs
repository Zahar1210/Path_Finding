using UnityEditor;

namespace FindPath
{
    [CustomEditor(typeof(ObstacleObject))]
    public class ObstacleObjectEditor : Editor
    {
        #region SerializeFiedProperites
    
        private SerializedProperty _obstacleType;
        private SerializedProperty _checkTime;
        private SerializedProperty _colliders;
        private SerializedProperty _layerMask;
        private SerializedProperty _checkSize;
    
        #endregion
    
        private void OnEnable()
        {
            _obstacleType = serializedObject.FindProperty("obstacleObjectType");
            _checkTime = serializedObject.FindProperty("checkInterval");
            _colliders = serializedObject.FindProperty("colliders");
            _layerMask = serializedObject.FindProperty("layerMask");
            _checkSize = serializedObject.FindProperty("checkRadius");
        }
        
        public override void OnInspectorGUI()
        {
            // EditorGUILayout.Space(9);
            ObstacleObject obstacleObjects = (ObstacleObject)target;
            
            serializedObject.Update();
            
            EditorGUILayout.PropertyField(_obstacleType);
            EditorGUILayout.PropertyField(_colliders);
            EditorGUILayout.PropertyField(_layerMask);
            EditorGUILayout.PropertyField(_checkSize);
            
            EditorGUILayout.Space(3);
            
            if (obstacleObjects.ObstacleObjectType == ObstacleObjectType.Dynamic)
            {
                EditorGUILayout.PropertyField(_checkTime);
            }
            
            serializedObject.ApplyModifiedProperties();
        }
    }
}
