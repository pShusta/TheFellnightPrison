/* Copyright © 2014 Apex Software. All rights reserved. */
namespace Apex.WorldGeometry
{
    using System.Collections;
    using Apex.DataStructures;
    using Apex.Services;
    using Apex.Steering;
    using Apex.Utilities;
    using UnityEngine;

    /// <summary>
    /// Base class for height settings providers for cells.
    /// </summary>
    public abstract class HeightSettingsProviderBase : IHeightSettingsProvider
    {
        /// <summary>
        /// The cell matrix
        /// </summary>
        protected readonly CellMatrix _matrix;

        /// <summary>
        /// Initializes a new instance of the <see cref="HeightSettingsProviderBase"/> class.
        /// </summary>
        /// <param name="matrix">The matrix.</param>
        protected HeightSettingsProviderBase(CellMatrix matrix)
        {
            _matrix = matrix;
        }

        /// <summary>
        /// Assigns height settings to a portion of the matrix.
        /// </summary>
        /// <param name="bounds">The portion of the matrix to update.</param>
        /// <returns>An enumerator which once enumerated will do the update.</returns>
        public abstract IEnumerator AssignHeightSettings(MatrixBounds bounds);

        /// <summary>
        /// Gets the perpendicular offsets.
        /// </summary>
        /// <param name="dx">The x delta.</param>
        /// <param name="dz">The z delta.</param>
        /// <returns>An array with 3 entries, representing left side offset, no offset and right side offset.</returns>
        protected Vector3[] GetPerpendicularOffsets(int dx, int dz)
        {
            Vector3 ppd;
            var obstacleSensitivityRange = Mathf.Min(_matrix.cellSize / 2f, _matrix.obstacleSensitivityRange);

            if (dx != 0 && dz != 0)
            {
                var offSet = obstacleSensitivityRange / Consts.SquareRootTwo;
                ppd = new Vector3(offSet * -dx, 0.0f, offSet * dz);
            }
            else
            {
                ppd = new Vector3(obstacleSensitivityRange * dz, 0.0f, obstacleSensitivityRange * dx);
            }

            return new Vector3[]
            {
                Vector3.zero,
                ppd,
                ppd * -1
            };
        }

        /// <summary>
        /// Gets the height sampler.
        /// </summary>
        /// <returns>The height sampler</returns>
        protected ISampleHeightsSimple GetHeightSampler()
        {
            if (GameServices.heightStrategy.heightMode == HeightSamplingMode.HeightMap)
            {
                return _matrix;
            }

            return GameServices.heightStrategy.heightSampler;
        }
    }
}
