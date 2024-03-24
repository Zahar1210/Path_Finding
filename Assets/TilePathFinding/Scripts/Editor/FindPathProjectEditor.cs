using UnityEditor;

namespace FindPath
{
    [CustomEditor(typeof(FindPathProject))]
    public class FindPathProjectEditor : Editor
    {
        #region Variables

        private SerializedProperty _tileSize;
        private SerializedProperty _directions;
        private SerializedProperty _obstacleColor;
        private SerializedProperty _noObstacleColor;
        private SerializedProperty _pathColor;

        private bool _showTileParameters;
        private bool _showFindPathParameters;
        private bool _showGizmosParameters;

        #endregion


        private void OnEnable()
        {
            _tileSize = serializedObject.FindProperty("tileSize");
            _directions = serializedObject.FindProperty("directions");
            _obstacleColor = serializedObject.FindProperty("obstacleColor");
            _noObstacleColor = serializedObject.FindProperty("noObstacleColor");
            _pathColor = serializedObject.FindProperty("pathColor");
        }

        public override void OnInspectorGUI()
        {
            DrawFindPathInspector();
        }

        private void DrawFindPathInspector()
        {
            serializedObject.Update();

            #region Parameters

            EditorGUILayout.BeginVertical("box");
            EditorGUI.indentLevel = 1;

            // #region FindPathParameters
            //
            // EditorGUILayout.BeginVertical("box");
            // EditorGUILayout.Space(3);
            //
            // _showFindPathParameters = EditorGUILayout.Foldout(_showFindPathParameters, "Find Path Parameters", true);
            // if (_showFindPathParameters)
            // {
            //     EditorGUI.indentLevel = 0;
            //     
            //     EditorGUILayout.PropertyField(_findMode);
            //
            //     EditorGUI.indentLevel = 1;
            // }
            //
            // EditorGUILayout.EndVertical();
            //
            // #endregion

            #region TileParameters

            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.Space(3);

            _showTileParameters = EditorGUILayout.Foldout(_showTileParameters, "Tile Parameters", true);
            if (_showTileParameters)
            {
                EditorGUI.indentLevel = 0;

                EditorGUILayout.PropertyField(_tileSize);
                EditorGUILayout.PropertyField(_directions);

                EditorGUI.indentLevel = 1;
            }

            EditorGUILayout.EndVertical();

            #endregion

            #region GizmosParameters

            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.Space(3);

            _showGizmosParameters = EditorGUILayout.Foldout(_showGizmosParameters, "Gizmos Parameters", true);
            if (_showGizmosParameters)
            {
                EditorGUI.indentLevel = 0;

                EditorGUILayout.PropertyField(_obstacleColor);
                EditorGUILayout.PropertyField(_noObstacleColor);
                EditorGUILayout.PropertyField(_pathColor);

                EditorGUI.indentLevel = 1;
            }

            EditorGUILayout.EndVertical();

            #endregion

            EditorGUILayout.EndVertical();

            #endregion

            serializedObject.ApplyModifiedProperties();
        }
    }
}