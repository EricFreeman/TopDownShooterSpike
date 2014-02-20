using System.Collections.Generic;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TopDownShooterSpike.GameHelpers;
using TopDownShooterSpike.Managers;
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

        [XmlIgnore] 
        public List<Enemy> Enemies; 

        public override void LoadContent()
        {
            base.LoadContent();
            Image.LoadContent();

            Map = new Map();
            Map.LoadContent("TestLevel");

            Player = new Player();
            Player.LoadContent();
            Player.Image.Position = new Vector2(200, 200);

            Enemies = new List<Enemy>();
            Enemies.Add(new Enemy());
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            Image.Update(gameTime);
            Map.Update(gameTime);
            foreach (var enemy in Enemies)
            {
                enemy.Update(gameTime, Player, Map);
            }
            Player.Update(gameTime, Map);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            Image.Draw(spriteBatch);
            Map.Draw(spriteBatch);
            foreach (var enemy in Enemies)
            {
                enemy.Draw(spriteBatch);
            }
            Player.Draw(spriteBatch);
        }
    }
}
