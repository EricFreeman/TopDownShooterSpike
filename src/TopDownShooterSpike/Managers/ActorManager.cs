﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using TopDownShooterSpike.Simulation;

namespace TopDownShooterSpike.Managers
{
    public interface IActorManager
    {
        T CreateActor<T>(Func<IActorManager, IActorService, T> create) where T : Actor;
        void DestroyActor(Actor actor);
        void TearDown();
        IList<Actor> Actors { get; }
    }

    public interface IActorService
    {
        IActorEventAggregator EventAggregator { get; }
        IDeviceInputService InputService { get; }
        SimulationSettings SimulationSettings { get; }
        World PhysicsSystem { get; }
    }

    public class ActorManager : IActorManager, IActorService
    {
        private readonly Dictionary<int, Actor> _actorMap;
        private readonly List<Actor> _actorList;
        private readonly ActorEventAggregator _eventAggregator;
        private readonly World _physicsWorld;
        private readonly SimulationSettings _simulationSettings;
        private readonly IDeviceInputService _inputService;

        public ActorManager(IDeviceInputService deviceInputService)
        {
            _inputService = deviceInputService;
            _physicsWorld = new World(Vector2.Zero);
            _eventAggregator = new ActorEventAggregator();
            _actorMap = new Dictionary<int, Actor>();
            _actorList = new List<Actor>();
            _simulationSettings = new SimulationSettings();
        }

        public void TearDown()
        {
            _actorList.Clear();
            _actorMap.Clear();

            GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);
        }

        public void Update(GameTime gameTime)
        {
            _physicsWorld.Step((float)gameTime.ElapsedGameTime.TotalSeconds);

            for (int index = _actorList.Count - 1; index >= 0; index--)
            {
                var actor = _actorList[index];

                if(actor.Enabled)
                    actor.Update(gameTime);
            }
        }

        public T CreateActor<T>(Func<IActorManager, IActorService, T> create) where T : Actor
        {
            var actor = create(this, this);

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

        public IActorEventAggregator EventAggregator
        {
            get { return _eventAggregator; }
        }

        public IDeviceInputService InputService
        {
            get { return _inputService; }
        }

        public SimulationSettings SimulationSettings
        {
            get { return _simulationSettings; }
        }

        public World PhysicsSystem
        {
            get { return _physicsWorld; }
        }
    }
}