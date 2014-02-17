using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TopDownShooterSpike.Managers;
using TopDownShooterSpike.World;

namespace TopDownShooterSpike.GameHelpers
{
    public class Player
    {
        #region Properties

        public Image Image;
        public float Speed;
        public int CharacterWidth = 8;

        #endregion

        #region Hooks

        public void LoadContent()
        {
            Image = new Image
            {
                Texture = ScreenManager.Instance.Content.Load<Texture2D>("gfx/Player"),
                PubOffset = new Vector2(16, 16)
            };

            Speed = 3;
        }

        public void Update(GameTime gameTime, Map map)
        {
            var tiles = map.CloseTiles(Image.Position);

            if (InputManager.Instance.KeyDown(Keys.W, Keys.Up) &&
                !tiles.Any(x => x.CollisionBox.Any(y => y.Contains(new Point((int)Math.Round(Image.Position.X), (int)Math.Round(Image.Position.Y) - (int)Math.Round((Speed + CharacterWidth)))))))
                Image.Position.Y -= Speed;
            if (InputManager.Instance.KeyDown(Keys.S, Keys.Down) &&
                !tiles.Any(x => x.CollisionBox.Any(y => y.Contains(new Point((int)Math.Round(Image.Position.X), (int)Math.Round(Image.Position.Y) + (int)Math.Round((Speed + CharacterWidth)))))))
                Image.Position.Y += Speed;
            if (InputManager.Instance.KeyDown(Keys.A, Keys.Left) &&
                !tiles.Any(x => x.CollisionBox.Any(y => y.Contains(new Point((int)Math.Round(Image.Position.X) - (int)Math.Round((Speed + CharacterWidth)), (int)Math.Round(Image.Position.Y))))))
                Image.Position.X -= Speed;
            if (InputManager.Instance.KeyDown(Keys.D, Keys.Right) &&
                !tiles.Any(x => x.CollisionBox.Any(y => y.Contains(new Point((int)Math.Round(Image.Position.X) + (int)Math.Round((Speed + CharacterWidth)), (int)Math.Round(Image.Position.Y))))))
                Image.Position.X += Speed;

            CheckDoors(map);

            var v = ScreenManager.Instance.GraphicsDevice.Viewport;
            var direction = new Vector2(v.Width/2f, v.Height/2f) - InputManager.Instance.GetMousePostion();
            Image.Rotation = (float)(Math.Atan2(direction.Y, direction.X));

            ScreenManager.Instance.Camera.CenterOn(Image);
        }

        private void CheckDoors(Map map)
        {
            foreach (var door in map.Doors)
            {
                Vector2 collisionSpotA;
                Vector2 collisionSpotB;
                if (door.DoorImage.CollidesWith(Image, out collisionSpotA, out collisionSpotB))
                {
                    door.Push(Image, .05f, collisionSpotB);
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Image.Draw(spriteBatch);
        }

        #endregion
    }
}
