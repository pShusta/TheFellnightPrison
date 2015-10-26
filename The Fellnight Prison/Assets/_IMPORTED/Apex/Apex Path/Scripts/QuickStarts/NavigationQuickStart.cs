/* Copyright © 2014 Apex Software. All rights reserved. */
namespace Apex.QuickStarts
{
    using Apex.Debugging;
    using Apex.LoadBalancing;
    using Apex.PathFinding;
    using Apex.Services;
    using Apex.Steering;
    using Apex.Steering.Components;
    using Apex.Steering.HeightNavigation;
    using Apex.Units;
    using Apex.WorldGeometry;
    using UnityEngine;

    /// <summary>
    /// A component that adds all components necessary for an object to become a navigating unit.
    /// </summary>
    [AddComponentMenu("Apex/QuickStarts/Navigating Unit")]
    public partial class NavigationQuickStart : QuickStartBase
    {
        /// <summary>
        /// Sets up component on which the quick start is attached.
        /// </summary>
        protected sealed override void Setup()
        {
            var go = this.gameObject;
            ApexComponentMaster master;
            Rigidbody rb;

            //Add the required components
            AddIfMissing<Rigidbody>(go, false, out rb);
            bool toggleAll = AddIfMissing<ApexComponentMaster>(go, false, out master);
            AddIfMissing<HumanoidSpeedComponent>(go, false);
            AddIfMissing<UnitComponent>(go, false);
            AddIfMissing<SteerableUnitComponent>(go, false);
            AddIfMissing<DefaultHeightNavigator>(go, false);
            AddIfMissing<PathOptionsComponent>(go, false);
            AddIfMissing<SteerToAlignWithVelocity>(go, false);
            AddIfMissing<SteerForPathComponent>(go, 5);
            AddIfMissing<PathVisualizer>(go, false);

            ExtendForSteer();

            if (toggleAll)
            {
                master.ToggleAll();
            }

            //Adjust components
            if (rb != null)
            {
                rb.constraints |= RigidbodyConstraints.FreezeRotation;
                rb.useGravity = false;
            }

            GameObject gameWorld = null;

            var servicesInitializer = FindComponent<GameServicesInitializerComponent>();
            if (servicesInitializer != null)
            {
                gameWorld = servicesInitializer.gameObject;
            }
            else
            {
                gameWorld = new GameObject("Game World");
                gameWorld.AddComponent<GameServicesInitializerComponent>();
                Debug.Log("No game world found, creating one.");
            }

            toggleAll = AddIfMissing<ApexComponentMaster>(gameWorld, false, out master);

            AddIfMissing<NavigationSettingsComponent>(gameWorld, true);

            if (AddIfMissing<GridComponent>(gameWorld, true))
            {
                AddIfMissing<GridVisualizer>(gameWorld, false);
            }

            AddIfMissing<LayerMappingComponent>(gameWorld, true);
            AddIfMissing<PathServiceComponent>(gameWorld, true);
            AddIfMissing<LoadBalancerComponent>(gameWorld, true);

            Extend(gameWorld);

            if (toggleAll)
            {
                master.ToggleAll();
            }
        }

        partial void ExtendForSteer();

        /// <summary>
        /// Extends this quick start with additional components.
        /// </summary>
        /// <param name="gameWorld">The game world.</param>
        protected virtual void Extend(GameObject gameWorld)
        {
        }
    }
}
