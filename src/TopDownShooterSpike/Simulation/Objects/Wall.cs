using System.Collections.Generic;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using TopDownShooterSpike.Managers;

namespace TopDownShooterSpike.Simulation.Objects
{
    public class Wall : KActor
    {
        private Fixture _mainCollision;

        public Wall(IActorManager actorManager, IActorService service) : base(actorManager, service)
        {
            InitializeFixture(InitializeCollision);
        }

        private IEnumerable<Fixture> InitializeCollision(World arg1, Body arg2)
        {
            yield return (_mainCollision = FixtureFactory.AttachRectangle(Meter.FromInches(6), new Kilogram(1.5f), 1, Vector2.Zero, arg2, this));
        }

        protected override bool PreCollision(Fixture a, Fixture b)
        {
            return true;
        }

        protected override void PostCollision(Fixture a, Fixture b, Contact contact, ContactVelocityConstraint impulse)
        {

        }

        protected override bool Collision(Fixture a, Fixture b, Contact contact)
        {
            return true;
        }

        protected override void Separation(Fixture a, Fixture b)
        {
        }
    }
}