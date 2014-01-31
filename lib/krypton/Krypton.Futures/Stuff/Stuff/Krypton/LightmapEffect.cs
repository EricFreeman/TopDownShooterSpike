// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LightmapEffect.cs" company="Christopher Harris">
//   2011 Christopher Harris
// </copyright>
// <summary>
//   AssemblyInfo.cs
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Futures.Krypton
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// The Krypton Effect Wrapper
    /// </summary>
    public class LightmapEffect
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LightmapEffect"/> class.
        /// </summary>
        /// <param name="effect">The effect.</param>
        public LightmapEffect(Effect effect)
        {
            this.Effect = effect;
        }

        /// <summary>
        /// Gets the Effect.
        /// </summary>
        public Effect Effect { get; private set; }

        /// <summary>
        /// Gets or sets light texture.
        /// </summary>
        public Texture2D LightTexture
        {
            get { return this.Effect.Parameters["Texture0"].GetValueTexture2D(); }
            set { this.Effect.Parameters["Texture0"].SetValue(value); }
        }

        /// <summary>
        /// Gets or sets the light position.
        /// </summary>
        public Vector2 LightPosition
        {
            get { return this.Effect.Parameters["LightPosition"].GetValueVector2(); }
            set { this.Effect.Parameters["LightPosition"].SetValue(value); }
        }

        /// <summary>
        /// Gets or sets LightInesityFactor.
        /// </summary>
        public float LightInesityFactor
        {
            get { return this.Effect.Parameters["LightIntensityFactor"].GetValueSingle(); }
            set { this.Effect.Parameters["LightIntensityFactor"].SetValue(value); }
        }

        /// <summary>
        /// Gets the Solid Shadow Technique.
        /// </summary>
        public EffectTechnique SolidShadowTechnique
        {
            get { return this.Effect.Techniques["PointLight_Shadow_Solid"]; }
        }

        /// <summary>
        /// Gets the Illuminated Shadow Technique.
        /// </summary>
        public EffectTechnique IlluminatedShadowTechnique
        {
            get { return this.Effect.Techniques["PointLight_Shadow_Illuminated"]; }
        }

        /// <summary>
        /// Gets the Occluded Shadow Technique.
        /// </summary>
        public EffectTechnique OccludedShadowTechnique
        {
            get { return this.Effect.Techniques["PointLight_Shadow_Occluded"]; }
        }

        /// <summary>
        /// Gets the Point Light Technique.
        /// </summary>
        public EffectTechnique LightTechnique
        {
            get { return this.Effect.Techniques["PointLight_Light"]; }
        }

        /// <summary>
        /// Gets the Alpha-Clear Technique.
        /// </summary>
        public EffectTechnique AlphaClearTechnique
        {
            get { return this.Effect.Techniques["ClearTarget_Alpha"]; }
        }

        /// <summary>
        /// Sets the Matrix.
        /// </summary>
        public Matrix Matrix
        {
            set { this.Effect.Parameters["Matrix"].SetValue(value); }
        }

        /// <summary>
        /// Gets or sets the Active Technique.
        /// </summary>
        public EffectTechnique CurrentTechnique
        {
            get
            {
                return this.Effect.CurrentTechnique;
            }

            set
            {
                this.Effect.CurrentTechnique = value;
            }
        }
    }
}
