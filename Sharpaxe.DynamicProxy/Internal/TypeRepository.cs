using System;
using System.Collections.Concurrent;
using System.Reflection.Emit;

namespace Sharpaxe.DynamicProxy.Internal
{
    internal class TypeRepository : ITypeRepository
    {
        private readonly ModuleBuilder moduleBuilder;
        private readonly Func<Type, ModuleBuilder, Type> proxyTypeFactory;
        private readonly Func<Type, ModuleBuilder, Type> detectorTypeFactory;
        private readonly Func<Type, ModuleBuilder, Type> configuratorTypeFactory;

        private readonly ConcurrentDictionary<Type, Type> typeToProxyTypeMap;
        private readonly ConcurrentDictionary<Type, Type> typeToDetectorTypeMap;
        private readonly ConcurrentDictionary<Type, Type> typeToConfiguratorTypeMap;

        public TypeRepository(
            ModuleBuilder moduleBuilder,
            Func<Type, ModuleBuilder, Type> proxyTypeFactory,
            Func<Type, ModuleBuilder, Type> detectorTypeFactory,
            Func<Type, ModuleBuilder, Type> configuratorTypeFactory)
            : this()
        {
            this.moduleBuilder = moduleBuilder ?? throw new ArgumentNullException(nameof(moduleBuilder));
            this.proxyTypeFactory = proxyTypeFactory ?? throw new ArgumentNullException(nameof(proxyTypeFactory));
            this.detectorTypeFactory = detectorTypeFactory ?? throw new ArgumentNullException(nameof(detectorTypeFactory));
            this.configuratorTypeFactory = configuratorTypeFactory ?? throw new ArgumentNullException(nameof(configuratorTypeFactory));
        }

        private TypeRepository()
        {
            typeToProxyTypeMap = new ConcurrentDictionary<Type, Type>();
            typeToDetectorTypeMap = new ConcurrentDictionary<Type, Type>();
            typeToConfiguratorTypeMap = new ConcurrentDictionary<Type, Type>();
        }

        public (object, IPropertyDetector) CreatePropertyDetector(Type type)
        {
            var detectorType = typeToDetectorTypeMap.GetOrAdd(type, CreateDetectorType);
            var detectorInstance = Activator.CreateInstance(detectorType);
            return (detectorInstance, (IPropertyDetector)type);
        }

        public (object, IEventDetector, Action<object, EventArgs>) CreateEventDetector(Type type)
        {
            var detectorType = typeToDetectorTypeMap.GetOrAdd(type, CreateDetectorType);
            var signalInstance = new Action<object, EventArgs>((o, a) => { });
            var detectorInstance = Activator.CreateInstance(detectorType, signalInstance);
            return (detectorInstance, (IEventDetector)type, signalInstance);
        }

        public (object, IMethodDetector) CreateMethodDetector(Type type)
        {
            var detectorType = typeToDetectorTypeMap.GetOrAdd(type, CreateDetectorType);
            var detectorInstance = Activator.CreateInstance(detectorType);
            return (detectorInstance, (IMethodDetector)type);
        }

        public (object, IProxyConfigurator) CreateConfigurableProxy(Type type, object core)
        {
            var proxyType = typeToProxyTypeMap.GetOrAdd(type, CreateProxyType);
            var proxyInstance = Activator.CreateInstance(proxyType);
            var configuratorType = typeToConfiguratorTypeMap.GetOrAdd(type, CreateConfiguratorType);
            var configuratorInstance = Activator.CreateInstance(configuratorType, proxyInstance);
            return (proxyInstance, (IProxyConfigurator)configuratorInstance);
        }

        private Type CreateProxyType(Type type)
        {
            return proxyTypeFactory.Invoke(type, moduleBuilder);
        }

        private Type CreateDetectorType(Type type)
        {
            return detectorTypeFactory.Invoke(type, moduleBuilder);
        }

        private Type CreateConfiguratorType(Type type)
        {
            return configuratorTypeFactory.Invoke(type, moduleBuilder);
        }
    }
}
