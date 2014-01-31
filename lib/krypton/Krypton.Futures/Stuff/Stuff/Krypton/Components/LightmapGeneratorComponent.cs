// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FourViewportLightmapGeneratorComponent.cs" company="Christopher Harris">
//   2011 Christopher Harris
// </copyright>
// <summary>
//   AssemblyInfo.cs
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Futures.Krypton.Components
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Futures.Krypton.Design;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// The Default LightMapper
    /// </summary>
    public class LightmapGeneratorComponent : DrawableGameComponent, ILightmapProvider
    {
        /// <summary>
        /// The draw buffer.
        /// </summary>
        private LightmapDrawBuffer drawBuffer;

        /// <summary>
        /// Initializes a new instance of the <see cref="LightmapGeneratorComponent"/> class.
        /// </summary>
        /// <param name="game">The game.</param>
        /// <param name="passes">The passes.</param>
        public LightmapGeneratorComponent(Game game, IEnumerable<ILightmapPass> passes)
            : base(game)
        {
            if (passes == null)
            {
                throw new ArgumentNullException("passes");
            }

            this.Lights = new List<ILight>();
            this.Hulls = new List<IHull>();

            this.AmbientColor = Color.Black;

            this.Passes = passes;
        }

        /// <summary>
        /// Called before starting a pass, and before the viewport and matrix have been set.
        /// </summary>
        public event Action<ILightmapPass> PassStart;

        /// <summary>
        /// Called before drawing a pass, but after the viewport and matrix have been set.
        /// </summary>
        public event Action<ILightmapPass> PassComplete;

        /// <summary>
        /// Called after drawing a pass.
        /// </summary>
        public event Action<ILightmapPass> PassRunning;

        /// <summary>
        /// Gets the Lightmap Generator.
        /// </summary>
        public ILightmapGenerator Generator { get; private set; }

        /// <summary>
        /// Gets the lights.
        /// </summary>
        public IList<ILight> Lights { get; private set; }

        /// <summary>
        /// Gets the hulls.
        /// </summary>
        public IList<IHull> Hulls { get; private set; }

        /// <summary>
        /// Gets or sets the ambient color of the lightmap.
        /// </summary>
        public Color AmbientColor { get; set; }

        /// <summary>
        /// Gets the lightmap.
        /// </summary>
        Texture2D ILightmapProvider.Lightmap
        {
            get { return this.Lightmap; }
        }

        /// <summary>
        /// Gets or sets Passes.
        /// </summary>
        public IEnumerable<ILightmapPass> Passes { get; set; }

        /// <summary>
        /// Gets Effect.
        /// </summary>
        protected LightmapEffect Effect { get; private set; }

        /// <summary>
        /// Gets Lightmap.
        /// </summary>
        protected RenderTarget2D Lightmap { get; private set; }

        /// <summary>
        /// Called when the DrawableGameComponent needs to be drawn. Override this method with component-specific drawing code. Reference page contains links to related conceptual articles.
        /// </summary>
        /// <param name="gameTime">Time passed since the last call to Draw.</param>
        public override void Draw(GameTime gameTime)
        {
            this.GraphicsDevice.SetRenderTarget(this.Lightmap);
            this.GraphicsDevice.Clear(ClearOptions.Target | ClearOptions.Stencil, this.AmbientColor, 0, 1);

            this.DrawLightmap(gameTime);

            this.GraphicsDevice.SetRenderTarget(null);
        }

        /// <summary>
        /// Called before starting the pass, and before the viewport and matrix have been set.
        /// </summary>
        /// <param name="pass">The pass.</param>
        protected void OnPassStart(ILightmapPass pass)
        {
            var handler = this.PassStart;
            if (handler != null)
            {
                handler(pass);
            }
        }

        /// <summary>
        /// Called before drawing the pass, but after the viewport and matrix have been set.
        /// </summary>
        /// <param name="pass">The pass.</param>
        protected void OnPassRunning(ILightmapPass pass)
        {
            var handler = this.PassRunning;
            if (handler != null)
            {
                handler(pass);
            }
        }

        /// <summary>
        /// Called after drawing is complete.
        /// </summary>
        /// <param name="pass">The pass.</param>
        protected void OnPassComplete(ILightmapPass pass)
        {
            var handler = this.PassComplete;
            if (handler != null)
            {
                handler(pass);
            }
        }

        /// <summary>
        /// Draws all lights and hulls
        /// </summary>
        /// <param name="gameTime">The game Time.</param>
        protected void DrawLightmap(GameTime gameTime)
        {
            foreach (var pass in this.Passes)
            {
                // Start Pass
                this.OnPassStart(pass);
                this.GraphicsDevice.Viewport = pass.Viewport;
                this.Effect.Matrix = pass.Matrix;

                // Execute Pass
                this.OnPassRunning(pass);
                this.Generator.DrawLightmap(pass, this.Lights, this.Hulls);

                // Complete Pass
                this.OnPassComplete(pass);
            }
        }

        /// <summary>
        /// Called when graphics resources need to be loaded. Override this method to load any component-specific graphics resources.
        /// </summary>
        protected override void LoadContent()
        {
            this.LoadEffect();

            // Can add injection here, like above.
            this.drawBuffer = new LightmapDrawBuffer(this.GraphicsDevice);

            this.CreateLightmapTarget();

            this.GraphicsDevice.DeviceReset += this.GraphicsDeviceReset;
    
            this.Generator = new LightmapGenerator(this.GraphicsDevice, this.Effect, this.drawBuffer);
        }

        /// <summary>
        /// Loads the Lightmap Effect
        /// </summary>
        /// <exception cref="ContentLoadException">
        /// Throw if no Lightmap Effect can be located
        /// </exception>
        private void LoadEffect()
        {
            // Load Effect
            var effectProvider = this.Game.Components.OfType<ILightmapEffectProvider>().FirstOrDefault();

            if (effectProvider != null)
            {
                this.Effect = effectProvider.Effect;
            }
            else
            {
                var unwrappedEffect = this.Game.Content.Load<Effect>("Krypton/KryptonEffect");

                this.Effect = new LightmapEffect(unwrappedEffect);
            }

            if (this.Effect == null)
            {
                throw new ContentLoadException("Unable to obtain / load the Krypton Effect");
            }
        }

        /// <summary>
        /// Creates the lightmap texture based on the current backbuffer size
        /// </summary>
        private void CreateLightmapTarget()
        {
            if (this.Lightmap != null)
            {
                this.Lightmap.Dispose();
            }

            // Generate Lightmap Target
            this.Lightmap = new RenderTarget2D(
                this.GraphicsDevice,
                this.GraphicsDevice.PresentationParameters.BackBufferWidth,
                this.GraphicsDevice.PresentationParameters.BackBufferHeight,
                false,
                SurfaceFormat.Color,
                DepthFormat.Depth24Stencil8,
                0,
                RenderTargetUsage.PlatformContents);
        }

        /// <summary>
        /// Used to regenerate graphical resources once the graphics device has been reset
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The args.</param>
        private void GraphicsDeviceReset(object sender, EventArgs args)
        {
            this.CreateLightmapTarget();
        }
    }
}
