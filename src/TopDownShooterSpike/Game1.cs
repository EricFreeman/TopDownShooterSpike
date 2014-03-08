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
using TopDownShooterSpike.State;

#endregion

namespace TopDownShooterSpike
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game
    {

        readonly GraphicsDeviceManager _graphics;
        SpriteBatch _spriteBatch;
        private RenderManager _renderManager;
        private ActorManager _actorManager;
        private GameStateStack _gameStateManager;

        public Game1() : base()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
//            _screenManager = new ScreenManager(this, _graphics);
            _gameStateManager = new GameStateStack(this);
//            var audioManager = new AudioManager(this);
//            var inputManager = new InputManager(this);

//            var renderManager = new RenderManager(this);
//            var actorManager = new ActorManager();

            Services.AddService(typeof(ContentManager), Content);
            Services.AddService(typeof(IGameStateStack), _gameStateManager);
//            Services.AddService(typeof(AudioManager), audioManager);
//            Services.AddService(typeof(InputManager), inputManager);

            Components.Add(_gameStateManager);
//            Components.Add(inputManager);
//            Components.Add(audioManager);



            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _gameStateManager.Push(new GameplayState(Content, _spriteBatch));
//            ScreenManager.Instance.GraphicsDevice = GraphicsDevice;
//            ScreenManager.Instance.SpriteBatch = _spriteBatch;
//            ScreenManager.Instance.LoadContent(Content);
//            AudioManager.Instance.FadeTicks = 1;
        }

        protected override void UnloadContent()
        {
        }
    }


    public static class ServiceProviderExtensions
    {
        public static T GetService<T>(this IServiceProvider serviceProvider) where T : class
        {
            return serviceProvider.GetService(typeof (T)) as T;
        }
    }

    public static class LinqExtensions
    {
        public static void For<T>(this IEnumerable<T> collection, Action<T> loopBody)
        {
            if(loopBody == null)
                throw new ArgumentNullException();

            foreach (var item in collection)
            {
                loopBody(item);
            }
        }
    }
}
