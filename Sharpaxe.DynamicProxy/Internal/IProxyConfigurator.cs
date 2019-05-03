using System.Collections.Generic;
using System.Reflection;

namespace Sharpaxe.DynamicProxy.Internal
{
    internal interface IProxyConfigurator
    {
        void AddBeforePropertyGetterDecorator(PropertyInfo propertyInfo, ICollection<object> decoratorCollection);
        void AddAfterPropertyGetterDecorator(PropertyInfo propertyInfo, ICollection<object> decoratorCollection);
        void SetPropertyGetterProxy(PropertyInfo propertyInfo, object proxy);

        void AddBeforePropertySetterDecorator(PropertyInfo propertyInfo, ICollection<object> decoratorCollection);
        void AddAfterPropertySetterDecorator(PropertyInfo propertyInfo, ICollection<object> decoratorCollection);
        void SetPropertySetterProxy(PropertyInfo propertyInfo, object proxy);

        void AddBeforeEventDecorator(EventInfo eventInfo, ICollection<object> decoratorCollection);
        void AddAfterEventDecorator(EventInfo eventInfo, ICollection<object> decoratorCollection);
        void SetEventProxy(EventInfo eventInfo, object proxy);

        void AddBeforeMethodDecorator(MethodInfo methodInfo, ICollection<object> decoratorCollection);
        void AddAfterMethodDecorator(MethodInfo methodInfo, ICollection<object> decoratorCollection);
        void SetMethodProxy(MethodInfo methodInfo, object proxy);
    }
}
