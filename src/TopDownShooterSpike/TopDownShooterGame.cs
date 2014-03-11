#region Using Statements

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
using TopDownShooterSpike.Graphics;
using TopDownShooterSpike.Managers;
using TopDownShooterSpike.State;

#endregion

namespace TopDownShooterSpike
{
		
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class TopDownShooterGame : Game
    {
        private readonly IServiceContainer _container;
        readonly GraphicsDeviceManager _graphics;
        private IGameStateStack _gameStateManager;

        public TopDownShooterGame()
        {
            _container = new ServiceContainer(Services);

            _container.Inject<Game, TopDownShooterGame>(s => this);
            _container.Inject<IGraphicsDeviceManager, GraphicsDeviceManager>(s =>
            {
                var gdm = s.Create<GraphicsDeviceManager>();
                s.Inject<IGraphicsDeviceService, GraphicsDeviceManager>(gdm);
                s.Inject<GraphicsDeviceManager, GraphicsDeviceManager>(gdm);
                return gdm;
            }, ObjectScope.Singleton);
            _container.Inject<GraphicsDevice, GraphicsDevice>(s => s.Create<IGraphicsDeviceService>().GraphicsDevice);

            _graphics = _container.Create<GraphicsDeviceManager>();
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            _container.Inject<PrimitiveBatch, PrimitiveBatch>();
            _container.Inject<SpriteBatch, SpriteBatch>();
            _container.Inject<ContentManager, ContentManager>(Content);
            _container.Inject<GameComponentCollection, GameComponentCollection>(Components);

            _container.Inject<IDeviceInputService, InputManager>(ObjectScope.Singleton);
            _container.Inject<IGameStateStack, GameStateStack>(s =>
            {
                var stateStack = s.Create<GameStateStack>();

                // this is entry game state
                var state = s.Create<GameplayState>();

                stateStack.Push(state);

                return stateStack;
            }, ObjectScope.Singleton);

            _container.Inject<IActorManager, ActorManager>(ObjectScope.Singleton);
            _container.Inject<IRenderManager, RenderManager>(ObjectScope.Singleton);


//            Services.AddService(typeof(AudioManager), audioManager);
//            Services.AddService(typeof(InputManager), inputManager);

//            Components.Add(inputManager);
//            Components.Add(audioManager);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _container.Create<IGameStateStack>();
//            ScreenManager.Instance.GraphicsDevice = GraphicsDevice;
//            ScreenManager.Instance.SpriteBatch = _spriteBatch;
//            ScreenManager.Instance.LoadContent(Content);
//            AudioManager.Instance.FadeTicks = 1;
        }

        protected override void UnloadContent()
        {
            _container.Dispose();
        }
    }
}
