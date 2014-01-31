namespace Futures.Krypton.Design
{
    using System.Collections.Generic;

    /// <summary>
    /// A Lightmap Pass Provider
    /// </summary>
    public interface ILightmapPassProvider
    {
        /// <summary>
        /// Gets Passes.
        /// </summary>
        IEnumerable<ILightmapPass> Passes { get; }
    }

}