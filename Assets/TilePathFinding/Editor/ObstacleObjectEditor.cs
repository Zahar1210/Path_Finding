using UnityEditor;
using UnityEngine;

namespace FindPath
{
    // [CustomEditor(typeof(ObstacleObject))]
    // public class ObstacleObjectEditor : Editor
    // {
    //     #region SerializeFiedProperites
    //
    //     private SerializedProperty _obstacleType;
    //     private SerializedProperty _points;
    //     private SerializedProperty _updateError;
    //     private SerializedProperty _checkTime;
    //     private SerializedProperty _radius;
    //     private SerializedProperty _drawGizmos;
    //
    //     #endregion
    //
    //     private void OnEnable()
    //     {
    //         _radius = serializedObject.FindProperty("radius");
    //         _obstacleType = serializedObject.FindProperty("obstacleObjectType");
    //         _points = serializedObject.FindProperty("points");
    //         _updateError = serializedObject.FindProperty("updateError");
    //         _checkTime = serializedObject.FindProperty("checkTime");
    //         _drawGizmos = serializedObject.FindProperty("drawGizmos");
    //     }
    //
    //     public override void OnInspectorGUI()
    //     {
    //         EditorGUILayout.Space(9);
    //         ObstacleObject obstacleObject = (ObstacleObject)target;
    //
    //         serializedObject.Update();
    //         
    //         EditorGUILayout.PropertyField(_obstacleType);
    //         EditorGUILayout.PropertyField(_drawGizmos);
    //
    //         EditorGUILayout.Space(5);
    //         
    //         EditorGUILayout.PropertyField(_points);
    //         if (obstacleObject.points == null || obstacleObject.points.Length == 0)
    //         {
    //             if (GUILayout.Button("Set points"))
    //             {
    //                 // obstacleObject.
    //             }
    //         }
    //
    //         EditorGUILayout.Space(5);
    //
    //         if (obstacleObject.obstacleObjectType == ObstacleObjectType.Dynamic)
    //         {
    //             EditorGUILayout.PropertyField(_updateError);
    //             EditorGUILayout.PropertyField(_checkTime);
    //         }
    //         
    //         EditorGUILayout.PropertyField(_radius);
    //
    //         EditorGUILayout.PropertyField(_checkTime);
    //         
    //         serializedObject.ApplyModifiedProperties();
    //     }
    // }
}
