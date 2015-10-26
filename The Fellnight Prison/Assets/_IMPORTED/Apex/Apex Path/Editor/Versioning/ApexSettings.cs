/* Copyright © 2014 Apex Software. All rights reserved. */
namespace Apex.Editor.Versioning
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using UnityEditor;
    using UnityEngine;

    public class ApexSettings : ScriptableObject
    {
        private static ApexSettings _instance;

        [SerializeField, HideInInspector]
        private bool _allowAutomaticUpdateCheck;

        [SerializeField, HideInInspector]
        private int _updateCheckIntervalHours = 12;

        [SerializeField, HideInInspector]
        private string _lastUpdateCheck;

        [SerializeField, HideInInspector]
        private string _updateCheckBaseUrl = "http://apexgametoolsproductservice.azurewebsites.net";

        [SerializeField, HideInInspector]
        private string[] _knownPendingUpdates;

        private bool _isDirty;

        internal bool allowAutomaticUpdateCheck
        {
            get
            {
                return _allowAutomaticUpdateCheck;
            }

            set
            {
                if (_allowAutomaticUpdateCheck != value)
                {
                    _allowAutomaticUpdateCheck = value;
                    _isDirty = true;
                }
            }
        }

        internal int updateCheckIntervalHours
        {
            get
            {
                return _updateCheckIntervalHours;
            }

            set
            {
                if (_updateCheckIntervalHours != value)
                {
                    _updateCheckIntervalHours = value;
                    _isDirty = true;
                }
            }
        }

        internal string updateCheckBaseUrl
        {
            get { return _updateCheckBaseUrl; }
        }

        internal DateTime? lastUpdateCheck
        {
            get
            {
                if (string.IsNullOrEmpty(_lastUpdateCheck))
                {
                    return null;
                }

                return DateTime.Parse(_lastUpdateCheck, DateTimeFormatInfo.InvariantInfo, DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal);
            }
        }

        internal bool timeToUpdate
        {
            get
            {
                if (string.IsNullOrEmpty(_lastUpdateCheck) || !_allowAutomaticUpdateCheck)
                {
                    return _allowAutomaticUpdateCheck;
                }

                return (DateTime.UtcNow - this.lastUpdateCheck.Value).TotalHours > _updateCheckIntervalHours;
            }
        }

        internal string rootFolder
        {
            get;
            private set;
        }

        internal string dataFolder
        {
            get;
            private set;
        }

        internal string relativeDataFolder
        {
            get;
            private set;
        }

        internal bool isDirty
        {
            get { return _isDirty; }
        }

        internal static bool TryGetSettings(out ApexSettings settings)
        {
            if (_instance != null)
            {
                settings = _instance;
                return true;
            }

            string assetsFolder = Application.dataPath;

            var locations = Directory.GetFiles(assetsFolder, typeof(ApexSettings).Name + ".cs", SearchOption.AllDirectories);
            if (locations.Length != 1)
            {
                settings = null;
                return false;
            }

            var tmp = Path.GetDirectoryName(locations[0]);
            var apexRoot = NormalizePath(tmp.Substring(0, tmp.LastIndexOf("Apex Path", StringComparison.OrdinalIgnoreCase)));
            var dataFolder = CombinePath(apexRoot, "Editor/Data");
            var relativeDataFolder = dataFolder.Substring(dataFolder.IndexOf("Assets", StringComparison.OrdinalIgnoreCase));
            string settingsPath = CombinePath(relativeDataFolder, "ApexSettings.asset");

            if (string.IsNullOrEmpty(AssetDatabase.AssetPathToGUID(relativeDataFolder)))
            {
                var subhierarchy = relativeDataFolder.Split('/');
                var parent = subhierarchy[0];
                for (int i = 1; i < subhierarchy.Length; i++)
                {
                    var subFolder = subhierarchy[i];
                    var subPath = CombinePath(parent, subFolder);
                    if (string.IsNullOrEmpty(AssetDatabase.AssetPathToGUID(subPath)))
                    {
                        AssetDatabase.CreateFolder(parent, subFolder);
                    }

                    parent = subPath;
                }
            }
            else
            {
#if UNITY_5
                _instance = AssetDatabase.LoadAssetAtPath<ApexSettings>(settingsPath);
#else
                _instance = Resources.LoadAssetAtPath<ApexSettings>(settingsPath);
#endif

                if (_instance != null)
                {
                    _instance.rootFolder = apexRoot;
                    _instance.dataFolder = dataFolder;
                    _instance.relativeDataFolder = relativeDataFolder;
                    settings = _instance;
                    return true;
                }
            }

            //No settings found so create some
            _instance = ScriptableObject.CreateInstance<ApexSettings>();

            AssetDatabase.CreateAsset(_instance, settingsPath);
            AssetDatabase.SaveAssets();

            _instance.rootFolder = apexRoot;
            _instance.dataFolder = dataFolder;
            _instance.relativeDataFolder = relativeDataFolder;
            settings = _instance;
            return false;
        }

        internal HashSet<string> GetKnownPendingUpdates()
        {
            if (_knownPendingUpdates != null)
            {
                return new HashSet<string>(_knownPendingUpdates);
            }

            return new HashSet<string>();
        }

        internal void UpdateKnowPendingUpdates(string[] pendingUpdates)
        {
            _knownPendingUpdates = pendingUpdates;
            _isDirty = true;
        }

        internal void UpdateCompleted(string updatedCheckUrl)
        {
            _lastUpdateCheck = DateTime.UtcNow.ToString(DateTimeFormatInfo.InvariantInfo);

            if (!string.IsNullOrEmpty(updatedCheckUrl))
            {
                _updateCheckBaseUrl = updatedCheckUrl;
            }

            _isDirty = true;
        }

        internal void SaveChanges()
        {
            if (_isDirty)
            {
                EditorUtility.SetDirty(this);
                AssetDatabase.SaveAssets();
                _isDirty = false;
            }
        }

        private static string CombinePath(string partOne, string partTwo)
        {
            return string.Concat(
                NormalizePath(partOne),
                "/", 
                NormalizePath(partTwo));
        }

        private static string NormalizePath(string path)
        {
            return path.Replace('\\', '/').TrimEnd('/');
        }
    }
}
