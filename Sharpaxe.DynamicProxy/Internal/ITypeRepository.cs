using Sharpaxe.DynamicProxy.Internal.Detector;
using System;

namespace Sharpaxe.DynamicProxy.Internal
{
    internal interface ITypeRepository
    {
        (object, IEventDetector) CreateEventDetector(Type type);
        (object, IMethodDetector) CreateMethodDetector(Type type);
        (object, IPropertyGetterDetector) CreatePropertyGetterDetector(Type type);
        (object, IPropertySetterDetector) CreatePropertySetterDetector(Type type);

        (object, IProxyConfigurator) CreateConfigurableProxy(Type type, object core);
    }
}
