using System.Collections.Generic;
using System.Reflection;

namespace Sharpaxe.DynamicProxy.Internal.Proxy.NameProvider
{
    internal interface IMemberNamesProvider
    {
        string GetMethodProxyFieldName(MethodInfo methodInfo);
        string GetMethodWrapperMethodName(MethodInfo methodInfo);
        string GetMethodDecoratorsFieldName(MethodInfo methodInfo);

        string GetEventProxyFieldName(EventInfo eventInfo);
        string GetEventWrapperMethodName(EventInfo eventInfo);
        string GetEventDecoratorsFieldName(EventInfo eventInfo);
        string GetEventSubscribersFieldName(EventInfo eventInfo);

        string GetPropertySetterProxyFieldName(PropertyInfo propertyInfo);
        string GetPropertySetterWrapperMethodName(PropertyInfo propertyInfo);
        string GetPropertySetterDecoratorsFieldName(PropertyInfo propertyInfo);

        string GetPropertyGetterProxyFieldName(PropertyInfo propertyInfo);
        string GetPropertyGetterWrapperMethodName(PropertyInfo propertyInfo);
        string GetPropertyGetterDecoratorsFieldName(PropertyInfo propertyInfo);

        void ReserveNames(IEnumerable<string> namesToReserve);
    }
}
