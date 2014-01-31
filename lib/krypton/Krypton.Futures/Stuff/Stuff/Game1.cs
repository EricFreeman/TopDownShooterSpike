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
    public class Game1 : Game
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
        public Game1()
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

                var aspect = (float)w / h;

                const float VerticalUnits = 100f;

                var state = Keyboard.GetState();

                if (state.IsKeyUp(Keys.D1))
                {
                    yield return
                        new LightmapPass(
                            this.GraphicsDevice.Viewport,
                            Matrix.CreateRotationZ((float)this.time.TotalGameTime.TotalSeconds/10) *
                            Matrix.CreateOrthographic(VerticalUnits * aspect, VerticalUnits, 0, 1));
                }
                else
                {
                    var rowSize = h / 2;
                    var colSize = w / 2;

                    yield return
                        new LightmapPass(
                            new Viewport(0, 0, colSize, rowSize),
                            Matrix.CreateOrthographic(VerticalUnits * aspect, VerticalUnits, 0, 1));

                    yield return
                        new LightmapPass(
                            new Viewport(colSize, 0, colSize, rowSize),
                            Matrix.CreateOrthographic(VerticalUnits * aspect, VerticalUnits, 0, 1));

                    yield return
                        new LightmapPass(
                            new Viewport(0, rowSize, colSize, rowSize),
                            Matrix.CreateOrthographic(VerticalUnits * aspect, VerticalUnits, 0, 1));

                    yield return
                        new LightmapPass(
                            new Viewport(colSize, rowSize, colSize, rowSize),
                            Matrix.CreateOrthographic(VerticalUnits * aspect, VerticalUnits, 0, 1));
                }
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
                    Radius = 50,
                    Intensity = 0.5f,
                    Position = new Vector2(0, 0),
                };

            this.light.Position = Vector2.UnitX * 30;

            var r = new Random();

            this.GenerateRandomHullsAndLights(lightTexture, r);

            this.GenerateTestHullsAndLights(lightTexture);

            this.lightmapGenerator.Hulls.Add(this.hull);
            // this.lightmapGenerator.Lights.Add(this.light);
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

        /// <summary>
        /// Generates lights for a controlled test environment
        /// </summary>
        /// <param name="lightTexture">The light texture.</param>
        private void GenerateTestHullsAndLights(Texture2D lightTexture)
        {
            this.AddTestLight(lightTexture, new Vector2(74, 42));
            this.AddTestLight(lightTexture, new Vector2(74, -42));
            this.AddTestLight(lightTexture, new Vector2(-74, 42));
            this.AddTestLight(lightTexture, new Vector2(-74, -42));

            this.AddTestLight(lightTexture, new Vector2(15, 0));
            this.AddTestLight(lightTexture, new Vector2(-15, 0));
            this.AddTestLight(lightTexture, new Vector2(0, 15));
            this.AddTestLight(lightTexture, new Vector2(0, -15));
        }

        /// <summary>
        /// Generates a simple test light
        /// </summary>
        /// <param name="lightTexture">The light texture.</param>
        /// <param name="position">THe position.</param>
        private void AddTestLight(Texture2D lightTexture, Vector2 position)
        {
            this.lightmapGenerator.Lights.Add(
                new PointLight(lightTexture)
                    {
                        Radius = 15,
                        Color = Color.White,
                        Position = position,
                        Intensity = 0.65f,
                    });
        }

        /// <summary>
        /// Generates a set of random hulls and lights
        /// </summary>
        /// <param name="lightTexture">The light texture.</param>
        /// <param name="random">The random.</param>
        private void GenerateRandomHullsAndLights(Texture2D lightTexture, Random random)
        {
            // Generate some random lights
            for (var i = 0; i < 10; i++)
            {
                this.AddTestLight(lightTexture, random.NextVector(-50, 50));
            }

            // Generate some random hulls
            for (var i = 0; i < 100; i++)
            {
                this.lightmapGenerator.Hulls.Add(
                    HullFactory.CreateRectangle(2, 1).Position(random.NextVector(-50, 50)).Angle(random.NextAngle()));
            }
        }
    }
}