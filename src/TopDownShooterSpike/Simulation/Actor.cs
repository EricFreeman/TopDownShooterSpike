using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using TopDownShooterSpike.Graphics;
using TopDownShooterSpike.Managers;

namespace TopDownShooterSpike.Simulation
{
    public abstract class Actor : IDisposable, IComparable<Actor>
    {
        private static int _staticId = int.MinValue;

        private readonly int _id = _staticId++;
        protected readonly IActorManager ActorManager;
        protected readonly IActorService ActorService;
        private readonly IList<RenderObject> _renderObject;

        #region Fields

        protected Transform2D _transform;

        #endregion

        protected Actor(IActorManager actorManager, IActorService actorService)
        {
            _renderObject = new List<RenderObject>(8);

            Transform = Transform2D.Zero;
            Enabled = true;

            ActorManager = actorManager;
            ActorService = actorService;
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

        public void Destroy(Actor instigator = null)
        {
            OnDestroy(instigator);
            ActorManager.DestroyActor(this);
        }

        #region Overrides

        protected virtual void OnDispose(bool disposing) { }
        protected virtual void Tick(GameTime gameTime) { } 
        protected virtual void OnDestroy(Actor instigator) { } 

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

        public int Id
        {
            get { return _id; }
        }

        public bool Enabled { get; set; }

        public IList<RenderObject> RenderObject
        {
            get { return _renderObject; }
        }

        public virtual Transform2D Transform
        {
            get { return _transform; }
            set { _transform = value; }
        }
    }
}
