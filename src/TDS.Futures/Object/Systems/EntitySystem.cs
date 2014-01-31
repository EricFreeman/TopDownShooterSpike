using System;
using Microsoft.Xna.Framework;
using TDS.Futures.Object.Components;

namespace TDS.Futures.Object.Systems
{
    /// <summary>
    /// Processes the <see cref="IEntityComponent"/> list in an <see cref="Entity"/>
    /// </summary>
    public abstract class EntitySystem : IDisposable
    {
        private static int _staticId = int.MaxValue;
        private readonly int _id = _staticId--;

        ~EntitySystem()
        {
            Disposing(false);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Disposing(true);
            GC.SuppressFinalize(this);
        }

        #region Overrides

        public override int GetHashCode()
        {
            return _id + 7;
        }

        public override bool Equals(object obj)
        {
            return obj is EntitySystem && obj.GetHashCode() == _id;
        }

        public override string ToString()
        { return string.Format("{0}", GetType().Name); }

        #endregion

        #region Virtuals

        protected abstract void Disposing(bool disposing);

        /// <summary>
        /// Processes an <see cref="Entity"/>
        /// </summary>
        /// <param name="entity"></param>
        public abstract void ProcessEntity(Entity entity);

        /// <summary>
        /// Ticks this <see cref="EntitySystem"/>
        /// </summary>
        /// <param name="gameTime"></param>
        public virtual void OnTick(GameTime gameTime) { }

        /// <summary>
        /// Notfies that this <see cref="EntitySystem"/> is finished processing entities this frame
        /// </summary>
        public virtual void OnEndTick() { }

        #endregion

        /// <summary>
        /// Gets or sets if this <see cref="EntitySystem"/> should process <see cref="Entity"/> components
        /// </summary>
        public bool Enabled 
        { get; set; }

    }
}