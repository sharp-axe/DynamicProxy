using System;

namespace Sharpaxe.DynamicProxy.Internal
{
    internal interface ITypeRepository
    {
        (object, IMethodSelector) CreateMethodSelector(Type type);
        (object, IPropertyDetector) CreatePropertyDetector(Type type);
        (object, IEventDetector, Action<object, EventArgs>) CreateEventDetector(Type type);

        (object, IProxyConfigurator) CreateConfigurableProxy(Type type, object core);
    }
}
