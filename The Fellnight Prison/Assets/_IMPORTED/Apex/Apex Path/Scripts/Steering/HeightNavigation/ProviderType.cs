/* Copyright © 2014 Apex Software. All rights reserved. */
namespace Apex.Steering.HeightNavigation
{
    public enum ProviderType
    {
        None = 0,

        ZeroHeight = 1,

        HeightMapBoxFivePoint = 10,

        HeightMapBoxThreePoint = 11,

        HeightMapSphericalThreePoint = 12,

        RaycastBoxFivePoint = 20,

        RaycastBoxThreePoint = 21,

        RaycastSphericalThreePoint = 22,

        SphereCast = 30,

        CapsuleCast = 40,

        Custom = 100
    }
}
