/* Copyright © 2014 Apex Software. All rights reserved. */

namespace Apex.QuickStarts
{
    using System;
    using Apex.Steering;
    using UnityEngine;

    /// <summary>
    /// Base class for quick starts.
    /// </summary>
    [ExecuteInEditMode]
    public abstract class QuickStartBase : MonoBehaviour
    {
        private void Awake()
        {
            Setup();

            DestroyImmediate(this);
        }

        /// <summary>
        /// Sets up component on which the quick start is attached.
        /// </summary>
        protected abstract void Setup();

        /// <summary>
        /// Finds the first component of the specified type in the project.
        /// </summary>
        /// <typeparam name="T">The type of component to look for</typeparam>
        /// <returns>The component or null i not found</returns>
        protected T FindComponent<T>() where T : Component
        {
            var res = FindObjectsOfType(typeof(T));

            if (res != null && res.Length > 0)
            {
                return res[0] as T;
            }

            return null;
        }

        /// <summary>
        /// Adds a component of the specified type if it does not already exist
        /// </summary>
        /// <typeparam name="T">The type of component to add</typeparam>
        /// <param name="target">The target to which the component will be added.</param>
        /// <param name="globalSearch">if set to <c>true</c> the check to see if the component already exists will be done in the entire project, otherwise it will check the <paramref name="target"/>.</param>
        /// <param name="component">The component that was added.</param>
        /// <returns><c>true</c> if the component was added, otherwise <c>false</c></returns>
        protected bool AddIfMissing<T>(GameObject target, bool globalSearch, out T component) where T : Component
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

        /// <summary>
        /// Adds a component of the specified type if it does not already exist
        /// </summary>
        /// <typeparam name="T">The type of component to add</typeparam>
        /// <param name="target">The target to which the component will be added.</param>
        /// <param name="globalSearch">if set to <c>true</c> the check to see if the component already exists will be done in the entire project, otherwise it will check the <paramref name="target"/>.</param>
        /// <returns><c>true</c> if the component was added, otherwise <c>false</c></returns>
        protected bool AddIfMissing<T>(GameObject target, bool globalSearch) where T : Component
        {
            T component;
            return AddIfMissing(target, globalSearch, out component);
        }

        /// <summary>
        /// Adds a steering component of the specified type if it does not already exist
        /// </summary>
        /// <typeparam name="T">The type of component to add</typeparam>
        /// <param name="target">The target to which the component will be added.</param>
        /// <param name="priority">The priority.</param>
        /// <returns><c>true</c> if the component was added, otherwise <c>false</c></returns>
        protected bool AddIfMissing<T>(GameObject target, int priority) where T : SteeringComponent
        {
            T component;
            if (AddIfMissing(target, false, out component))
            {
                component.priority = priority;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Adds a component of the specified type if it does not already exist
        /// </summary>
        /// <typeparam name="T">>The type of component to add</typeparam>
        /// <typeparam name="TMarkerAttribute">The type of the marker attribute that identifies the component type</typeparam>
        /// <param name="target">The target to which the component will be added.</param>
        /// <param name="globalSearch">if set to <c>true</c> the check to see if the component already exists will be done in the entire project, otherwise it will check the <paramref name="target"/>.</param>
        /// <param name="component">The component that was added.</param>
        /// <returns><c>true</c> if the component was added, otherwise <c>false</c></returns>
        protected bool AddIfMissing<T, TMarkerAttribute>(GameObject target, bool globalSearch, out T component)
            where T : Component
            where TMarkerAttribute : Attribute
        {
            component = null;
            UnityEngine.Object[] candidates;
            if (globalSearch)
            {
                candidates = FindObjectsOfType(typeof(MonoBehaviour));
            }
            else
            {
                candidates = target.GetComponents<MonoBehaviour>();
            }

            foreach (var mb in candidates)
            {
#if NETFX_CORE
                //For some reason this causes an issue with Win8 store, even though it is the exact same code as elsewhere.
                //It is however totally irrelevant since this is actually editor stuff.
                //var typeInf = mb.GetType().GetTypeInfo();
                //if (typeInf.CustomAttributes.Any(a => a.AttributeType == typeof(TMarkerAttribute)))
                //{
                //    return false;
                //}
#else
                if (Attribute.IsDefined(mb.GetType(), typeof(TMarkerAttribute)))
                {
                    return false;
                }
#endif
            }

            component = target.AddComponent<T>();
            return true;
        }

        /// <summary>
        /// Adds a component of the specified type if it does not already exist
        /// </summary>
        /// <typeparam name="T">>The type of component to add</typeparam>
        /// <typeparam name="TMarkerAttribute">The type of the marker attribute that identifies the component type</typeparam>
        /// <param name="target">The target to which the component will be added.</param>
        /// <param name="globalSearch">if set to <c>true</c> the check to see if the component already exists will be done in the entire project, otherwise it will check the <paramref name="target"/>.</param>
        /// <returns><c>true</c> if the component was added, otherwise <c>false</c></returns>
        protected bool AddIfMissing<T, TMarkerAttribute>(GameObject target, bool globalSearch)
            where T : Component
            where TMarkerAttribute : Attribute
        {
            T component;
            return AddIfMissing<T, TMarkerAttribute>(target, globalSearch, out component);
        }

        /// <summary>
        /// Removes a single component of type T on the specifed game object.
        /// </summary>
        /// <typeparam name="T">The type parameter, i.e. what type of component should be removed.</typeparam>
        /// <param name="go">The game object on which the expected component is.</param>
        /// <returns><c>True</c> if it finds and destroys the desired component, <c>false</c> otherwise.</returns>
        protected bool NukeSingle<T>(GameObject go) where T : Component
        {
            var res = go.GetComponent<T>();
            if (res != null)
            {
                DestroyImmediate(res, true);
                return true;
            }

            return false;
        }
    }
}