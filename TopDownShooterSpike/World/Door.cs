using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TopDownShooterSpike.Managers;

namespace TopDownShooterSpike.World
{
    public class Door
    {
        private readonly Texture2D _doorTexture = _manager.Load<Texture2D>("gfx/door");
        private readonly Texture2D _doorCapTexture = _manager.Load<Texture2D>("gfx/door cap");

        private static readonly ContentManager _manager = new ContentManager(ScreenManager.Instance.Content.ServiceProvider, "Content");

        public Image DoorImage;
        private Image _doorCap;

        public Door()
        {
            DoorImage = new Image
            {
                Texture = _doorTexture
            };
            _doorCap = new Image
            {
                Texture = _doorCapTexture
            };
        }

        public void SetupDoor(Vector2 position, Vector2 offset, float rotation, Vector2 capPosition)
        {
            DoorImage.Position = position;
            DoorImage.PubOffset = offset;
            DoorImage.Rotation = rotation;

            _doorCap.Position = capPosition;
        }

        public void Update(GameTime gameTime)
        {
            
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            DoorImage.Draw(spriteBatch);
            _doorCap.Draw(spriteBatch);
        }
    }
}
