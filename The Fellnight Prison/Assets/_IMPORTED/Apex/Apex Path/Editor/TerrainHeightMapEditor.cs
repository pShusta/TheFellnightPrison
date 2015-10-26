namespace Apex.Editor
{
    using Apex.WorldGeometry;
    using UnityEditor;
    using UnityEngine;

    [CustomEditor(typeof(TerrainHeightMap), false)]
    public class TerrainHeightMapEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            if (Application.isPlaying)
            {
                EditorGUILayout.HelpBox("These settings cannot be edited in play mode.", MessageType.Info);
                return;
            }

            DrawDefaultInspector();
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
