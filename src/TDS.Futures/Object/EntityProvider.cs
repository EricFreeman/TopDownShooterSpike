using System;

namespace TDS.Futures.Object
{
    public class EntityProvider 
    {
        private readonly EntityManager _manager;
        private readonly Pool<Entity> _entityPool;

        public EntityProvider(EntityManager manager)
        {
            _manager = manager;
            _entityPool = new Pool<Entity>();
        }

        public Entity Create(Action<Entity> createAction = null)
        {
            var entity = TryRetrieve();

            _manager.AddEntity(entity);

            if (createAction != null)
                createAction(entity);

            return entity;
        }

        public Entity Create(EntityDefinition definition)
        {
            return Create(definition.PopulateEntity);
        }

        public void Destroy(Entity entity)
        {
            _manager.RemoveEntity(entity);
        }

        /// <summary>
        /// Destroys an entity and then caches the instance in a pool
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public void Cache(Entity entity)
        {
            Destroy(entity);
            entity.Reset();
            _entityPool.Put(entity);
        }

        private Entity TryRetrieve()
        {
            return _entityPool.Any() ? _entityPool.Get() : new Entity();
        }
    }
}
