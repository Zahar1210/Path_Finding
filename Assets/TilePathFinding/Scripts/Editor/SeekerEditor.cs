using UnityEditor;
using UnityEngine;

namespace FindPath
{
    [CustomEditor(typeof(Agent))]
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

        private bool _showModes;
        private bool _showModePrams;
        
        private GUIStyle _headerStyle;
        
        #endregion
        
        private void OnEnable()
        {
            _count = serializedObject.FindProperty("_count");
            _angle = serializedObject.FindProperty("_angle");
            _radius = serializedObject.FindProperty("_radius");
            _findMode = serializedObject.FindProperty("_pathFindMode");
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
            
            Agent agent = (Agent)target;

            EditorGUILayout.BeginVertical("box");
            EditorGUI.indentLevel = 1;
            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.Space(3);
            
            _showModes = EditorGUILayout.Foldout(_showModes, "Mode", true);
            if (_showModes)
            {
                EditorGUI.indentLevel = 0;
                
                EditorGUILayout.PropertyField(_findPathProject);
                EditorGUILayout.Space(3);
                EditorGUILayout.PropertyField(_findMode);
                EditorGUILayout.PropertyField(_pathTrigger);
                EditorGUILayout.PropertyField(_pathReason);
                EditorGUILayout.PropertyField(_pathDynamic);

                EditorGUI.indentLevel = 1;
            }
            EditorGUILayout.EndVertical();
            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.Space(3);
            
            _showModePrams = EditorGUILayout.Foldout(_showModePrams, "Mode Parameters", true);
            if (_showModePrams)
            {
                EditorGUI.indentLevel = 0;
                
                EditorGUILayout.Space(5);
                EditorGUILayout.LabelField("Path Trigger Params:" + " " + agent.PathTrigger, EditorStyles.boldLabel);
                
                switch (agent.PathTrigger)
                {
                    case PathTrigger.TargetPosition:
                        EditorGUILayout.PropertyField(_targetLayer);
                        EditorGUILayout.PropertyField(_targetType);
                        
                        if (agent.TargetType == TargetType.ArrayMode)
                        {
                            EditorGUILayout.PropertyField(_arrayTargets);
                        }
                        else if (agent.TargetType == TargetType.SoloMode)
                        {
                            EditorGUILayout.PropertyField(_soloTarget);
                        }
                        
                        EditorGUILayout.PropertyField(_targetDirection);
                        EditorGUILayout.PropertyField(_targetSelectMode);
                        EditorGUILayout.PropertyField(_checkInterval);
                        EditorGUILayout.PropertyField(_count);
                        break;
                    case PathTrigger.MouseInput:
                        EditorGUILayout.PropertyField(_mouseLayerMask);
                        EditorGUILayout.PropertyField(_mouseSide);
                        break;
                    case PathTrigger.MousePosition:
                        EditorGUILayout.PropertyField(_mouseLayerMask);
                        break;
                }
                
                EditorGUILayout.Space(5);
                EditorGUILayout.LabelField("Path Reason Params:" + " " + agent.PathReason, EditorStyles.boldLabel);
                
                switch (agent.PathReason)
                {
                    case PathReason.Always:
                        // Код для случая, когда PathReason равно Always
                        break;
                    case PathReason.Distance:
                        EditorGUILayout.PropertyField(_maxDistance);
                        break;
                    case PathReason.FieldOfViewAngle:
                        EditorGUILayout.PropertyField(_targetLayerMask);
                        EditorGUILayout.PropertyField(_radius);
                        EditorGUILayout.PropertyField(_angle);
                        EditorGUILayout.PropertyField(_obstacleLayerMask);
                        break;
                    case PathReason.FieldOfViewOverlap:
                        EditorGUILayout.PropertyField(_radius);
                        EditorGUILayout.PropertyField(_targetLayerMask);
                        break;
                    case PathReason.Radius:
                        EditorGUILayout.PropertyField(_differenceX);
                        EditorGUILayout.PropertyField(_differenceY);
                        EditorGUILayout.PropertyField(_differenceZ);
                        break;
                }
                
                EditorGUILayout.Space(5);
                EditorGUILayout.LabelField("Path Dynamic Params:" + " " + agent.PathDynamic, EditorStyles.boldLabel);

                switch (agent.PathDynamic)
                {
                    
                    case  PathDynamic.ChangeObstacle:
                        EditorGUILayout.LabelField("Path Dynamic ChangeObstacle", EditorStyles.whiteLabel);
                        break;
                    case  PathDynamic.CombinedIntervalTarget:
                        EditorGUILayout.LabelField("Path Dynamic CombinedIntervalTarget", EditorStyles.whiteLabel);

                        break;
                    case  PathDynamic.CombinedIntervalObstacle:
                        EditorGUILayout.LabelField("Path Dynamic CombinedIntervalObstacle", EditorStyles.whiteLabel);

                        break;
                    case PathDynamic.Initial:
                        EditorGUILayout.LabelField("Path Dynamic Initial", EditorStyles.whiteLabel);
                        break;
                    case PathDynamic.Interval:
                        EditorGUILayout.LabelField("Path Dynamic Interval", EditorStyles.whiteLabel);
                        break;
                }

                
                EditorGUI.indentLevel = 1;
            }
            
            EditorGUILayout.EndVertical();
            
            EditorGUILayout.EndVertical();
            
            serializedObject.ApplyModifiedProperties();
        }
    }
}