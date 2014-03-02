using Microsoft.Xna.Framework.Graphics;
using TopDownShooterSpike.GameHelpers;

namespace TopDownShooterSpike.Simulation
{

    public abstract class RenderObject
    {
        public abstract void Draw(RenderContext context);
    }

    public class RenderContext
    {
        private Camera _camera;
        private SpriteBatch _spriteBatch;

        public SpriteBatch SpriteBatch
        {
            get { return _spriteBatch; }
        }

        public Camera Camera
        {
            get { return _camera; }
        }
    }
}
