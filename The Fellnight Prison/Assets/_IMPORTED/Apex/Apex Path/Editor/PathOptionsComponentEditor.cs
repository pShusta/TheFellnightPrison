namespace Apex.Editor
{
    using Apex.PathFinding;
    using UnityEditor;
    using UnityEngine;

    [CustomEditor(typeof(PathOptionsComponent), false), CanEditMultipleObjects]
    public class PathOptionsComponentEditor : Editor
    {
        private SerializedProperty _pathingPriority;
        private SerializedProperty _usePathSmoothing;
        private SerializedProperty _allowCornerCutting;
        private SerializedProperty _preventDiagonalMoves;
        private SerializedProperty _navigateToNearestIfBlocked;
        private SerializedProperty _maxEscapeCellDistanceIfOriginBlocked;
        private SerializedProperty _nextNodeDistance;
        private SerializedProperty _requestNextWaypointDistance;
        private SerializedProperty _announceAllNodes;
        private SerializedProperty _replanMode;
        private SerializedProperty _replanInterval;

        public override void OnInspectorGUI()
        {
            if (Application.isPlaying)
            {
                EditorGUILayout.HelpBox("These settings cannot be edited in play mode.", MessageType.Info);
                return;
            }

            this.serializedObject.Update();

            EditorUtilities.Section("Path Finder Options");
            EditorGUILayout.PropertyField(_pathingPriority);
            EditorGUILayout.PropertyField(_usePathSmoothing);
            EditorGUILayout.PropertyField(_allowCornerCutting);
            EditorGUILayout.PropertyField(_preventDiagonalMoves);
            EditorGUILayout.PropertyField(_navigateToNearestIfBlocked);
            EditorGUILayout.PropertyField(_maxEscapeCellDistanceIfOriginBlocked);

            EditorUtilities.Section("Path Following");
            EditorGUILayout.PropertyField(_nextNodeDistance);
            EditorGUILayout.PropertyField(_requestNextWaypointDistance);
            EditorGUILayout.PropertyField(_announceAllNodes);

            EditorUtilities.Section("Replanning");
            EditorGUILayout.PropertyField(_replanMode);
            EditorGUILayout.PropertyField(_replanInterval);

            this.serializedObject.ApplyModifiedProperties();
        }

        private void OnEnable()
        {
            _pathingPriority = this.serializedObject.FindProperty("_pathingPriority");
            _usePathSmoothing = this.serializedObject.FindProperty("_usePathSmoothing");
            _allowCornerCutting = this.serializedObject.FindProperty("_allowCornerCutting");
            _preventDiagonalMoves = this.serializedObject.FindProperty("_preventDiagonalMoves");
            _navigateToNearestIfBlocked = this.serializedObject.FindProperty("_navigateToNearestIfBlocked");
            _maxEscapeCellDistanceIfOriginBlocked = this.serializedObject.FindProperty("_maxEscapeCellDistanceIfOriginBlocked");
            _nextNodeDistance = this.serializedObject.FindProperty("_nextNodeDistance");
            _requestNextWaypointDistance = this.serializedObject.FindProperty("_requestNextWaypointDistance");
            _announceAllNodes = this.serializedObject.FindProperty("_announceAllNodes");
            _replanMode = this.serializedObject.FindProperty("_replanMode");
            _replanInterval = this.serializedObject.FindProperty("_replanInterval");
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
