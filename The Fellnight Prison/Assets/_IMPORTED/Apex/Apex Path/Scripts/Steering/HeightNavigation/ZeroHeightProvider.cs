/* Copyright © 2014 Apex Software. All rights reserved. */
namespace Apex.Steering.HeightNavigation
{
    public class ZeroHeightProvider : IUnitHeightProvider
    {
        public float GetHeightDelta(SteeringInput input)
        {
            var grid = input.grid;
            if (grid == null)
            {
                return 0f;
            }

            var unit = input.unit;

            return grid.origin.y - (unit.position.y - unit.baseToPositionOffset);
        }
    }
}
