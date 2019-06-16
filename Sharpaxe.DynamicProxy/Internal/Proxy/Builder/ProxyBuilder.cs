using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace Sharpaxe.DynamicProxy.Internal.Proxy
{
    internal class ProxyBuilder
    {
        private readonly Type targetType;
        private readonly ModuleBuilder moduleBuilder;
        private readonly HashSet<string> reservedInstanceFieldsNames;

        private TypeBuilder typeBuilder;

        private FieldInfo targetFieldInfo;
        private ReadOnlyDictionary<EventInfo, MemberInfo> eventsInfoToMemberInfoMap;
        private ReadOnlyDictionary<MethodInfo, MemberInfo> methodInfoToMemberInfoMap;
        private ReadOnlyDictionary<PropertyInfo, MemberInfo> propertyInfoToGetterMemberInfoMap;
        private ReadOnlyDictionary<PropertyInfo, MemberInfo> propertyInfoToSetterMemberInfoMap;

        public ProxyBuilder(Type targetType, ModuleBuilder moduleBuilder)
            : this()
        {
            this.targetType = targetType ?? throw new ArgumentNullException(nameof(targetType));
            this.moduleBuilder = moduleBuilder ?? throw new ArgumentNullException(nameof(moduleBuilder));
        }

        private ProxyBuilder()
        {
            reservedInstanceFieldsNames = new HashSet<string>();
        }

        public Type CreateProxyType()
        {
            InitializeInfo();
            InitializeTypeBuilder();

            DefineInstanceFields();
            DefineConstructor();

#warning Has not been finished
            var type = typeBuilder.CreateType();

            return type;
        }

        private void InitializeInfo()
        {
            (var eventsInfo, var methodsInfo, var propertiesInfo) = targetType.GetAllInterfaceMembers();

            var propertiesWithGetter = propertiesInfo.Where(p => p.GetGetMethod() != null && p.GetGetMethod().IsPublic);
            var propertiesWithSetter = propertiesInfo.Where(p => p.GetSetMethod() != null && p.GetSetMethod().IsPublic);

            eventsInfoToMemberInfoMap = eventsInfo.ToReadOnlyDictionary(ei => ei, ei => new MemberInfo());
            methodInfoToMemberInfoMap = methodsInfo.ToReadOnlyDictionary(mi => mi, mi => new MemberInfo());
            propertyInfoToGetterMemberInfoMap = propertiesWithGetter.ToReadOnlyDictionary(pi => pi, pi => new MemberInfo());
            propertyInfoToSetterMemberInfoMap = propertiesWithSetter.ToReadOnlyDictionary(pi => pi, pi => new MemberInfo());
        }

        private void InitializeTypeBuilder()
        {
            typeBuilder =
                moduleBuilder.DefineType(
                    GetTypeName(),
                    TypeAttributes.Class | TypeAttributes.Public,
                    typeof(object),
#warning Temporary
                    //new Type[] { targetType });
                    new Type[] { });
        }

        private string GetTypeName()
        {
            return $"{targetType.Name}``_DynamicProxy";
        }

        private void DefineInstanceFields()
        {
            DefineTargetTypeInstanceField();
            DefineMethodsInstanceFields();
            DefineEventsInstanceFields();
            DefinePropertiesGettersInstanceFields();
            DefinePropertiesSettersInstanceFields();
        }

        private void DefineConstructor()
        {
            var constructor = typeBuilder.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, new Type[] { targetType });
            var ILGenerator = constructor.GetILGenerator();

            // Call System.Object empty constructor
            ILGenerator.Emit(OpCodes.Ldarg_0);
            ILGenerator.Emit(OpCodes.Call, typeof(object).GetConstructor(new Type[0]));

            // Set target field
            ILGenerator.Emit(OpCodes.Ldarg_1);
            ILGenerator.Emit(OpCodes.Stfld, targetFieldInfo);

            // Return
            ILGenerator.Emit(OpCodes.Ret);
        }

        private void DefineTargetTypeInstanceField()
        {
            targetFieldInfo = typeBuilder.DefineField("target", targetType, FieldAttributes.Private);
        }

        private void DefineMethodsInstanceFields()
        {
            foreach (var kvp in methodInfoToMemberInfoMap)
            {
                var methodInfo = kvp.Key;
                var memberInfo = kvp.Value;

                memberInfo.ProxyInstanceFieldInfo =
                    typeBuilder.DefineField(GetMethodProxyFieldName(methodInfo), GetMethodProxyType(methodInfo), FieldAttributes.Private);

                memberInfo.DecoratorsLinkedListInstanceFieldInfo =
                    typeBuilder.DefineField(GetMethodDecoratorsFieldName(methodInfo), GetMethodDecoratorsType(methodInfo), FieldAttributes.Private);
            }
        }

        private void DefineEventsInstanceFields()
        {
            foreach (var kvp in eventsInfoToMemberInfoMap)
            {
                var eventInfo = kvp.Key;
                var memberInfo = kvp.Value;

                memberInfo.ProxyInstanceFieldInfo =
                    typeBuilder.DefineField(GetEventProxyFieldName(eventInfo), GetEventProxyFieldType(eventInfo), FieldAttributes.Private);

                memberInfo.DecoratorsLinkedListInstanceFieldInfo =
                    typeBuilder.DefineField(GetEventDecoratorsFieldName(eventInfo), GetEventDecoratorsFieldType(eventInfo), FieldAttributes.Private);
            }
        }

        private void DefinePropertiesGettersInstanceFields()
        {
            foreach (var kvp in propertyInfoToGetterMemberInfoMap)
            {
                var propertyInfo = kvp.Key;
                var memberInfo = kvp.Value;

                memberInfo.ProxyInstanceFieldInfo =
                    typeBuilder.DefineField(GetPropertyGetterProxyFieldName(propertyInfo), GetPropertyGetterProxyFieldType(propertyInfo), FieldAttributes.Private);

                memberInfo.DecoratorsLinkedListInstanceFieldInfo =
                    typeBuilder.DefineField(GetPropertyGetterDecoratorsFieldName(propertyInfo), GetPropertyGetterDecoratorsFieldType(propertyInfo), FieldAttributes.Private);
            }
        }

        private void DefinePropertiesSettersInstanceFields()
        {
            foreach (var kvp in propertyInfoToSetterMemberInfoMap)
            {
                var propertyInfo = kvp.Key;
                var memberInfo = kvp.Value;

                memberInfo.ProxyInstanceFieldInfo =
                    typeBuilder.DefineField(GetPropertySetterProxyFieldName(propertyInfo), GetPropertySetterProxyFieldType(propertyInfo), FieldAttributes.Private);

                memberInfo.DecoratorsLinkedListInstanceFieldInfo =
                    typeBuilder.DefineField(GetPropertySetterDecoratorsFieldName(propertyInfo), GetPropertySetterDecoratorsFieldType(propertyInfo), FieldAttributes.Private);
            }
        }

        private string GetMethodProxyFieldName(MethodInfo methodInfo)
        {
            return GetUnreservedFieldName($"{methodInfo.Name}MethodProxy");
        }

        private string GetMethodDecoratorsFieldName(MethodInfo methodInfo)
        {
            return GetUnreservedFieldName($"{methodInfo.Name}MethodDecorators");
        }

        private string GetUnreservedFieldName(string fieldNamePattern)
        {
            string fieldName;
            int fieldIndex = 0;
            do
            {
                fieldName = $"{fieldNamePattern}{fieldIndex++}";
            }
            while (reservedInstanceFieldsNames.Contains(fieldName));

            reservedInstanceFieldsNames.Add(fieldName);
            return fieldName;
        }

        private static string GetEventProxyFieldName(EventInfo eventInfo)
        {
            return $"{eventInfo.Name}EventProxy";
        }

        private static string GetEventDecoratorsFieldName(EventInfo eventInfo)
        {
            return $"{eventInfo.Name}EventDecorators";
        }

        private static string GetPropertyGetterProxyFieldName(PropertyInfo propertyInfo)
        {
            return $"{propertyInfo.Name}GetterProxy";
        }

        private static string GetPropertyGetterDecoratorsFieldName(PropertyInfo propertyInfo)
        {
            return $"{propertyInfo.Name}GetterDecorators";
        }

        private static string GetPropertySetterProxyFieldName(PropertyInfo propertyInfo)
        {
            return $"{propertyInfo.Name}SetterProxy";
        }

        private static string GetPropertySetterDecoratorsFieldName(PropertyInfo propertyInfo)
        {
            return $"{propertyInfo.Name}SetterDecorators";
        }

        private static Type GetEventProxyFieldType(EventInfo eventInfo)
        {
            return GetMethodProxyType(eventInfo.EventHandlerType.GetMethod("Invoke"));
        }

        private static Type GetEventDecoratorsFieldType(EventInfo eventInfo)
        {
            return GetMethodDecoratorsType(eventInfo.EventHandlerType.GetMethod("Invoke"));
        }

        private static Type GetPropertyGetterProxyFieldType(PropertyInfo propertyInfo)
        {
            return GetMethodProxyType(propertyInfo.GetGetMethod());
        }

        private static Type GetPropertyGetterDecoratorsFieldType(PropertyInfo propertyInfo)
        {
            return GetMethodDecoratorsType(propertyInfo.GetGetMethod());
        }

        private static Type GetPropertySetterProxyFieldType(PropertyInfo propertyInfo)
        {
            return GetMethodProxyType(propertyInfo.GetSetMethod());
        }

        private static Type GetPropertySetterDecoratorsFieldType(PropertyInfo propertyInfo)
        {
            return GetMethodDecoratorsType(propertyInfo.GetSetMethod());
        }

        private static Type GetMethodProxyType(MethodInfo methodInfo)
        {
            var methodDelegateType = methodInfo.MakeGenericDelegateType();
            var methodArguments = methodInfo.GetParameters().Select(p => p.ParameterType);
            
            switch (methodInfo.ReturnType)
            {
                case Type voidReturnType when voidReturnType == typeof(void):
                    return methodDelegateType.Concat(methodArguments).MakeGenericDelegateAction();

                case Type nonVoidReturnType:
                    return methodDelegateType.Concat(methodArguments).Concat(nonVoidReturnType).MakeGenericDelegateFunction();

                case null:
                    throw new ArgumentException("Method info return type is null", $"{nameof(methodInfo)}.{nameof(methodInfo.ReturnType)}");
            }
        }

        private static Type GetMethodDecoratorsType(MethodInfo methodInfo)
        {
            var methodArgumentsType = methodInfo.GetParameters().Select(p => p.ParameterType);

            Type beforeDecoratorType = methodArgumentsType.MakeGenericDelegateAction();

            Type afterDecoratorType;
            switch (methodInfo.ReturnType)
            {
                case Type voidReturnType when voidReturnType == typeof(void):
                    afterDecoratorType = methodArgumentsType.MakeGenericDelegateAction();
                    break;

                case Type nonVoidReturnType:
                    afterDecoratorType = methodArgumentsType.Concat(nonVoidReturnType).MakeGenericDelegateAction();
                    break;

                case null:
                    throw new ArgumentNullException("Method info return type is null", $"{nameof(methodInfo)}.{nameof(methodInfo.ReturnType)}");
            }

            return 
                typeof(LinkedList<>).MakeGenericType(
                    typeof(ValueTuple<,>).MakeGenericType(
                        beforeDecoratorType.ConcatInstances(afterDecoratorType).ToArray()));
        }

        private class MemberInfo
        {
            public FieldInfo ProxyInstanceFieldInfo { get; set; }
            public FieldInfo DecoratorsLinkedListInstanceFieldInfo { get; set; }
        }
    }
}
