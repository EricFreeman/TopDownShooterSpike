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
    using System;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// A Lightmap Generator Pass
    /// </summary>
    public class LightmapPass : ILightmapPass
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LightmapPass"/> class.
        /// </summary>
        /// <param name="viewport">The viewport.</param>
        /// <param name="matrix">The matrix.</param>
        public LightmapPass(Viewport viewport, Matrix matrix)
        {
            this.Viewport = viewport;
            this.Matrix = matrix;
        }

        /// <summary>
        /// Gets Viewport.
        /// </summary>
        public virtual Viewport Viewport { get; private set; }

        /// <summary>
        /// Gets Matrix.
        /// </summary>
        public virtual Matrix Matrix { get; private set; }

        /// <summary>
        /// Called before starting the pass, and before the viewport and matrix have been set.
        /// </summary>
        public virtual void OnPassStart()
        {
        }

        /// <summary>
        /// Called before drawing the pass, but after the viewport and matrix have been set.
        /// </summary>
        public virtual void OnPassRunning()
        {
        }

        /// <summary>
        /// Called after drawing is complete.
        /// </summary>
        public virtual void OnPassComplete()
        {
        }
    }
}