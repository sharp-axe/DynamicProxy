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
        private readonly HashSet<string> reservedMembersNames;

        private TypeBuilder typeBuilder;

        private FieldInfo targetFieldInfo;
        private ReadOnlyDictionary<MethodInfo, MemberInfo> methodInfoToMemberInfoMap;
        private ReadOnlyDictionary<PropertyInfo, MemberInfo> propertyInfoToGetterMemberInfoMap;
        private ReadOnlyDictionary<PropertyInfo, MemberInfo> propertyInfoToSetterMemberInfoMap;
        private ReadOnlyDictionary<EventInfo, EventMemberInfo> eventsInfoToMemberInfoMap;

        public ProxyBuilder(Type targetType, ModuleBuilder moduleBuilder)
            : this()
        {
            this.targetType = targetType ?? throw new ArgumentNullException(nameof(targetType));
            this.moduleBuilder = moduleBuilder ?? throw new ArgumentNullException(nameof(moduleBuilder));
        }

        private ProxyBuilder()
        {
            reservedMembersNames = new HashSet<string>();
        }

        public Type CreateProxyType()
        {
            InitializeInfo();
            InitializeTypeBuilder();

            DefineInstanceFields();
            DefineConstructor();

            DefineTargetInterfaceMemebers();

            return typeBuilder.CreateType();
        }

        private void InitializeInfo()
        {
            (var eventsInfo, var methodsInfo, var propertiesInfo) = targetType.GetAllInterfaceMembers();

            var propertiesWithGetter = propertiesInfo.Where(p => p.GetGetMethod() != null && p.GetGetMethod().IsPublic);
            var propertiesWithSetter = propertiesInfo.Where(p => p.GetSetMethod() != null && p.GetSetMethod().IsPublic);

            methodInfoToMemberInfoMap = methodsInfo.ToReadOnlyDictionary(mi => mi, mi => new MemberInfo());
            propertyInfoToGetterMemberInfoMap = propertiesWithGetter.ToReadOnlyDictionary(pi => pi, pi => new MemberInfo());
            propertyInfoToSetterMemberInfoMap = propertiesWithSetter.ToReadOnlyDictionary(pi => pi, pi => new MemberInfo());
            eventsInfoToMemberInfoMap = eventsInfo.ToReadOnlyDictionary(ei => ei, ei => new EventMemberInfo());

            reservedMembersNames.IntersectWith(
                eventsInfo.Select(ei => ei.Name)
                .Concat(methodsInfo.Select(mi => mi.Name))
                .Concat(propertiesInfo.Select(pi => pi.Name)));
        }

        private void InitializeTypeBuilder()
        {
            typeBuilder =
                moduleBuilder.DefineType(
                    GetTypeName(),
                    TypeAttributes.Class | TypeAttributes.Public,
                    typeof(object),
                    new Type[] { targetType });
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
                    typeBuilder.DefineField(GetMethodDecoratorsFieldName(methodInfo), GetMethodDecoratorsListType(methodInfo), FieldAttributes.Private);
            }
        }

        private string GetMethodProxyFieldName(MethodInfo methodInfo)
        {
            return GetUnreservedMemberName($"{methodInfo.Name}MethodProxy");
        }

        private string GetMethodDecoratorsFieldName(MethodInfo methodInfo)
        {
            return GetUnreservedMemberName($"{methodInfo.Name}MethodDecorators");
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

                memberInfo.EventDisplayType = GetEventDisplayType(eventInfo);

                memberInfo.SubscribersMapFieldInfo =
                    typeBuilder.DefineField(GetEventSubscribersFieldName(eventInfo), GetEventSubscribersFieldType(eventInfo), FieldAttributes.Private);
            }
        }

        private string GetEventProxyFieldName(EventInfo eventInfo)
        {
            return GetUnreservedMemberName($"{eventInfo.Name}EventProxy");
        }

        private string GetEventDecoratorsFieldName(EventInfo eventInfo)
        {
            return GetUnreservedMemberName($"{eventInfo.Name}EventDecorators");
        }

        private string GetEventSubscribersFieldName(EventInfo eventInfo)
        {
            return GetUnreservedMemberName($"{eventInfo.Name}EventSubscribers");
        }

        private Type GetEventDisplayType(EventInfo eventInfo)
        {
            var eventInvokeMethod = eventInfo.EventHandlerType.GetMethod("Invoke");
            var eventInvokeMethodArguments = eventInvokeMethod.GetMethodArgumentsTypes();
            var eventInvokeDelegateType = eventInvokeMethod.MakeGenericDelegateType();
            var wrapperMethodDelegateType = eventInvokeDelegateType.ToEnumerable().Concat(eventInvokeMethodArguments).MakeGenericDelegateAction();

            var displayType =
                moduleBuilder.DefineType(
                    GetEventDisplayTypeName(eventInfo),
                   TypeAttributes.Class | TypeAttributes.Public,
                   typeof(object));

            var targetDelegateField = displayType.DefineField("target", eventInvokeDelegateType, FieldAttributes.Private);
            var wrapperMethodField = displayType.DefineField("wrapper", wrapperMethodDelegateType, FieldAttributes.Private);

            var constructor = displayType.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, new Type[] { eventInvokeDelegateType, wrapperMethodDelegateType });
            var constructorILGenerator = constructor.GetILGenerator();
            constructorILGenerator.Emit(OpCodes.Ldarg_0);
            constructorILGenerator.Emit(OpCodes.Call, typeof(object).GetConstructor(new Type[0]));
            constructorILGenerator.Emit(OpCodes.Ldarg_0);
            constructorILGenerator.Emit(OpCodes.Ldarg_1);
            constructorILGenerator.Emit(OpCodes.Stfld, targetDelegateField);
            constructorILGenerator.Emit(OpCodes.Ldarg_0);
            constructorILGenerator.Emit(OpCodes.Ldarg_2);
            constructorILGenerator.Emit(OpCodes.Stfld, wrapperMethodField);
            constructorILGenerator.Emit(OpCodes.Ret);

            var invokeMethod =
                displayType.DefineMethod(
                    "Invoke",
                    MethodAttributes.Public,
                    typeof(void),
                    eventInvokeMethodArguments);

            var invokeMethodILGenerator = invokeMethod.GetILGenerator();
            invokeMethodILGenerator.Emit(OpCodes.Ldfld, wrapperMethodField);
            invokeMethodILGenerator.Emit(OpCodes.Ldfld, targetDelegateField);
            invokeMethodILGenerator.EmitLoadArgumentsRange(0, eventInvokeMethodArguments.Length);
            invokeMethodILGenerator.Emit(OpCodes.Callvirt, wrapperMethodDelegateType.GetMethod("Invoke"));
            invokeMethodILGenerator.Emit(OpCodes.Ret);

            return displayType.CreateType();
        }

        private string GetEventDisplayTypeName(EventInfo eventInfo)
        {
            return $"{GetTypeName()}__{eventInfo.Name}DisplayClass";
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

        private string GetPropertyGetterProxyFieldName(PropertyInfo propertyInfo)
        {
            return GetUnreservedMemberName($"{propertyInfo.Name}GetterProxy");
        }

        private string GetPropertyGetterDecoratorsFieldName(PropertyInfo propertyInfo)
        {
            return GetUnreservedMemberName($"{propertyInfo.Name}GetterDecorators");
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

        private string GetPropertySetterProxyFieldName(PropertyInfo propertyInfo)
        {
            return GetUnreservedMemberName($"{propertyInfo.Name}SetterProxy");
        }

        private string GetPropertySetterDecoratorsFieldName(PropertyInfo propertyInfo)
        {
            return GetUnreservedMemberName($"{propertyInfo.Name}SetterDecorators");
        }

        private string GetUnreservedMemberName(string memberNamePattern)
        {
            string memberName;
            int memberIndex = 0;
            do
            {
                memberName = $"{memberNamePattern}{memberIndex++}";
            }
            while (reservedMembersNames.Contains(memberName));

            reservedMembersNames.Add(memberName);
            return memberName;
        }

        private void DefineConstructor()
        {
            var constructor = typeBuilder.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, new Type[] { targetType });
            var ILGenerator = constructor.GetILGenerator();

            // Call System.Object empty constructor
            ILGenerator.Emit(OpCodes.Ldarg_0);
            ILGenerator.Emit(OpCodes.Call, typeof(object).GetConstructor(new Type[0]));

            // Set target field
            ILGenerator.Emit(OpCodes.Ldarg_0);
            ILGenerator.Emit(OpCodes.Ldarg_1);
            ILGenerator.Emit(OpCodes.Stfld, targetFieldInfo);

            var memberInfos =
                methodInfoToMemberInfoMap.Values
                .Concat(eventsInfoToMemberInfoMap.Values)
                .Concat(propertyInfoToGetterMemberInfoMap.Values)
                .Concat(propertyInfoToSetterMemberInfoMap.Values);

            foreach (var mi in memberInfos)
            {
                ILGenerator.Emit(OpCodes.Ldarg_0);
                ILGenerator.Emit(OpCodes.Newobj, mi.DecoratorsLinkedListInstanceFieldInfo.FieldType.GetConstructor(new Type[] { }));
                ILGenerator.Emit(OpCodes.Stfld, mi.DecoratorsLinkedListInstanceFieldInfo);
            }

            var eventInfos = eventsInfoToMemberInfoMap.Values;

            foreach (var ei in eventInfos)
            {
                ILGenerator.Emit(OpCodes.Ldarg_0);
                ILGenerator.Emit(OpCodes.Newobj, ei.SubscribersMapFieldInfo.FieldType.GetConstructor(new Type[] { }));
                ILGenerator.Emit(OpCodes.Stfld, ei.SubscribersMapFieldInfo);
            }

            // Return
            ILGenerator.Emit(OpCodes.Ret);
        }

        private void DefineTargetInterfaceMemebers()
        {
            DefineInterfaceMethods();
            DefineInterfaceEvents();
            DefineInterfacePropertiesGetters();
            DefineInterfacePropertiesSetters();
        }

        private void DefineInterfaceMethods()
        {
            foreach (var kvp in methodInfoToMemberInfoMap)
            {
                DefineMethodOverride(kvp.Key, kvp.Value, GetMethodWrapperMethodName(kvp.Key));
            }
        }

        private string GetMethodWrapperMethodName(MethodInfo methodInfo)
        {
            return GetUnreservedMemberName($"{methodInfo.Name}MethodWrapper");
        }

        private void DefineInterfaceEvents()
        {
            foreach (var kvp in eventsInfoToMemberInfoMap)
            {
                var eventInfo = kvp.Key;
                var memberInfo = kvp.Value;

                var eventInvokeMethod = eventInfo.EventHandlerType.GetMethod("Invoke");
                var eventInvokeMethodArguments = eventInvokeMethod.GetMethodArgumentsTypes();
                var eventInvokeDelegateType = eventInvokeMethod.MakeGenericDelegateType();
                var wrapperMethodDelegateType = eventInvokeDelegateType.ToEnumerable().Concat(eventInvokeMethodArguments).MakeGenericDelegateAction();

                var wrapperMethod = DefineWrapperMethod(eventInfo.EventHandlerType.GetMethod("Invoke"), memberInfo, GetEventWrapperMethodName(eventInfo));

                var addMethod = eventInfo.AddMethod;
                var addMethodOverriden =
                    typeBuilder.DefineMethod(
                        addMethod.Name,
                        MethodAttributes.Public | MethodAttributes.Virtual,
                        addMethod.ReturnType,
                        addMethod.GetMethodArgumentsTypes());

                var addMethodILGenerator = addMethodOverriden.GetILGenerator();

                addMethodILGenerator.DeclareLocal(eventInvokeDelegateType);

                addMethodILGenerator.Emit(OpCodes.Ldarg_1);
                addMethodILGenerator.Emit(OpCodes.Ldarg_0);
                addMethodILGenerator.Emit(OpCodes.Ldvirtftn, wrapperMethod);
                addMethodILGenerator.Emit(OpCodes.Newobj, wrapperMethodDelegateType.GetConstructor(new Type[] { typeof(Object), typeof(IntPtr) }));
                addMethodILGenerator.Emit(OpCodes.Newobj, memberInfo.EventDisplayType.GetConstructor(new Type[] { eventInvokeDelegateType, wrapperMethodDelegateType }));

                addMethodILGenerator.Emit(OpCodes.Ldvirtftn, memberInfo.EventDisplayType.GetMethod("Invoke"));
                addMethodILGenerator.Emit(OpCodes.Newobj, eventInvokeDelegateType.GetConstructor(new Type[] { typeof(Object), typeof(IntPtr) }));
                addMethodILGenerator.Emit(OpCodes.Stloc_0);

                addMethodILGenerator.Emit(OpCodes.Ldarg_0);
                addMethodILGenerator.Emit(OpCodes.Ldfld, memberInfo.SubscribersMapFieldInfo);
                addMethodILGenerator.Emit(OpCodes.Ldarg_1);
                addMethodILGenerator.Emit(OpCodes.Ldloc_0);
                addMethodILGenerator.Emit(OpCodes.Callvirt, memberInfo.SubscribersMapFieldInfo.FieldType.GetMethod("Add"));

                addMethodILGenerator.Emit(OpCodes.Ldarg_0);
                addMethodILGenerator.Emit(OpCodes.Ldfld, targetFieldInfo);
                addMethodILGenerator.Emit(OpCodes.Ldloc_0);
                addMethodILGenerator.Emit(OpCodes.Callvirt, addMethod);

                addMethodILGenerator.Emit(OpCodes.Ret);

                typeBuilder.DefineMethodOverride(addMethodOverriden, addMethod);

                var removeMethod = eventInfo.RemoveMethod;
                var removeMethodOverriden =
                    typeBuilder.DefineMethod(
                        removeMethod.Name,
                        MethodAttributes.Public | MethodAttributes.Virtual,
                        removeMethod.ReturnType,
                        removeMethod.GetMethodArgumentsTypes());

                var removeMethodILGenerator = removeMethodOverriden.GetILGenerator();

                var removeMethodReturnLabel = removeMethodILGenerator.DefineLabel();

                removeMethodILGenerator.Emit(OpCodes.Ldarg_0);
                removeMethodILGenerator.Emit(OpCodes.Ldfld, memberInfo.SubscribersMapFieldInfo);
                removeMethodILGenerator.Emit(OpCodes.Ldarg_1);
                removeMethodILGenerator.Emit(OpCodes.Callvirt, memberInfo.SubscribersMapFieldInfo.FieldType.GetMethod("ContainsKey"));
                removeMethodILGenerator.Emit(OpCodes.Brfalse_S, removeMethodReturnLabel);

                removeMethodILGenerator.Emit(OpCodes.Ldarg_0);
                removeMethodILGenerator.Emit(OpCodes.Ldfld, targetFieldInfo);
                removeMethodILGenerator.Emit(OpCodes.Ldarg_0);
                removeMethodILGenerator.Emit(OpCodes.Ldfld, memberInfo.SubscribersMapFieldInfo);
                removeMethodILGenerator.Emit(OpCodes.Ldarg_1);
                removeMethodILGenerator.Emit(OpCodes.Callvirt, memberInfo.SubscribersMapFieldInfo.FieldType.GetProperty("Item").GetGetMethod());
                removeMethodILGenerator.Emit(OpCodes.Callvirt, removeMethod);

                removeMethodILGenerator.Emit(OpCodes.Ldarg_0);
                removeMethodILGenerator.Emit(OpCodes.Ldfld, memberInfo.SubscribersMapFieldInfo);
                removeMethodILGenerator.Emit(OpCodes.Ldarg_1);
                removeMethodILGenerator.Emit(OpCodes.Callvirt, memberInfo.SubscribersMapFieldInfo.FieldType.GetMethod("Remove", new Type[] { eventInvokeDelegateType }));

                removeMethodILGenerator.MarkLabel(removeMethodReturnLabel);
                removeMethodILGenerator.Emit(OpCodes.Ret);

                typeBuilder.DefineMethodOverride(removeMethodOverriden, removeMethod);
            }
        }

        private string GetEventWrapperMethodName(EventInfo eventInfo)
        {
            return GetUnreservedMemberName($"{eventInfo.Name}EventWrapper");
        }

        private void DefineInterfacePropertiesGetters()
        {
            foreach (var kvp in propertyInfoToGetterMemberInfoMap)
            {
                DefineMethodOverride(kvp.Key.GetGetMethod(), kvp.Value, GetPropertyGetterWrapperMethodName(kvp.Key));
            }
        }

        private string GetPropertyGetterWrapperMethodName(PropertyInfo propertyInfo)
        {
            return GetUnreservedMemberName($"{propertyInfo.Name}GetterWrapper");
        }

        private void DefineInterfacePropertiesSetters()
        {
            foreach (var kvp in propertyInfoToSetterMemberInfoMap)
            {
                DefineMethodOverride(kvp.Key.GetSetMethod(), kvp.Value, GetPropertySetterWrapperMethodName(kvp.Key));
            }
        }

        private string GetPropertySetterWrapperMethodName(PropertyInfo propertyInfo)
        {
            return GetUnreservedMemberName($"{propertyInfo.Name}SetterWrapper");
        }

        private void DefineMethodOverride(MethodInfo methodInfo, MemberInfo memberInfo, string methodName)
        {
            var wrapperMethod = DefineWrapperMethod(methodInfo, memberInfo, methodName);

            var method =
                typeBuilder.DefineMethod(
                    methodName,
                    MethodAttributes.Public | MethodAttributes.Virtual,
                    methodInfo.ReturnType,
                    methodInfo.GetMethodArgumentsTypes());

            var ILGenerator = method.GetILGenerator();

            ILGenerator.Emit(OpCodes.Ldarg_0);
            ILGenerator.Emit(OpCodes.Ldarg_0);
            ILGenerator.Emit(OpCodes.Ldfld, targetFieldInfo);
            ILGenerator.Emit(OpCodes.Dup);
            ILGenerator.Emit(OpCodes.Ldvirtftn, methodInfo);
            ILGenerator.Emit(OpCodes.Newobj, methodInfo.MakeGenericDelegateType().GetConstructor(new Type[] { typeof(object), typeof(IntPtr) }));
            ILGenerator.EmitLoadArgumentsRange(1, methodInfo.GetMethodArgumentsTypes().Length);
            ILGenerator.Emit(OpCodes.Call, wrapperMethod);
            ILGenerator.Emit(OpCodes.Ret);

            typeBuilder.DefineMethodOverride(method, methodInfo);
        }

        private MethodInfo DefineWrapperMethod(MethodInfo methodInfo, MemberInfo memberInfo, string methodName)
        {
            var methodDelegateType = methodInfo.MakeGenericDelegateType();
            var methodArgumentsTypes = methodInfo.GetMethodArgumentsTypes();
            var decoratorsListType = GetMethodDecoratorsListType(methodInfo);
            var decoratorsListItemType = GetMethodDecoratorsListItemType(methodInfo);
            var decoratorsPairType = GetMethodDecoratorsPairType(methodInfo);
            var beforeDecoratorType = GetMethodBeforeDecoratorType(methodInfo);
            var proxyType = GetMethodProxyType(methodInfo);
            var afterDecoratorType = GetMethodAfterDecoratorType(methodInfo);

            var method =
                typeBuilder.DefineMethod(
                    methodName,
                    MethodAttributes.Private,
                    methodInfo.ReturnType,
                    methodDelegateType.ToEnumerable().Concat(methodArgumentsTypes).ToArray());


            var ILGenerator = method.GetILGenerator();

            var beforeDecoratorsWhileBodyLabel = ILGenerator.DefineLabel();
            var getNextDecoratorsPairLabel = ILGenerator.DefineLabel();
            var beforeDecoratorsWhileStatementLabel = ILGenerator.DefineLabel();

            var proxyLabel = ILGenerator.DefineLabel();
            var setCurrentToLastDecoratorsNodeLabel = ILGenerator.DefineLabel();

            var afterDecoratorsWhileBodyLabel = ILGenerator.DefineLabel();
            var getPreviousDecoratorsPairLabel = ILGenerator.DefineLabel();
            var afterDecoratorsWhileStatementLabel = ILGenerator.DefineLabel();

            var catchBlockDecoratorsWhileBodyLabel = ILGenerator.DefineLabel();
            var catchBlockGetPreviousDecoratorsPair = ILGenerator.DefineLabel();
            var catchBlockDecoratorsWhileStatementLabel = ILGenerator.DefineLabel();
            var catchBlockRethrowLabel = ILGenerator.DefineLabel();

            var returnLabel = ILGenerator.DefineLabel();

            // Store first decorators pair in current variable
            ILGenerator.DeclareLocal(decoratorsListItemType); // Local 0
            ILGenerator.Emit(OpCodes.Ldarg_0);
            ILGenerator.Emit(OpCodes.Ldfld, memberInfo.DecoratorsLinkedListInstanceFieldInfo);
            ILGenerator.Emit(OpCodes.Callvirt, decoratorsListType.GetProperty("First").GetGetMethod());
            ILGenerator.Emit(OpCodes.Stloc_0);

            // Store new list of exceptions in exceptions variable
            ILGenerator.DeclareLocal(typeof(List<Exception>)); // Local 1
            ILGenerator.Emit(OpCodes.Newobj, typeof(List<Exception>).GetConstructor(new Type[] { }));
            ILGenerator.Emit(OpCodes.Stloc_1);

            ILGenerator.DeclareLocal(typeof(Exception)); // Local 2 - temp exception variable

            // Store default value of return type in result variable
            if (methodInfo.ReturnType != typeof(void))
            {
                ILGenerator.DeclareLocal(methodInfo.ReturnType); // Local 3
                ILGenerator.EmitCreateTypeDefaultValueOnStack(methodInfo.ReturnType);
                ILGenerator.Emit(OpCodes.Stloc_3);
            }

            ILGenerator.BeginExceptionBlock();

            ILGenerator.Emit(OpCodes.Br_S, beforeDecoratorsWhileStatementLabel);

            // Execute before decorators while body start
            ILGenerator.MarkLabel(beforeDecoratorsWhileBodyLabel);

            // Invoke before decorator if not null
            ILGenerator.Emit(OpCodes.Ldloc_0);
            ILGenerator.Emit(OpCodes.Callvirt, decoratorsListItemType.GetProperty("Value").GetGetMethod());
            ILGenerator.Emit(OpCodes.Ldfld, decoratorsPairType.GetField("Item1"));
            ILGenerator.Emit(OpCodes.Brfalse_S, getNextDecoratorsPairLabel);
            ILGenerator.Emit(OpCodes.Ldloc_0);
            ILGenerator.Emit(OpCodes.Callvirt, decoratorsListItemType.GetProperty("Value").GetGetMethod());
            ILGenerator.Emit(OpCodes.Ldfld, decoratorsPairType.GetField("Item1"));
            ILGenerator.EmitLoadArgumentsRange(2, methodArgumentsTypes.Length);
            ILGenerator.Emit(OpCodes.Callvirt, beforeDecoratorType.GetMethod("Invoke"));

            // Get next decorators pair
            ILGenerator.MarkLabel(getNextDecoratorsPairLabel);
            ILGenerator.Emit(OpCodes.Ldloc_0);
            ILGenerator.Emit(OpCodes.Callvirt, decoratorsListItemType.GetProperty("Next").GetGetMethod());
            ILGenerator.Emit(OpCodes.Stloc_0);
            // Execute before decorators while body finish

            // Execute before decorators while statement
            ILGenerator.MarkLabel(beforeDecoratorsWhileStatementLabel);
            ILGenerator.Emit(OpCodes.Ldloc_0);
            ILGenerator.Emit(OpCodes.Ldnull);
            ILGenerator.Emit(OpCodes.Cgt_Un);
            ILGenerator.Emit(OpCodes.Brtrue_S, beforeDecoratorsWhileBodyLabel);

            // Check if proxy is null
            ILGenerator.Emit(OpCodes.Ldarg_0);
            ILGenerator.Emit(OpCodes.Ldfld, memberInfo.ProxyInstanceFieldInfo);
            ILGenerator.Emit(OpCodes.Brtrue_S, proxyLabel);

            // Execute target method
            ILGenerator.Emit(OpCodes.Ldarg_1);
            ILGenerator.EmitLoadArgumentsRange(2, methodArgumentsTypes.Length);
            ILGenerator.Emit(OpCodes.Callvirt, methodDelegateType.GetMethod("Invoke"));
            if (methodInfo.ReturnType != typeof(void))
            {
                ILGenerator.Emit(OpCodes.Stloc_3);
            }
            ILGenerator.Emit(OpCodes.Br_S, setCurrentToLastDecoratorsNodeLabel);

            // Execute proxy
            ILGenerator.MarkLabel(proxyLabel);
            ILGenerator.Emit(OpCodes.Ldarg_0);
            ILGenerator.Emit(OpCodes.Ldfld, memberInfo.ProxyInstanceFieldInfo);
            ILGenerator.Emit(OpCodes.Ldarg_1);
            ILGenerator.EmitLoadArgumentsRange(2, methodArgumentsTypes.Length);
            ILGenerator.Emit(OpCodes.Callvirt, proxyType.GetMethod("Invoke"));
            if (methodInfo.ReturnType != typeof(void))
            {
                ILGenerator.Emit(OpCodes.Stloc_3);
            }

            // Store last decorators pair in current variable
            ILGenerator.MarkLabel(setCurrentToLastDecoratorsNodeLabel);
            ILGenerator.Emit(OpCodes.Ldarg_0);
            ILGenerator.Emit(OpCodes.Ldfld, memberInfo.DecoratorsLinkedListInstanceFieldInfo);
            ILGenerator.Emit(OpCodes.Callvirt, decoratorsListType.GetProperty("Last").GetGetMethod());
            ILGenerator.Emit(OpCodes.Stloc_0);
            ILGenerator.Emit(OpCodes.Br_S, afterDecoratorsWhileStatementLabel);

            // Execute after decorators while body start
            ILGenerator.MarkLabel(afterDecoratorsWhileBodyLabel);

            // Invoke after decorator if it is not null
            ILGenerator.Emit(OpCodes.Ldloc_0);
            ILGenerator.Emit(OpCodes.Callvirt, decoratorsListItemType.GetProperty("Value").GetGetMethod());
            ILGenerator.Emit(OpCodes.Ldfld, decoratorsPairType.GetField("Item2"));
            ILGenerator.Emit(OpCodes.Brfalse_S, getPreviousDecoratorsPairLabel);
            ILGenerator.Emit(OpCodes.Ldloc_0);
            ILGenerator.Emit(OpCodes.Callvirt, decoratorsListItemType.GetProperty("Value").GetGetMethod());
            ILGenerator.Emit(OpCodes.Ldfld, decoratorsPairType.GetField("Item2"));
            ILGenerator.EmitLoadArgumentsRange(2, methodArgumentsTypes.Length);
            if (methodInfo.ReturnType != typeof(void))
            {
                ILGenerator.Emit(OpCodes.Ldloc_3);
            }
            ILGenerator.Emit(OpCodes.Callvirt, afterDecoratorType.GetMethod("Invoke"));

            // Get previous decorators pair
            ILGenerator.MarkLabel(getPreviousDecoratorsPairLabel);
            ILGenerator.Emit(OpCodes.Ldloc_0);
            ILGenerator.Emit(OpCodes.Callvirt, decoratorsListItemType.GetProperty("Previous").GetGetMethod());
            ILGenerator.Emit(OpCodes.Stloc_0);
            // Execute after decorators while body finish

            // Execute after decorators while statement
            ILGenerator.MarkLabel(afterDecoratorsWhileStatementLabel);
            ILGenerator.Emit(OpCodes.Ldloc_0);
            ILGenerator.Emit(OpCodes.Brtrue, afterDecoratorsWhileBodyLabel);
            ILGenerator.Emit(OpCodes.Leave, returnLabel);

            // Main block catch start
            ILGenerator.BeginCatchBlock(typeof(Exception));

            // Add main block exception to list
            ILGenerator.Emit(OpCodes.Stloc_2);
            ILGenerator.Emit(OpCodes.Ldloc_1);
            ILGenerator.Emit(OpCodes.Ldloc_2);
            ILGenerator.Emit(OpCodes.Callvirt, typeof(List<Exception>).GetMethod("Add", new Type[] { typeof(Exception) }));

            // Set current to last element of decorators list if it is null
            ILGenerator.Emit(OpCodes.Ldloc_0);
            ILGenerator.Emit(OpCodes.Brtrue, catchBlockDecoratorsWhileStatementLabel);
            ILGenerator.Emit(OpCodes.Ldarg_0);
            ILGenerator.Emit(OpCodes.Ldfld, memberInfo.DecoratorsLinkedListInstanceFieldInfo);
            ILGenerator.Emit(OpCodes.Callvirt, memberInfo.DecoratorsLinkedListInstanceFieldInfo.FieldType.GetProperty("Last").GetGetMethod());
            ILGenerator.Emit(OpCodes.Stloc_0);
            ILGenerator.Emit(OpCodes.Br, catchBlockDecoratorsWhileStatementLabel);

            // Catch block while body start
            ILGenerator.MarkLabel(catchBlockDecoratorsWhileBodyLabel);

            // Invoke after decorator if both after decorator and before decorator are not null
            ILGenerator.Emit(OpCodes.Ldloc_0);
            ILGenerator.Emit(OpCodes.Callvirt, decoratorsListItemType.GetProperty("Value").GetGetMethod());
            ILGenerator.Emit(OpCodes.Ldfld, decoratorsPairType.GetField("Item1"));
            ILGenerator.Emit(OpCodes.Brfalse, catchBlockGetPreviousDecoratorsPair);
            ILGenerator.Emit(OpCodes.Ldloc_0);
            ILGenerator.Emit(OpCodes.Callvirt, decoratorsListItemType.GetProperty("Value").GetGetMethod());
            ILGenerator.Emit(OpCodes.Ldfld, decoratorsPairType.GetField("Item2"));
            ILGenerator.Emit(OpCodes.Brfalse, catchBlockGetPreviousDecoratorsPair);

            // Invoke catching exception into list
            ILGenerator.BeginExceptionBlock();
            ILGenerator.Emit(OpCodes.Ldloc_0);
            ILGenerator.Emit(OpCodes.Callvirt, decoratorsListItemType.GetProperty("Value").GetGetMethod());
            ILGenerator.Emit(OpCodes.Ldfld, decoratorsPairType.GetField("Item2"));
            ILGenerator.EmitLoadArgumentsRange(2, methodArgumentsTypes.Length);
            if (methodInfo.ReturnType != typeof(void))
            {
                ILGenerator.Emit(OpCodes.Ldloc_3);
            }
            ILGenerator.Emit(OpCodes.Callvirt, afterDecoratorType.GetMethod("Invoke"));
            ILGenerator.Emit(OpCodes.Leave, catchBlockGetPreviousDecoratorsPair);
            ILGenerator.BeginCatchBlock(typeof(Exception));
            ILGenerator.Emit(OpCodes.Stloc_2);
            ILGenerator.Emit(OpCodes.Ldloc_1);
            ILGenerator.Emit(OpCodes.Ldloc_2);
            ILGenerator.Emit(OpCodes.Callvirt, typeof(List<Exception>).GetMethod("Add", new Type[] { typeof(Exception) }));
            ILGenerator.EndExceptionBlock();

            // Get previous decorator
            ILGenerator.MarkLabel(catchBlockGetPreviousDecoratorsPair);
            ILGenerator.Emit(OpCodes.Ldloc_0);
            ILGenerator.Emit(OpCodes.Callvirt, decoratorsListItemType.GetProperty("Previous").GetGetMethod());
            ILGenerator.Emit(OpCodes.Stloc_0);

            // Catch block while body finish

            // Catch block while statement check
            ILGenerator.MarkLabel(catchBlockDecoratorsWhileStatementLabel);
            ILGenerator.Emit(OpCodes.Ldloc_0);
            ILGenerator.Emit(OpCodes.Brtrue, catchBlockDecoratorsWhileBodyLabel);

            // Throw aggregated exception if an after decorators in catch block has thrown an exception
            ILGenerator.Emit(OpCodes.Ldloc_1);
            ILGenerator.Emit(OpCodes.Callvirt, typeof(List<Exception>).GetProperty("Count").GetGetMethod());
            ILGenerator.Emit(OpCodes.Ldc_I4_1);
            ILGenerator.Emit(OpCodes.Ble, catchBlockRethrowLabel);
            ILGenerator.Emit(OpCodes.Ldloc_1);
            ILGenerator.Emit(OpCodes.Newobj, typeof(AggregateException).GetConstructor(new Type[] { typeof(IEnumerable<Exception>) }));
            ILGenerator.Emit(OpCodes.Throw);

            ILGenerator.MarkLabel(catchBlockRethrowLabel);
            ILGenerator.Emit(OpCodes.Rethrow);

            ILGenerator.EndExceptionBlock();
            // Main block catch finish

            // Return
            ILGenerator.MarkLabel(returnLabel);
            if (methodInfo.ReturnType != typeof(void))
            {
                ILGenerator.Emit(OpCodes.Ldloc_3);
            }
            ILGenerator.Emit(OpCodes.Ret);

            return method;
        }

        private static Type GetEventProxyFieldType(EventInfo eventInfo)
        {
            return GetMethodProxyType(eventInfo.EventHandlerType.GetMethod("Invoke"));
        }

        private static Type GetEventDecoratorsFieldType(EventInfo eventInfo)
        {
            return GetMethodDecoratorsListType(eventInfo.EventHandlerType.GetMethod("Invoke"));
        }

        private static Type GetEventSubscribersFieldType(EventInfo eventInfo)
        {
            var eventDelegate = eventInfo.EventHandlerType.GetMethod("Invoke").MakeGenericDelegateType();
            return typeof(Dictionary<,>).MakeGenericType(eventDelegate, eventDelegate);
        }

        private static Type GetPropertyGetterProxyFieldType(PropertyInfo propertyInfo)
        {
            return GetMethodProxyType(propertyInfo.GetGetMethod());
        }

        private static Type GetPropertyGetterDecoratorsFieldType(PropertyInfo propertyInfo)
        {
            return GetMethodDecoratorsListType(propertyInfo.GetGetMethod());
        }

        private static Type GetPropertySetterProxyFieldType(PropertyInfo propertyInfo)
        {
            return GetMethodProxyType(propertyInfo.GetSetMethod());
        }

        private static Type GetPropertySetterDecoratorsFieldType(PropertyInfo propertyInfo)
        {
            return GetMethodDecoratorsListType(propertyInfo.GetSetMethod());
        }

        private static Type GetMethodProxyType(MethodInfo methodInfo)
        {
            var methodDelegateType = methodInfo.MakeGenericDelegateType();
            var methodArguments = methodInfo.GetMethodArgumentsTypes();
            
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

        private static Type GetMethodDecoratorsListType(MethodInfo methodInfo)
        {
            return typeof(LinkedList<>).MakeGenericType(GetMethodDecoratorsPairType(methodInfo));
        }

        private static Type GetMethodDecoratorsListItemType(MethodInfo methodInfo)
        {
            return typeof(LinkedListNode<>).MakeGenericType(GetMethodDecoratorsPairType(methodInfo));
        }

        private static Type GetMethodDecoratorsPairType(MethodInfo methodInfo)
        {
            return typeof(ValueTuple<,>).MakeGenericType(GetMethodBeforeDecoratorType(methodInfo).ConcatInstances(GetMethodAfterDecoratorType(methodInfo)).ToArray());
        }

        private static Type GetMethodBeforeDecoratorType(MethodInfo methodInfo)
        {
            return methodInfo.GetMethodArgumentsTypes().MakeGenericDelegateAction();
        }

        private static Type GetMethodAfterDecoratorType(MethodInfo methodInfo)
        {
            switch (methodInfo.ReturnType)
            {
                case Type voidReturnType when voidReturnType == typeof(void):
                    return methodInfo.GetMethodArgumentsTypes().MakeGenericDelegateAction();

                case Type nonVoidReturnType:
                    return methodInfo.GetMethodArgumentsTypes().Concat(nonVoidReturnType).MakeGenericDelegateAction();

                case null:
                    throw new ArgumentNullException("Method info return type is null", $"{nameof(methodInfo)}.{nameof(methodInfo.ReturnType)}");
            }
        }

        private class MemberInfo
        {
            public FieldInfo ProxyInstanceFieldInfo { get; set; }
            public FieldInfo DecoratorsLinkedListInstanceFieldInfo { get; set; }
        }

        private class EventMemberInfo : MemberInfo
        {
            public Type EventDisplayType { get; set; }
            public FieldInfo SubscribersMapFieldInfo { get; set; }
        }
    }
}
