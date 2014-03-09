using System.Collections.Generic;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;
using FarseerPhysics.Factories;
using TopDownShooterSpike.Managers;

namespace TopDownShooterSpike.Simulation.Objects
{
    public class Entity : KActor
    {
        private Fixture _mainCollision;

        public Entity(IActorManager actorManager, IActorService service) : base(actorManager, service)
        {
            InitializeFixture(CreateFixtures);

            MainBody.Mass = 30;
        }

        private IEnumerable<Fixture> CreateFixtures(World world , Body body)
        {
            yield return _mainCollision = FixtureFactory.AttachCircle(Meter.FromInches(18), 1, body, this);
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
