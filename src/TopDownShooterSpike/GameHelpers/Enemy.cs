using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TopDownShooterSpike.Managers;

namespace TopDownShooterSpike.GameHelpers
{
    public class Enemy
    {
        private Image _image;
        public EnemyState State;

        private List<Vector2> Waypoints = new List<Vector2>();      // for patroling
        private int currentPoint;

        private Vector2 lastKnownPosition;                          // when searching for player

        private float _patrolSpeed = .75f;
        private float _runSpeed = 1.25f;
        private float _woundedSpeed = .33f;

        private float coneAngle = 60f;

        public Enemy()
        {
            _image = new Image
            {
                Texture = ScreenManager.Instance.Content.Load<Texture2D>("gfx/Enemy"),
                Position = new Vector2(200, 400),
                PubOffset = new Vector2(16, 16)
            };

            State = EnemyState.Patrolling;

            Waypoints.Add(_image.Position);
            Waypoints.Add(new Vector2(100f, 400f));
            Waypoints.Add(new Vector2(100f, 350f));
            Waypoints.Add(new Vector2(200f, 400f));
            Waypoints.Add(new Vector2(200f, 350f));
            currentPoint = 0;
        }

        public void Update(GameTime gameTime, Player player)
        {
            if (State == EnemyState.Patrolling)
            {
                var dis = _image.Position - Waypoints[currentPoint];
                if (Math.Abs(dis.X) < _patrolSpeed && Math.Abs(dis.Y) < _patrolSpeed)
                    currentPoint = Waypoints.Count - 1 > currentPoint ? currentPoint + 1 : 0;

                var move = Waypoints[currentPoint] - _image.Position;
                move.Normalize();

                _image.Position += move * _patrolSpeed;
                _image.Rotation = RotateTowards(move, _image.Rotation, .1f);

                if (CanEnemySeePlayer(RadianToVector2(_image.Rotation), _image.Position, player.Image.Position, coneAngle))
                    State = EnemyState.Attack;
            }
            else if(State == EnemyState.Attack)
            {
                var move = player.Image.Position - _image.Position;
                move.Normalize();

                _image.Position += move * _patrolSpeed;
                _image.Rotation = RotateTowards(move, _image.Rotation, .1f);
            }
        }

        private Vector2 RadianToVector2(float myAngleInRadians)
        {
            return new Vector2((float) Math.Cos(myAngleInRadians), -(float) Math.Sin(myAngleInRadians));
        }

        private float RotateTowards(Vector2 move, float curr, float tween)
        {
            var shouldBe = (float)Math.Atan2(move.Y, move.X) + (float)Math.PI;
            if (Math.Abs(curr - shouldBe) < tween || Math.Abs(curr - shouldBe + Math.PI * 2) < tween || Math.Abs(curr - shouldBe - Math.PI * 2) < tween)
                curr = shouldBe;

            float rotationDifference = curr - shouldBe;

            //if difference is greater than 180 degrees, reverse rotating direction
            //by adding or subtracting 360 degrees
            if (Math.Abs(rotationDifference) > Math.PI)
                rotationDifference += rotationDifference > 0f ? (float)(-1 * Math.PI * 2) : (float)(Math.PI * 2);

            //based on difference, rotate enemy angle
            if (rotationDifference < 0)
                curr += tween;
            else if (rotationDifference > 0)
                curr -= tween;

            return curr;
        }

        public bool CanEnemySeePlayer(Vector2 enemyLookAtDirection, Vector2 enemyPosition, Vector2 playerPosition, float cone)
        {
            Vector2 directionEnemyToPlayer = playerPosition - enemyPosition;
            directionEnemyToPlayer.Normalize();
            return (Vector2.Dot(directionEnemyToPlayer, enemyLookAtDirection) > (float)Math.Cos(MathHelper.ToRadians(cone / 2f)));
        } 

        public void Draw(SpriteBatch spriteBatch)
        {
            _image.Draw(spriteBatch);
        }
    }

    public enum EnemyState
    {
        Idle,
        Patrolling,
        Alerted,
        Searching,
        Attack,
        Hide,
        RunAway
    }
}