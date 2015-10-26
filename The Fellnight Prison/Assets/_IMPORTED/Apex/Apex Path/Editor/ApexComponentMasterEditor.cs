namespace Apex.Editor
{
    using System;
    using System.Linq;
    using Apex.Editor.Versioning;
    using UnityEditor;
    using UnityEngine;

    [CustomEditor(typeof(ApexComponentMaster), false), CanEditMultipleObjects]
    public class ApexComponentMasterEditor : Editor
    {
        private static GUIStyle _disabledLabel;

        private static GUIStyle disabledLabel
        {
            get
            {
                if (_disabledLabel == null)
                {
                    _disabledLabel = new GUIStyle(EditorStyles.label);
                    _disabledLabel.normal.textColor = Color.gray;
                }

                return _disabledLabel;
            }
        }

        public override void OnInspectorGUI()
        {
            ApexSettings settings;
            if (ApexSettings.TryGetSettings(out settings))
            {
                var info = new IconInfo
                {
                    key = "ApexPath",
                    path = string.Concat(settings.relativeDataFolder, "/ApexPath.png")
                };

                var icon = ProductManager.GetIcon(info);

                EditorGUILayout.LabelField(new GUIContent(" Apex Components", icon));
            }
            else
            {
                EditorGUILayout.LabelField("Apex Components");
            }

            EditorGUI.indentLevel++;

            var master = this.target as ApexComponentMaster;
            foreach (var category in master.componentCategories)
            {
                EditorGUILayout.BeginVertical("Box");
                EditorGUILayout.GetControlRect(true, 16f, EditorStyles.foldout);
                Rect foldRect = GUILayoutUtility.GetLastRect();

                category.isOpen = EditorGUI.Foldout(foldRect, category.isOpen, category.name, true);
                if (!category.isOpen)
                {
                    EditorGUILayout.EndVertical();
                    continue;
                }

                bool requiresCleanup = false;
                foreach (ApexComponentMaster.ComponentInfo c in category)
                {
                    if (c.component == null || c.component.Equals(null))
                    {
                        requiresCleanup = true;
                        continue;
                    }

                    var lblStyle = c.component.enabled ? EditorStyles.label : disabledLabel;

                    var visible = EditorGUILayout.ToggleLeft(c.name, c.isVisible, lblStyle);
                    if (visible != c.isVisible)
                    {
                        if (this.targets.Length > 1)
                        {
                            foreach (var t in this.targets)
                            {
                                ((ApexComponentMaster)t).Toggle(c.name, visible);
                            }
                        }
                        else
                        {
                            master.Toggle(c);
                        }

                        EditorUtility.SetDirty(master);
                    }
                }

                EditorGUILayout.EndVertical();

                if (requiresCleanup)
                {
                    foreach (var t in this.targets)
                    {
                        ((ApexComponentMaster)t).Cleanup();
                    }
                }
            }

            EditorGUI.indentLevel--;
            if (GUILayout.Button("Toggle All"))
            {
                foreach (var t in this.targets)
                {
                    var tmp = t as ApexComponentMaster;
                    tmp.ToggleAll();
                    EditorUtility.SetDirty(tmp);
                }
            }
        }

        private void OnEnable()
        {
            foreach (var t in this.targets)
            {
                var master = t as ApexComponentMaster;

                if (master.Init())
                {
                    EditorUtility.SetDirty(master);
                }
            }
        }

        private void OnDestroy()
        {
            if (this.target == null)
            {
                foreach (var go in Selection.gameObjects)
                {
                    var allApex = from c in go.GetComponents<MonoBehaviour>()
                                  where Attribute.IsDefined(c.GetType(), typeof(ApexComponentAttribute))
                                  select c;

                    foreach (var c in allApex)
                    {
                        c.hideFlags = HideFlags.None;
                    }
                }
            }
        }
    }
}
