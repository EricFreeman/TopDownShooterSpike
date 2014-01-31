namespace TDS.Futures.Object.Components
{
    /// <summary>
    /// A component for an <see cref="Entity"/>
    /// </summary>
    public interface IEntityComponent
    {
        /// <summary>
        /// Initializes this component with default values
        /// </summary>
        void Initialize();

        /// <summary>
        /// Deep clones this component, returning a new instance with matching values
        /// </summary>
        /// <returns></returns>
        IEntityComponent Clone();

        /// <summary>
        /// Gets the unique identifier for this entity component
        /// </summary>
        int Id { get; }
    }

    /// <summary>
    /// A component for an <see cref="Entity"/>
    /// </summary>
    public abstract class EntityComponent : IEntityComponent
    {
        private static int _componentId = 0;
        private readonly int _id = _componentId++;

        public abstract void Initialize();
        public abstract IEntityComponent Clone();

        #region Overrides

        public override bool Equals(object obj)
        {
            var entityComponent = obj as IEntityComponent;
            return entityComponent != null && entityComponent.Id == _id;
        }

        public override int GetHashCode()
        {
            return _id + 7;
        }
        #endregion

        public int Id
        {
            get { return _id; }
        }
    }
}