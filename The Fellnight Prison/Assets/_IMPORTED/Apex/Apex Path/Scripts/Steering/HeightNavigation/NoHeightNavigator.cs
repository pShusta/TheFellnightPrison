/* Copyright © 2014 Apex Software. All rights reserved. */
namespace Apex.Steering.HeightNavigation
{
    public sealed class NoHeightNavigator : IHeightNavigator
    {
        public static readonly NoHeightNavigator Instance = new NoHeightNavigator();

        public float gravity
        {
            get { return 0f; }
        }

        public HeightOutput GetHeightOutput(SteeringInput input, float effectiveMaxSpeed)
        {
            return new HeightOutput
            {
                finalVelocity = input.currentSpatialVelocity,
                isGrounded = true
            };
        }

        public void CloneFrom(IHeightNavigator other)
        {
            /* NOOP */
        }
    }
}
