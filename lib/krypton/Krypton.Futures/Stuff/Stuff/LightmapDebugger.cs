// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LightmapDebugger.cs" company="Christopher Harris">
//   2011 Christopher Harris
// </copyright>
// <summary>
//   A Lightmap Debugger
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Futures
{
    using System.Linq;

    using Futures.Krypton.Components;
    using Futures.Krypton.Design;
    using Futures.Krypton.Lights;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// A Lightmap Debugger
    /// </summary>
    public class LightmapDebugger : DrawableGameComponent
    {
        /// <summary>
        /// The Lightmap Generator
        /// </summary>
        private readonly LightmapGeneratorComponent lightmapGenerator;

        /// <summary>
        /// The Sprite Batch
        /// </summary>
        private SpriteBatch batch;

        /// <summary>
        /// The Texture
        /// </summary>
        private Texture2D texture;

        /// <summary>
        /// Initializes a new instance of the <see cref="LightmapDebugger"/> class.
        /// </summary>
        /// <param name="game">The game.</param>
        /// <param name="lightmapGenerator">The lightmap generator.</param>
        public LightmapDebugger(Game game, LightmapGeneratorComponent lightmapGenerator)
            : base(game)
        {
            this.lightmapGenerator = lightmapGenerator;
        }

        public override void Draw(GameTime gameTime)
        {
            this.batch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);

            foreach (var pass in this.lightmapGenerator.Passes)
            {
                this.DrawPass(pass);
            }

            this.batch.End();
        }

        protected override void LoadContent()
        {
            this.batch = new SpriteBatch(this.GraphicsDevice);
            this.texture = this.Game.Content.Load<Texture2D>("Krypton/Debug/tex");
        }

        private void DrawPass(ILightmapPass pass)
        {
            foreach (var light in this.lightmapGenerator.Lights)
            {
                if (light is PointLight)
                {
                    this.DrawLight(pass, (PointLight)light);
                }
            }

            foreach (var hull in this.lightmapGenerator.Hulls)
            {
                this.DrawHull(pass, hull);
            }
        }

        private void DrawHull(ILightmapPass pass, IHull hull)
        {
        }

        private void DrawLight(ILightmapPass pass, PointLight light)
        {
            var v = Vector2.Transform(new Vector2(light.Position.X, light.Position.Y), pass.Matrix);
            v = ScreenToPixel(pass, v);

            Vector2 v1, v2, v3, v4;

            this.GraphicsDevice.ScissorRectangle= GetViewport(pass, light, out v2, out v1, out v4, out v3);

            this.batch.Draw(this.texture, v, null, Color.White, 0, Vector2.One * 8, Vector2.One, SpriteEffects.None, 0);

            this.batch.Draw(this.texture, v1, null, Color.White, 0, Vector2.One * 8, Vector2.One / 2, SpriteEffects.None, 0);
            this.batch.Draw(this.texture, v2, null, Color.White, 0, Vector2.One * 8, Vector2.One / 2, SpriteEffects.None, 0);
            this.batch.Draw(this.texture, v3, null, Color.White, 0, Vector2.One * 8, Vector2.One / 2, SpriteEffects.None, 0);
            this.batch.Draw(this.texture, v4, null, Color.White, 0, Vector2.One * 8, Vector2.One / 2, SpriteEffects.None, 0);
        }

        private static Rectangle GetViewport(
            ILightmapPass pass, PointLight light, out Vector2 v2, out Vector2 v1, out Vector2 v4, out Vector2 v3)
        {
            v1 = Vector2.Transform(new Vector2(light.Position.X + light.Radius, light.Position.Y + light.Radius), pass.Matrix);
            v2 = Vector2.Transform(new Vector2(light.Position.X - light.Radius, light.Position.Y + light.Radius), pass.Matrix);
            v3 = Vector2.Transform(new Vector2(light.Position.X - light.Radius, light.Position.Y - light.Radius), pass.Matrix);
            v4 = Vector2.Transform(new Vector2(light.Position.X + light.Radius, light.Position.Y - light.Radius), pass.Matrix);

            v1 = ScreenToPixel(pass, v1);
            v2 = ScreenToPixel(pass, v2);
            v3 = ScreenToPixel(pass, v3);
            v4 = ScreenToPixel(pass, v4);

            var min = new[] { v1, v2, v3, v4 }.Aggregate(Vector2.Min);
            var max = new[] { v1, v2, v3, v4 }.Aggregate(Vector2.Max);

            var rect = new Rectangle((int)min.X, (int)min.Y, (int)(max.X - min.X), (int)(max.Y - min.Y));

            return Rectangle.Intersect(rect, pass.Viewport.Bounds);
        }

        private static Vector2 ScreenToPixel(ILightmapPass pass, Vector2 v)
        {
            v.X = ((1 + v.X) / 2) * pass.Viewport.Width + pass.Viewport.X;
            v.Y = ((1 - v.Y) / 2) * pass.Viewport.Height + pass.Viewport.Y;

            return v;
        }
    }
}