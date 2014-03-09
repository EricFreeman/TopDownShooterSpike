using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TopDownShooterSpike.Simulation;

namespace TopDownShooterSpike.Graphics
{
    public class RenderContext
    {
        private readonly PrimitiveBatch _primitiveBatch;
        private readonly SpriteBatch _spriteBatch;
        private readonly SimulationSettings _simSettings;

        public RenderContext(PrimitiveBatch primitiveBatch, SpriteBatch spriteBatch)
        {
            _primitiveBatch = primitiveBatch;
            _spriteBatch = spriteBatch;
            _simSettings = new SimulationSettings();
            Camera = new Camera
            {
                Transform = Transform2D.Zero,
                Viewport =
                    new Rectangle(0, 0, spriteBatch.GraphicsDevice.Viewport.Width,
                        spriteBatch.GraphicsDevice.Viewport.Height)
            };
        }

        public SimulationSettings SimulationSettings { get { return _simSettings; } }
        public GameTime GameTime { get; set; }
        public Camera Camera { get; set; }

        public PrimitiveBatch PrimitiveBatch
        {
            get { return _primitiveBatch; }
        }

        public SpriteBatch SpriteBatch
        {
            get { return _spriteBatch; }
        }

    }
}