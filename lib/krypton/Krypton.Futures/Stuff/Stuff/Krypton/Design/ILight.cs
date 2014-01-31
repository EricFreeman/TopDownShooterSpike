// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ILight.cs" company="Christopher Harris">
//   2011 Christopher Harris
// </copyright>
// <summary>
//   AssemblyInfo.cs
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using Futures.Krypton.Lights;
using Microsoft.Xna.Framework;

namespace Futures.Krypton.Design
{
    using System.Collections.Generic;

    /// <summary>
    /// A Krypton Light Source
    /// </summary>
    public interface ILight
    {
        /// <summary>
        /// Gets a value indicating whether the light is on.
        /// </summary>
        bool On { get; }

        /// <summary>
        /// Gets Outline
        /// </summary>
        IEnumerable<Vector2> Outline { get; }

        /// <summary>
        /// Draws the light with shadows from the hulls
        /// </summary>
        /// <param name="lightmapEffect">The lightmap effect.</param>
        /// <param name="helper">The helper.</param>
        /// <param name="hulls">The hulls.</param>
        void Draw(LightmapEffect lightmapEffect, ILightmapPass pass, ILightmapDrawBuffer helper, IEnumerable<IHull> hulls);
    }
}