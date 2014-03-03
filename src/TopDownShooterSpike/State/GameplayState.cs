using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TopDownShooterSpike.Managers;

namespace TopDownShooterSpike.State
{
    public sealed class GameplayState : GameState
    {
        private readonly ContentManager _content;
        private readonly SpriteBatch _spriteBatch;
        private ActorManager _actorManager;
        private RenderManager _renderManager;

        public GameplayState(ContentManager content, SpriteBatch spriteBatch)
        {
            _content = content;
            _spriteBatch = spriteBatch;
        }

        #region initialization

        public void InitializeWorld(IActorManager manager, IActorService _service)
        {
            
        }

        #endregion

        #region boilerplate

        public override void Initialize()
        {
            _actorManager = new ActorManager();
            _renderManager = new RenderManager(_actorManager, _spriteBatch, _content);

            // initialize game here
            InitializeWorld(_actorManager, _actorManager);
        }

        public override void TearDown()
        {
            _actorManager.TearDown();
        }

        public override void Update(GameTime gameTime)
        {
            _actorManager.Update(gameTime);
//            _renderManager.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            _renderManager.Draw(gameTime);
        }

        #endregion
    }
}
