using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TopDownShooterSpike.World
{
    public class Tile
    {
        public Texture2D Image;
        public Image Item; // the optional item you can place in a tile

        public string[] Walls;
        public bool Collision;
        public Rectangle CollisionBox;
    }
}
