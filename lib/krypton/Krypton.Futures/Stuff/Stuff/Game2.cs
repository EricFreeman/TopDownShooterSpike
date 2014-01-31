// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Game1.cs" company="Christopher Harris">
//   2011 Christopher Harris
// </copyright>
// <summary>
//   AssemblyInfo.cs
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Futures
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Futures.Common;
    using Futures.Krypton;
    using Futures.Krypton.Components;
    using Futures.Krypton.Design;
    using Futures.Krypton.Factories;
    using Futures.Krypton.Fluent;
    using Futures.Krypton.Lights;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;

    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game2 : Game
    {
        /// <summary>
        /// The Graphics Device Manager
        /// </summary>
        private readonly GraphicsDeviceManager graphics;

        /// <summary>
        /// The LightMapper
        /// </summary>
        private readonly LightmapGeneratorComponent lightmapGenerator;

        /// <summary>
        /// The LightMapPresenter
        /// </summary>
        private readonly LightmapPresenterComponent lightmapPresenter;

        /// <summary>
        /// The BackBufferSlider
        /// </summary>
        private readonly BackBufferSlider backBufferSlider;

        /// <summary>
        /// The Default Light
        /// </summary>
        private PointLight light;

        /// <summary>
        /// The Default Hull
        /// </summary>
        private Hull hull;

        private GameTime time;

        /// <summary>
        /// Initializes a new instance of the <see cref="Game1"/> class.
        /// </summary>
        public Game2()
        {
            this.graphics = new GraphicsDeviceManager(this);
            this.Content.RootDirectory = "Content";

            this.Window.AllowUserResizing = true;

            this.lightmapGenerator = new LightmapGeneratorComponent(this, this.Passes);
            this.lightmapPresenter = new LightmapPresenterComponent(this, this.lightmapGenerator);

            this.backBufferSlider = new BackBufferSlider(this);

            this.Components.Add(this.lightmapGenerator);
            this.Components.Add(this.backBufferSlider);
            this.Components.Add(this.lightmapPresenter);
        }

        protected IEnumerable<ILightmapPass> Passes
        {
            get
            {
                var w = this.GraphicsDevice.Viewport.Width;
                var h = this.GraphicsDevice.Viewport.Height;

                var aspect = (float)w/h;

                const float VerticalUnits = 100f;

                yield return
                    new LightmapPass(
                        this.GraphicsDevice.Viewport,
                        Matrix.CreateOrthographic(VerticalUnits*aspect, VerticalUnits, 0, 1));
            }
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            this.hull = HullFactory.CreateRectangle(10, 10);

            var lightTexture = TextureFactory.CreatePoint(this.GraphicsDevice, 256);

            this.light = new PointLight(lightTexture)
                {
                    Radius = 100,
                    Intensity = 0.5f,
                    Position = new Vector2(0, 0),
                    ShadowType = ShadowType.Illuminated,
                };

            this.light.Position = Vector2.UnitX * 30;

            var r = new Random();

            this.GenerateTestHulls();

            this.lightmapGenerator.Hulls.Add(this.hull);
            this.lightmapGenerator.Lights.Add(this.light);
        }

        private void GenerateTestHulls()
        {
            for (var x = 0; x < 20; x++)
            {
                for (var y = 0; y < 20; y++)
                {
                    this.lightmapGenerator.Hulls.Add(HullFactory.CreateRectangle(2, 2).Position((x*20) - 10, (y*20) - 10));
                }
            }
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                this.Exit();
            }

            this.hull.Angle = (float)Math.Sin(gameTime.TotalGameTime.TotalSeconds) * MathHelper.PiOver4;

            this.time = gameTime;

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            this.lightmapPresenter.Visible = Keyboard.GetState().IsKeyUp(Keys.L);

            base.Draw(gameTime);
        }
    }
}