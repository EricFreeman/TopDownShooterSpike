using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TopDownShooterSpike.GameHelpers
{
    public class Player
    {
        public Image Image;
        public float Speed;

        public void LoadContent()
        {
            Image = new Image
            {
                Texture = ScreenManager.Instance.Content.Load<Texture2D>("gfx/Player"),
                PubOffset = new Vector2(16, 16)
            };

            Speed = 5;
        }

        public void Update(GameTime gameTime)
        {
            if (InputManager.Instance.KeyDown(Keys.W, Keys.Up))
                Image.Position.Y -= Speed;
            if (InputManager.Instance.KeyDown(Keys.S, Keys.Down))
                Image.Position.Y += Speed;
            if (InputManager.Instance.KeyDown(Keys.A, Keys.Left))
                Image.Position.X -= Speed;
            if (InputManager.Instance.KeyDown(Keys.D, Keys.Right))
                Image.Position.X += Speed;

            var v = ScreenManager.Instance.GraphicsDevice.Viewport;
            var direction = new Vector2(v.Width/2f, v.Height/2f) - InputManager.Instance.GetMousePostion();
            Image.Rotation = (float)(Math.Atan2(direction.Y, direction.X));

            ScreenManager.Instance.Camera.CenterOn(Image);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Image.Draw(spriteBatch);
        }
    }
}
