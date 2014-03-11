using System.Collections.Generic;
using Microsoft.Xna.Framework;
using TopDownShooterSpike.Graphics;
using TopDownShooterSpike.Simulation;

namespace TopDownShooterSpike.Managers
{
    public interface IRenderManager
    {
        void SetActiveCamera(Camera camera);
        void Draw(GameTime gameTime);
        void Update(GameTime gameTime, IList<Actor> actors);
        Camera ActiveCamera { get; }
    }
}