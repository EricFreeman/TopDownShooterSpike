using System;
using Microsoft.Xna.Framework;
using TopDownShooterSpike.Graphics;
using TopDownShooterSpike.Managers;

namespace TopDownShooterSpike.Simulation
{
    public abstract class Actor : IDisposable, IComparable<Actor>
    {
        private readonly IActorManager _actorManager;
        private readonly IServiceProvider _services;
        private static int _staticId = int.MinValue;
        private readonly int _id = _staticId++;

        #region Fields

        private float _rotation;
        private Vector2 _position;

        #endregion

        protected Actor(IActorManager actorManager, IServiceProvider services)
        {
            _actorManager = actorManager;
            _services = services;
            Enabled = true;
        }

        ~Actor() { OnDispose(false);}

        public void Dispose()
        {
            OnDispose(true);
            GC.SuppressFinalize(this);
        }

        public void Update(GameTime gameTime)
        {
            Tick(gameTime);
        }


        #region Overrides

        protected virtual void OnDispose(bool disposing) { }
        protected virtual void Tick(GameTime gameTime) { } 

        public override bool Equals(object obj)
        {
            //       
            // See the full list of guidelines at
            //   http://go.microsoft.com/fwlink/?LinkID=85237  
            // and also the guidance for operator== at
            //   http://go.microsoft.com/fwlink/?LinkId=85238
            //
            var otherActor = obj as Actor;

            if (obj == null || GetType() != obj.GetType())
                return false;

            return _id == otherActor._id;
        }

        public override int GetHashCode()
        {
            return _id + 41;
        }

        public int CompareTo(Actor other)
        {
            return _id.CompareTo(other._id);
        }

        #endregion

        public virtual float Rotation
        {
            get { return _rotation; }
            set { _rotation = value; }
        }

        public virtual Vector2 Position
        {
            get { return _position; }
            set { _position = value; }
        }

        public int Id
        {
            get { return _id; }
        }

        public bool Enabled { get; set; }

        public RenderObject RenderObject { get; set; }
    }
}
