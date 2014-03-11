using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using TopDownShooterSpike.Simulation;

namespace TopDownShooterSpike.Managers
{
    public interface IActorManager : IDisposable
    {
        T CreateActor<T>(Func<IActorManager, IActorService, T> create) where T : Actor;
        void DestroyActor(Actor actor);
        void TearDown();
        IList<Actor> Actors { get; }
        void Update(GameTime gameTime);
    }
}