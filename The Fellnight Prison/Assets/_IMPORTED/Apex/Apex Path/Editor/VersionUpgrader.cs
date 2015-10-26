/* Copyright © 2014 Apex Software. All rights reserved. */
namespace Apex.Editor
{
    using System;
    using System.Collections.Generic;
    using Apex;
    using Apex.PathFinding;
    using Apex.Services;
    using Apex.Steering;
    using Apex.Steering.Components;
    using Apex.Steering.HeightNavigation;
    using Apex.Units;
    using Apex.WorldGeometry;
    using UnityEditor;
    using UnityEngine;

#pragma warning disable 0618
    public static partial class VersionUpgrader
    {
        private delegate void Initializer<T>(GameObject go, T item, bool added);

        internal static void Upgrade()
        {
            Replace<TurnableUnitComponent, SteerToAlignWithVelocity>(
                (a, b) =>
                {
                    b.alignWithElevation = (a.ignoreAxis == WorldGeometry.Axis.None);
                });

            var nsc = AddGameWorldItem<NavigationSettingsComponent>(
                (gw, ns, added) =>
                {
                    if (added)
                    {
                        var grid = FindComponent<GridComponent>();
                        if (grid == null)
                        {
                            return;
                        }

                        ns.heightSamplingGranularity = grid.heightGranularity;

                        var unitnav = ns.unitsHeightNavigationCapability;
                        unitnav.maxClimbHeight = grid.maxScaleHeight;
                        unitnav.maxSlopeAngle = grid.maxWalkableSlopeAngle;
                        ns.unitsHeightNavigationCapability = unitnav;
                    }

                    //Fix old sphere cast option
                    if ((int)ns.heightSampling == 3)
                    {
                        ns.heightSampling = HeightSamplingMode.Raycast;
                    }
                });

            //Ensure each unit has a height navigator
            var suc = GetAllNonPrefabInstances<SteerableUnitComponent>();
            foreach (var c in suc)
            {
                if (c.As<IHeightNavigator>() == null)
                {
                    var dhn = c.gameObject.AddComponent<DefaultHeightNavigator>();

                    dhn.gravity = c.gravity;
                    dhn.groundStickynessFactor = c.groundStickynessFactor;
                    dhn.terminalVelocity = c.terminalVelocity;

                    DefaultHeightNavigatorEditor.EnsureValidProvider(dhn, nsc.heightSampling);

                    //Set the default constraints of the rigidbody, we no longer want to force this on awake
                    var rb = c.GetComponent<Rigidbody>();
                    if (rb != null)
                    {
                        rb.constraints |= RigidbodyConstraints.FreezeRotation;
                    }
                }
            }

            //Get path finder options from steer for path
            var sfp = GetAllNonPrefabInstances<SteerForPathComponent>();
            FixPathOptions(sfp);

            //Get selection visual from selectable unit if present
            var selectables = GetAllNonPrefabInstances<SelectableUnitComponent>();
            FixSelectables(selectables);

            //Set any basic avoidance to the same prio as steer for path
            var sfba = GetAllNonPrefabInstances<SteerForBasicAvoidanceComponent>();
            foreach (var a in sfba)
            {
                if (a.priority == 0)
                {
                    a.priority = 5;
                }
            }

            var units = GetAllNonPrefabInstances<UnitComponent>();
            ApexComponentMaster m;
            foreach (var u in units)
            {
                if (AddIfMissing<ApexComponentMaster>(u.gameObject, false, out m))
                {
                    while (UnityEditorInternal.ComponentUtility.MoveComponentUp(m))
                    {
                        /* NOOP */
                    }
                }
            }

            AddGameWorldItem<ApexComponentMaster>();

            Debug.Log("Scene and Prefabs were successfully updated to the latest version of Apex Path.");
            Debug.LogWarning("Please note that all prefab instances that had modified properties have been reset to the values of their prefab (Only applies to Apex Path Components).");

            UpdateSteer();
        }

        static partial void UpdateSteer();

        private static void FixPathOptions(IEnumerable<SteerForPathComponent> sfp)
        {
            foreach (var source in sfp)
            {
                if (source.priority == 0)
                {
                    source.priority = 5;
                }

                var go = source.gameObject;

                PathOptionsComponent po;
                if (AddIfMissing<PathOptionsComponent>(go, false, out po))
                {
                    po.allowCornerCutting = source.allowCornerCutting;
                    po.maxEscapeCellDistanceIfOriginBlocked = source.maxEscapeCellDistanceIfOriginBlocked;
                    po.navigateToNearestIfBlocked = source.navigateToNearestIfBlocked;
                    po.pathingPriority = source.pathingPriority;
                    po.preventDiagonalMoves = source.preventDiagonalMoves;
                    po.usePathSmoothing = source.usePathSmoothing;
                    po.replanInterval = source.replanInterval;
                    po.replanMode = source.replanMode;
                    po.requestNextWaypointDistance = source.requestNextWaypointDistance;
                    po.nextNodeDistance = source.nextNodeDistance;
                    po.announceAllNodes = source.announceAllNodes;
                }
            }
        }

        private static void FixSelectables(IEnumerable<SelectableUnitComponent> selectables)
        {
            foreach (var source in selectables)
            {
                var go = source.gameObject;

                var unit = go.GetComponent<UnitComponent>();
                unit.isSelectable = true;
                unit.selectionVisual = source.selectionVisual;

                Component.DestroyImmediate(source, true);
            }
        }

        private static void Nuke<T>() where T : Component
        {
            var res = Resources.FindObjectsOfTypeAll<T>();
            foreach (var c in res)
            {
                Component.DestroyImmediate(c, true);
            }
        }

        private static void Replace<T, TNew>(Action<T, TNew> configure = null)
            where T : Component
            where TNew : Component
        {
            var res = GetAllNonPrefabInstances<T>();
            foreach (var c in res)
            {
                var go = c.gameObject;
                var cnew = go.AddComponent<TNew>();

                if (configure != null)
                {
                    configure(c, cnew);
                }

                Component.DestroyImmediate(c, true);
            }
        }

        private static T AddGameWorldItem<T>(Initializer<T> init = null) where T : Component
        {
            GameObject gameWorld = null;

            var servicesInitializer = FindComponent<GameServicesInitializerComponent>();
            if (servicesInitializer != null)
            {
                gameWorld = servicesInitializer.gameObject;

                T item;
                var added = AddIfMissing<T>(gameWorld, false, out item);

                if (init != null)
                {
                    init(gameWorld, item, added);
                }

                return item;
            }

            return null;
        }

        private static void AddItem<T>(GameObject target, Initializer<T> init = null) where T : Component
        {
            T item;
            var added = AddIfMissing<T>(target, false, out item);

            if (init != null)
            {
                init(target, item, added);
            }
        }

        private static bool AddIfMissing<T>(GameObject target, bool globalSearch) where T : Component
        {
            T component;
            return AddIfMissing<T>(target, globalSearch, out component);
        }

        private static bool AddIfMissing<T>(GameObject target, bool globalSearch, out T component) where T : Component
        {
            if (globalSearch)
            {
                component = FindComponent<T>();
            }
            else
            {
                component = target.GetComponent<T>();
            }

            if (component == null)
            {
                component = target.AddComponent<T>();
                return true;
            }

            return false;
        }

        private static IEnumerable<T> GetAllNonPrefabInstances<T>() where T : UnityEngine.Object
        {
            var items = Resources.FindObjectsOfTypeAll<T>();
            for (int i = 0; i < items.Length; i++)
            {
                if (items[i] != null && !items[i].Equals(null) && PrefabUtility.GetPrefabType(items[i]) != PrefabType.PrefabInstance)
                {
                    yield return items[i];
                }
            }
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
