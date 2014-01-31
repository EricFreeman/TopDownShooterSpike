// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IHull.cs" company="Christopher Harris">
//   2011 Christopher Harris
// </copyright>
// <summary>
//   AssemblyInfo.cs
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Futures.Krypton.Design
{
    /// <summary>
    /// A Krypton Shadow Source
    /// </summary>
    public interface IHull : IBoundingCircle
    {
        /// <summary>
        /// Draws the hull.
        /// </summary>
        /// <param name="drawBuffer">The Draw Buffer</param>
        void Draw(ILightmapDrawBuffer drawBuffer);
    }
}