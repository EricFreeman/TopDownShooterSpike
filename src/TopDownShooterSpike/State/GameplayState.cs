using FarseerPhysics.Dynamics.Contacts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TopDownShooterSpike.Graphics;
using TopDownShooterSpike.Managers;
using TopDownShooterSpike.Simulation;
using TopDownShooterSpike.Simulation.Objects;

namespace TopDownShooterSpike.State
{
    public sealed class GameplayState : GameState
    {
        private readonly ContentManager _content;
        private readonly SpriteBatch _spriteBatch;
        private readonly PrimitiveBatch _primitiveBatch;

        private ActorManager _actorManager;
        private RenderManager _renderManager;

        public GameplayState(ContentManager content, PrimitiveBatch primitiveBatch, SpriteBatch spriteBatch)
        {
            _content = content;
            _spriteBatch = spriteBatch;
            _primitiveBatch = primitiveBatch;
        }

        #region initialization

        public void InitializeWorld()
        {
            _actorManager.CreateActor((man, serv) => new Map(man, serv, new DefaultTileProvider(_content), 32, 32));
            _actorManager.CreateActor((man, serv) => new Wall(man, serv, _content, new Vector2(300, 300)));
        }

        #endregion

        #region boilerplate

        public override void Initialize()
        {
            _actorManager = new ActorManager();
            _renderManager = new RenderManager(_actorManager, _primitiveBatch, _spriteBatch, _content);

            // initialize game here
            InitializeWorld();
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
