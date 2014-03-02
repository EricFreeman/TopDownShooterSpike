using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.Xna.Framework;
using TopDownShooterSpike.Simulation;

namespace TopDownShooterSpike.Managers
{
    public interface IActorManager
    {
        T CreateActor<T>() where T : Actor;
        void DestroyActor(Actor actor);
        IList<Actor> Actors { get; }
    }

    public class ActorManager : GameComponent, IActorManager
    {
        private readonly Dictionary<int, Actor> _actorMap;
        private readonly List<Actor> _actorList;

        public ActorManager(Game game) : base(game)
        {
            _actorMap = new Dictionary<int, Actor>();
            _actorList = new List<Actor>();
        }

        public override void Update(GameTime gameTime)
        {
            for (int index = _actorList.Count - 1; index >= 0; index--)
            {
                var actor = _actorList[index];

                if(actor.Enabled)
                    actor.Update(gameTime);
            }
        }

        public T CreateActor<T>() where T : Actor
        {
            var actorType = typeof (T);
            var actor = Activator.CreateInstance(actorType, this, Game.Services) as T;

            if (actor != null)
            {
                _actorMap.Add(actor.Id, actor);
                _actorList.Add(actor);

                return actor;
            }

            throw new InvalidOperationException("This should never happen.");
        }

        public void DestroyActor(Actor actor)
        {
            if (_actorList.Remove(actor))
                _actorMap.Remove(actor.Id);
            else
                throw new ArgumentException();
        }

        public IList<Actor> Actors
        {
            get { return new ReadOnlyCollection<Actor>(_actorList); }
        }
    }
}