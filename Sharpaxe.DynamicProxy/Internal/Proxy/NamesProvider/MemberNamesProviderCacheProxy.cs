using System;
using System.Collections.Generic;
using System.Reflection;

namespace Sharpaxe.DynamicProxy.Internal.Proxy.NameProvider
{
    internal class MemberNamesProviderCacheProxy : IMemberNamesProvider
    {
        private readonly IMemberNamesProvider target;

        private readonly Dictionary<EventInfo, string> eventProxyFieldNameCacheMap;
        private readonly Dictionary<EventInfo, string> eventWrapperMethodNameCacheMap;
        private readonly Dictionary<EventInfo, string> eventDecoratorsFieldNameCacheMap;
        private readonly Dictionary<EventInfo, string> eventSubscribersFieldNameCacheMap;

        private readonly Dictionary<MethodInfo, string> methodProxyFieldNameCacheMap;
        private readonly Dictionary<MethodInfo, string> methodWrapperMethodNameCacheMap;
        private readonly Dictionary<MethodInfo, string> methodDecoratorsFieldNameCacheMap;

        private readonly Dictionary<PropertyInfo, string> propertyGetterProxyFieldNameCacheMap;
        private readonly Dictionary<PropertyInfo, string> propertyGetterWrapperMethodNameCacheMap;
        private readonly Dictionary<PropertyInfo, string> propertyGetterDecoratorsFieldNameCacheMap;

        private readonly Dictionary<PropertyInfo, string> propertySetterProxyFieldNameCacheMap;
        private readonly Dictionary<PropertyInfo, string> propertySetterWrapperMethodNameCacheMap;
        private readonly Dictionary<PropertyInfo, string> propertySetterDecoratorsFieldNameCacheMap;

        public MemberNamesProviderCacheProxy(IMemberNamesProvider target)
            : this()
        {
            this.target = target ?? throw new ArgumentNullException(nameof(target));
        }

        private MemberNamesProviderCacheProxy()
        {
            eventProxyFieldNameCacheMap = new Dictionary<EventInfo, string>();
            eventWrapperMethodNameCacheMap = new Dictionary<EventInfo, string>();
            eventDecoratorsFieldNameCacheMap = new Dictionary<EventInfo, string>();
            eventSubscribersFieldNameCacheMap = new Dictionary<EventInfo, string>();

            methodProxyFieldNameCacheMap = new Dictionary<MethodInfo, string>();
            methodWrapperMethodNameCacheMap = new Dictionary<MethodInfo, string>();
            methodDecoratorsFieldNameCacheMap = new Dictionary<MethodInfo, string>();

            propertyGetterProxyFieldNameCacheMap = new Dictionary<PropertyInfo, string>();
            propertyGetterWrapperMethodNameCacheMap = new Dictionary<PropertyInfo, string>();
            propertyGetterDecoratorsFieldNameCacheMap = new Dictionary<PropertyInfo, string>();

            propertySetterProxyFieldNameCacheMap = new Dictionary<PropertyInfo, string>();
            propertySetterWrapperMethodNameCacheMap = new Dictionary<PropertyInfo, string>();
            propertySetterDecoratorsFieldNameCacheMap = new Dictionary<PropertyInfo, string>();
        }

        public string GetEventDecoratorsFieldName(EventInfo eventInfo)
        {
            return eventDecoratorsFieldNameCacheMap.GetOrAdd(eventInfo, target.GetEventDecoratorsFieldName);
        }

        public string GetEventProxyFieldName(EventInfo eventInfo)
        {
            return eventProxyFieldNameCacheMap.GetOrAdd(eventInfo, target.GetEventProxyFieldName);
        }

        public string GetEventSubscribersFieldName(EventInfo eventInfo)
        {
            return eventSubscribersFieldNameCacheMap.GetOrAdd(eventInfo, target.GetEventSubscribersFieldName);
        }

        public string GetEventWrapperMethodName(EventInfo eventInfo)
        {
            return eventWrapperMethodNameCacheMap.GetOrAdd(eventInfo, target.GetEventWrapperMethodName);
        }

        public string GetMethodDecoratorsFieldName(MethodInfo methodInfo)
        {
            return methodDecoratorsFieldNameCacheMap.GetOrAdd(methodInfo, target.GetMethodDecoratorsFieldName);
        }

        public string GetMethodProxyFieldName(MethodInfo methodInfo)
        {
            return methodProxyFieldNameCacheMap.GetOrAdd(methodInfo, target.GetMethodProxyFieldName);
        }

        public string GetMethodWrapperMethodName(MethodInfo methodInfo)
        {
            return methodWrapperMethodNameCacheMap.GetOrAdd(methodInfo, target.GetMethodWrapperMethodName);
        }

        public string GetPropertyGetterDecoratorsFieldName(PropertyInfo propertyInfo)
        {
            return propertyGetterDecoratorsFieldNameCacheMap.GetOrAdd(propertyInfo, target.GetPropertyGetterDecoratorsFieldName);
        }

        public string GetPropertyGetterProxyFieldName(PropertyInfo propertyInfo)
        {
            return propertyGetterProxyFieldNameCacheMap.GetOrAdd(propertyInfo, target.GetPropertyGetterProxyFieldName);
        }

        public string GetPropertyGetterWrapperMethodName(PropertyInfo propertyInfo)
        {
            return propertyGetterWrapperMethodNameCacheMap.GetOrAdd(propertyInfo, target.GetPropertyGetterWrapperMethodName);
        }

        public string GetPropertySetterDecoratorsFieldName(PropertyInfo propertyInfo)
        {
            return propertySetterDecoratorsFieldNameCacheMap.GetOrAdd(propertyInfo, target.GetPropertySetterDecoratorsFieldName);
        }

        public string GetPropertySetterProxyFieldName(PropertyInfo propertyInfo)
        {
            return propertySetterProxyFieldNameCacheMap.GetOrAdd(propertyInfo, target.GetPropertySetterProxyFieldName);
        }

        public string GetPropertySetterWrapperMethodName(PropertyInfo propertyInfo)
        {
            return propertySetterWrapperMethodNameCacheMap.GetOrAdd(propertyInfo, target.GetPropertySetterWrapperMethodName);
        }

        public void ReserveNames(IEnumerable<string> namesToReserve)
        {
            target.ReserveNames(namesToReserve);
        }
    }
}
