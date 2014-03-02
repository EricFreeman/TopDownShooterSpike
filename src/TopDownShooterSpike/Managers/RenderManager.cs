using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace TopDownShooterSpike.Managers
{
    public class EntityManager : GameComponent
    {
        public EntityManager(Game game) : base(game)
        {
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }

    public class RenderManager : DrawableGameComponent
    {
        public RenderManager(Game game) : base(game)
        {
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }
    }
}
