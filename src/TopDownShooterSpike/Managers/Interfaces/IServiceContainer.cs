using System;

namespace TopDownShooterSpike.Managers
{
    public interface IServiceContainer : IDisposable
    {
        T Create<T>() where T : class;
        void Inject<TInterface, TImplementation>(Func<IServiceContainer, TImplementation> factoryFunc, ObjectScope scope = ObjectScope.Transient) where TImplementation : class, TInterface;
        void Inject<TInterface, TImplementation>(TImplementation implementation) where TImplementation : class, TInterface;
        void Inject<TInterface, TImplementation>(ObjectScope scope = ObjectScope.Transient) where TImplementation : class, TInterface;
    }
}
