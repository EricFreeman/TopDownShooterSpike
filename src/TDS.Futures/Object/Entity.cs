using System;
using System.Collections.Generic;
using TDS.Futures.Object.Components;

namespace TDS.Futures.Object
{
    /// <summary>
    /// A game object that exists in a game world. 
    /// </summary>
    public class Entity
    {
        private static int _staticId = 0;

        private readonly int _id;
        private readonly EntityComponentList _componentList;

        /// <summary>
        /// Initializes a new instance of <see cref="Entity"/> with the specified ID
        /// </summary>
        /// <param name="id"></param>
        public Entity(int id)
        {
            _id = id;
            _componentList = new EntityComponentList(this);
        }

        /// <summary>
        /// Initializes a new instance of <see cref="Entity"/>
        /// </summary>
        public Entity() : this(_staticId++) { }

        /// <summary>
        /// Resets this entity 
        /// </summary>
        public void Reset()
        {
            Enabled = false;
            _componentList.Clear();
        }

        #region Overrides 

        public override int GetHashCode()
        {
            return (_id + 7) * _componentList.GetHashCode();
        }

        public override string ToString()
        {
            return String.Format("Entity:{0}", _id);
        }

        public override bool Equals(object obj)
        {
            return obj.GetHashCode() == GetHashCode() && obj is Entity;
        }

        #endregion

        /// <summary>
        /// Gets the components that describe the behavior of this <see cref="Entity"/>
        /// </summary>
        public EntityComponentList Components
        {
            get { return _componentList; }
        }

        /// <summary>
        /// Gets or sets if this <see cref="Entity"/> should be processed by the <see cref="EntityManager"/>
        /// </summary>
        public bool Enabled { get; set; }
    }

    /// <summary>
    /// Various extension methods for <see cref="Entity"/>, <see cref="IEntityComponent"/>, and <see cref="EntityComponentList"/>
    /// </summary>
    public static class Extensions
    {
        public static void AddRange(this EntityComponentList componentList, params IEntityComponent[] entityComponents)
        {
            foreach (var entityComponent in entityComponents)
                componentList.AddComponent(entityComponent);
        }

        public static void AddRange(this EntityComponentList componentList,  IEnumerable<IEntityComponent> entityComponents)
        {
            foreach (var entityComponent in entityComponents)
                componentList.AddComponent(entityComponent);
        }
    }
}
