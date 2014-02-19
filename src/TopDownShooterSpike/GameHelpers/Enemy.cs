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
            currentPoint = 0;
        }

        public void Update(GameTime gameTime)
        {
            if (State == EnemyState.Patrolling)
            {
                var dis = _image.Position - Waypoints[currentPoint];
                if (Math.Abs(dis.X) < _patrolSpeed && Math.Abs(dis.Y) < _patrolSpeed)
                    if (Waypoints.Count - 1 > currentPoint)
                        currentPoint++;
                    else
                        currentPoint = 0;

                var move = Waypoints[currentPoint] - _image.Position;
                move.Normalize();

                _image.Position += move * _patrolSpeed;
                _image.Rotation = (float)Math.Atan2(move.Y, move.X) + (float)Math.PI;
            }
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