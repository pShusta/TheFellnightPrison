/* Copyright © 2014 Apex Software. All rights reserved. */
namespace Apex.WorldGeometry
{
    using System.Collections;
    using Apex.DataStructures;

    /// <summary>
    /// The height settings provider for <see cref="FlatCell"/>s.
    /// </summary>
    public class FlatCellHeightSettingsProvider : HeightSettingsProviderBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FlatCellHeightSettingsProvider"/> class.
        /// </summary>
        /// <param name="matrix">The matrix.</param>
        public FlatCellHeightSettingsProvider(CellMatrix matrix)
            : base(matrix)
        {           
        }

        /// <summary>
        /// Assigns height settings to a portion of the matrix.
        /// </summary>
        /// <param name="bounds">The portion of the matrix to update.</param>
        /// <returns>
        /// An enumerator which once enumerated will do the update.
        /// </returns>
        public override IEnumerator AssignHeightSettings(MatrixBounds bounds)
        {
            yield break;
        }
    }
}
