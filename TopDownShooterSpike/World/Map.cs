using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Xml;

namespace TopDownShooterSpike.World
{
    public class Map
    {
        public Tile[,] _map;
        private static readonly ContentManager _manager = new ContentManager(ScreenManager.Instance.Content.ServiceProvider, "Content");
        private Texture2D wall = _manager.Load<Texture2D>("gfx/wall");

        public void LoadContent(string name)
        {
            var doc = new XmlDocument();
            doc.Load("Content/Levels/" + name + ".xml");

            var level = doc.SelectSingleNode("/Level");
            var rows = level.SelectNodes("Row");
            var c = rows[0].SelectNodes("Column");

            _map = new Tile[c.Count, rows.Count];

            for (int y = 0; y < rows.Count; y++)
            {
                var columns = rows[y].SelectNodes("Column");
                for (int x = 0; x < columns.Count; x++)
                {
                    var curr = rows[y].SelectNodes("Column")[x];
                    _map[x, y] = new Tile
                    {
                        Image = _manager.Load<Texture2D>("gfx/Tiles/" + curr.SelectSingleNode("Tile").InnerText + ".png"),
                        Walls = curr.SelectSingleNode("Walls") != null ? curr.SelectSingleNode("Walls").InnerText.Split(',') : new string[4]
                    };
                }
            }
        }

        public void UnloadContent()
        {
            
        }

        public void Update(GameTime gameTime)
        {
            
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int x = 0; x < _map.GetLength(0); x ++)
            {
                for (int y = 0; y < _map.GetLength(1); y++)
                {
                    spriteBatch.Draw(_map[x, y].Image, new Vector2(x * 64, y * 64), Color.White);

                    if (_map[x, y].Walls[0] == "1")
                        spriteBatch.Draw(wall, new Vector2(x * 64, y * 64), Color.White);
                    if (_map[x, y].Walls[1] == "1")
                        spriteBatch.Draw(wall, new Vector2(x * 64, y * 64 + 16), new Rectangle(0, 0, 16, 64), Color.White, (float)(Math.PI * 1.5f), Vector2.Zero, 1f, SpriteEffects.None, 1f);
                    if (_map[x, y].Walls[2] == "1")
                        spriteBatch.Draw(wall, new Vector2(x * 64 + 64, y * 64), Color.White);
                    if (_map[x, y].Walls[3] == "1")
                        spriteBatch.Draw(wall, new Vector2(x * 64, y * 64 + 64), new Rectangle(0, 0, 16, 64), Color.White, (float)(Math.PI * 1.5f), Vector2.Zero, 1f, SpriteEffects.None, 1f);
                }
            }
        }
    }
}
