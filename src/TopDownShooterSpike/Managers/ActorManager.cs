using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using TopDownShooterSpike.Simulation;

namespace TopDownShooterSpike.Managers
{
    public interface IActorManager
    {
        Actor CreateActor<T>(Func<T> actorFunc) where T : Actor;
        void DestroyActor(Actor actor);
        void DestroyActor(int id);
    }

    public class ActorManager : GameComponent, IActorManager
    {
        private readonly Dictionary<int, Actor> _actorMap;

        public ActorManager(Game game) : base(game)
        {
            _actorMap = new Dictionary<int, Actor>();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public Actor CreateActor<T>() where T : Actor
        {
            
        }

        public void DestroyActor(Actor actor)
        {
            throw new System.NotImplementedException();
        }
    }
}