// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LightmapPresenterComponent.cs" company="Christopher Harris">
//   2011 Christopher Harris
// </copyright>
// <summary>
//   AssemblyInfo.cs
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Futures.Krypton.Components
{
    using Futures.Common;
    using Futures.Krypton.Design;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// A Lightmap Presenter Component
    /// </summary>
    public class LightmapPresenterComponent : DrawableGameComponent
    {
        /// <summary>
        /// A LightMapper to get the Lightmap from
        /// </summary>
        private readonly ILightmapProvider lightmapProvider;

        /// <summary>
        /// A SpriteBatch for presenting the Lightmap
        /// </summary>
        private SpriteBatch spriteBatch;

        /// <summary>
        /// A Mulitplicative BlendState for presenting the Lightmap
        /// </summary>
        private BlendState blendState;

        /// <summary>
        /// Initializes a new instance of the <see cref="LightmapPresenterComponent"/> class.
        /// </summary>
        /// <param name="game">The game.</param>
        /// <param name="lightmapProvider">The lightmap presenter.</param>
        public LightmapPresenterComponent(Game game, ILightmapProvider lightmapProvider)
            : base(game)
        {
            this.lightmapProvider = lightmapProvider;
        }

        /// <summary>
        /// Called when the DrawableGameComponent needs to be drawn. Override this method with component-specific drawing code. Reference page contains links to related conceptual articles.
        /// </summary>
        /// <param name="gameTime">Time passed since the last call to Draw.</param>
        public override void Draw(GameTime gameTime)
        {
            this.spriteBatch.DrawToTarget(this.blendState, this.lightmapProvider.Lightmap);
        }

        /// <summary>
        /// Called when graphics resources need to be loaded. Override this method to load any component-specific graphics resources.
        /// </summary>
        protected override void LoadContent()
        {
            this.spriteBatch = new SpriteBatch(this.GraphicsDevice);

            this.blendState = new BlendState
            {
                ColorBlendFunction = BlendFunction.Add,
                ColorDestinationBlend = Blend.SourceColor,
                ColorSourceBlend = Blend.Zero,
                AlphaBlendFunction = BlendFunction.Add,
                AlphaDestinationBlend = Blend.SourceAlpha,
                AlphaSourceBlend = Blend.Zero,
            };
        }
    }
}