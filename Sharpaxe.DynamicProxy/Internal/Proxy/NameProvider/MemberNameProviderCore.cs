using System;
using System.Collections.Generic;
using System.Reflection;

namespace Sharpaxe.DynamicProxy.Internal.Proxy.NameProvider
{
    internal class MemberNameProviderCore : IMemberNameProvider
    {
        private readonly HashSet<string> reservedNames;

        public MemberNameProviderCore()
        {
            reservedNames = new HashSet<string>();
        }

        public string GetEventDecoratorsFieldName(EventInfo eventInfo)
        {
            return GetUnreservedMemberName($"{eventInfo.Name}EventDecorators");
        }

        public string GetEventProxyFieldName(EventInfo eventInfo)
        {
            return GetUnreservedMemberName($"{eventInfo.Name}EventProxy");
        }

        public string GetEventSubscribersFieldName(EventInfo eventInfo)
        {
            return GetUnreservedMemberName($"{eventInfo.Name}EventSubscribers");
        }

        public string GetEventWrapperMethodName(EventInfo eventInfo)
        {
            return GetUnreservedMemberName($"{eventInfo.Name}EventWrapper");
        }

        public string GetMethodDecoratorsFieldName(MethodInfo methodInfo)
        {
            return GetUnreservedMemberName($"{methodInfo.Name}MethodDecorators");
        }

        public string GetMethodProxyFieldName(MethodInfo methodInfo)
        {
            return GetUnreservedMemberName($"{methodInfo.Name}MethodProxy");
        }

        public string GetMethodWrapperMethodName(MethodInfo methodInfo)
        {
            return GetUnreservedMemberName($"{methodInfo.Name}MethodWrapper");
        }

        public string GetPropertyGetterDecoratorsFieldName(PropertyInfo propertyInfo)
        {
            return GetUnreservedMemberName($"{propertyInfo.Name}GetterDecorators");
        }

        public string GetPropertyGetterProxyFieldName(PropertyInfo propertyInfo)
        {
            return GetUnreservedMemberName($"{propertyInfo.Name}GetterProxy");
        }

        public string GetPropertyGetterWrapperMethodName(PropertyInfo propertyInfo)
        {
            return GetUnreservedMemberName($"{propertyInfo.Name}GetterWrapper");
        }

        public string GetPropertySetterDecoratorsFieldName(PropertyInfo propertyInfo)
        {
            return GetUnreservedMemberName($"{propertyInfo.Name}SetterDecorators");
        }

        public string GetPropertySetterProxyFieldName(PropertyInfo propertyInfo)
        {
            return GetUnreservedMemberName($"{propertyInfo.Name}SetterProxy");
        }

        public string GetPropertySetterWrapperMethodName(PropertyInfo propertyInfo)
        {
            return GetUnreservedMemberName($"{propertyInfo.Name}SetterWrapper");
        }

        private string GetUnreservedMemberName(string memberNamePattern)
        {
            string memberName;
            int memberIndex = 0;
            do
            {
                memberName = $"{memberNamePattern}{memberIndex++}";
            }
            while (reservedNames.Contains(memberName));

            reservedNames.Add(memberName);
            return memberName;
        }
    }
}
