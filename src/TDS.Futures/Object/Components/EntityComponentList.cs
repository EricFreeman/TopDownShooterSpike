using System;
using System.Collections.Generic;

namespace TDS.Futures.Object.Components
{
    /// <summary>
    /// Maintains an <see cref="Entity"/>'s components
    /// </summary>
    public sealed class EntityComponentList
    {
        private readonly Entity _owner;
        private readonly IDictionary<Type, IEntityComponent> _components;
        private int _componentHash;

        /// <summary>
        /// Initializes a new instance of <see cref="EntityComponentList"/>
        /// </summary>
        /// <param name="owner"></param>
        public EntityComponentList(Entity owner)
        {
            _owner = owner;
            _components = new Dictionary<Type, IEntityComponent>();
            Clear();
        }

        /// <summary>
        /// Gets a <see cref="IEntityComponent"/> from this <see cref="EntityComponentList"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public T QueryComponent<T>() where T : class, IEntityComponent
        {
            var type = typeof (T);
            if (!_components.ContainsKey(type))
                throw new ArgumentException("The specified component could not be found.");
                
            return _components[type] as T;
        }

        /// <summary>
        /// Adds a <see cref="IEntityComponent"/> to this component list
        /// </summary>
        /// <param name="component"></param>
        /// <typeparam name="T"></typeparam>
        /// <exception cref="ArgumentException"></exception>
        public void AddComponent<T>(T component) where T : class, IEntityComponent
        {
            var type = component.GetType();
            if (_components.ContainsKey(type))
                throw new ArgumentException("The specified component has already been added.");

            _components.Add(type, component);
            _componentHash = _components.GetHashCode() + 13;
        }

        /// <summary>
        /// Clears the components in this <see cref="EntityComponentList"/> 
        /// </summary>
        public void Clear()
        {
            _components.Clear();
            _componentHash = _components.GetHashCode() + 13;
        }

        public override int GetHashCode()
        {
           return 23 + _componentHash;
        }

        public int Count
        {
            get { return _components.Count; }
        }

        public IEntityComponent[] Components { get; set; }
    }
}