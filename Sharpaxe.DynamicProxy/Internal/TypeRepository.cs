using Sharpaxe.DynamicProxy.Internal.Detector;
using System;
using System.Collections.Concurrent;
using System.Reflection.Emit;

namespace Sharpaxe.DynamicProxy.Internal
{
    internal class TypeRepository : ITypeRepository
    {
        private readonly ModuleBuilder moduleBuilder;

        private readonly Func<Type, ModuleBuilder, Type> proxyTypeCreator;
        private readonly Func<Type, ModuleBuilder, Type> configuratorTypeCreator;
        private readonly Func<Type, ModuleBuilder, Type> eventDetectorTypeCreator;
        private readonly Func<Type, ModuleBuilder, Type> methodDetectorTypeCreator;
        private readonly Func<Type, ModuleBuilder, Type> propertyGetterDetectorTypeCreator;
        private readonly Func<Type, ModuleBuilder, Type> propertySetterDetectorTypeCreator;


        private readonly ConcurrentDictionary<Type, Type> typeToProxyTypeMap;
        private readonly ConcurrentDictionary<Type, Type> typeToConfiguratorTypeMap;
        private readonly ConcurrentDictionary<Type, Type> typeToEventPropertyDetectorTypeMap;
        private readonly ConcurrentDictionary<Type, Type> typeToPropertyGetterDetectorTypeMap;
        private readonly ConcurrentDictionary<Type, Type> typeToPropertySetterDetectorTypeMap;
        private readonly ConcurrentDictionary<Type, IMethodDetector> typeToMethodDetectorInstanceMap;


        public TypeRepository(
            ModuleBuilder moduleBuilder,
            Func<Type, ModuleBuilder, Type> proxyTypeCreator,
            Func<Type, ModuleBuilder, Type> configuratorTypeCreator,
            Func<Type, ModuleBuilder, Type> eventDetectorTypeCreator,
            Func<Type, ModuleBuilder, Type> methodDetectorTypeCreator,
            Func<Type, ModuleBuilder, Type> propertyGetterDetectorTypeCreator,
            Func<Type, ModuleBuilder, Type> propertySetterDetectorTypeCreator)
            : this()
        {
            this.moduleBuilder = moduleBuilder ?? throw new ArgumentNullException(nameof(moduleBuilder));
            this.proxyTypeCreator = proxyTypeCreator ?? throw new ArgumentNullException(nameof(proxyTypeCreator));
            this.configuratorTypeCreator = configuratorTypeCreator ?? throw new ArgumentNullException(nameof(configuratorTypeCreator));
            this.eventDetectorTypeCreator = eventDetectorTypeCreator ?? throw new ArgumentNullException(nameof(eventDetectorTypeCreator));
            this.methodDetectorTypeCreator = methodDetectorTypeCreator ?? throw new ArgumentNullException(nameof(methodDetectorTypeCreator));
            this.propertyGetterDetectorTypeCreator = propertyGetterDetectorTypeCreator ?? throw new ArgumentNullException(nameof(propertyGetterDetectorTypeCreator));
            this.propertySetterDetectorTypeCreator = propertySetterDetectorTypeCreator ?? throw new ArgumentNullException(nameof(propertySetterDetectorTypeCreator));
        }

        private TypeRepository()
        {
            typeToProxyTypeMap = new ConcurrentDictionary<Type, Type>();
            typeToConfiguratorTypeMap = new ConcurrentDictionary<Type, Type>();
            typeToEventPropertyDetectorTypeMap = new ConcurrentDictionary<Type, Type>();
            typeToPropertyGetterDetectorTypeMap = new ConcurrentDictionary<Type, Type>();
            typeToPropertySetterDetectorTypeMap = new ConcurrentDictionary<Type, Type>();
            typeToMethodDetectorInstanceMap = new ConcurrentDictionary<Type, IMethodDetector>();
        }

        public (object, IProxyConfigurator) CreateConfigurableProxy(Type type, object core)
        {
            var proxyType = typeToProxyTypeMap.GetOrAdd(type, CreateProxyType);
            var proxyInstance = Activator.CreateInstance(proxyType, core);

            var configuratorType = typeToConfiguratorTypeMap.GetOrAdd(type, CreateConfiguratorType);
            var configuratorInstance = Activator.CreateInstance(configuratorType, proxyInstance);

            return (proxyInstance, (IProxyConfigurator)configuratorInstance);
        }

        public (object, IEventDetector) CreateEventDetector(Type type)
        {
            var detectorType = typeToEventPropertyDetectorTypeMap.GetOrAdd(type, CreateEventDetectorType);
            var detectorInstance = Activator.CreateInstance(detectorType);
            return (detectorInstance, (IEventDetector)type);
        }

        public (object, IMethodDetector) CreateMethodDetector(Type type)
        {
            var detectorInstance = typeToMethodDetectorInstanceMap.GetOrAdd(type, CreateMethodDetectorInstance);
            return (detectorInstance, (IMethodDetector)type);
        }

        public (object, IPropertyGetterDetector) CreatePropertyGetterDetector(Type type)
        {
            var detectorType = typeToPropertyGetterDetectorTypeMap.GetOrAdd(type, CreatePropertyGetterDetectorType);
            var detectorInstance = Activator.CreateInstance(detectorType);
            return (detectorInstance, (IPropertyGetterDetector)detectorInstance);
        }

        public (object, IPropertySetterDetector) CreatePropertySetterDetector(Type type)
        {

            var detectorType = typeToPropertySetterDetectorTypeMap.GetOrAdd(type, CreatePropertySetterDetectorType);
            var detectorInstance = Activator.CreateInstance(detectorType);
            return (detectorInstance, (IPropertySetterDetector)detectorInstance);
        }

        private Type CreateProxyType(Type type)
        {
            return proxyTypeCreator.Invoke(type, moduleBuilder);
        }

        private Type CreateConfiguratorType(Type type)
        {
            return configuratorTypeCreator.Invoke(type, moduleBuilder);
        }

        private Type CreateEventDetectorType(Type type)
        {
            return eventDetectorTypeCreator.Invoke(type, moduleBuilder);
        }
        
        private IMethodDetector CreateMethodDetectorInstance(Type type)
        {
            return (IMethodDetector)Activator.CreateInstance(methodDetectorTypeCreator.Invoke(type, moduleBuilder));
        }

        private Type CreatePropertyGetterDetectorType(Type type)
        {
            return propertyGetterDetectorTypeCreator.Invoke(type, moduleBuilder);
        }

        private Type CreatePropertySetterDetectorType(Type type)
        {
            return propertySetterDetectorTypeCreator.Invoke(type, moduleBuilder);
        }
    }
}
