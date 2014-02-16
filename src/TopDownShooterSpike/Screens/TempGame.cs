using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TopDownShooterSpike.GameHelpers;
using TopDownShooterSpike.World;

namespace TopDownShooterSpike.Screens
{
    public class TempGame : GameScreen
    {
        public Image Image;

        [XmlIgnore]
        public Map Map;

        [XmlIgnore] 
        public Player Player;

        public override void LoadContent()
        {
            base.LoadContent();
            Image.LoadContent();

            Map = new Map();
            Map.LoadContent("TestLevel");

            Player = new Player();
            Player.LoadContent();
            Player.Image.Position = new Vector2(200, 200);
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            Image.Update(gameTime);
            Map.Update(gameTime);
            Player.Update(gameTime, Map);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            Image.Draw(spriteBatch);
            Map.Draw(spriteBatch);
            Player.Draw(spriteBatch);
        }
    }
}
