// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PointLightSolid.cs" company="Christopher Harris">
//   2011 Christopher Harris
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Futures.Krypton.Lights
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

    using Futures.Common;
    using Futures.Krypton.Design;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// A Krypton Point Light
    /// </summary>
    [DebuggerDisplay("Solid: {Position} {Radius} {Color} {Intensity}")]
    public class PointLight : ILight
    {
        /// <summary>
        /// The Radius
        /// </summary>
        private float radius;

        /// <summary>
        /// Initializes a new instance of the <see cref="PointLightSolid"/> class.
        /// </summary>
        /// <param name="texture">The texture.</param>
        public PointLight(Texture2D texture)
        {
            this.On = true;
            this.Texture = texture;
            this.Intensity = 1;
            this.Radius = 1;
            this.Color = Color.White;
            this.ShadowType = ShadowType.Solid;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the light is on.
        /// </summary>
        public bool On { get; set; }

        /// <summary>
        /// Gets Outline
        /// </summary>
        public IEnumerable<Vector2> Outline
        {
            get { return null; }
        }

        /// <summary>
        /// Gets or sets Color.
        /// </summary>
        public Color Color { get; set; }

        /// <summary>
        /// Gets or sets Radius.
        /// </summary>
        public float Radius
        {
            get
            {
                return this.radius;
            }

            set
            {
                this.radius = value;
                this.RadiusSquared = value * value;
            }
        }

        /// <summary>
        /// Gets RadiusSquared.
        /// </summary>
        public float RadiusSquared { get; private set; }

        /// <summary>
        /// Gets Texture.
        /// </summary>
        public Texture2D Texture { get; private set; }

        /// <summary>
        /// Gets or sets Position.
        /// </summary>
        public Vector2 Position { get; set; }

        /// <summary>
        /// Gets or sets Intensity.
        /// </summary>
        public float Intensity { get; set; }

        /// <summary>
        /// Gets IntensityFactor.
        /// </summary>
        public float IntensityFactor
        {
            get
            {
                return 1 / (this.Intensity * this.Intensity);
            }
        }

        /// <summary>
        /// Gets or Sets ShadowType.
        /// </summary>
        public ShadowType ShadowType { get; set; }

        /// <summary>
        /// Draws the light with shadows from the hulls
        /// </summary>
        /// <param name="lightmapEffect">The lightmap effect.</param>
        /// <param name="helper">The helper.</param>
        /// <param name="hulls">The hulls.</param>
        public void Draw(LightmapEffect lightmapEffect, ILightmapPass lightmapPass, ILightmapDrawBuffer helper, IEnumerable<IHull> hulls)
        {
            lightmapEffect.Effect.GraphicsDevice.ScissorRectangle = lightmapPass.GetScissor(this);

            // lightmapEffect.Effect.GraphicsDevice.ScissorRectangle = new Rectangle(0, 0, 200, 200);

            // 1) Clear hull buffers
            helper.Clear();

            // 2) Prepare to draw hulls
            foreach (var hull in hulls.Where(x => Vector2.DistanceSquared(this.Position, x.Position) < this.RadiusSquared + x.RadiusSquared))
            {
                hull.Draw(helper);
            }

            // 3) Set lightmapEffect stuff
            lightmapEffect.LightPosition = this.Position;
            lightmapEffect.LightTexture = this.Texture;
            lightmapEffect.LightInesityFactor = this.IntensityFactor;

            switch (this.ShadowType)
            {
                case ShadowType.Illuminated:
                    lightmapEffect.CurrentTechnique = lightmapEffect.IlluminatedShadowTechnique;
                    break;
                case ShadowType.Occluded:
                    lightmapEffect.CurrentTechnique = lightmapEffect.OccludedShadowTechnique;
                    break;
                default:
                    lightmapEffect.CurrentTechnique = lightmapEffect.SolidShadowTechnique;
                    break;
            }

            // 4) Draw hulls for each shadow pass
            foreach (var pass in lightmapEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                helper.Draw();
            }

            // 5) Draw light for each light pass
            lightmapEffect.CurrentTechnique = lightmapEffect.LightTechnique;

            foreach (var pass in lightmapEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                helper.DrawClippedFov(this.Position, 0f, this.Radius * 2, this.Color, MathHelper.TwoPi);
            }

            // 6) Clear the target's alpha chanel
            lightmapEffect.CurrentTechnique = lightmapEffect.AlphaClearTechnique;

            //lightmapEffect.Effect.GraphicsDevice.ScissorRectangle = lightmapEffect.Effect.GraphicsDevice.Viewport.Bounds;

            foreach (var pass in lightmapEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                helper.DrawUnitQuad();
            }
        }
    }
}
