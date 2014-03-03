using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using TopDownShooterSpike.Managers;

namespace TopDownShooterSpike.Simulation.Objects
{
    public class Entity : KActor
    {
        private Fixture _mainCollision;

        public Entity(IActorManager actorManager, IActorService service) : base(actorManager, service)
        {
            InitializeFixture((w, b) =>
            {
                _mainCollision = FixtureFactory.AttachCircle(0.25f, 1, b, this);
            });

            MainBody.Mass = 30;
        }

    }
}
