// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BackBufferSlider.cs" company="Christopher Harris">
//   2011 Christopher Harris
// </copyright>
// <summary>
//   AssemblyInfo.cs
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Futures
{
    using System;

    using Microsoft.Xna.Framework;

    /// <summary>
    /// A Component that clears the backbuffer to a changing color
    /// </summary>
    public class BackBufferSlider : DrawableGameComponent
    {
        /// <summary>
        /// Amount of Red
        /// </summary>
        private float r;

        /// <summary>
        /// Amount of Green
        /// </summary>
        private float g;

        /// <summary>
        /// Amount of Blue
        /// </summary>
        private float b;

        /// <summary>
        /// Initializes a new instance of the <see cref="BackBufferSlider"/> class.
        /// </summary>
        /// <param name="game">
        /// The game.
        /// </param>
        public BackBufferSlider(Game game)
            : base(game)
        {
        }

        /// <summary>
        /// Called when the GameComponent needs to be updated. Override this method with component-specific update code.
        /// </summary>
        /// <param name="gameTime">Time elapsed since the last call to Update</param>
        public override void Update(GameTime gameTime)
        {
            this.r = ((float)Math.Cos(gameTime.TotalGameTime.TotalSeconds * 1) + 2) / 3;
            this.g = ((float)Math.Cos(gameTime.TotalGameTime.TotalSeconds * 2) + 2) / 3;
            this.b = ((float)Math.Cos(gameTime.TotalGameTime.TotalSeconds * 3) + 2) / 3;
        }

        /// <summary>
        /// Called when the DrawableGameComponent needs to be drawn. Override this method with component-specific drawing code. Reference page contains links to related conceptual articles.
        /// </summary>
        /// <param name="gameTime">Time passed since the last call to Draw.</param>
        public override void Draw(GameTime gameTime)
        {
            this.GraphicsDevice.Clear(new Color(this.r, this.g, this.b));
        }
    }
}