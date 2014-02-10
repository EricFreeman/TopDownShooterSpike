using System;
using System.Collections.Generic;
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

        private readonly Texture2D _wall = _manager.Load<Texture2D>("gfx/wall");
        private readonly Texture2D _wallCap = _manager.Load<Texture2D>("gfx/wall cap");

        #region Helper Methods

        public List<Tile> CloseTiles(Vector2 pos)
        {
            var x = (int)pos.X/64;
            var y = (int)pos.Y/64;

            var rtn = new List<Tile>();

            rtn.Add(_map[x, y]);
            
            if(x - 1 >= 0)
                rtn.Add(_map[x - 1, y]);

            if(x + 1 < _map.GetLength(0))
                rtn.Add(_map[x + 1, y]);

            if(y - 1 >= 0)
                rtn.Add(_map[x, y - 1]);

            if (x - 1 >= 0 && y - 1 >= 0)
                rtn.Add(_map[x - 1, y - 1]);

            if (x + 1 < _map.GetLength(0) && y - 1 >= 0)
                rtn.Add(_map[x + 1, y - 1]);

            if(y + 1 < _map.GetLength(1))
                rtn.Add(_map[x, y + 1]);

            if (x - 1 >= 0 && y + 1 < _map.GetLength(1))
                rtn.Add(_map[x - 1, y + 1]);

            if (x + 1 < _map.GetLength(0) && y + 1 < _map.GetLength(1))
                rtn.Add(_map[x + 1, y + 1]);

            return rtn;
        }

        #endregion

        #region Hooks

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
                        Walls = curr.SelectSingleNode("Walls") != null ? curr.SelectSingleNode("Walls").InnerText.Split(',') : new string[2],
                        CollisionBox = new List<Rectangle>()
                    };

                    //collision
                    if (curr.SelectSingleNode("Collision") != null)
                    {
                        _map[x, y].CollisionBox.Add(new Rectangle(x*64, y*64, 64, 64));
                    }
                    else
                    {
                        if (_map[x, y].Walls[0] == "1")
                            _map[x, y].CollisionBox.Add(new Rectangle(x*64, y*64, 16, 64));
                        if (_map[x, y].Walls[1] == "1")
                            _map[x, y].CollisionBox.Add(new Rectangle(x*64, y*64, 64, 16));
                    }

                    var item = curr.SelectSingleNode("Item");
                    if (item != null)
                    {
                        var image = new Image
                        {
                            Texture = _manager.Load<Texture2D>("gfx/Items/" + item.SelectSingleNode("Type").InnerText)
                        };
                        if (item.SelectSingleNode("Rotation") != null)
                            image.Rotation = int.Parse(item.SelectSingleNode("Rotation").InnerText)*
                                             (float) (Math.PI/180f);
                        if (item.SelectSingleNode("Position") != null)
                        {
                            var pos = item.SelectSingleNode("Position").InnerText.Split(',');
                            image.Position = new Vector2(int.Parse(pos[0]), int.Parse(pos[1]));
                        }

                        image.LoadContent();

                        _map[x, y].Item = image;
                    }
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
            #region Floors, Walls, and Items

            for (int x = 0; x < _map.GetLength(0); x++)
            {
                for (int y = 0; y < _map.GetLength(1); y++)
                {
                    //floor
                    spriteBatch.Draw(_map[x, y].Image, new Vector2(x * 64, y * 64), Color.White);

                    //item
                    if(_map[x, y].Item != null)
                        spriteBatch.Draw(_map[x, y].Item.Texture, new Vector2(x * 64, y * 64) + _map[x, y].Item.Position, null,
                            Color.White, _map[x,y].Item.Rotation, Vector2.Zero, 1f, SpriteEffects.None, 1f);

                    #region Walls

                    if (_map[x, y].Walls[0] == "1")
                    {
                        spriteBatch.Draw(_wall, new Vector2(x * 64, y * 64), Color.White);
                    }
                    if (_map[x, y].Walls[1] == "1")
                    {
                        spriteBatch.Draw(_wall, new Vector2(x * 64, y * 64 + 16), new Rectangle(0, 0, 16, 64), Color.White,
                            (float)(Math.PI * 1.5f), Vector2.Zero, 1f, SpriteEffects.None, 1f);
                    }

                    #endregion
                }
            }

            #endregion

            #region Wall Caps

            for (int x = 0; x < _map.GetLength(0); x++)
            {
                for (int y = 0; y < _map.GetLength(1); y++)
                {
                    if (_map[x, y].Walls[0] == "1")
                    {
                        if (_map[x, y - 1].Walls[0] != "1")
                            spriteBatch.Draw(_wallCap, new Vector2(x * 64, y * 64), Color.White);
                        if (_map[x, y + 1].Walls[0] != "1" && _map[x, y + 1].Walls[1] != "1" && _map[x - 1, y + 1].Walls[1] != "1")
                            spriteBatch.Draw(_wallCap, new Vector2(x * 64, (y + 1) * 64 - 16), Color.White);
                    }
                    if (_map[x, y].Walls[1] == "1")
                    {
                        if (_map[x - 1, y].Walls[1] != "1")
                            spriteBatch.Draw(_wallCap, new Vector2(x * 64, y * 64), Color.White);
                        if (_map[x + 1, y].Walls[1] != "1" && _map[x + 1, y].Walls[0] != "1" && _map[x + 1, y - 1].Walls[0] != "1")
                            spriteBatch.Draw(_wallCap, new Vector2((x + 1) * 64 - 16, y * 64), Color.White);

                        //literally only for the bottom right corner which has trouble "capping" because cap needs to be where there is no wall
                        else if (_map[x + 1, y].Walls[1] != "1" && _map[x + 1, y].Walls[0] != "1" && _map[x + 1, y - 1].Walls[0] == "1")
                            spriteBatch.Draw(_wallCap, new Vector2((x + 1) * 64, y * 64), Color.White);
                    }
                }
            }

            #endregion
        }

        #endregion
    }
}
