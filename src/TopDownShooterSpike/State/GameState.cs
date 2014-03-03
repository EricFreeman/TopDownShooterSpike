using System;
using Microsoft.Xna.Framework;

namespace TopDownShooterSpike.State
{
    public abstract class GameState : IDisposable
    {
        protected IServiceProvider _services;

        ~GameState() { OnDispose(false); }

        public void Dispose()
        {
            OnDispose(true);
            GC.SuppressFinalize(this);
        }

        #region Overrides
        
        protected virtual void OnDispose(bool disposing) { }

        public virtual void Initialize() { }
        public virtual void TearDown() { } 

        protected virtual void Entry() { }
        protected virtual void Exit() { }

        public abstract void Update(GameTime gameTime);

        public abstract void Draw(GameTime gameTime);

        #endregion
    }
}
