using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using Krypton;
using Krypton.Lights;

namespace KryptonTestbed
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class KryptonDemoGame : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;

        KryptonEngine krypton;

        private Texture2D mLightTexture;
        private int mNumLights = 25;
        private int mNumHorzontalHulls = 20;
        private int mNumVerticalHulls = 20;

        private float mVerticalUnits = 50;

        private Light2D mLight2D;

        Random mRandom = new Random();

        public KryptonDemoGame()
        {
            // Setup the graphics device manager with some default settings
            this.graphics = new GraphicsDeviceManager(this);
            this.graphics.PreferredBackBufferWidth = 1280;
            this.graphics.PreferredBackBufferHeight = 720;

            // Allow the window to be resized (to demonstrate render target recreation)
            this.Window.AllowUserResizing = true;

            // Setup the content manager with some default settings
            this.Content.RootDirectory = "Content";

            // Create Krypton
            this.krypton = new KryptonEngine(this, "KryptonEffect");

            // As a side note, you may want Krypton to be used as a GameComponent.
            // To do this, you would simply add the following line of code and remove the Initialize and Draw function of krypton below:
            // this.Components.Add(this.krypton);
        }

        protected override void Initialize()
        {
            // Make sure to initialize krpyton, unless it has been added to the Game's list of Components
            this.krypton.Initialize();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new simple point light texture to use for the lights
            this.mLightTexture = LightTextureBuilder.CreatePointLight(this.GraphicsDevice, 512);

            // Create some lights and hulls
            this.CreateLights(mLightTexture, this.mNumLights);
            this.CreateHulls(this.mNumHorzontalHulls, this.mNumVerticalHulls);

            // Create a light we can control
            this.mLight2D = new Light2D()
            {
                Texture = this.mLightTexture,
                X = 0,
                Y = 0,
                Range = 25,
                Color = Color.Multiply(Color.CornflowerBlue, 2.0f),
                ShadowType = ShadowType.Occluded
            };

            this.krypton.Lights.Add(this.mLight2D);
        }

        private void CreateLights(Texture2D texture, int count)
        {
            // Make some random lights!
            for (int i = 0; i < count; i++)
            {
                byte r = (byte)(this.mRandom.Next(255 - 64) + 64);
                byte g = (byte)(this.mRandom.Next(255 - 64) + 64);
                byte b = (byte)(this.mRandom.Next(255 - 64) + 64);

                Light2D light = new Light2D()
                {
                    Texture = texture,
                    Range = (float)(this.mRandom.NextDouble() * 5 + 5),
                    Color = new Color(r,g,b),
                    //Intensity = (float)(this.mRandom.NextDouble() * 0.25 + 0.75),
                    Intensity = 1f,
                    Angle = MathHelper.TwoPi * (float)this.mRandom.NextDouble(),
                    X = (float)(this.mRandom.NextDouble() * 50 - 25),
                    Y = (float)(this.mRandom.NextDouble() * 50 - 25),
                };

                // Here we set the light's field of view
                if (i % 2 == 0)
                {
                    light.Fov = MathHelper.PiOver2 * (float)(this.mRandom.NextDouble() * 0.75 + 0.25);
                }

                this.krypton.Lights.Add(light);
            }
        }

        private void CreateHulls(int x, int y)
        {
            float w = 50;
            float h = 50;

            // Make lines of lines of hulls!
            for (int j = 0; j < y; j++)
            {
                // Make lines of hulls!
                for (int i = 0; i < x; i++)
                {
                    var posX = (((i + 0.5f) * w) / x) - w / 2 + (j % 2 == 0 ? w / x / 2 : 0);
                    var posY = (((j + 0.5f) * h) / y) - h / 2; // +(i % 2 == 0 ? h / y / 4 : 0);

                    var hull = ShadowHull.CreateRectangle(Vector2.One*1f);
                    hull.Position.X = posX;
                    hull.Position.Y = posY;
                    hull.Scale.X = (float)(this.mRandom.NextDouble() * 0.75f + 0.25f);
                    hull.Scale.Y = (float)(this.mRandom.NextDouble() * 0.75f + 0.25f);

                    krypton.Hulls.Add(hull);
                }
            }
        }

        protected override void UnloadContent()
        {
            // Not sure if anything actually NEEDS to go here, as the game exits immediately upon unloading content. Please advise if you think this is bad :)
        }

        protected override void Update(GameTime gameTime)
        {
            // Make sure the user doesn't want to quit (but why would they?)
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.Escape))
                this.Exit();

            // make it much simpler to deal with the time :)
            var t = (float)gameTime.ElapsedGameTime.TotalSeconds;

            var speed = 5;

            // Allow for randomization of lights and hulls, to demonstrait that each hull and light is individually rendered
            if (Keyboard.GetState().IsKeyDown(Keys.R))
            {
                // randomize lights
                foreach (Light2D light in this.krypton.Lights)
                {
                    light.Position += Vector2.UnitY * (float)(this.mRandom.NextDouble() * 2 - 1) * t * speed;
                    light.Position += Vector2.UnitX * (float)(this.mRandom.NextDouble() * 2 - 1) * t * speed;
                    light.Angle -= MathHelper.TwoPi * (float)(this.mRandom.NextDouble() * 2 - 1) * t * speed;
                }

                // randomize hulls
                foreach (var hull in this.krypton.Hulls)
                {
                    hull.Position += Vector2.UnitY * (float)(this.mRandom.NextDouble() * 2 - 1) * t * speed;
                    hull.Position += Vector2.UnitX * (float)(this.mRandom.NextDouble() * 2 - 1) * t * speed;
                    hull.Angle -= MathHelper.TwoPi * (float)(this.mRandom.NextDouble() * 2 - 1) * t * speed;
                }
            }

            var keyboard = Keyboard.GetState();

            // Light Position Controls
            if (keyboard.IsKeyDown(Keys.Up))
                this.mLight2D.Y += t * speed;

            if (keyboard.IsKeyDown(Keys.Down))
                this.mLight2D.Y -= t * speed;

            if (keyboard.IsKeyDown(Keys.Right))
                this.mLight2D.X += t * speed;

            if (keyboard.IsKeyDown(Keys.Left))
                this.mLight2D.X -= t * speed;

            // Shadow Type Controls
            if (keyboard.IsKeyDown(Keys.D1))
                this.mLight2D.ShadowType = ShadowType.Solid;

            if (keyboard.IsKeyDown(Keys.D2))
                this.mLight2D.ShadowType = ShadowType.Illuminated;

            if (keyboard.IsKeyDown(Keys.D3))
                this.mLight2D.ShadowType = ShadowType.Occluded;
            
            // Shadow Opacity Controls
            if (keyboard.IsKeyDown(Keys.O))
                this.krypton.Hulls.ForEach(x => x.Opacity = MathHelper.Clamp(x.Opacity - t, 0, 1));
            
            if (keyboard.IsKeyDown(Keys.P))
                this.krypton.Hulls.ForEach(x => x.Opacity = MathHelper.Clamp(x.Opacity + t, 0, 1));

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            // Create a world view projection matrix to use with krypton
            Matrix world = Matrix.Identity;
            Matrix view = Matrix.CreateTranslation(new Vector3(0, 0, 0) * -1f);
            Matrix projection = Matrix.CreateOrthographic(this.mVerticalUnits * this.GraphicsDevice.Viewport.AspectRatio, this.mVerticalUnits, 0, 1);
            Matrix wvp = world * view * projection;

            // Assign the matrix and pre-render the lightmap.
            // Make sure not to change the position of any lights or shadow hulls after this call, as it won't take effect till the next frame!
            this.krypton.Matrix = wvp;
            this.krypton.LightMapPrepare();

            // Make sure we clear the backbuffer *after* Krypton is done pre-rendering
            this.GraphicsDevice.Clear(Color.White);

            // ----- DRAW STUFF HERE ----- //
            // By drawing here, you ensure that your scene is properly lit by krypton.
            // Drawing after KryptonEngine.Draw will cause you objects to be drawn on top of the lightmap (can be useful, fyi)
            // ----- DRAW STUFF HERE ----- //

            // Draw hulls
            this.DebugDrawHulls(true);

            // Draw krypton (This can be omited if krypton is in the Component list. It will simply draw krypton when base.Draw is called
            this.krypton.Draw(gameTime);

            if (Keyboard.GetState().IsKeyDown(Keys.H))
            {
                // Draw hulls
                this.DebugDrawHulls(false);
            }

            if (Keyboard.GetState().IsKeyDown(Keys.L))
            {
                // Draw hulls
                this.DebugDrawLights();
            }

            base.Draw(gameTime);
        }

        private void DebugDrawHulls(bool drawSolid)
        {
            this.krypton.RenderHelper.Effect.CurrentTechnique = this.krypton.RenderHelper.Effect.Techniques["DebugDraw"];

            this.GraphicsDevice.RasterizerState = new RasterizerState()
            {
                CullMode = CullMode.None,
                FillMode = drawSolid ? FillMode.Solid : FillMode.WireFrame,
            };

            // Clear the helpers vertices
            this.krypton.RenderHelper.ShadowHullVertices.Clear();
            this.krypton.RenderHelper.ShadowHullIndicies.Clear();

            foreach (var hull in krypton.Hulls)
            {
                this.krypton.RenderHelper.BufferAddShadowHull(hull);
            }


            foreach (var effectPass in krypton.RenderHelper.Effect.CurrentTechnique.Passes)
            {
                effectPass.Apply();
                this.krypton.RenderHelper.BufferDraw();
            }

        }

        private void DebugDrawLights()
        {
            this.krypton.RenderHelper.Effect.CurrentTechnique = this.krypton.RenderHelper.Effect.Techniques["DebugDraw"];

            this.GraphicsDevice.RasterizerState = new RasterizerState()
            {
                CullMode = CullMode.None,
                FillMode = FillMode.WireFrame,
            };

            // Clear the helpers vertices
            this.krypton.RenderHelper.ShadowHullVertices.Clear();
            this.krypton.RenderHelper.ShadowHullIndicies.Clear();

            foreach (Light2D light in krypton.Lights)
            {
                this.krypton.RenderHelper.BufferAddBoundOutline(light.Bounds);
            }

            foreach (var effectPass in krypton.RenderHelper.Effect.CurrentTechnique.Passes)
            {
                effectPass.Apply();
                this.krypton.RenderHelper.BufferDraw();
            }

        }
    }
}
