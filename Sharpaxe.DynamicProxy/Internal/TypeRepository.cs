using Sharpaxe.DynamicProxy.Internal.Detector;
using Sharpaxe.DynamicProxy.Internal.Detector.Builder;
using Sharpaxe.DynamicProxy.Internal.Proxy;
using Sharpaxe.DynamicProxy.Internal.Proxy.Builder;
using Sharpaxe.DynamicProxy.Internal.Proxy.NameProvider;
using System;
using System.Collections.Concurrent;
using System.Reflection.Emit;

namespace Sharpaxe.DynamicProxy.Internal
{
    internal class TypeRepository : ITypeRepository
    {
        private readonly ModuleBuilder moduleBuilder;

        private readonly ConcurrentDictionary<Type, Type> typeToEventPropertyDetectorTypeMap;
        private readonly ConcurrentDictionary<Type, Type> typeToPropertyGetterDetectorTypeMap;
        private readonly ConcurrentDictionary<Type, Type> typeToPropertySetterDetectorTypeMap;
        private readonly ConcurrentDictionary<Type, IMethodDetector> typeToMethodDetectorInstanceMap;
        private readonly ConcurrentDictionary<Type, ValueTuple<Type, Type>> typeToProxyTypeAndConfiguratorTypeMap;


        public TypeRepository(ModuleBuilder moduleBuilder)
            : this()
        {
            this.moduleBuilder = moduleBuilder ?? throw new ArgumentNullException(nameof(moduleBuilder));
        }

        private TypeRepository()
        {
            typeToMethodDetectorInstanceMap = new ConcurrentDictionary<Type, IMethodDetector>();
            typeToEventPropertyDetectorTypeMap = new ConcurrentDictionary<Type, Type>();
            typeToPropertyGetterDetectorTypeMap = new ConcurrentDictionary<Type, Type>();
            typeToPropertySetterDetectorTypeMap = new ConcurrentDictionary<Type, Type>();
            typeToProxyTypeAndConfiguratorTypeMap = new ConcurrentDictionary<Type, ValueTuple<Type, Type>>();
        }

        public (object, IProxyConfigurator) CreateConfigurableProxy(Type type, object core)
        {
            (var proxyType, var configuratorType) = typeToProxyTypeAndConfiguratorTypeMap.GetOrAdd(type, t => CreateProxyTypeAndConfiguratorType(t));

            var proxyInstance = Activator.CreateInstance(proxyType, core);
            var configuratorInstance = Activator.CreateInstance(configuratorType, proxyInstance);

            return (proxyInstance, (IProxyConfigurator)configuratorInstance);
        }

        public (object, IEventDetector) CreateEventDetector(Type type)
        {
            var detectorType = typeToEventPropertyDetectorTypeMap.GetOrAdd(type, t => new EventDetectorBuilder(t, moduleBuilder).CreateDetectorType());
            var detectorInstance = Activator.CreateInstance(detectorType);
            return (detectorInstance, (IEventDetector)detectorInstance);
        }

        public (object, IMethodDetector) CreateMethodDetector(Type type)
        {
            var detectorInstance = typeToMethodDetectorInstanceMap.GetOrAdd(type, t => (IMethodDetector)Activator.CreateInstance(new MethodDetectorBuilder(type, moduleBuilder).CreateDetectorType()));
            return (detectorInstance, (IMethodDetector)detectorInstance);
        }

        public (object, IPropertyGetterDetector) CreatePropertyGetterDetector(Type type)
        {
            var detectorType = typeToPropertyGetterDetectorTypeMap.GetOrAdd(type, t => new PropertyGetterDetectorBuilder(t, moduleBuilder).CreateDetectorType());
            var detectorInstance = Activator.CreateInstance(detectorType);
            return (detectorInstance, (IPropertyGetterDetector)detectorInstance);
        }

        public (object, IPropertySetterDetector) CreatePropertySetterDetector(Type type)
        {

            var detectorType = typeToPropertySetterDetectorTypeMap.GetOrAdd(type, t => new PropertySetterDetectorBuilder(t, moduleBuilder).CreateDetectorType());
            var detectorInstance = Activator.CreateInstance(detectorType);
            return (detectorInstance, (IPropertySetterDetector)detectorInstance);
        }

        private (Type, Type) CreateProxyTypeAndConfiguratorType(Type type)
        {
            var memberNamesProvider = new MemberNamesProviderCacheProxy(new MemberNamesProviderCore());

            var proxyType = new ProxyBuilder(type, memberNamesProvider, moduleBuilder).CreateProxyType();
            var configuratorType = new ConfiguratorBuilder(type, proxyType, memberNamesProvider, moduleBuilder).CreateConfiguratorType();

            return (proxyType, configuratorType);
        }
    }
}
