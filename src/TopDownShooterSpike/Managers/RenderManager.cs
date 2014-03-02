using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TopDownShooterSpike.Graphics;
using TopDownShooterSpike.Simulation;

namespace TopDownShooterSpike.Managers
{
    public interface IRenderManager
    {
        
    }

    public class RenderManager : DrawableGameComponent, IRenderManager
    {
        private readonly IServiceProvider _serviceProvider;
        private IActorManager _actorManager;
        private RenderContext _context;

        public RenderManager(Game game) : base(game)
        {
            _serviceProvider = game.Services;
        }

        public override void Initialize()
        {
            _actorManager = _serviceProvider.GetService<IActorManager>();
            var spriteBatch = _serviceProvider.GetService<SpriteBatch>();

            _context = new RenderContext(spriteBatch, Game.Content);
        }

        public override void Draw(GameTime gameTime)
        {
            _context.GameTime = gameTime;

            var actorsToRender = _actorManager.Actors;
            for (int index = 0; index < actorsToRender.Count; index++)
            {
                var actor = actorsToRender[index];
                var renderObject = actor.RenderObject;

                if (actor.Enabled && renderObject.Visible)
                {
                    if (renderObject.Lit)
                    {
                        
                    }
                    else
                    {
                        
                    }

                    renderObject.Draw(_context);
                }
            }
        }
    }
}
