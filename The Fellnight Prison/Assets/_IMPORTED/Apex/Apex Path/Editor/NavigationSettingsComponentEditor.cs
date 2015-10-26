/* Copyright © 2014 Apex Software. All rights reserved. */

namespace Apex.Editor
{
    using Apex.Steering;
    using UnityEditor;
    using UnityEngine;

    [CustomEditor(typeof(NavigationSettingsComponent), false)]
    public partial class NavigationSettingsComponentEditor : Editor
    {
        private SerializedProperty _heightSampling;
        private SerializedProperty _heightMapDetail;
        private SerializedProperty _heightSamplingGranularity;
        private SerializedProperty _ledgeThreshold;
        private SerializedProperty _useGlobalHeightNavigationSettings;
        private SerializedProperty _unitsHeightNavigationCapability;

        public override void OnInspectorGUI()
        {
            if (Application.isPlaying)
            {
                EditorGUILayout.HelpBox("These settings cannot be edited in play mode.", MessageType.Info);
                return;
            }

            this.serializedObject.Update();
            EditorUtilities.Section("Height Sampling");
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(_heightSampling);
            if (EditorGUI.EndChangeCheck())
            {
                DefaultHeightNavigatorEditor.EnsureValidProviders((HeightSamplingMode)_heightSampling.intValue);
            }

            if (_heightSampling.intValue == (int)HeightSamplingMode.HeightMap)
            {
                EditorGUILayout.PropertyField(_heightMapDetail);
            }

            if (_heightSampling.intValue != (int)HeightSamplingMode.NoHeightSampling)
            {
                EditorGUILayout.PropertyField(_heightSamplingGranularity);
                EditorGUILayout.PropertyField(_ledgeThreshold);

                EditorGUILayout.Separator();
                EditorGUI.indentLevel--;
                EditorGUILayout.PropertyField(_useGlobalHeightNavigationSettings);
                if (_useGlobalHeightNavigationSettings.boolValue)
                {
                    EditorGUILayout.PropertyField(_unitsHeightNavigationCapability, GUIContent.none, true);
                }
            }

            InspectorGUI();

            this.serializedObject.ApplyModifiedProperties();

            if (GUI.changed)
            {
                var t = this.target as NavigationSettingsComponent;
                t.Refresh();
            }
        }

        partial void Initialize();

        partial void InspectorGUI();

        private void OnEnable()
        {
            _heightSampling = this.serializedObject.FindProperty("heightSampling");
            _heightMapDetail = this.serializedObject.FindProperty("heightMapDetail");
            _heightSamplingGranularity = this.serializedObject.FindProperty("heightSamplingGranularity");
            _ledgeThreshold = this.serializedObject.FindProperty("ledgeThreshold");
            _useGlobalHeightNavigationSettings = this.serializedObject.FindProperty("useGlobalHeightNavigationSettings");
            _unitsHeightNavigationCapability = this.serializedObject.FindProperty("unitsHeightNavigationCapability");

            Initialize();
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