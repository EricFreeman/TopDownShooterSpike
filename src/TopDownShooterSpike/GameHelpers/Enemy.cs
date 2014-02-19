using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TopDownShooterSpike.Managers;

namespace TopDownShooterSpike.GameHelpers
{
    public class Enemy
    {
        private Image _image;

        public Enemy()
        {
            _image = new Image
            {
                Texture = ScreenManager.Instance.Content.Load<Texture2D>("gfx/Enemy"),
                Position = new Vector2(200, 400)
            };
        }

        public void Update(GameTime gameTime)
        {
            
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _image.Draw(spriteBatch);
        }
    }
}