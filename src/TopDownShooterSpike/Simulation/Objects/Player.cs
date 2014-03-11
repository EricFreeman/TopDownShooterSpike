using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using TopDownShooterSpike.Graphics;
using TopDownShooterSpike.Managers;

namespace TopDownShooterSpike.Simulation.Objects
{
    public class Player : Pawn
    {
        public readonly float movementAmount = Kilogram.FromPounds(900);
        private readonly Camera _camera;

        public Player(IActorService service) : base(service)
        {
            var player_body = service.CreateRenderObject<SpriteRenderObject>();
            player_body.LoadTexture("gfx/Player/hero_body");
            RenderObject.Add(player_body);
            player_body.Transform.ZIndex = 0.1f;

            var player_head = service.CreateRenderObject<SpriteRenderObject>();
            player_head.LoadTexture("gfx/Player/hero_head");
            RenderObject.Add(player_head);
            player_head.Transform.ZIndex = 0;

            _camera = service.RenderManager.ActiveCamera;

            MainBody.LinearDamping = 6;
            MainBody.Friction = 0.2f;
        }

        protected override void Tick(GameTime gameTime)
        {
            base.Tick(gameTime);

            var movementDelta = Vector2.Zero;
            var inputService = ActorService.InputService;

            if (inputService.KeyDown(Keys.W))
                movementDelta.Y += movementAmount;
            else if (inputService.KeyDown(Keys.S)) 
                movementDelta.Y -= movementAmount;

            if (inputService.KeyDown(Keys.A))
                movementDelta.X += movementAmount;
            else if (inputService.KeyDown(Keys.D)) 
                movementDelta.X -= movementAmount;

            if (inputService.KeyDown(Keys.Q))
                Transform.Rotation += 0.01f;
            else if (inputService.KeyDown(Keys.E))
                Transform.Rotation -= 0.01f;

            if (movementDelta != Vector2.Zero)
            {
                
                Move(movementDelta);
            }
            _camera.MoveTo(Transform.Position);
            OnEachRenderObject(x => x.Transform = Transform);
        }

        protected override void BeginDraw(GameTime gameTime)
        {
        }
    }
}