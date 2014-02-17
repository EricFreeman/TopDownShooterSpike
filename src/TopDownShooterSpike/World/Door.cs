using System;
using System.Runtime.InteropServices;
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

        public float Force = 0f; // force being put on door making it swing open
        private float minRot;
        private float maxRot;

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

        /// <summary>
        /// Push the door from position with given force
        /// </summary>
        /// <param name="from">Where force is coming from (bullet, player, etc)</param>
        /// <param name="force">How much force is beign exerted</param>
        /// <param name="spot">Spot on door where player touched it</param>
        public void Push(Image from, float force, Vector2 spot)
        {
            if (Math.Abs(Force) > 1f) return;

            var o1 = Vector2.Transform(spot, Matrix.CreateRotationX(DoorImage.Rotation));

            if (o1.X < DoorImage.Texture.Width/2)
                Force -= force;
            else
                Force += force;
        }

        public void SetupDoor(Vector2 position, Vector2 offset, float rotation, Vector2 capPosition)
        {
            DoorImage.Position = position;
            DoorImage.PubOffset = offset;
            DoorImage.Rotation = rotation;
            minRot = rotation - (float)(120 * Math.PI / 180);
            maxRot = rotation + (float)(120 * Math.PI / 180);

            _doorCap.Position = capPosition;
        }

        public void Update(GameTime gameTime)
        {
            if (Force != 0f)
                DoorImage.Rotation += Force;

            DoorImage.Rotation = MathHelper.Clamp(DoorImage.Rotation, minRot, maxRot);
            
            if(Force > 0)
                Force -= .01f;
            else if (Force < 0)
                Force += .01f;
            
            if (Force > -.01f && Force < .01f)
                Force = 0f;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            DoorImage.Draw(spriteBatch);
            _doorCap.Draw(spriteBatch);
        }
    }
}
