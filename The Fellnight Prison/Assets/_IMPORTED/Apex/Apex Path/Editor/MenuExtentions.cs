namespace Apex.Editor
{
    using Apex.QuickStarts;
    using Apex.Services;
    using UnityEditor;
    using UnityEngine;

    public static class MenuExtentions
    {
        [MenuItem("GameObject/Create Other/Apex/Game World")]
        public static void GameWorldMenu()
        {
            GameObject go;

            var servicesInitializer = FindComponent<GameServicesInitializerComponent>();
            if (servicesInitializer != null)
            {
                go = servicesInitializer.gameObject;
            }
            else
            {
                go = new GameObject("Game World");
            }

            go.AddComponent<GameWorldQuickStart>();
        }

        [MenuItem("Tools/Apex/Upgrade Scene")]
        public static void CleanupMenu()
        {
            VersionUpgrader.Upgrade();
        }

        [MenuItem("Tools/Apex/Attributes Utility", false, 100)]
        public static void AttributesUtility()
        {
            EditorWindow.GetWindow<AttributesUtilityWindow>(true, "Apex Path - Attributes Utility");
        }

        [MenuItem("Tools/Apex/Grid Field Utility", false, 100)]
        public static void GridFieldUtility()
        {
            EditorWindow.GetWindow<GridSticherUtilityWindow>(true, "Apex Path - Grid Field Utility");
        }

        [MenuItem("Tools/Apex/Products", false, 200)]
        public static void ProductsWindow()
        {
            EditorWindow.GetWindow<ProductsWindow>(true, "Apex - Products");
        }

        private static T FindComponent<T>() where T : Component
        {
            var res = UnityEngine.Object.FindObjectsOfType(typeof(T));

            if (res != null && res.Length > 0)
            {
                return res[0] as T;
            }

            return null;
        }
    }
}
