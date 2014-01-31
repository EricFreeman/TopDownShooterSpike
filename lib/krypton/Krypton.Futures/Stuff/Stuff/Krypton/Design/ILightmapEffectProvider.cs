// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ILightmapEffectProvider.cs" company="Christopher Harris">
//   2011 Christopher Harris
// </copyright>
// <summary>
//   AssemblyInfo.cs
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Futures.Krypton.Design
{
    /// <summary>
    /// A LightMapperEffect Provider
    /// </summary>
    public interface ILightmapEffectProvider
    {
        /// <summary>
        /// Gets the effect.
        /// </summary>
        LightmapEffect Effect { get; }
    }
}