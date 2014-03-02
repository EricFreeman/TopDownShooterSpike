using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TopDownShooterSpike.GameHelpers;

namespace TopDownShooterSpike.Graphics
{
    public class RenderContext
    {
        private readonly SpriteBatch _spriteBatch;
//        private PrimitiveBatch _primitiveBatch;

        private readonly ContentManager _content;

        public RenderContext(SpriteBatch spriteBatch, ContentManager content)
        {
            _spriteBatch = spriteBatch;
            _content = content;
        }


        public GameTime GameTime { get; set; }
        public Camera Camera { get; set; }

        public SpriteBatch SpriteBatch
        {
            get { return _spriteBatch; }
        }

    }
}