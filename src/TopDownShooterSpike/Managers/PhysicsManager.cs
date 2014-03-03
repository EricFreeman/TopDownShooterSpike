using Microsoft.Xna.Framework;

namespace TopDownShooterSpike.Managers
{
    public interface IPhysicsManager
    {
        FarseerPhysics.Dynamics.World PhysicsWorld { get; }
    }

    public class PhysicsManager 
    {
        private readonly FarseerPhysics.Dynamics.World _physicsWorld;

        public PhysicsManager()
        {
            _physicsWorld = new FarseerPhysics.Dynamics.World(Vector2.Zero);
        }

        public void Update(GameTime gameTime)
        {
            _physicsWorld.Step((float)gameTime.ElapsedGameTime.TotalSeconds);
        }

        public FarseerPhysics.Dynamics.World PhysicsWorld
        {
            get { return _physicsWorld; }
        }
    }
}