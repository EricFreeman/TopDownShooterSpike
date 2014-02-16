using Microsoft.Xna.Framework;
using TopDownShooterSpike.Managers;

namespace TopDownShooterSpike.GameHelpers
{
    public class Camera
    {
        #region Private Properties

        private float _zoom;
        public Matrix _transform;
        public Vector2 _pos;
        private float _rotation;

        #endregion

        #region Public Properties

        public float Zoom
        {
            get { return _zoom; }
            set { _zoom = value < 0.1f ? 0.1f : value > 2f ? 2f : value; }
        }

        public float Rotation
        {
            get { return _rotation; }
            set { _rotation = value; }
        }

        public void Move(Vector2 amount)
        {
            _pos += amount;
        }
        public Vector2 Pos
        {
            get { return _pos; }
            set { _pos = value; }
        }

        #endregion

        #region Constructor

        public Camera()
        {
            _zoom = 1.0f;
            _rotation = 0.0f;
            _pos = Vector2.Zero;
        }

        #endregion

        #region Helper Methods

        public Matrix GetTransformation()
        {
            _transform = Matrix.CreateTranslation(new Vector3(-_pos.X, -_pos.Y, 0)) *
                         Matrix.CreateRotationZ(Rotation) *
                         Matrix.CreateScale(new Vector3(Zoom, Zoom, 1)) *
                         Matrix.CreateTranslation(new Vector3(ScreenManager.Instance.GraphicsDevice.Viewport.Width * 0.5f,
                                                              ScreenManager.Instance.GraphicsDevice.Viewport.Height * 0.5f, 
                                                              0));
            return _transform;
        }

        public void CenterOn(Image i)
        {
            Pos = new Vector2(i.Position.X, i.Position.Y);
        }

        #endregion
    }
}
