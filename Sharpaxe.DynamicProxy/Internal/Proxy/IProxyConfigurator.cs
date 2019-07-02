using System;
using System.Collections.Generic;
using System.Reflection;

namespace Sharpaxe.DynamicProxy.Internal.Proxy
{
#warning Should be an internal interface - change it after skip clr visibility check has been done
    public interface IProxyConfigurator
    {
        void SetEventProxy(EventInfo eventInfo, object proxy);
        void SetEventDecorators(EventInfo eventInfo, ICollection<ValueTuple<object, object>> decoratorsCollection);

        void SetMethodProxy(MethodInfo methodInfo, object proxy);
        void SetMethodDecorators(MethodInfo methodInfo, ICollection<ValueTuple<object, object>> decoratorsCollection);
        
        void SetPropertyGetterProxy(PropertyInfo propertyInfo, object proxy);
        void SetPropertyGetterDecorators(PropertyInfo propertyInfo, ICollection<ValueTuple<object, object>> decoratorsCollection);

        void SetPropertySetterProxy(PropertyInfo propertyInfo, object proxy);
        void SetPropertySetterDecorators(PropertyInfo propertyInfo, ICollection<ValueTuple<object, object>> decoratorsCollection);
    }
}
