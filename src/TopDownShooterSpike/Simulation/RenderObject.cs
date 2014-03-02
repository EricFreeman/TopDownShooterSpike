using Microsoft.Xna.Framework.Graphics;
using TopDownShooterSpike.GameHelpers;

namespace TopDownShooterSpike.Simulation
{
    public abstract class RenderObject
    {
        public abstract void Draw(RenderContext context);
    }

    public class RenderContext
    {
        public SpriteBatch SpriteBatch { get; }
        public Camera Camera { get; }
    }
}
