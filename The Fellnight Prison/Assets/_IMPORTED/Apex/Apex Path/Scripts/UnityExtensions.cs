/* Copyright © 2014 Apex Software. All rights reserved. */
namespace Apex
{
    using Apex.PathFinding;
    using Apex.Services;
    using Apex.Units;
    using Apex.WorldGeometry;
    using UnityEngine;

    /// <summary>
    /// Various extension to Unity types.
    /// </summary>
    public static class UnityExtensions
    {
        private static readonly Plane _xzPlane = new Plane(Vector3.up, Vector3.zero);

        /// <summary>
        /// Gets the collider at position.
        /// </summary>
        /// <param name="cam">The camera.</param>
        /// <param name="screenPos">The screen position.</param>
        /// <param name="layerMask">The layer mask.</param>
        /// <param name="maxDistance">The maximum distance.</param>
        /// <returns>The first collider found in the game world at the specified screen position.</returns>
        public static Collider GetColliderAtPosition(this Camera cam, Vector3 screenPos, LayerMask layerMask, float maxDistance = 1000.0f)
        {
            RaycastHit hit;
            var ray = cam.ScreenPointToRay(screenPos);

            if (Physics.Raycast(ray, out hit, maxDistance, layerMask))
            {
                return hit.collider;
            }

            return null;
        }

        /// <summary>
        /// Casts a ray from the camera to the specified position.
        /// </summary>
        /// <param name="cam">The camera.</param>
        /// <param name="screenPos">The screen position.</param>
        /// <param name="layerMask">The layer mask.</param>
        /// <param name="maxDistance">The maximum distance.</param>
        /// <param name="hit">The hit details.</param>
        /// <returns><c>true</c> if the ray hit something, otherwise <c>false</c></returns>
        public static bool ScreenToLayerHit(this Camera cam, Vector3 screenPos, LayerMask layerMask, float maxDistance, out RaycastHit hit)
        {
            var ray = cam.ScreenPointToRay(screenPos);
            return Physics.Raycast(ray, out hit, maxDistance, layerMask);
        }

        /// <summary>
        /// Casts a ray from the camera to the xz plane through the specified screen point and returns the point the ray intersects the xz plane in world coordinates.
        /// </summary>
        /// <param name="cam">The camera.</param>
        /// <param name="screenPos">The screen position.</param>
        /// <returns>The intersection point on the xz plane in world coordinates</returns>
        public static Vector3 ScreenToGroundPoint(this Camera cam, Vector3 screenPos)
        {
            var ray = cam.ScreenPointToRay(screenPos);

            float d;
            if (_xzPlane.Raycast(ray, out d))
            {
                return ray.GetPoint(d);
            }

            return Vector3.zero;
        }

        /// <summary>
        /// Casts a ray from the camera to the xz plane through the specified screen point and returns the point the ray intersects the xz plane in world coordinates.
        /// </summary>
        /// <param name="cam">The camera.</param>
        /// <param name="screenPos">The screen position.</param>
        /// <param name="groundHeight">Height (y-coordinate) that the ground level is at.</param>
        /// <returns>The intersection point on the xz plane in world coordinates</returns>
        public static Vector3 ScreenToGroundPoint(this Camera cam, Vector3 screenPos, float groundHeight)
        {
            var ray = cam.ScreenPointToRay(screenPos);
            var xzElevatedPlane = new Plane(Vector3.up, new Vector3(0f, groundHeight, 0f));

            float d;
            if (xzElevatedPlane.Raycast(ray, out d))
            {
                return ray.GetPoint(d);
            }

            return Vector3.zero;
        }

        /// <summary>
        /// Adjusts an axis.
        /// </summary>
        /// <param name="target">The target to adjust.</param>
        /// <param name="source">The source used for the adjust.</param>
        /// <param name="targetAxis">The target axis.</param>
        /// <returns>The target vector with <paramref name="targetAxis"/> changed to that of <paramref name="source"/></returns>
        public static Vector3 AdjustAxis(this Vector3 target, Vector3 source, Axis targetAxis)
        {
            switch (targetAxis)
            {
                case Axis.Y:
                {
                    target.y = source.y;
                    break;
                }

                case Axis.X:
                {
                    target.x = source.x;
                    break;
                }

                case Axis.Z:
                {
                    target.z = source.z;
                    break;
                }
            }

            return target;
        }

        /// <summary>
        /// Adjusts an axis.
        /// </summary>
        /// <param name="target">The target to adjust.</param>
        /// <param name="value">The adjustment.</param>
        /// <param name="targetAxis">The target axis.</param>
        /// <returns>The target vector with <paramref name="targetAxis"/> changed to <paramref name="value"/></returns>
        public static Vector3 AdjustAxis(this Vector3 target, float value, Axis targetAxis)
        {
            switch (targetAxis)
            {
                case Axis.Y:
                {
                    target.y = value;
                    break;
                }

                case Axis.X:
                {
                    target.x = value;
                    break;
                }

                case Axis.Z:
                {
                    target.z = value;
                    break;
                }
            }

            return target;
        }

        /// <summary>
        /// Checks if one vector is approximately equal to another
        /// </summary>
        /// <param name="me">Me.</param>
        /// <param name="other">The other.</param>
        /// <param name="allowedDifference">The allowed difference.</param>
        /// <returns><c>true</c> if the are approximately equal, otherwise <c>false</c></returns>
        public static bool Approximately(this Vector3 me, Vector3 other, float allowedDifference)
        {
            var dx = me.x - other.x;
            if (dx < -allowedDifference || dx > allowedDifference)
            {
                return false;
            }

            var dy = me.y - other.y;
            if (dy < -allowedDifference || dy > allowedDifference)
            {
                return false;
            }

            var dz = me.z - other.z;

            return (dz >= -allowedDifference) && (dz <= allowedDifference);
        }

        /// <summary>
        /// Get the direction between two point in the xz plane only
        /// </summary>
        /// <param name="from">The from position.</param>
        /// <param name="to">The to position.</param>
        /// <returns>The direction vector between the two points.</returns>
		public static Vector3 DirToXZ(this Vector3 from, Vector3 to)
        {
            return new Vector3(to.x - from.x, 0f, to.z - from.z);
        }

        /// <summary>
        /// Discards the y-component of the vector
        /// </summary>
        /// <param name="v">The vector.</param>
        /// <returns>The vector with y set to 0</returns>
        public static Vector3 OnlyXZ(this Vector3 v)
        {
            v.y = 0f;
            return v;
        }

        /// <summary>
        /// Wraps the vector in an IPositioned structure
        /// </summary>
        /// <param name="pos">The position.</param>
        /// <returns>The wrapped position</returns>
        public static IPositioned AsPositioned(this Vector3 pos)
        {
            return new Position(pos);
        }

        /// <summary>
        /// Gets the first MonoBehavior on the component's game object that is of type T. This is different from GetComponent in that the type can be an interface or class that is not itself a component.
        /// It is however a relatively slow operation, and should not be used in actions that happen frequently, e.g. Update.
        /// </summary>
        /// <typeparam name="T">The type of behavior to look for</typeparam>
        /// <param name="c">The component whose siblings will be inspected if they are of type T</param>
        /// <param name="searchParent">if set to <c>true</c> the parent transform will also be inspected if no match is found on the current component's transform.</param>
        /// <param name="required">if set to <c>true</c> and the requested component is not found, an exception will be thrown.</param>
        /// <returns>
        /// The T behavior sibling of the component or null if not found.
        /// </returns>
        public static T As<T>(this Component c, bool searchParent = false, bool required = false) where T : class
        {
            if (c.Equals(null))
            {
                return null;
            }

            return As<T>(c.gameObject, searchParent, required);
        }

        /// <summary>
        /// Gets the first MonoBehavior on the component's game object that is of type T. This is different from GetComponent in that the type can be an interface or class that is not itself a component.
        /// It is however a relatively slow operation, and should not be used in actions that happen frequently, e.g. Update.
        /// </summary>
        /// <typeparam name="T">The type of behavior to look for</typeparam>
        /// <param name="c">The component whose siblings will be inspected if they are of type T</param>
        /// <param name="searchParent">if set to <c>true</c> the parent transform will also be inspected if no match is found on the current component's transform.</param>
        /// <param name="required">if set to <c>true</c> and the requested component is not found, an exception will be thrown.</param>
        /// <returns>
        /// The T behavior sibling of the component or null if not found.
        /// </returns>
        public static T As<T>(this IGameObjectComponent c, bool searchParent = false, bool required = false) where T : class
        {
            if (c.Equals(null))
            {
                return null;
            }

            return As<T>(c.gameObject, searchParent, required);
        }

        /// <summary>
        /// Gets the first MonoBehavior on the game object that is of type T. This is different from GetComponent in that the type can be an interface or class that is not itself a component.
        /// It is however a relatively slow operation, and should not be used in actions that happen frequently, e.g. Update.
        /// </summary>
        /// <typeparam name="T">The type of behavior to look for</typeparam>
        /// <param name="go">The game object whose components will be inspected if they are of type T</param>
        /// <param name="searchParent">if set to <c>true</c> the parent transform will also be inspected if no match is found on the current game object.</param>
        /// <param name="required">if set to <c>true</c> and the requested component is not found, an exception will be thrown.</param>
        /// <returns>
        /// The T behavior or null if not found.
        /// </returns>
        public static T As<T>(this GameObject go, bool searchParent = false, bool required = false) where T : class
        {
            if (go.Equals(null))
            {
                return null;
            }

            var c = go.GetComponent(typeof(T)) as T;

            if (c == null && searchParent && go.transform.parent != null)
            {
                return As<T>(go.transform.parent.gameObject, false, required);
            }

            if (c == null && required)
            {
                throw new MissingComponentException(string.Format("Game object {0} does not have a component of type {1}.", go.name, typeof(T).Name));
            }

            return c;
        }

        /// <summary>
        /// Warns if multiple instances of the component exists on its game object.
        /// </summary>
        /// <param name="component">The component.</param>
        public static void WarnIfMultipleInstances(this MonoBehaviour component)
        {
            var t = component.GetType();

            if (component.GetComponents(t).Length > 1)
            {
                Debug.LogWarning(string.Format("GameObject '{0}' defines multiple instances of '{1}' which is not recommended.", component.gameObject.name, t.Name));
            }
        }

        /// <summary>
        /// Warns if multiple instances of the component exists on its game object.
        /// </summary>
        /// <param name="component">The component.</param>
        public static void WarnIfMultipleInstances<TInterface>(this MonoBehaviour component) where TInterface : class
        {
            int counter = 0;
            var components = component.GetComponents<MonoBehaviour>();
            for (int i = 0; i < components.Length; i++)
            {
                var v = components[i] as TInterface;
                if (v != null)
                {
                    counter++;
                }
            }

            if (counter > 1)
            {
                Debug.LogWarning(string.Format("GameObject '{0}' defines multiple component implementing '{1}' which is not recommended.", component.gameObject.name, typeof(TInterface).Name));
            }
        }

        /// <summary>
        /// Determines whether another bounds overlaps this one (and vice versa).
        /// </summary>
        /// <param name="a">This bounds.</param>
        /// <param name="b">The other bounds.</param>
        /// <returns><c>true</c> if they overlap, otherwise false.</returns>
        public static bool Overlaps(this Bounds a, Bounds b)
        {
            if ((b.max.x <= a.min.x) || (b.min.x >= a.max.x))
            {
                return false;
            }

            return ((b.max.z > a.min.z) && (b.min.z < a.max.z));
        }

        public static Bounds Translate(this Bounds b, Vector3 translation)
        {
            b.center = b.center + translation;
            return b;
        }

        public static Bounds Translate(this Bounds b, float x, float y, float z)
        {
            var center = b.center;
            center.x += x;
            center.y += y;
            center.z += z;
            b.center = center;
            return b;
        }

        public static Bounds DeltaSize(this Bounds b, Vector3 delta)
        {
            b.size = b.size + delta;
            return b;
        }

        public static Bounds DeltaSize(this Bounds b, float dx, float dy, float dz)
        {
            var size = b.size;
            size.x += dx;
            size.y += dy;
            size.z += dz;
            b.size = size;
            return b;
        }

        public static Bounds Intersection(this Bounds a, Bounds b)
        {
            var min = new Vector3(Mathf.Max(a.min.x, b.min.x), Mathf.Max(a.min.y, b.min.y), Mathf.Max(a.min.z, b.min.z));
            var max = new Vector3(Mathf.Min(a.max.x, b.max.x), Mathf.Min(a.max.y, b.max.y), Mathf.Min(a.max.z, b.max.z));

            Bounds res = new Bounds();
            res.SetMinMax(min, max);

            return res;
        }

        /// <summary>
        /// Gets the unit facade for the unit on which this component resides.
        /// </summary>
        /// <param name="c">The component.</param>
        /// <param name="createIfMissing">Controls whether the facade is created if missing.</param>
        /// <returns>The unit facade for the unit.</returns>
        public static IUnitFacade GetUnitFacade(this Component c, bool createIfMissing = true)
        {
            return GameServices.gameStateManager.GetUnitFacade(c.gameObject, createIfMissing);
        }

        /// <summary>
        /// Gets the unit facade for the unit represented by this game object.
        /// </summary>
        /// <param name="go">The game object.</param>
        /// <param name="createIfMissing">Controls whether the facade is created if missing.</param>
        /// <returns>The unit facade for the unit.</returns>
        public static IUnitFacade GetUnitFacade(this GameObject go, bool createIfMissing = true)
        {
            return GameServices.gameStateManager.GetUnitFacade(go, createIfMissing);
        }

        /// <summary>
        /// Gets the unit facade for the unit represented by this component.
        /// </summary>
        /// <param name="goc">The game object related component.</param>
        /// <param name="createIfMissing">Controls whether the facade is created if missing.</param>
        /// <returns>The unit facade for the unit.</returns>
        public static IUnitFacade GetUnitFacade(this IGameObjectComponent goc, bool createIfMissing = true)
        {
            return GameServices.gameStateManager.GetUnitFacade(goc.gameObject, createIfMissing);
        }

        /// <summary>
        /// Gets a specialized unit facade for the unit on which this component resides.
        /// </summary>
        /// <param name="c">The component.</param>
        /// <param name="createIfMissing">Controls whether the facade is created if missing.</param>
        /// <returns>The unit facade for the unit.</returns>
        public static T GetUnitFacade<T>(this Component c, bool createIfMissing = true) where T : class, IUnitFacade, new()
        {
            return GameServices.gameStateManager.GetUnitFacade<T>(c.gameObject, createIfMissing);
        }

        /// <summary>
        /// Gets a specialized unit facade for the unit on which this component resides.
        /// </summary>
        /// <param name="go">The game object.</param>
        /// <param name="createIfMissing">Controls whether the facade is created if missing.</param>
        /// <returns>The unit facade for the unit.</returns>
        public static T GetUnitFacade<T>(this GameObject go, bool createIfMissing = true) where T : class, IUnitFacade, new()
        {
            return GameServices.gameStateManager.GetUnitFacade<T>(go, createIfMissing);
        }

        /// <summary>
        /// Gets a specialized unit facade for the unit on which this component resides.
        /// </summary>
        /// <param name="goc">The game object related component.</param>
        /// <param name="createIfMissing">Controls whether the facade is created if missing.</param>
        /// <returns>The unit facade for the unit.</returns>
        public static T GetUnitFacade<T>(this IGameObjectComponent goc, bool createIfMissing = true) where T : class, IUnitFacade, new()
        {
            return GameServices.gameStateManager.GetUnitFacade<T>(goc.gameObject, createIfMissing);
        }

        /// <summary>
        /// Adds a component to a game object if it is missing.
        /// </summary>
        /// <typeparam name="T">The component type-</typeparam>
        /// <param name="host">The host game object.</param>
        /// <param name="component">The component regardless of whether it was just added or already existed.</param>
        /// <returns><c>true</c> if the component was added; or <c>false</c> if the component already exists on the game object.</returns>
        public static bool AddIfMissing<T>(this GameObject host, out T component) where T : Component
        {
            component = host.GetComponent<T>();

            if (component == null)
            {
                component = host.AddComponent<T>();
                return true;
            }

            return false;
        }
    }
}
