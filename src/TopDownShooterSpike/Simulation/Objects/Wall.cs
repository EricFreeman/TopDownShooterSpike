using System.Collections.Generic;
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
        private const float WallHeight = 32;

        public Wall(IActorManager actorManager, IActorService service) : base(actorManager, service)
        {
            InitializeFixture(InitializeCollision);
        }

        public Wall(IActorManager actorManager, IActorService service, ContentManager content, Vector2 position) : base(actorManager, service)
        {
            InitializeFixture(InitializeCollision);
            _mainCollision.Body.Position = position;
            var renderobj = new SpriteRenderObject();

            renderobj.Sprite = content.Load<Texture2D>("gfx/wall");
            renderobj.Transform = new Transform2D
            {
                Position = position,
                Rotation = 0f,
                Scale = new Vector2(WallWidth / renderobj.Sprite.Width, WallHeight / renderobj.Sprite.Height),
                ZIndex = 0f
            };
            RenderObject.Add(renderobj);
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