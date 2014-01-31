using Microsoft.Xna.Framework;

namespace TDS.Futures.Object.Components
{
    /// <summary>
    /// A component that describes an <see cref="Entity"/>'s position in 3D space
    /// </summary>
    public class Position2DComponent : EntityComponent
    {
        private Transform2D _transform;

        /// <summary>
        /// Initializes this component with default values
        /// </summary>
        public override void Initialize()
        { _transform = Transform2D.Zero; }

        /// <summary>
        /// Deep clones this component, returning a new instance with matching values
        /// </summary>
        /// <returns></returns>
        public override IEntityComponent Clone()
        {
            return new Position2DComponent { _transform = _transform };
        }

        /// <summary>
        /// Gets or set the transform on this component
        /// </summary>
        public Transform2D Transform
        {
            get { return _transform; }
            set { _transform = value; }
        }

        /// <summary>
        /// Gets or sets the location for this component
        /// </summary>
        public Vector2 Location
        {
            get { return _transform.Position; }
            set { _transform.Position = value; }
        }

        /// <summary>
        /// Gets or sets the rotation for this component
        /// </summary>
        public float Rotation
        {
            get { return _transform.Rotation; }
            set { _transform.Rotation = value; }
        }


        /// <summary>
        /// Gets or sets a vector that controls horizontal and vertical scaling
        /// </summary>
        public Vector2 MultiAxisScale
        {
            get { return _transform.Scale; }
            set { _transform.Scale = value; }
        }

        /// <summary>
        /// Gets or sets the scale of this transform as a single value
        /// </summary>
        public float Scale
        {
            get { return (_transform.Scale.X + _transform.Scale.Y) / 2.0f; }
            set
            {
                _transform.Scale.X = value;
                _transform.Scale.Y = value;
            }
        }

    }

    public struct Transform2D
    {
        public float Rotation;
        public Vector2 Scale;
        public Vector2 Position;

        public static Transform2D Zero
        {
            get { return default(Transform2D); }
        }
    }
}