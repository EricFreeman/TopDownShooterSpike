using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TopDownShooterSpike.World;

namespace TopDownShooterSpike.Screens
{
    public class TempGame : GameScreen
    {
        public Image Image;

        [XmlIgnore]
        public Map Map;

        public override void LoadContent()
        {
            base.LoadContent();
            Image.LoadContent();

            Map = new Map();
            Map.LoadContent("TestLevel");
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
            Image.UnloadContent();
            Map.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            Image.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            Image.Draw(spriteBatch);
            Map.Draw(spriteBatch);
        }
    }
}
