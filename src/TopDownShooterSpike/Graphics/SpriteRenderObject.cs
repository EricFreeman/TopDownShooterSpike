using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TopDownShooterSpike.Simulation;

namespace TopDownShooterSpike.Graphics
{
    public class MapRenderObject : RenderObject
    {
        private readonly ITileProvider _provider;
        private readonly Map _map;

        public MapRenderObject(Map map, ITileProvider provider)
        {
            _map = map;
            _provider = provider;
        }

        public override void Draw(RenderContext context)
        {
            var spriteBatch = context.SpriteBatch;
            var width = _map.Width;
            var height = _map.Height;
            var tileSize = context.SimulationSettings.TileDimensionAsWorldUnits;

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    var tileType = _map[x, y];
                    Texture2D texture;
                    _provider.GetTexture(ref tileType, out texture);

                    var position = Transform.Position;

                    position.X = tileSize*x + Transform.Position.X;
                    position.Y = tileSize*y + Transform.Position.Y;

                    var rectangle = new Rectangle((int)position.X, (int)position.Y, (int)tileSize, (int)tileSize);

                    spriteBatch.Draw(texture, rectangle, Color.White);
                }
            }
        }
    }

    public class SpriteRenderObject : RenderObject
    {
        private Texture2D _sprite;

        public SpriteRenderObject()
        {
            SourceRectangle = null;
            Color = Color.White;
        }

        public override void Draw(RenderContext context)
        {
            context.SpriteBatch.Draw(Sprite, 
                Transform.Position, 
                SourceRectangle, 
                Color, 
                Transform.Rotation, 
                Origin, 
                Transform.Scale, 
                SpriteEffects.None, 
                Transform.ZIndex);
        }

        public Texture2D Sprite
        {
            get { return _sprite; }
            set
            {
                _sprite = value;

                if(_sprite != null)
                    Origin = new Vector2(_sprite.Width / 2.0f, _sprite.Height / 2.0f);
            }
        }

        public Rectangle? SourceRectangle { get; set; }
        public Vector2 Origin { get; set; }
        public Color Color { get; set; }

        public override bool Visible
        {
            get { return base.Visible && _sprite != null; }
            set { base.Visible = value; }
        }
    }
}