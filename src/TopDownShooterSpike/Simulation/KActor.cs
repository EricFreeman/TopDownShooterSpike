using System;
using System.Collections.Generic;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using TopDownShooterSpike.Graphics;
using TopDownShooterSpike.Managers;

namespace TopDownShooterSpike.Simulation
{
    public abstract class KActor : Actor
    {
        protected World _world;
        private readonly Body _mainBody;

        protected KActor(IActorManager actorManager, IActorService service) : base(actorManager, service)
        {
            _mainBody = BodyFactory.CreateBody(service.PhysicsSystem, Transform.Position, Transform.Rotation, this);
        }

        public void InitializeFixture(Func<World, Body, IEnumerable<Fixture>> fixtureFunc)
        {
           fixtureFunc(ActorService.PhysicsSystem, MainBody).For(x =>
           {
               x.OnCollision += OnCollision;
               x.OnSeparation += OnSeparation;
               x.AfterCollision += AfterCollision;
               x.BeforeCollision += BeforeCollision;
           });
        }

        // any collision object attached to this actor WILL invoke these functions, 
        // good for if you want general collision detection/response
        #region Aggregate Collision Functions

        private void AfterCollision(Fixture fixturea, Fixture fixtureb, Contact contact, ContactVelocityConstraint impulse)
        {
            PostCollision(fixturea, fixtureb, contact, impulse);
        }

        private bool BeforeCollision(Fixture fixturea, Fixture fixtureb)
        {
            var collisionResult = PreCollision(fixturea, fixtureb);
            return collisionResult;
        }

        private bool OnCollision(Fixture a, Fixture b, Contact contact)
        {
            var collisionResult =  Collision(a, b, contact);
            return collisionResult;
        }

        private void OnSeparation(Fixture a, Fixture b)
        {
            Separation(a, b);
        }

        protected abstract bool PreCollision(Fixture a, Fixture b);
        protected abstract void PostCollision(Fixture a, Fixture b, Contact contact, ContactVelocityConstraint impulse);

        protected abstract bool Collision(Fixture a, Fixture b, Contact contact);
        protected abstract void Separation(Fixture a, Fixture b);

        #endregion

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
                if (_mainBody != null)
                {
                    base.Transform = value;
                    _mainBody.SetTransform(value.Position, value.Rotation);
                }
            }
        }
    }
}