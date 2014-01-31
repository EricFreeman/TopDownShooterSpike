// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LightmapGenerator.cs" company="Christopher Harris">
//   2011 Christopher Harris
// </copyright>
// <summary>
//   The Krypton LightMapper
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Futures.Krypton
{
    using System.Collections.Generic;

    using Futures.Krypton.Design;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// The Krypton LightMapper
    /// </summary>
    public class LightmapGenerator : ILightmapGenerator
    {
        /// <summary>
        /// The Graphics Device
        /// </summary>
        private readonly GraphicsDevice device;

        /// <summary>
        /// The lightmap effect
        /// </summary>
        private readonly LightmapEffect lightmapEffect;

        /// <summary>
        /// The draw buffer.
        /// </summary>
        private readonly ILightmapDrawBuffer drawBuffer;

        /// <summary>
        /// Initializes a new instance of the <see cref="LightmapGenerator"/> class.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="lightmapEffect">The lightmap effect.</param>
        /// <param name="drawBuffer">The draw buffer.</param>
        public LightmapGenerator(GraphicsDevice device, LightmapEffect lightmapEffect, ILightmapDrawBuffer drawBuffer)
        {
            this.device = device;
            this.lightmapEffect = lightmapEffect;
            this.drawBuffer = drawBuffer;
        }

        /// <summary>
        /// Gets or sets the ambient color of the lightmap.
        /// </summary>
        public Color AmbientColor { get; set; }

        /// <summary>
        /// Draws all lights and hulls to the current render target
        /// </summary>
        /// <param name="pass">The pass.</param>
        /// <param name="lights">The lights.</param>
        /// <param name="hulls">The hulls.</param>
        public void DrawLightmap(ILightmapPass pass, IList<ILight> lights, IList<IHull> hulls)
        {
            // Krypton Draws Here
            foreach (var light in lights)
            {
                this.device.Clear(ClearOptions.Stencil, Color.Black, 0, 0);

                light.Draw(this.lightmapEffect, pass, this.drawBuffer, hulls);
            }
        }
    }
}
