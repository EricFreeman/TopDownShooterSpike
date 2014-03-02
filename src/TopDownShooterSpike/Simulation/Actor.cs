using System;
using Microsoft.Xna.Framework;
using TopDownShooterSpike.Managers;

namespace TopDownShooterSpike.Simulation
{
    public abstract class Actor : IDisposable, IComparable<Actor>
    {
        private readonly ActorManager _actorManager;
        private static int _staticId = int.MinValue;
        private readonly int _id = _staticId++;

        #region Fields

        private float _rotation;
        private Vector2 _position;

        #endregion

        protected Actor(ActorManager actorManager)
        {
            _actorManager = actorManager;
        }

        ~Actor() { OnDispose(false);}

        public void Dispose()
        {
            OnDispose(true);
            GC.SuppressFinalize(this);
        }

        #region Overrides

        protected virtual void OnDispose(bool disposing) { }

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

    }
}
