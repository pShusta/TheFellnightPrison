using UnityEngine;
using System.Collections.Generic;

namespace BehaviorDesigner.Runtime.Tasks.Movement
{
    [TaskDescription("Check to see if the any object specified by the object list or tag is within the distance specified of the current agent.")]
    [TaskCategory("Movement")]
    [HelpURL("http://www.opsive.com/assets/BehaviorDesigner/Movement/documentation.php?id=18")]
    [TaskIcon("Assets/Behavior Designer Movement/Editor/Icons/{SkinColor}WithinDistanceIcon.png")]
    public class WithinDistance : Conditional
    {
        [Tooltip("Should the 2D version be used?")]
        public bool usePhysics2D;
        [Tooltip("The distance that the object needs to be within")]
        public SharedFloat magnitude;
        [Tooltip("If true, the object must be within line of sight to be within distance. For example, if this option is enabled then an object behind a wall will not be within distance even though it may " +
                 "be physically close to the other object")]
        public SharedBool lineOfSight;
        [Tooltip("The LayerMask of the objects to ignore when performing the line of sight check")]
        public LayerMask ignoreLayerMask = 1 << LayerMask.NameToLayer("Ignore Raycast");
        [Tooltip("The offset relative to the pivot position")]
        public SharedVector3 offset;
        [Tooltip("The target offset relative to the pivot position")]
        public SharedVector3 targetOffset;
        [Tooltip("An array of objects to check to see if they are within distance")]
        public SharedGameObjectList objects;
        [Tooltip("If the object list is null then find the potential objects based off of the tag")]
        public SharedString objectTag;
        [Tooltip("The object variable that will be set when a object is found what the object is")]
        public SharedGameObject returnedObject;

        // distance * distance, optimization so we don't have to take the square root
        private float sqrMagnitude;

        public override void OnAwake()
        {
            // initialize the variables
            sqrMagnitude = magnitude.Value * magnitude.Value;
        }

        public override void OnStart()
        {
            // if objects is null then find all of the objects using the objectTag
            if (!string.IsNullOrEmpty(objectTag.Value)) {
                var gameObjects = GameObject.FindGameObjectsWithTag(objectTag.Value);
                objects.Value = new List<GameObject>();
                for (int i = 0; i < gameObjects.Length; ++i) {
                    objects.Value.Add(gameObjects[i]);
                }
            }
        }

        // returns success if any object is within distance of the current object. Otherwise it will return failure
        public override TaskStatus OnUpdate()
        {
            if (transform == null || objects.Value == null)
                return TaskStatus.Failure;

            Vector3 direction;
            // check each object. All it takes is one object to be able to return success
            for (int i = 0; i < objects.Value.Count; ++i) {
                direction = objects.Value[i].transform.position - (transform.position + offset.Value);
                // check to see if the square magnitude is less than what is specified
                if (Vector3.SqrMagnitude(direction) < sqrMagnitude) {
                    // the magnitude is less. If lineOfSight is true do one more check
                    if (lineOfSight.Value) {
                        if (MovementUtility.LineOfSight(transform, offset.Value, objects.Value[i], targetOffset.Value, usePhysics2D, ignoreLayerMask.value)) {
                            // the object has a magnitude less than the specified magnitude and is within sight. Set the object and return success
                            returnedObject.Value = objects.Value[i];
                            return TaskStatus.Success;
                        }
                    } else {
                        // the object has a magnitude less than the specified magnitude. Set the object and return success
                        returnedObject.Value = objects.Value[i];
                        return TaskStatus.Success;
                    }
                }
            }
            // no objects are within distance. Return failure
            return TaskStatus.Failure;
        }

        // Draw the seeing radius
        public override void OnDrawGizmos()
        {
#if UNITY_EDITOR
            if (Owner == null || magnitude == null) {
                return;
            }
            var oldColor = UnityEditor.Handles.color;
            UnityEditor.Handles.color = Color.yellow;
            UnityEditor.Handles.DrawWireDisc(Owner.transform.position, usePhysics2D ? Owner.transform.forward : Owner.transform.up, magnitude.Value);
            UnityEditor.Handles.color = oldColor;
#endif
        }
    }
}