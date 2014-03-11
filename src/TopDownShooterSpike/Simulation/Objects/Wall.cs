using System;
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
        private float _length;
        private const float WallWidth = 32;

        public Wall(IActorService service) : base(service)
        {
            InitializeFixture(InitializeCollision);
        }

        private void InitializeWall(Vector2 startPosition, Vector2 endPosition)
        {
            // TODO: might be smart to make the body's position the center of the wall
            // If you create a rectangle fixure, farseer automatically does this for you
            _mainCollision.Body.Position = startPosition; 

            var needed = Vector2.Distance(startPosition, endPosition) / WallWidth;
            var direction = Vector2.Normalize(endPosition - startPosition);

            for (int i = 0; i < needed; i++)
            {
                var renderobj = ActorService.CreateRenderObject<SpriteRenderObject>();
                renderobj.LoadTexture("gfx/wall");

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

        #region Collision

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

        #endregion

        public float Length
        {
            get { return _length; }
            set
            {
                if(value <= 0 || value >= 10)
                    throw new ArgumentOutOfRangeException();

                _length = MathHelper.Max(value, float.Epsilon);

                var position = Transform.Position;

                // rotate the start and end positions and then
                // extend them out 
                var startPosition = Vector2.Transform(position + new Vector2(-_length, 0), Transform.RotationMatrix());
                var endPosition = Vector2.Transform(position + new Vector2(_length, 0), Transform.RotationMatrix());

                InitializeWall(startPosition, endPosition);
            }
        }
    }
}