using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TopDownShooterSpike.Graphics;
using TopDownShooterSpike.Managers;

namespace TopDownShooterSpike.Simulation.Objects
{
    public class Wall : KActor
    {
        private Fixture _mainCollision;
        private const float WallWidth = 32;

        public Wall(IActorManager actorManager, IActorService service) : base(actorManager, service)
        {
            InitializeFixture(InitializeCollision);
        }

        public Wall(IActorManager actorManager, IActorService service, ContentManager content, Vector2 startPosition, Vector2 endPosition) : base(actorManager, service)
        {
            InitializeFixture(InitializeCollision);
            _mainCollision.Body.Position = startPosition; //TODO: Make the collision correct

            var tex = content.Load<Texture2D>("gfx/wall");
            var needed = Vector2.Distance(startPosition, endPosition) / WallWidth;
            var direction = Vector2.Normalize(endPosition - startPosition);

            for (int i = 0; i < needed; i++)
            {
                var renderobj = new SpriteRenderObject();

                renderobj.Sprite = tex;
                renderobj.Transform = new Transform2D
                {
                    Position = startPosition + direction * (WallWidth * i),
                    Rotation = (float)Math.Atan2(startPosition.Y - endPosition.Y, startPosition.X - endPosition.X),
                    Scale = new Vector2(WallWidth/renderobj.Sprite.Width, WallWidth/renderobj.Sprite.Height),
                    ZIndex = 0f
                };
                RenderObject.Add(renderobj);
            }
        }

        private IEnumerable<Fixture> InitializeCollision(World arg1, Body arg2)
        {
            yield return (_mainCollision = FixtureFactory.AttachRectangle(Meter.FromInches(6), new Kilogram(1.5f), 1, Vector2.Zero, arg2, this));
        }

        protected override bool PreCollision(Fixture a, Fixture b)
        {
            return true;
        }

        protected override void PostCollision(Fixture a, Fixture b, Contact contact, ContactVelocityConstraint impulse)
        {

        }

        protected override bool Collision(Fixture a, Fixture b, Contact contact)
        {
            return true;
        }

        protected override void Separation(Fixture a, Fixture b)
        {
        }
    }
}