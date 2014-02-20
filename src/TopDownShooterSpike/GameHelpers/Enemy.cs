using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TopDownShooterSpike.Managers;
using TopDownShooterSpike.World;

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
        private float viewDistance = 400f;

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

        public void Update(GameTime gameTime, Player player, Map map)
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

                if (CanEnemySeePlayer(RadianToVector2(_image.Rotation), _image.Position, player.Image.Position, coneAngle, map))
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

        public bool CanEnemySeePlayer(Vector2 enemyLookAtDirection, Vector2 enemyPosition, Vector2 playerPosition, float cone, Map map)
        {
            // player close enough to enemy
            var inDistance = Vector2.Distance(enemyPosition, playerPosition) < viewDistance;
            if (!inDistance) return false;


            // player in enemy view cone
            Vector2 directionEnemyToPlayer = playerPosition - enemyPosition;
            directionEnemyToPlayer.Normalize();
            var inCone = (Vector2.Dot(directionEnemyToPlayer, enemyLookAtDirection) > (float)Math.Cos(MathHelper.ToRadians(cone / 2f)));
            if (!inCone) return false;


            // no walls between player and enemy
            var tilePos = GetPointsOnLine((int)enemyPosition.X, (int)enemyPosition.Y, (int)playerPosition.X, (int)playerPosition.Y);
            return CheckPointsForWalls(tilePos, map);
        }

        public bool CheckPointsForWalls(List<Point> points, Map map)
        {
            return false;
        }

        // Bresenham's Line Algorithm
        public List<Point> GetPointsOnLine(int x0, int y0, int x1, int y1)
        {
            bool steep = Math.Abs(y1 - y0) > Math.Abs(x1 - x0);
            if (steep)
            {
                int t;
                t = x0; // swap x0 and y0
                x0 = y0;
                y0 = t;
                t = x1; // swap x1 and y1
                x1 = y1;
                y1 = t;
            }
            if (x0 > x1)
            {
                int t;
                t = x0; // swap x0 and x1
                x0 = x1;
                x1 = t;
                t = y0; // swap y0 and y1
                y0 = y1;
                y1 = t;
            }
            int dx = x1 - x0;
            int dy = Math.Abs(y1 - y0);
            int error = dx / 2;
            int ystep = (y0 < y1) ? 1 : -1;
            int y = y0;

            var rtn = new List<Point>();
            for (int x = x0; x <= x1; x+=16)
            {
                rtn.Add(new Point((steep ? y : x), (steep ? x : y)));
                error = error - dy;
                if (error < 0)
                {
                    y += ystep;
                    error += dx;
                }
            }

            return rtn;
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