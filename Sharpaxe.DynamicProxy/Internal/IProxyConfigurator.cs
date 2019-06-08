using System;
using System.Collections.Generic;
using System.Reflection;

namespace Sharpaxe.DynamicProxy.Internal
{
    internal interface IProxyConfigurator
    {
        void AddPropertyGetterDecorators(PropertyInfo propertyInfo, ICollection<ValueTuple<object, object>> decoratorsCollection);
        void SetPropertyGetterProxy(PropertyInfo propertyInfo, object proxy);

        void AddPropertySetterDecorators(PropertyInfo propertyInfo, ICollection<ValueTuple<object, object>> decoratorsCollection);
        void SetPropertySetterProxy(PropertyInfo propertyInfo, object proxy);

        void AddEventDecorators(EventInfo eventInfo, ICollection<ValueTuple<object, object>> decoratorsCollection);
        void SetEventProxy(EventInfo eventInfo, object proxy);

        void AddMethodDecorators(MethodInfo methodInfo, ICollection<ValueTuple<object, object>> decoratorsCollection);
        void SetMethodProxy(MethodInfo methodInfo, object proxy);
    }
}
