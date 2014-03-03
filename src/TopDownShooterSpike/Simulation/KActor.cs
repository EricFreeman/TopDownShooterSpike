using System;
using System.Collections.Generic;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using TopDownShooterSpike.Graphics;
using TopDownShooterSpike.Managers;

namespace TopDownShooterSpike.Simulation
{
    public abstract class KActor : Actor
    {
        protected FarseerPhysics.Dynamics.World _world;
        private readonly Body _mainBody;

        protected KActor(IActorManager actorManager, IActorService service) : base(actorManager, service)
        {
            _mainBody = BodyFactory.CreateBody(service.PhysicsSystem, Transform.Position, Transform.Rotation, this);
        }

        public void InitializeFixture(Action<World, Body> fixtureFunc)
        {
            fixtureFunc(ActorService.PhysicsSystem, MainBody);
        }

        public new void Update(GameTime gameTime)
        {
            _transform.Position = _mainBody.Position;
            _transform.Rotation = _mainBody.Rotation;
            
            base.Update(gameTime);
        }

        public Body MainBody
        {
            get { return _mainBody; }
        }

        public override Transform2D Transform
        {
            get { return base.Transform; }
            set
            {
                if (_mainBody.IsStatic)
                {
                    base.Transform = value;
                    _mainBody.SetTransform(value.Position, value.Rotation);
                }
            }
        }
    }
}