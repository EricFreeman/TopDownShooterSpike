using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using TopDownShooterSpike.Graphics;
using TopDownShooterSpike.Managers;
using TopDownShooterSpike.Simulation.Objects;

namespace TopDownShooterSpike.State
{
    public sealed class GameplayState : GameState
    {
        private readonly ContentManager _content;

        private readonly IActorManager _actorManager;
        private readonly IRenderManager _renderManager;

        public GameplayState(IActorManager actorManager, IRenderManager renderManager, ContentManager content)
        {
            _content = content;

            _actorManager = actorManager;
            _renderManager = renderManager;
        }

        #region Initialization

        public void InitializeWorld()
        {
            _actorManager.CreateActor((man, serv) => new Map(serv, new DefaultTileProvider(_content), 32, 32));
            _actorManager.CreateActor((man, serv) => new Wall(serv));
        }

        #endregion

        #region boilerplate

        public override void Initialize()
        {
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
            _renderManager.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            _renderManager.Draw(gameTime);
        }

        #endregion
    }
}
