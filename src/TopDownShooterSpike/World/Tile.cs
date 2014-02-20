using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TopDownShooterSpike.World
{
    public class Tile
    {
        public Texture2D Image;
        public Image Item; // the optional item you can place in a tile

        public string[] Walls;
        public List<Rectangle> CollisionBox; // areas you can't move through
        public List<Rectangle> VisionBox; // areas you can't see through

        public bool IsVisionAndBlockingCollision(Point p)
        {
            return CollisionBox.Any(x => x.Contains(p)) ||
                   VisionBox.Any(x => x.Contains(p));
        }

        public bool IsBlockingCollision(Point p)
        {
            return CollisionBox.Any(x => x.Contains(p));
        }

        public bool IsVisionCollision(Point p)
        {
            return VisionBox.Any(x => x.Contains(p));
        }
    }
}
