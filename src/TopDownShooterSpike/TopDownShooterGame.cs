#region Using Statements

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
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

            _container.Create<GraphicsDeviceManager>();
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            _container.Inject<PrimitiveBatch, PrimitiveBatch>();
            _container.Inject<SpriteBatch, SpriteBatch>();
            _container.Inject<ContentManager, ContentManager>(Content);
            _container.Inject<GameComponentCollection, GameComponentCollection>(Components);

            // make sure if you add a game component that 
            // it adds itself to the components collection in the constructor
            // the code is cleaner this way
            _container.Inject<IDeviceInputService, InputManager>(ObjectScope.Singleton);
//            _container.Inject<IAudioManager, AudioManager>(ObjectScope.Singleton);

            _container.Inject<IGameStateStack, GameStateStack>(s =>
            {
                var stateStack = s.Create<GameStateStack>();

                // this is the entry game state
                // eventually this will be the title screen
                var state = s.Create<GameplayState>();

                stateStack.Push(state);

                return stateStack;
            }, ObjectScope.Singleton);

            _container.Inject<IActorManager, ActorManager>(ObjectScope.Singleton);
            _container.Inject<IRenderManager, RenderManager>(ObjectScope.Singleton);

            base.Initialize();
        }

        protected override void LoadContent()
        { _container.Create<IGameStateStack>(); }

        protected override void UnloadContent()
        { _container.Dispose(); }
    }
}