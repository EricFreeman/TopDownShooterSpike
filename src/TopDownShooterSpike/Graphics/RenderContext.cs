using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TopDownShooterSpike.Managers;
using TopDownShooterSpike.Simulation;

namespace TopDownShooterSpike.Graphics
{
    public class RenderContext
    {
        private readonly SpriteBatch _spriteBatch;
//        private PrimitiveBatch _primitiveBatch;

        private readonly ContentManager _content;
        private readonly SimulationSettings _simSettings;

        public RenderContext( SpriteBatch spriteBatch, ContentManager content)
        {
            _spriteBatch = spriteBatch;
            _content = content;
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

        public SpriteBatch SpriteBatch
        {
            get { return _spriteBatch; }
        }

    }
}