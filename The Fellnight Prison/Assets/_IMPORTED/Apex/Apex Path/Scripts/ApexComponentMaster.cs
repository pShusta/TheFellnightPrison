/* Copyright © 2014 Apex Software. All rights reserved. */
#pragma warning disable 1591
namespace Apex
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
#if NETFX_CORE
    using System.Reflection;
#endif
    using Apex.DataStructures;
    using Apex.Utilities;
    using UnityEngine;

    /// <summary>
    /// Consolidates Apex Components
    /// </summary>
    [AddComponentMenu("Apex/Common/Apex Component Master")]
    public class ApexComponentMaster : MonoBehaviour
    {
        private bool _initComplete;
        private Dictionary<int, ComponentInfo> _components = new Dictionary<int, ComponentInfo>();
        private Dictionary<string, ComponentCategory> _categories = new Dictionary<string, ComponentCategory>();

        [SerializeField]
        private bool _firstTime = true;

        /// <summary>
        /// Gets the component categories.
        /// </summary>
        /// <value>
        /// The component categories.
        /// </value>
        public IEnumerable<ComponentCategory> componentCategories
        {
            get
            {
                var sortedCategories = _categories.Keys.OrderBy(s => s);
                foreach (var catName in sortedCategories)
                {
                    yield return _categories[catName];
                }
            }
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        /// <returns></returns>
        public bool Init()
        {
            _initComplete = false;

            var all = this.GetComponents<MonoBehaviour>();

            foreach (var component in all)
            {
                Register(component);
            }

            var comparer = new FunctionComparer<ComponentInfo>((a, b) => a.name.CompareTo(b.name));
            foreach (var cat in _categories.Values)
            {
                cat.Sort(comparer);
            }

            _initComplete = true;

            if (_firstTime)
            {
                _firstTime = false;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Registers the specified component.
        /// </summary>
        /// <param name="component">The component.</param>
        public void Register(MonoBehaviour component)
        {
            if (component == null || component.Equals(null))
            {
                return;
            }

            var id = component.GetInstanceID();

            if (!_components.ContainsKey(id))
            {
                var type = component.GetType();
#if NETFX_CORE
                var aca = type.GetTypeInfo().GetCustomAttribute<ApexComponentAttribute>();
#else
                var aca = Attribute.GetCustomAttribute(type, typeof(ApexComponentAttribute)) as ApexComponentAttribute;
#endif
                if (aca == null)
                {
                    return;
                }

                if (_firstTime)
                {
                    component.hideFlags = HideFlags.HideInInspector;
                }

                var cinfo = new ComponentInfo
                {
                    component = component,
                    id = id,
                    name = type.Name.Replace("Component", string.Empty).ExpandFromPascal(),
                    isVisible = component.hideFlags != HideFlags.HideInInspector
                };

                _components.Add(id, cinfo);

                ComponentCategory cat;
                if (!_categories.TryGetValue(aca.category, out cat))
                {
                    cat = new ComponentCategory { name = aca.category };
                    _categories.Add(aca.category, cat);
                }

                cat.Add(cinfo);
                cinfo.category = cat;

                if (_initComplete)
                {
                    cat.Sort(new FunctionComparer<ComponentInfo>((a, b) => a.name.CompareTo(b.name)));
                    cat.isOpen = true;
                }
            }
        }

        /// <summary>
        /// Toggles the specified component.
        /// </summary>
        /// <param name="cinfo">The component info.</param>
        public void Toggle(ComponentInfo cinfo)
        {
            cinfo.isVisible = !cinfo.isVisible;

            cinfo.component.hideFlags = cinfo.isVisible ? HideFlags.None : HideFlags.HideInInspector;
        }

        /// <summary>
        /// Toggles the specified component by name.
        /// </summary>
        /// <param name="componentName">Name of the component.</param>
        /// <param name="visible">toggle visible or not.</param>
        public void Toggle(string componentName, bool visible)
        {
            _components
                .Values.Where(c => c.name == componentName)
                .Apply(c =>
                    {
                        c.isVisible = visible;
                        c.component.hideFlags = visible ? HideFlags.None : HideFlags.HideInInspector;
                    });
        }

        /// <summary>
        /// Toggles all components
        /// </summary>
        public void ToggleAll()
        {
            bool visible = !_components.Values.Any(c => c.isVisible);

            foreach (var c in _components.Values)
            {
                c.isVisible = visible;
                c.component.hideFlags = visible ? HideFlags.None : HideFlags.HideInInspector;
            }
        }

        /// <summary>
        /// Cleanups this instance.
        /// </summary>
        public void Cleanup()
        {
            ComponentInfo toRemove = null;
            foreach (var c in _components.Values)
            {
                if (c.component.Equals(null))
                {
                    toRemove = c;
                }
            }

            if (toRemove != null)
            {
                _components.Remove(toRemove.id);
                toRemove.category.Remove(toRemove);

                if (toRemove.category.count == 0)
                {
                    _categories.Remove(toRemove.category.name);
                }
            }
        }

        /// <summary>
        /// Category wrapper
        /// </summary>
        public class ComponentCategory : DynamicArray<ComponentInfo>
        {
            public bool isOpen;
            public string name;

            public ComponentCategory()
                : base(5)
            {
            }
        }

        public class ComponentInfo
        {
            public MonoBehaviour component;

            public ComponentCategory category;

            public string name;

            public int id;

            public bool isVisible;
        }
    }
}
