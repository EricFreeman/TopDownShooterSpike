using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TopDownShooterSpike.Graphics
{
    public class RenderContext
    {
        private readonly SpriteBatch _spriteBatch;
//        private PrimitiveBatch _primitiveBatch;

        private readonly ContentManager _content;

        public RenderContext( SpriteBatch spriteBatch, ContentManager content)
        {
            _spriteBatch = spriteBatch;
            _content = content;
            Camera = new Camera
            {
                Transform = Transform2D.Zero,
                Viewport =
                    new Rectangle(0, 0, spriteBatch.GraphicsDevice.Viewport.Width,
                        spriteBatch.GraphicsDevice.Viewport.Height)
            };
        }

        public GameTime GameTime { get; set; }
        public Camera Camera { get; set; }

        public SpriteBatch SpriteBatch
        {
            get { return _spriteBatch; }
        }

    }
}