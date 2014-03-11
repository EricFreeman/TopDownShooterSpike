using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TopDownShooterSpike.Graphics;
using TopDownShooterSpike.Simulation;

namespace TopDownShooterSpike.Managers
{
    public class RenderManager : IRenderManager
    {
        private readonly GraphicsDevice _graphicsDevice;
        private IList<Actor> _actors;
        private readonly RenderContext _context;

        public RenderManager(GraphicsDevice graphicsDevice, PrimitiveBatch primitiveBatch, SpriteBatch spriteBatch)
        {
            _graphicsDevice = graphicsDevice;
            _context = new RenderContext(primitiveBatch, spriteBatch);
        }

        public void SetActiveCamera(Camera camera)
        {
            _context.Camera = camera;
        }

        public void Update(GameTime gameTime, IList<Actor> actors)
        {
            _actors = actors;
        }


        private void RenderObjectsForActor(IEnumerable<RenderObject> renderObject)
        {
            foreach (var obj in renderObject)
            {
                if (obj.Visible)
                {
                    // this is dumb and faggy
                    if (obj.Lit)
                    {
                        
                    }
                    else
                    {
                        
                    }

                    obj.Draw(_context);
                }
            }
        }

        public void Draw(GameTime gameTime)
        {
            _context.GameTime = gameTime;

            _graphicsDevice.Clear(Color.Black);

            _context.SpriteBatch.Begin(SpriteSortMode.Deferred,
                                       BlendState.AlphaBlend,
                                       SamplerState.LinearWrap,
                                       DepthStencilState.None,
                                       RasterizerState.CullCounterClockwise,
                                       null, 
                                       _context.Camera.Transform.Combine());

            var actorsToRender = _actors;

            for (int index = 0; index < actorsToRender.Count; index++)
            {
                var actor = actorsToRender[index];
                var renderObject = actor.RenderObject;

                if (actor.Enabled)
                    RenderObjectsForActor(renderObject);
            }

            _context.SpriteBatch.End();
        }

        public Camera ActiveCamera
        {
            get { return _context.Camera; }
        }
    }
}
