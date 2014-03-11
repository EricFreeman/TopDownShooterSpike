﻿using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TopDownShooterSpike.Graphics;

namespace TopDownShooterSpike.Managers
{
    public interface IRenderManager
    {
        void SetActiveCamera(Camera camera);
        void Draw(GameTime gameTime);
        void Update(GameTime gameTime);
    }

    public class RenderManager : IRenderManager
    {
        private readonly IActorManager _actorManager;
        private readonly RenderContext _context;

        public RenderManager(IActorManager actorManager, PrimitiveBatch primitiveBatch, SpriteBatch spriteBatch, ContentManager content)
        {
            _actorManager = actorManager;
            _context = new RenderContext(primitiveBatch, spriteBatch);
        }

        public void SetActiveCamera(Camera camera)
        {
            _context.Camera = camera;
        }

        public void Update(GameTime gameTime)
        {
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

            _context.SpriteBatch.Begin(SpriteSortMode.Deferred,
                                       BlendState.AlphaBlend,
                                       SamplerState.LinearWrap,
                                       DepthStencilState.None,
                                       RasterizerState.CullCounterClockwise,
                                       null, 
                                       _context.Camera.Transform.Combine());

            var actorsToRender = _actorManager.Actors;

            for (int index = 0; index < actorsToRender.Count; index++)
            {
                var actor = actorsToRender[index];
                var renderObject = actor.RenderObject;

                if (actor.Enabled)
                    RenderObjectsForActor(renderObject);
            }

            _context.SpriteBatch.End();
        }
    }
}
