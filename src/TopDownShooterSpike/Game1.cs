#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
using TopDownShooterSpike.Managers;

#endregion

namespace TopDownShooterSpike
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game
    {
        private ScreenManager _screenManager;

        readonly GraphicsDeviceManager _graphics;
        SpriteBatch _spriteBatch;
        private AudioManager _audioManager;
        private InputManager _inputManager;
        private RenderManager _renderManager;
        private ActorManager _actorManager;

        public Game1() : base()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            _screenManager = new ScreenManager(this, _graphics);
            _audioManager = new AudioManager(this);
            _inputManager = new InputManager(this);
            _renderManager = new RenderManager(this);
            _actorManager = new ActorManager(this);

            Services.AddService(typeof(IActorManager), _actorManager);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
//            ScreenManager.Instance.GraphicsDevice = GraphicsDevice;
//            ScreenManager.Instance.SpriteBatch = _spriteBatch;
//            ScreenManager.Instance.LoadContent(Content);
//            AudioManager.Instance.FadeTicks = 1;
        }

        protected override void UnloadContent()
        {
        }
    }
}
