/* Copyright © 2014 Apex Software. All rights reserved. */

namespace Apex.Editor
{
    using Apex.WorldGeometry;
    using UnityEditor;
    using UnityEngine;

    [CustomEditor(typeof(LayerMappingComponent), false), CanEditMultipleObjects]
    public class LayerMappingComponentEditor : Editor
    {
        private SerializedProperty _staticObstacleLayer;
        private SerializedProperty _terrainLayer;
        private SerializedProperty _unitLayer;

        public override void OnInspectorGUI()
        {
            if (Application.isPlaying)
            {
                EditorGUILayout.HelpBox("These settings cannot be edited in play mode.", MessageType.Info);
                return;
            }

            this.serializedObject.Update();
            EditorGUILayout.Separator();
            EditorGUILayout.PropertyField(_staticObstacleLayer);
            EditorGUILayout.PropertyField(_terrainLayer);
            EditorGUILayout.PropertyField(_unitLayer);
            this.serializedObject.ApplyModifiedProperties();
        }

        private void OnEnable()
        {
            _staticObstacleLayer = this.serializedObject.FindProperty("staticObstacleLayer");
            _terrainLayer = this.serializedObject.FindProperty("terrainLayer");
            _unitLayer = this.serializedObject.FindProperty("unitLayer");
        }

        private void OnDestroy()
        {
            if (this.target == null)
            {
                EditorUtilities.CleanupComponentMaster();
            }
        }
    }
}
