// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ILightmapGeneratorPass.cs" company="Christopher Harris">
//   2011 Christopher Harris
// </copyright>
// <summary>
//   A Lightmap Generator Pass
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Futures.Krypton.Design
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// A Lightmap Generator Pass
    /// </summary>
    public interface ILightmapPass
    {
        /// <summary>
        /// Gets Viewport.
        /// </summary>
        Viewport Viewport { get; }

        /// <summary>
        /// Gets Matrix.
        /// </summary>
        Matrix Matrix { get; }

        /// <summary>
        /// Called before starting the pass, and before the viewport and matrix have been set.
        /// </summary>
        void OnPassStart();

        /// <summary>
        /// Called before drawing the pass, but after the viewport and matrix have been set.
        /// </summary>
        void OnPassRunning();

        /// <summary>
        /// Called after drawing is complete.
        /// </summary>
        void OnPassComplete();
    }
}