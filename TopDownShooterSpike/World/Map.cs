using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Xml;
using TopDownShooterSpike.Managers;

namespace TopDownShooterSpike.World
{
    public class Map
    {
        #region Properties

        public Tile[,] _map;
        private readonly List<Image> _wallCaps = new List<Image>();

        private List<Door> _doors = new List<Door>();

        private readonly Texture2D _wall = _manager.Load<Texture2D>("gfx/wall");
        private readonly Texture2D _wallCap = _manager.Load<Texture2D>("gfx/wall cap");

        private static readonly ContentManager _manager = new ContentManager(ScreenManager.Instance.Content.ServiceProvider, "Content");

        private const int TILE_SIZE = 64;
        private const int WALL_SIZE = 16;

        #endregion

        #region Helper Methods

        /// <summary>
        /// Return all tiles that are 1 or less tiles away
        /// TODO: Make it work with variable length input for vision and hearing checks
        /// </summary>
        /// <param name="pos">Return tiles close to this position</param>
        /// <returns></returns>
        public List<Tile> CloseTiles(Vector2 pos)
        {
            var x = (int)pos.X / TILE_SIZE;
            var y = (int)pos.Y / TILE_SIZE;

            var rtn = new List<Tile>();

            #region Rug

            rtn.Add(_map[x, y]);

            if (x - 1 >= 0)
                rtn.Add(_map[x - 1, y]);

            if (x + 1 < _map.GetLength(0))
                rtn.Add(_map[x + 1, y]);

            if (y - 1 >= 0)
                rtn.Add(_map[x, y - 1]);

            if (x - 1 >= 0 && y - 1 >= 0)
                rtn.Add(_map[x - 1, y - 1]);

            if (x + 1 < _map.GetLength(0) && y - 1 >= 0)
                rtn.Add(_map[x + 1, y - 1]);

            if (y + 1 < _map.GetLength(1))
                rtn.Add(_map[x, y + 1]);

            if (x - 1 >= 0 && y + 1 < _map.GetLength(1))
                rtn.Add(_map[x - 1, y + 1]);

            if (x + 1 < _map.GetLength(0) && y + 1 < _map.GetLength(1))
                rtn.Add(_map[x + 1, y + 1]);

            #endregion

            return rtn;
        }

        #endregion

        #region Hooks

        #region Load Content

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
                    var currentNode = rows[y].SelectNodes("Column")[x];
                    var current = new Tile
                    {
                        Image = _manager.Load<Texture2D>("gfx/Tiles/" + currentNode.SelectSingleNode("Tile").InnerText + ".png"),
                        Walls = currentNode.SelectSingleNode("Walls") != null ? currentNode.SelectSingleNode("Walls").InnerText.Split(',') : new string[2],
                        CollisionBox = new List<Rectangle>()
                    };

                    CreateCollision(currentNode, current, x, y);
                    current.Item = CreateItem(currentNode);
                    CreateDoor(currentNode, x, y);

                    _map[x, y] = current;
                }
            }

            FindWallCaps();
        }

        private void CreateDoor(XmlNode currentNode, int x, int y)
        {
            var door = currentNode.SelectSingleNode("Door");
            if (door != null)
            {
                var doorPos = door.InnerText.Split(',');

                if (doorPos[0] == "1")
                {
                    var d = new Door();
                    d.SetupDoor(
                        new Vector2(x * TILE_SIZE + (WALL_SIZE / 2), y * TILE_SIZE + (WALL_SIZE / 2)),  //  position
                        new Vector2(WALL_SIZE / 2, WALL_SIZE / 2),                                      //  offset
                        0f,                                                                             //  rotation
                        new Vector2(x * TILE_SIZE, y * TILE_SIZE));                                     //  cap position
                    _doors.Add(d);
                }
                if (doorPos[1] == "1")
                {
                    var d = new Door();
                    d.SetupDoor(
                        new Vector2(x * TILE_SIZE + (WALL_SIZE / 2), y * TILE_SIZE + (WALL_SIZE / 2)),  //  position
                        new Vector2(WALL_SIZE / 2, WALL_SIZE / 2),                                      //  offset
                        -90f * (float)Math.PI / 180f,                                                   //  rotation
                        new Vector2(x * TILE_SIZE, y * TILE_SIZE));                                     //  cap position
                    _doors.Add(d);
                }
            }
        }

        private static void CreateCollision(XmlNode currentNode, Tile current, int x, int y)
        {
            // if there is no innertext, collision is full cell
            if (currentNode.SelectSingleNode("Collision") != null &&
                currentNode.SelectSingleNode("Collision").InnerText == string.Empty)
            {
                //full tiles
                current.CollisionBox.Add(new Rectangle(x * TILE_SIZE, y * TILE_SIZE, TILE_SIZE, TILE_SIZE));
            }
            else
            {
                //partial tiles
                if (currentNode.SelectSingleNode("Collision") != null)
                {
                    var col = currentNode.SelectSingleNode("Collision").InnerText.Split(',');
                    current.CollisionBox.Add(new Rectangle((x * TILE_SIZE) + int.Parse(col[0]), (y * TILE_SIZE) + int.Parse(col[1]),
                        int.Parse(col[2]), int.Parse(col[3])));
                }

                //walls
                if (current.Walls[0] == "1")
                    current.CollisionBox.Add(new Rectangle(x * TILE_SIZE, y * TILE_SIZE, WALL_SIZE, TILE_SIZE));
                if (current.Walls[1] == "1")
                    current.CollisionBox.Add(new Rectangle(x * TILE_SIZE, y * TILE_SIZE, TILE_SIZE, WALL_SIZE));
            }
        }

        private Image CreateItem(XmlNode currentNode)
        {
            var item = currentNode.SelectSingleNode("Item");
            if (item != null)
            {
                var image = new Image
                {
                    Texture = _manager.Load<Texture2D>("gfx/Items/" + item.SelectSingleNode("Type").InnerText)
                };
                if (item.SelectSingleNode("Rotation") != null)
                    image.Rotation = int.Parse(item.SelectSingleNode("Rotation").InnerText) *
                                     (float)(Math.PI / 180f);
                if (item.SelectSingleNode("Position") != null)
                {
                    var pos = item.SelectSingleNode("Position").InnerText.Split(',');
                    image.Position = new Vector2(int.Parse(pos[0]), int.Parse(pos[1]));
                }

                image.LoadContent();
                return image;
            }

            return null;
        }

        private void FindWallCaps()
        {
            for (int x = 0; x < _map.GetLength(0); x++)
            {
                for (int y = 0; y < _map.GetLength(1); y++)
                {
                    if (_map[x, y].Walls[0] == "1")
                    {
                        if (_map[x, y - 1].Walls[0] != "1")
                            AddCap(new Vector2(x * TILE_SIZE, y * TILE_SIZE));
                        if (_map[x, y + 1].Walls[0] != "1" && _map[x, y + 1].Walls[1] != "1" && _map[x - 1, y + 1].Walls[1] != "1")
                            AddCap(new Vector2(x * TILE_SIZE, (y + 1) * TILE_SIZE - WALL_SIZE));
                        if (_map[x, y + 1].Walls[0] != "1" && _map[x, y + 1].Walls[1] == "1")
                            AddCap(new Vector2(x * TILE_SIZE, (y + 1) * TILE_SIZE));
                    }
                    if (_map[x, y].Walls[1] == "1")
                    {
                        if (_map[x - 1, y].Walls[1] != "1")
                            AddCap(new Vector2(x * TILE_SIZE, y * TILE_SIZE));
                        if (_map[x + 1, y].Walls[1] != "1" && _map[x + 1, y].Walls[0] != "1" && _map[x + 1, y - 1].Walls[0] != "1")
                            AddCap(new Vector2((x + 1) * TILE_SIZE - WALL_SIZE, y * TILE_SIZE));

                        //literally only for the bottom right corner which has trouble "capping" because cap needs to be where there is no wall
                        else if (_map[x + 1, y].Walls[1] != "1" && _map[x + 1, y].Walls[0] != "1" && _map[x + 1, y - 1].Walls[0] == "1")
                            AddCap(new Vector2((x + 1) * TILE_SIZE, y * TILE_SIZE));
                    }
                }
            }
        }

        private void AddCap(Vector2 pos)
        {
            //don't need to draw the caps twice
            if (_wallCaps.All(x => x.Position != pos))
                _wallCaps.Add(new Image()
                {
                    Texture = _wallCap,
                    Position = pos
                });
        }

        #endregion

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
                    spriteBatch.Draw(_map[x, y].Image, new Vector2(x * TILE_SIZE, y * TILE_SIZE), Color.White);

                    //item
                    if (_map[x, y].Item != null)
                        spriteBatch.Draw(_map[x, y].Item.Texture, new Vector2(x * TILE_SIZE, y * TILE_SIZE) + _map[x, y].Item.Position, null,
                            Color.White, _map[x, y].Item.Rotation, Vector2.Zero, 1f, SpriteEffects.None, 1f);

                    #region Walls

                    if (_map[x, y].Walls[0] == "1")
                    {
                        spriteBatch.Draw(_wall, new Vector2(x * TILE_SIZE, y * TILE_SIZE), Color.White);
                    }
                    if (_map[x, y].Walls[1] == "1")
                    {
                        spriteBatch.Draw(_wall, new Vector2(x * TILE_SIZE, y * TILE_SIZE + WALL_SIZE), new Rectangle(0, 0, WALL_SIZE, TILE_SIZE), Color.White,
                            (float)(Math.PI * 1.5f), Vector2.Zero, 1f, SpriteEffects.None, 1f);
                    }

                    #endregion

                    #region Collision Boxes

                    //                    foreach (var rec in _map[x, y].CollisionBox)
                    //                    {
                    //                        var rect = new Texture2D(ScreenManager.Instance.GraphicsDevice, rec.Width, rec.Height);
                    //
                    //                        var data = new Color[80 * 30];
                    //                        for (int i = 0; i < data.Length; ++i) data[i] = Color.Chocolate;
                    //                        rect.SetData(data);
                    //
                    //                        var coor = new Vector2(rec.X, rec.Y);
                    //                        spriteBatch.Draw(rect, coor, Color.White * .4f);
                    //                    }

                    #endregion
                }
            }

            #endregion

            #region Doors

            foreach (var door in _doors)
                door.Draw(spriteBatch);

            #endregion

            #region Wall Caps

            foreach (var cap in _wallCaps)
                cap.Draw(spriteBatch);


            #endregion
        }

        #endregion
    }
}