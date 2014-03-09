using Microsoft.Xna.Framework;

namespace TopDownShooterSpike.Graphics
{
    public class Camera 
    {
        private Transform2D _transform;

        public void MoveTo(Vector2 absolutePosition)
        {
            _transform.Position = absolutePosition;
        }

        public void RotateTo(float absoluteRotation)
        {
            _transform.Rotation = absoluteRotation;
        }
        
        public Transform2D Transform
        {
            get { return _transform; }
            set { _transform = value; }
        }

        public Rectangle Viewport { get; set; }
    }
}
