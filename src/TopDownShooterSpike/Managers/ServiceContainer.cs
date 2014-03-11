using System;
using Ninject;
using Ninject.Syntax;

namespace TopDownShooterSpike.Managers
{
    public class ServiceContainer : IServiceContainer
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly StandardKernel _kernel;

        public ServiceContainer(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _kernel = new StandardKernel();
            
            Inject<IServiceContainer, ServiceContainer>(this);
        }

        ~ServiceContainer() { Dispose(false); }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if(disposing)
                _kernel.Dispose();
        }

        public T Create<T>() where T : class
        {
            return _serviceProvider.GetService(typeof (T)) as T ?? _kernel.Get<T>();
        }

        public void Inject<TInterface, TImplementation>(Func<IServiceContainer, TImplementation> factoryFunc, ObjectScope scope = ObjectScope.Transient) where TImplementation : class, TInterface
        {
            var scopeBinding = _kernel.Bind<TInterface>().ToMethod(context => factoryFunc(this));
            SetBinding(scope, scopeBinding);
        }

        public void Inject<TInterface, TImplementation>(TImplementation implementation) where TImplementation : class, TInterface
        {
            _kernel.Bind<TInterface>().ToConstant(implementation);
        }

        public void Inject<TInterface, TImplementation>(ObjectScope scope = ObjectScope.Transient) where TImplementation : class, TInterface
        {
            var scopeBinding = _kernel.Bind<TInterface>().To<TImplementation>();
            SetBinding(scope, scopeBinding);
        }

        private void SetBinding<T>(ObjectScope scope, IBindingWhenInNamedWithOrOnSyntax<T> scopeBinding)
        {
            if (scope == ObjectScope.Transient)
                scopeBinding.InTransientScope();
            else
                scopeBinding.InSingletonScope();
        }
    }

    public enum ObjectScope
    {
        Transient,
        Singleton
    }
}
