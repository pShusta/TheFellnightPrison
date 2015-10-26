using UnityEngine;
using Apex;
using Apex.Units;
using System;

namespace BehaviorDesigner.Runtime.Tasks.Movement.ApexPath
{
    // This is the base class for all Apex Path Movement tasks that involve a group of agents. It will cache the necessary references and handle stopping the task
    public abstract class ApexPathGroupMovement : GroupMovement
    {
        [Tooltip("All of the agents")]
        public GameObject[] agents;
        [Tooltip("The speed of the agents")]
        public SharedFloat speed = 10;
        [Tooltip("The amount of time (in seconds) to delay sending a new path request to Apex Path")]
        public SharedFloat pathRequestDelay = 0;

        // The corresponding transforms of the agents
        protected Transform[] transforms;
        private IUnitFacade[] unitFacades;

        private Vector3[] prevDestination;
        private float[] lastPathTime;

        public override void OnAwake()
        {
            // cache for quick lookup
            transforms = new Transform[agents.Length];
            unitFacades = new IUnitFacade[agents.Length];
            prevDestination = new Vector3[agents.Length];
            lastPathTime = new float[agents.Length];

            for (int i = 0; i < agents.Length; ++i) {
                transforms[i] = agents[i].transform;
                unitFacades[i] = agents[i].GetUnitFacade();
                unitFacades[i].SetPreferredSpeed(speed.Value);
            }
        }

        public override void OnStart()
        {
            for (int i = 0; i < agents.Length; ++i) {
                prevDestination[i] = Vector3.zero;
                lastPathTime[i] = -pathRequestDelay.Value;
            }
        }

        protected override bool SetDestination(int index, Vector3 destination)
        {
            if (destination != prevDestination[index] && lastPathTime[index] + pathRequestDelay.Value <= Time.time) {
                unitFacades[index].MoveTo(destination, false);
                prevDestination[index] = destination;
                lastPathTime[index] = Time.time;
            }
            return true;
        }

        protected override Vector3 Velocity(int index)
        {
            return unitFacades[index].velocity;
        }

        public override void OnEnd()
        {
            // Stop the agents
            for (int i = 0; i < unitFacades.Length; ++i) {
                if (unitFacades[i] != null) {
                    unitFacades[i].Stop();
                }
            }
        }

        public override void OnReset()
        {
            speed = 10;
            pathRequestDelay = 0;
        }
    }
}