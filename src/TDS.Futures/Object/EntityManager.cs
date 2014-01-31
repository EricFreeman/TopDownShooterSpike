using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using TDS.Futures.Object.Systems;

namespace TDS.Futures.Object
{
    /// <summary>
    /// Manages entities and system processing for their components
    /// </summary>
    public class EntityManager : IDisposable
    {
        #region Fields

        private readonly IList<Entity> _entityList;
        private readonly IList<EntitySystem> _entitySystems;

        #endregion

        /// <summary>
        /// Initializes a new instance of <see cref="EntityManager"/>
        /// </summary>
        public EntityManager()
        {
            _entityList = new List<Entity>(512);
            _entitySystems = new List<EntitySystem>(16);
        }

        ~EntityManager()
        { Dispose(false); }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose() { Dispose(true); }
        
        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                foreach (var entitySystem in _entitySystems)
                    entitySystem.Dispose();

                GC.SuppressFinalize(this);
            }
        }

        /// <summary>
        /// Ticks all entities and systems
        /// </summary>
        /// <param name="gameTime"></param>
        public void Tick(GameTime gameTime)
        {
            for (int entitySystemIndex = 0; entitySystemIndex < _entitySystems.Count; entitySystemIndex++)
            {
                var entitySystem = _entitySystems[entitySystemIndex];
                if (entitySystem.Enabled)
                {
                    entitySystem.OnTick(gameTime);

                    for (int entityIndex = _entityList.Count - 1; entityIndex >= 0; entityIndex--)
                    {
                        var entity = _entityList[entityIndex];

                        if (entity.Enabled)
                            entitySystem.ProcessEntity(entity);
                    }
                    
                    entitySystem.OnEndTick();
                }
            }
        }

        /// <summary>
        /// Removes an <see cref="Entity"/> from this <see cref="EntityManager"/>
        /// </summary>
        /// <param name="e"></param>
        public void RemoveEntity(Entity e)
        {
            if(!_entityList.Remove(e))
                throw new ArgumentException("This entity was not added to the manager.");
        }

        /// <summary>
        /// Adds an <see cref="Entity"/> to the <see cref="EntityManager"/>
        /// </summary>
        /// <param name="e"></param>
        public void AddEntity(Entity e)
        {
            if(_entityList.Contains(e))
                throw new ArgumentException();

            _entityList.Add(e);
        }

        /// <summary>
        /// Adds an <see cref="EntitySystem"/> to the manager
        /// </summary>
        /// <param name="system"></param>
        /// <exception cref="ArgumentException"></exception>
        public void AddEntitySystem(EntitySystem system)
        {
            if(_entitySystems.Contains(system))
                throw new ArgumentException();

            _entitySystems.Add(system);
        }

        #region Properties

        /// <summary>
        /// Gets the number of <see cref="Entity"/> objects added to this <see cref="EntityManager"/>
        /// </summary>
        public int EnityCount
        {
            get { return _entityList.Count; }
        }

        /// <summary>
        /// Gets the number of systems added to this <see cref="EntityManager"/>
        /// </summary>
        public int SystemCount
        {
            get { return _entitySystems.Count; }
        }

        #endregion
    }
}
