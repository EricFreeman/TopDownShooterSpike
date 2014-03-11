using System.Collections.Generic;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;
using FarseerPhysics.Factories;
using TopDownShooterSpike.Managers;

namespace TopDownShooterSpike.Simulation.Objects
{
    /// <summary>
    /// A character in the game
    /// </summary>
    public class Pawn : KActor
    {
        private Fixture _mainCollision;

        public Pawn(IActorService service) : base(service)
        {
            InitializeFixture(CreateFixtures);
            MainBody.Mass = Kilogram.FromPounds(180);

            MainBody.BodyType = BodyType.Dynamic;
            MainBody.Enabled = true;
            MainBody.Friction = 0.45f;
        }

        private IEnumerable<Fixture> CreateFixtures(World world , Body body)
        { yield return _mainCollision = FixtureFactory.AttachCircle(Meter.FromInches(18), 1, body, this); }

        protected override bool PreCollision(Fixture a, Fixture b) { return true; }
        protected override bool Collision(Fixture a, Fixture b, Contact contact) { return true; }
        protected override void PostCollision(Fixture a, Fixture b, Contact contact, ContactVelocityConstraint impulse) { }
        protected override void Separation(Fixture a, Fixture b) { }
    }
}
