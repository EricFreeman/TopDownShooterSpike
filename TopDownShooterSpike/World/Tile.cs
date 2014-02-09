using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TopDownShooterSpike.World
{
    public class Tile
    {
        public Texture2D Image;
        public string[] Walls;
        public Rectangle CollisionBox;
    }
}
