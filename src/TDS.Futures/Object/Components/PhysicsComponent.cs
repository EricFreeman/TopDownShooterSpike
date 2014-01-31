using Microsoft.Xna.Framework;

namespace TDS.Futures.Object.Components
{
    /// <summary>
    /// A component that describes how an <see cref="Entity"/> should move in a physical simulation
    /// </summary>
    public class PhysicsComponent : EntityComponent
    {
        private Vector3 _velocity;
        private Vector3 _acceleration;
        private Vector3 _angularVelocity;
        private float _mass;

        /// <summary>
        /// Initializes this component with default values
        /// </summary>
        public override void Initialize()
        {
            Velocity = Acceleration = AngularVelocity = Vector3.Zero;
            Mass = 0;
        }

        /// <summary>
        /// Deep clones this component, returning a new instance with matching values
        /// </summary>
        /// <returns/>
        public override IEntityComponent Clone()
        {
            return new PhysicsComponent
                {
                    _velocity =  Velocity,
                    _acceleration =  Acceleration,
                    _angularVelocity = AngularVelocity,
                    _mass = Mass,
                };
        }

        public Vector3 Velocity
        {
            get { return _velocity; }
            set { _velocity = value; }
        }

        public Vector3 Acceleration
        {
            get { return _acceleration; }
            set { _acceleration = value; }
        }

        public Vector3 AngularVelocity
        {
            get { return _angularVelocity; }
            set { _angularVelocity = value; }
        }

        public float Mass
        {
            get { return _mass; }
            set { _mass = value; }
        }
    }
}