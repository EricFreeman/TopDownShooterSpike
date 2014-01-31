using System;
using System.Collections.Generic;
using TDS.Futures.Object.Components;

namespace TDS.Futures.Object
{
    /// <summary>
    /// An initializer for <see cref="Entity"/> objects
    /// </summary>
    public class EntityDefinition
    {
        private readonly Func<IEnumerable<IEntityComponent>> _componentFactory;

        /// <summary>
        /// Initializes a new instance of <see cref="EntityDefinition"/>
        /// </summary>
        /// <param name="componentFactory"></param>
        public EntityDefinition(Func<IEnumerable<IEntityComponent>> componentFactory)
        {
            if(componentFactory == null)
                throw new ArgumentNullException("componentFactory");

            _componentFactory = componentFactory;
        }

        public void PopulateEntity(Entity entity)
        {
            var componentList = entity.Components;
            componentList.Clear();
            componentList.AddRange(_componentFactory());
        }
    }
}