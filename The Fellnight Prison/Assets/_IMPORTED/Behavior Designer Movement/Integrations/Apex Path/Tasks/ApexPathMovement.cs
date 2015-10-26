using UnityEngine;
using Apex;
using Apex.Units;
using Apex.Messages;
using Apex.Services;
using System;

namespace BehaviorDesigner.Runtime.Tasks.Movement.ApexPath
{
    // This is the base class for all Apex Path Movement tasks that only involve a single agent. It will cache the necessary references
    // and handle the messages received from Apex Path
    public abstract class ApexPathMovement : Movement, IHandleMessage<UnitNavigationEventMessage>
    {
        [Tooltip("The speed of the agent")]
        public SharedFloat speed = 10;
        [Tooltip("The amount of time (in seconds) to delay sending a new path request to Apex Path")]
        public SharedFloat pathRequestDelay = 0;

        // has the agent arrived at its destination?
        private bool arrived = false;
        private IUnitFacade unitFacade;
        private Vector3 prevDestination;
        private float lastPathTime;

        public override void OnAwake()
        {
            // cache for quick lookup
            unitFacade = gameObject.GetUnitFacade();
        }

        // Subscribe to the event messages
        public override void OnStart()
        {
            GameServices.messageBus.Subscribe(this);
            unitFacade.SetPreferredSpeed(speed.Value);
            prevDestination = Vector3.zero;
            lastPathTime = -pathRequestDelay.Value;
            arrived = false;
        }

        protected override bool SetDestination(Vector3 target)
        {
            if (target != prevDestination && lastPathTime + pathRequestDelay.Value <= Time.time) {
                unitFacade.MoveTo(target, false);
                prevDestination = target;
                lastPathTime = Time.time;
                arrived = false;
            }
            return true;
        }

        protected override bool HasArrived()
        {
            return arrived;
        }

        protected override Vector3 Velocity()
        {
            return unitFacade.velocity;
        }

        // Stop the path and unsubscribe from feature messages
        public override void OnEnd()
        {
            unitFacade.Stop();
            GameServices.messageBus.Unsubscribe(this);
        }

        // Receive the callback when the destination has been reached
        public virtual void Handle(UnitNavigationEventMessage message)
        {
            if (!message.entity.Equals(gameObject)) {
                return;
            }

            switch (message.eventCode) {
                case UnitNavigationEventMessage.Event.DestinationReached:
                    arrived = true;
                    break;
            }
        }

        public override void OnReset()
        {
            speed = 10;
            pathRequestDelay = 0;
        }
    }
}