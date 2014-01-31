// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LightmapPassProvider.cs" company="Christopher Harris">
//   2011 Christopher Harris
// </copyright>
// <summary>
//   Defines the LightmapPassProvider type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Futures.Krypton
{
    using System.Collections.Generic;

    using Design;

    /// <summary>
    /// A Lightmap Pass Provider
    /// </summary>
    public class LightmapPassProvider : ILightmapPassProvider
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LightmapPassProvider"/> class.
        /// </summary>
        /// <param name="passes">The passes.</param>
        public LightmapPassProvider(IEnumerable<ILightmapPass> passes)
        {
            this.Passes = passes;
        }

        /// <summary>
        /// Gets Passes.
        /// </summary>
        public IEnumerable<ILightmapPass> Passes { get; private set; }
    }
}