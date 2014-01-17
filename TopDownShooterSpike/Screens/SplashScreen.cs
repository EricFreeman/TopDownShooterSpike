using System.IO;
using System.Net.Mime;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TopDownShooterSpike.Screens
{
    public class SplashScreen : GameScreen
    {
        private Texture2D _image;
        public string Path;

        public override void LoadContent()
        {
            base.LoadContent();

            _image = Content.Load<Texture2D>(Path);
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_image, Vector2.Zero, Color.White);
            base.Draw(spriteBatch);
        }
    }
}
