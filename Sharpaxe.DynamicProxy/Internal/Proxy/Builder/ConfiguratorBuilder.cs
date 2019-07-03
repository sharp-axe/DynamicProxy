using Sharpaxe.DynamicProxy.Internal.Proxy.NameProvider;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace Sharpaxe.DynamicProxy.Internal.Proxy.Builder
{
#warning Make single decorators setter IL-code generator
    internal class ConfiguratorBuilder
    {
        private readonly Type targetType;
        private readonly Type proxyType;
        private readonly IMemberNamesProvider memberNamesProvider;
        private readonly ModuleBuilder moduleBuilder;

        private TypeBuilder typeBuilder;

        private Dictionary<EventInfo, int> eventInfoToIndexMap;
        private Dictionary<MethodInfo, int> methodInfoToIndexMap;
        private Dictionary<PropertyInfo, int> propertyInfoToIndexMap;

        private FieldInfo targetInstanceFieldInfo;
        private FieldInfo eventInfoToIndexMapStaticFieldInfo;
        private FieldInfo methodInfoToIndexMapStaticFieldInfo;
        private FieldInfo propertyInfoToIndexMapStaticFieldInfo;

        private FieldInfo eventInfoToProxyFieldInfoMapStaticFieldInfo;
        private Dictionary<EventInfo, FieldInfo> eventInfoToProxyFieldInfoMap;

        private FieldInfo eventInfoToDecoratorsFieldInfoMapStaticFieldInfo;
        private Dictionary<EventInfo, FieldInfo> eventInfoToDecoratorsFieldInfoMap;

        private FieldInfo methodInfoToProxyFieldInfoMapStaticFieldInfo;
        private Dictionary<MethodInfo, FieldInfo> methodInfoToProxyFieldInfoMap;

        private FieldInfo methodInfoToDecoratorsFieldInfoMapStaticFieldInfo;
        private Dictionary<MethodInfo, FieldInfo> methodInfoToDecoratorsFieldInfoMap;

        private FieldInfo propertyInfoToGetterProxyFieldInfoMapStaticFieldInfo;
        private Dictionary<PropertyInfo, FieldInfo> propertyInfoToGetterProxyFieldInfoMap;

        private FieldInfo propertyInfoToGetterDecoratorsFieldInfoMapStaticFieldInfo;
        private Dictionary<PropertyInfo, FieldInfo> propertyInfoToGetterDecoratorsFieldInfoMap;

        private FieldInfo propertyInfoToSetterProxyFieldInfoMapStaticFieldInfo;
        private Dictionary<PropertyInfo, FieldInfo> propertyInfoToSetterProxyFieldInfoMap;

        private FieldInfo propertyInfoToSetterDecoratorsFieldInfoMapStaticFieldInfo;
        private Dictionary<PropertyInfo, FieldInfo> propertyInfoToSetterDecoratorsFieldInfoMap;

        public ConfiguratorBuilder(Type targetType, Type proxyType, IMemberNamesProvider memberNamesProvider, ModuleBuilder moduleBuilder)
        {
            this.targetType = targetType ?? throw new ArgumentNullException(nameof(targetType));
            this.proxyType = proxyType ?? throw new ArgumentNullException(nameof(proxyType));
            this.memberNamesProvider = memberNamesProvider ?? throw new ArgumentNullException(nameof(memberNamesProvider));
            this.moduleBuilder = moduleBuilder ?? throw new ArgumentNullException(nameof(moduleBuilder));
        }

        public Type CreateConfiguratorType()
        {
            InitializeInfo();
            InitializeTypeBuilder();

            DefineStaticFields();
            DefineInstanceFields();
            DefineConstructor();

            DefineInstanceFunctions();

            var type = typeBuilder.CreateType();
            SetStaticFields(type);
            return type;
        }

        private void InitializeInfo()
        {
            (var eventsInfos, var methodsInfos, var propertiesInfos) = targetType.GetAllInterfaceMembers();

            eventInfoToIndexMap = eventsInfos.Select((ei, i) => new KeyValuePair<EventInfo, int>(ei, i)).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            methodInfoToIndexMap = methodsInfos.Select((mi, i) => new KeyValuePair<MethodInfo, int>(mi, i)).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            propertyInfoToIndexMap = propertiesInfos.Select((pi, i) => new KeyValuePair<PropertyInfo, int>(pi, i)).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

            eventInfoToProxyFieldInfoMap = eventsInfos.ToDictionary(ei => ei, ei => proxyType.GetField(memberNamesProvider.GetEventProxyFieldName(ei), BindingFlags.NonPublic | BindingFlags.Instance));
            eventInfoToDecoratorsFieldInfoMap = eventsInfos.ToDictionary(ei => ei, ei => proxyType.GetField(memberNamesProvider.GetEventDecoratorsFieldName(ei), BindingFlags.NonPublic | BindingFlags.Instance));

            methodInfoToProxyFieldInfoMap = methodsInfos.ToDictionary(mi => mi, mi => proxyType.GetField(memberNamesProvider.GetMethodProxyFieldName(mi), BindingFlags.NonPublic | BindingFlags.Instance));
            methodInfoToDecoratorsFieldInfoMap = methodsInfos.ToDictionary(mi => mi, mi => proxyType.GetField(memberNamesProvider.GetMethodDecoratorsFieldName(mi), BindingFlags.NonPublic | BindingFlags.Instance));

            propertyInfoToGetterProxyFieldInfoMap = propertiesInfos
                .Where(pi => pi.GetGetMethod() != null && pi.GetGetMethod().IsPublic)
                .ToDictionary(pi => pi, pi => proxyType.GetField(memberNamesProvider.GetPropertyGetterProxyFieldName(pi), BindingFlags.NonPublic | BindingFlags.Instance));

            propertyInfoToGetterDecoratorsFieldInfoMap = propertiesInfos
                .Where(pi => pi.GetGetMethod() != null && pi.GetGetMethod().IsPublic)
                .ToDictionary(pi => pi, pi => proxyType.GetField(memberNamesProvider.GetPropertyGetterDecoratorsFieldName(pi), BindingFlags.NonPublic | BindingFlags.Instance));

            propertyInfoToSetterProxyFieldInfoMap = propertiesInfos
                .Where(pi => pi.GetSetMethod() != null && pi.GetSetMethod().IsPublic)
                .ToDictionary(pi => pi, pi => proxyType.GetField(memberNamesProvider.GetPropertySetterProxyFieldName(pi), BindingFlags.NonPublic | BindingFlags.Instance));

            propertyInfoToSetterDecoratorsFieldInfoMap = propertiesInfos
                .Where(pi => pi.GetSetMethod() != null && pi.GetSetMethod().IsPublic)
                .ToDictionary(pi => pi, pi => proxyType.GetField(memberNamesProvider.GetPropertySetterDecoratorsFieldName(pi), BindingFlags.NonPublic | BindingFlags.Instance));
        }

        private void InitializeTypeBuilder()
        {
            typeBuilder =
                moduleBuilder.DefineType(
                    GetConfiguratorTypeName(),
                    TypeAttributes.Class | TypeAttributes.Public,
                    typeof(object),
                    new Type[] { typeof(IProxyConfigurator) });
        }

        private string GetConfiguratorTypeName()
        {
            return $"{targetType.Name}``_Configurator";
        }

        private void DefineStaticFields()
        {
            eventInfoToIndexMapStaticFieldInfo = typeBuilder.DefineField("eventInfoToIndexMap", typeof(Dictionary<EventInfo, int>), FieldAttributes.Private | FieldAttributes.Static);
            methodInfoToIndexMapStaticFieldInfo = typeBuilder.DefineField("methodInfoToIndexMap", typeof(Dictionary<MethodInfo, int>), FieldAttributes.Private | FieldAttributes.Static);
            propertyInfoToIndexMapStaticFieldInfo = typeBuilder.DefineField("propertyInfoToIndexMap", typeof(Dictionary<PropertyInfo, int>), FieldAttributes.Private | FieldAttributes.Static);

            eventInfoToProxyFieldInfoMapStaticFieldInfo = typeBuilder.DefineField("eventInfoToProxyFieldInfoMap", typeof(Dictionary<EventInfo, FieldInfo>), FieldAttributes.Private | FieldAttributes.Static);
            eventInfoToDecoratorsFieldInfoMapStaticFieldInfo = typeBuilder.DefineField("eventInfoToDecoratorsFieldInfoMap", typeof(Dictionary<EventInfo, FieldInfo>), FieldAttributes.Private | FieldAttributes.Static);
            methodInfoToProxyFieldInfoMapStaticFieldInfo = typeBuilder.DefineField("methodInfoToProxyFieldInfoMap", typeof(Dictionary<MethodInfo, FieldInfo>), FieldAttributes.Private | FieldAttributes.Static);
            methodInfoToDecoratorsFieldInfoMapStaticFieldInfo = typeBuilder.DefineField("methodInfoToDecoratorsFieldInfoMap", typeof(Dictionary<MethodInfo, FieldInfo>), FieldAttributes.Private | FieldAttributes.Static);
            propertyInfoToGetterProxyFieldInfoMapStaticFieldInfo = typeBuilder.DefineField("propertyInfoToGetterProxyFieldInfoMap", typeof(Dictionary<PropertyInfo, FieldInfo>), FieldAttributes.Private | FieldAttributes.Static);
            propertyInfoToGetterDecoratorsFieldInfoMapStaticFieldInfo = typeBuilder.DefineField("propertyInfoToGetterDecoratorsFieldInfoMap", typeof(Dictionary<PropertyInfo, FieldInfo>), FieldAttributes.Private | FieldAttributes.Static);
            propertyInfoToSetterProxyFieldInfoMapStaticFieldInfo = typeBuilder.DefineField("propertyInfoToSetterProxyFieldInfoMap", typeof(Dictionary<PropertyInfo, FieldInfo>), FieldAttributes.Private | FieldAttributes.Static);
            propertyInfoToSetterDecoratorsFieldInfoMapStaticFieldInfo = typeBuilder.DefineField("propertyInfoToSetterDecoratorsFieldInfoMap", typeof(Dictionary<PropertyInfo, FieldInfo>), FieldAttributes.Private | FieldAttributes.Static);
        }

        private void DefineInstanceFields()
        {
            targetInstanceFieldInfo = typeBuilder.DefineField("target", proxyType, FieldAttributes.Private);
        }

        private void DefineConstructor()
        {
            var constructor = typeBuilder.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, new Type[] { proxyType });

            var ILGenerator = constructor.GetILGenerator();

            ILGenerator.Emit(OpCodes.Ldarg_0);
            ILGenerator.Emit(OpCodes.Call, typeof(object).GetConstructor(new Type[0]));
            ILGenerator.Emit(OpCodes.Ldarg_0);
            ILGenerator.Emit(OpCodes.Ldarg_1);
            ILGenerator.Emit(OpCodes.Stfld, targetInstanceFieldInfo);
            ILGenerator.Emit(OpCodes.Ret);
        }

        private void DefineInstanceFunctions()
        {
            DefineSetEventProxyMethod();
            DefineSetEventDecoratorMethod();

            DefineSetMethodProxyMethod();
            DefineSetMethodDecoratorsMethod();

            DefineSetPropertyGetterProxyMethod();
            DefineSetPropertyGetterDecoratorsMethod();

            DefineSetPropertySetterProxyMethod();
            DefineSetPropertySetterDecoratorsMethod();
        }

        private void DefineSetEventProxyMethod()
        {
            var methodInfo = typeof(IProxyConfigurator).GetMethod("SetEventProxy");

            var methodBuilder =
                typeBuilder.DefineMethod(
                    methodInfo.Name,
                    MethodAttributes.Public | MethodAttributes.Virtual,
                    methodInfo.ReturnType,
                    methodInfo.GetMethodArgumentsTypes());

            var ILGenerator = methodBuilder.GetILGenerator();

            var bodyLabel = ILGenerator.DefineLabel();

            ILGenerator.DeclareLocal(typeof(FieldInfo)); // Local 0
            ILGenerator.Emit(OpCodes.Ldsfld, eventInfoToProxyFieldInfoMapStaticFieldInfo);
            ILGenerator.Emit(OpCodes.Ldarg_1);
            ILGenerator.Emit(OpCodes.Ldloca_S, (byte)0);
            ILGenerator.Emit(OpCodes.Callvirt, typeof(Dictionary<EventInfo, FieldInfo>).GetMethod("TryGetValue"));
            ILGenerator.Emit(OpCodes.Brtrue_S, bodyLabel);

            ILGenerator.Emit(OpCodes.Newobj, typeof(ArgumentException).GetConstructor(new Type[0]));
            ILGenerator.Emit(OpCodes.Throw);

            ILGenerator.MarkLabel(bodyLabel);

            ILGenerator.Emit(OpCodes.Ldloc_0);
            ILGenerator.Emit(OpCodes.Ldarg_0);
            ILGenerator.Emit(OpCodes.Ldfld, targetInstanceFieldInfo);
            ILGenerator.Emit(OpCodes.Ldarg_2);
            ILGenerator.Emit(OpCodes.Callvirt, typeof(FieldInfo).GetMethod("SetValue", new Type[] { typeof(object), typeof(object) }));

            ILGenerator.Emit(OpCodes.Ret);

            typeBuilder.DefineMethodOverride(methodBuilder, methodInfo);
        }

        private void DefineSetEventDecoratorMethod()
        {
            var methodInfo = typeof(IProxyConfigurator).GetMethod("SetEventDecorators");

            var methodBuilder =
                typeBuilder.DefineMethod(
                    methodInfo.Name,
                    MethodAttributes.Public | MethodAttributes.Virtual,
                    methodInfo.ReturnType,
                    methodInfo.GetMethodArgumentsTypes());

            var ILGenerator = methodBuilder.GetILGenerator();

            var mainBodyLabel = ILGenerator.DefineLabel();
            var throwExceptionLabel = ILGenerator.DefineLabel();

            ILGenerator.DeclareLocal(typeof(int)); // Local 0
            ILGenerator.DeclareLocal(typeof(FieldInfo)); // Local 1
            ILGenerator.DeclareLocal(typeof(IEnumerator<ValueTuple<object, object>>)); // Local 2
            ILGenerator.DeclareLocal(typeof(ValueTuple<object, object>)); // Local 3

            ILGenerator.Emit(OpCodes.Ldsfld, eventInfoToIndexMapStaticFieldInfo);
            ILGenerator.Emit(OpCodes.Ldarg_1);
            ILGenerator.Emit(OpCodes.Ldloca_S, (byte)0);
            ILGenerator.Emit(OpCodes.Callvirt, typeof(Dictionary<EventInfo, int>).GetMethod("TryGetValue"));
            ILGenerator.Emit(OpCodes.Brfalse, throwExceptionLabel);

            ILGenerator.Emit(OpCodes.Ldsfld, eventInfoToDecoratorsFieldInfoMapStaticFieldInfo);
            ILGenerator.Emit(OpCodes.Ldarg_1);
            ILGenerator.Emit(OpCodes.Ldloca_S, (byte)1);
            ILGenerator.Emit(OpCodes.Callvirt, typeof(Dictionary<EventInfo, FieldInfo>).GetMethod("TryGetValue"));
            ILGenerator.Emit(OpCodes.Brtrue, mainBodyLabel);

            ILGenerator.MarkLabel(throwExceptionLabel);

            ILGenerator.Emit(OpCodes.Newobj, typeof(ArgumentException).GetConstructor(new Type[0]));
            ILGenerator.Emit(OpCodes.Throw);

            ILGenerator.MarkLabel(mainBodyLabel);

            foreach (var kvp in eventInfoToIndexMap)
            {
                var beforeDecoratorType = kvp.Key.EventHandlerType.GetMethod("Invoke").MakeGenericDelegateType();
                var afterDecoratorType = kvp.Key.EventHandlerType.GetMethod("Invoke").MakeGenericDelegateType();

                var decoratorsPairType = typeof(ValueTuple<,>).MakeGenericType(beforeDecoratorType, afterDecoratorType);


                var jumpOverLabel = ILGenerator.DefineLabel();
                var foreachBodyLabel = ILGenerator.DefineLabel();
                var foreachStatementLabel = ILGenerator.DefineLabel();

                ILGenerator.Emit(OpCodes.Ldloc_0);
                ILGenerator.Emit(OpCodes.Ldc_I4, kvp.Value);
                ILGenerator.Emit(OpCodes.Bne_Un, jumpOverLabel);

                ILGenerator.BeginExceptionBlock();

                ILGenerator.Emit(OpCodes.Ldarg_2);
                ILGenerator.Emit(OpCodes.Callvirt, typeof(IEnumerable<ValueTuple<object, object>>).GetMethod("GetEnumerator"));
                ILGenerator.Emit(OpCodes.Stloc_2);
                ILGenerator.Emit(OpCodes.Br, foreachStatementLabel);

                ILGenerator.MarkLabel(foreachBodyLabel);
                ILGenerator.Emit(OpCodes.Ldloc_1);
                ILGenerator.Emit(OpCodes.Ldarg_0);
                ILGenerator.Emit(OpCodes.Ldfld, targetInstanceFieldInfo);
                ILGenerator.Emit(OpCodes.Callvirt, typeof(FieldInfo).GetMethod("GetValue"));
                ILGenerator.Emit(OpCodes.Ldloc_2);
                ILGenerator.Emit(OpCodes.Callvirt, typeof(IEnumerator<ValueTuple<object, object>>).GetProperty("Current").GetGetMethod());
                ILGenerator.Emit(OpCodes.Stloc_3);
                ILGenerator.Emit(OpCodes.Ldloc_3);
                ILGenerator.Emit(OpCodes.Ldfld, typeof(ValueTuple<object, object>).GetField("Item1"));
                ILGenerator.Emit(OpCodes.Ldloc_3);
                ILGenerator.Emit(OpCodes.Ldfld, typeof(ValueTuple<object, object>).GetField("Item2"));
                ILGenerator.Emit(OpCodes.Newobj, decoratorsPairType.GetConstructor(new Type[] { beforeDecoratorType, afterDecoratorType }));
                ILGenerator.Emit(OpCodes.Callvirt, eventInfoToDecoratorsFieldInfoMap[kvp.Key].FieldType.GetMethod("AddLast", new Type[] { decoratorsPairType }));
                ILGenerator.Emit(OpCodes.Pop);

                ILGenerator.MarkLabel(foreachStatementLabel);
                ILGenerator.Emit(OpCodes.Ldloc_2);
                ILGenerator.Emit(OpCodes.Callvirt, typeof(IEnumerator).GetMethod("MoveNext"));
                ILGenerator.Emit(OpCodes.Brtrue, foreachBodyLabel);
                ILGenerator.Emit(OpCodes.Leave, jumpOverLabel);

                ILGenerator.BeginFinallyBlock();
                ILGenerator.Emit(OpCodes.Ldloc_2);
                ILGenerator.Emit(OpCodes.Callvirt, typeof(IDisposable).GetMethod("Dispose"));
                ILGenerator.Emit(OpCodes.Endfinally);

                ILGenerator.EndExceptionBlock();

                ILGenerator.MarkLabel(jumpOverLabel);
            }

            ILGenerator.Emit(OpCodes.Ret);

            typeBuilder.DefineMethodOverride(methodBuilder, methodInfo);
        }

        private void DefineSetMethodProxyMethod()
        {
            var methodInfo = typeof(IProxyConfigurator).GetMethod("SetMethodProxy");

            var methodBuilder =
                typeBuilder.DefineMethod(
                    methodInfo.Name,
                    MethodAttributes.Public | MethodAttributes.Virtual,
                    methodInfo.ReturnType,
                    methodInfo.GetMethodArgumentsTypes());

            var ILGenerator = methodBuilder.GetILGenerator();

            var bodyLabel = ILGenerator.DefineLabel();

            ILGenerator.DeclareLocal(typeof(FieldInfo)); // Local 0
            ILGenerator.Emit(OpCodes.Ldsfld, methodInfoToProxyFieldInfoMapStaticFieldInfo);
            ILGenerator.Emit(OpCodes.Ldarg_1);
            ILGenerator.Emit(OpCodes.Ldloca_S, (byte)0);
            ILGenerator.Emit(OpCodes.Callvirt, typeof(Dictionary<MethodInfo, FieldInfo>).GetMethod("TryGetValue"));
            ILGenerator.Emit(OpCodes.Brtrue_S, bodyLabel);

            ILGenerator.Emit(OpCodes.Newobj, typeof(ArgumentException).GetConstructor(new Type[0]));
            ILGenerator.Emit(OpCodes.Throw);

            ILGenerator.MarkLabel(bodyLabel);

            ILGenerator.Emit(OpCodes.Ldloc_0);
            ILGenerator.Emit(OpCodes.Ldarg_0);
            ILGenerator.Emit(OpCodes.Ldfld, targetInstanceFieldInfo);
            ILGenerator.Emit(OpCodes.Ldarg_2);
            ILGenerator.Emit(OpCodes.Callvirt, typeof(FieldInfo).GetMethod("SetValue", new Type[] { typeof(object), typeof(object) }));

            ILGenerator.Emit(OpCodes.Ret);

            typeBuilder.DefineMethodOverride(methodBuilder, methodInfo);
        }

        private void DefineSetMethodDecoratorsMethod()
        {
            var methodInfo = typeof(IProxyConfigurator).GetMethod("SetMethodDecorators");

            var methodBuilder =
                typeBuilder.DefineMethod(
                    methodInfo.Name,
                    MethodAttributes.Public | MethodAttributes.Virtual,
                    methodInfo.ReturnType,
                    methodInfo.GetMethodArgumentsTypes());

            var ILGenerator = methodBuilder.GetILGenerator();

            var mainBodyLabel = ILGenerator.DefineLabel();
            var throwExceptionLabel = ILGenerator.DefineLabel();

            ILGenerator.DeclareLocal(typeof(int)); // Local 0
            ILGenerator.DeclareLocal(typeof(FieldInfo)); // Local 1
            ILGenerator.DeclareLocal(typeof(IEnumerator<ValueTuple<object, object>>)); // Local 2
            ILGenerator.DeclareLocal(typeof(ValueTuple<object, object>)); // Local 3

            ILGenerator.Emit(OpCodes.Ldsfld, methodInfoToIndexMapStaticFieldInfo);
            ILGenerator.Emit(OpCodes.Ldarg_1);
            ILGenerator.Emit(OpCodes.Ldloca_S, (byte)0);
            ILGenerator.Emit(OpCodes.Callvirt, typeof(Dictionary<MethodInfo, int>).GetMethod("TryGetValue"));
            ILGenerator.Emit(OpCodes.Brfalse, throwExceptionLabel);

            ILGenerator.Emit(OpCodes.Ldsfld, methodInfoToDecoratorsFieldInfoMapStaticFieldInfo);
            ILGenerator.Emit(OpCodes.Ldarg_1);
            ILGenerator.Emit(OpCodes.Ldloca_S, (byte)1);
            ILGenerator.Emit(OpCodes.Callvirt, typeof(Dictionary<MethodInfo, FieldInfo>).GetMethod("TryGetValue"));
            ILGenerator.Emit(OpCodes.Brtrue, mainBodyLabel);

            ILGenerator.MarkLabel(throwExceptionLabel);

            ILGenerator.Emit(OpCodes.Newobj, typeof(ArgumentException).GetConstructor(new Type[0]));
            ILGenerator.Emit(OpCodes.Throw);

            ILGenerator.MarkLabel(mainBodyLabel);

            foreach (var kvp in methodInfoToIndexMap.Where(kvp => !kvp.Key.HasOutParameters()))
            {
                var beforeDecoratorType = kvp.Key.GetMethodArgumentsTypes().MakeGenericDelegateAction();
                var afterDecoratorType = kvp.Key.ReturnType == typeof(void)
                      ? kvp.Key.GetMethodArgumentsTypes().MakeGenericDelegateAction()
                      : kvp.Key.GetMethodArgumentsTypes().Concat(kvp.Key.ReturnType).MakeGenericDelegateAction();

                var decoratorsPairType = typeof(ValueTuple<,>).MakeGenericType(beforeDecoratorType, afterDecoratorType);


                var jumpOverLabel = ILGenerator.DefineLabel();
                var foreachBodyLabel = ILGenerator.DefineLabel();
                var foreachStatementLabel = ILGenerator.DefineLabel();

                ILGenerator.Emit(OpCodes.Ldloc_0);
                ILGenerator.Emit(OpCodes.Ldc_I4, kvp.Value);
                ILGenerator.Emit(OpCodes.Bne_Un, jumpOverLabel);

                ILGenerator.BeginExceptionBlock();

                ILGenerator.Emit(OpCodes.Ldarg_2);
                ILGenerator.Emit(OpCodes.Callvirt, typeof(IEnumerable<ValueTuple<object, object>>).GetMethod("GetEnumerator"));
                ILGenerator.Emit(OpCodes.Stloc_2);
                ILGenerator.Emit(OpCodes.Br, foreachStatementLabel);

                ILGenerator.MarkLabel(foreachBodyLabel);
                ILGenerator.Emit(OpCodes.Ldloc_1);
                ILGenerator.Emit(OpCodes.Ldarg_0);
                ILGenerator.Emit(OpCodes.Ldfld, targetInstanceFieldInfo);
                ILGenerator.Emit(OpCodes.Callvirt, typeof(FieldInfo).GetMethod("GetValue"));
                ILGenerator.Emit(OpCodes.Ldloc_2);
                ILGenerator.Emit(OpCodes.Callvirt, typeof(IEnumerator<ValueTuple<object, object>>).GetProperty("Current").GetGetMethod());
                ILGenerator.Emit(OpCodes.Stloc_3);
                ILGenerator.Emit(OpCodes.Ldloc_3);
                ILGenerator.Emit(OpCodes.Ldfld, typeof(ValueTuple<object, object>).GetField("Item1"));
                ILGenerator.Emit(OpCodes.Ldloc_3);
                ILGenerator.Emit(OpCodes.Ldfld, typeof(ValueTuple<object, object>).GetField("Item2"));
                ILGenerator.Emit(OpCodes.Newobj, decoratorsPairType.GetConstructor(new Type[] { beforeDecoratorType, afterDecoratorType }));
                ILGenerator.Emit(OpCodes.Callvirt, methodInfoToDecoratorsFieldInfoMap[kvp.Key].FieldType.GetMethod("AddLast", new Type[] { decoratorsPairType }));
                ILGenerator.Emit(OpCodes.Pop);

                ILGenerator.MarkLabel(foreachStatementLabel);
                ILGenerator.Emit(OpCodes.Ldloc_2);
                ILGenerator.Emit(OpCodes.Callvirt, typeof(IEnumerator).GetMethod("MoveNext"));
                ILGenerator.Emit(OpCodes.Brtrue, foreachBodyLabel);
                ILGenerator.Emit(OpCodes.Leave, jumpOverLabel);

                ILGenerator.BeginFinallyBlock();
                ILGenerator.Emit(OpCodes.Ldloc_2);
                ILGenerator.Emit(OpCodes.Callvirt, typeof(IDisposable).GetMethod("Dispose"));
                ILGenerator.Emit(OpCodes.Endfinally);

                ILGenerator.EndExceptionBlock();

                ILGenerator.MarkLabel(jumpOverLabel);
            }

            ILGenerator.Emit(OpCodes.Ret);

            typeBuilder.DefineMethodOverride(methodBuilder, methodInfo);
        }

        private void DefineSetPropertyGetterProxyMethod()
        {
            var methodInfo = typeof(IProxyConfigurator).GetMethod("SetPropertyGetterProxy");

            var methodBuilder =
                typeBuilder.DefineMethod(
                    methodInfo.Name,
                    MethodAttributes.Public | MethodAttributes.Virtual,
                    methodInfo.ReturnType,
                    methodInfo.GetMethodArgumentsTypes());

            var ILGenerator = methodBuilder.GetILGenerator();

            var bodyLabel = ILGenerator.DefineLabel();

            ILGenerator.DeclareLocal(typeof(FieldInfo)); // Local 0
            ILGenerator.Emit(OpCodes.Ldsfld, propertyInfoToGetterProxyFieldInfoMapStaticFieldInfo);
            ILGenerator.Emit(OpCodes.Ldarg_1);
            ILGenerator.Emit(OpCodes.Ldloca_S, (byte)0);
            ILGenerator.Emit(OpCodes.Callvirt, typeof(Dictionary<PropertyInfo, FieldInfo>).GetMethod("TryGetValue"));
            ILGenerator.Emit(OpCodes.Brtrue_S, bodyLabel);

            ILGenerator.Emit(OpCodes.Newobj, typeof(ArgumentException).GetConstructor(new Type[0]));
            ILGenerator.Emit(OpCodes.Throw);

            ILGenerator.MarkLabel(bodyLabel);

            ILGenerator.Emit(OpCodes.Ldloc_0);
            ILGenerator.Emit(OpCodes.Ldarg_0);
            ILGenerator.Emit(OpCodes.Ldfld, targetInstanceFieldInfo);
            ILGenerator.Emit(OpCodes.Ldarg_2);
            ILGenerator.Emit(OpCodes.Callvirt, typeof(FieldInfo).GetMethod("SetValue", new Type[] { typeof(object), typeof(object) }));

            ILGenerator.Emit(OpCodes.Ret);

            typeBuilder.DefineMethodOverride(methodBuilder, methodInfo);
        }

        private void DefineSetPropertyGetterDecoratorsMethod()
        {
            var methodInfo = typeof(IProxyConfigurator).GetMethod("SetPropertyGetterDecorators");

            var methodBuilder =
                typeBuilder.DefineMethod(
                    methodInfo.Name,
                    MethodAttributes.Public | MethodAttributes.Virtual,
                    methodInfo.ReturnType,
                    methodInfo.GetMethodArgumentsTypes());

            var ILGenerator = methodBuilder.GetILGenerator();

            var mainBodyLabel = ILGenerator.DefineLabel();
            var throwExceptionLabel = ILGenerator.DefineLabel();

            ILGenerator.DeclareLocal(typeof(int)); // Local 0
            ILGenerator.DeclareLocal(typeof(FieldInfo)); // Local 1
            ILGenerator.DeclareLocal(typeof(IEnumerator<ValueTuple<object, object>>)); // Local 2
            ILGenerator.DeclareLocal(typeof(ValueTuple<object, object>)); // Local 3

            ILGenerator.Emit(OpCodes.Ldsfld, propertyInfoToIndexMapStaticFieldInfo);
            ILGenerator.Emit(OpCodes.Ldarg_1);
            ILGenerator.Emit(OpCodes.Ldloca_S, (byte)0);
            ILGenerator.Emit(OpCodes.Callvirt, typeof(Dictionary<PropertyInfo, int>).GetMethod("TryGetValue"));
            ILGenerator.Emit(OpCodes.Brfalse, throwExceptionLabel);

            ILGenerator.Emit(OpCodes.Ldsfld, propertyInfoToGetterDecoratorsFieldInfoMapStaticFieldInfo);
            ILGenerator.Emit(OpCodes.Ldarg_1);
            ILGenerator.Emit(OpCodes.Ldloca_S, (byte)1);
            ILGenerator.Emit(OpCodes.Callvirt, typeof(Dictionary<PropertyInfo, FieldInfo>).GetMethod("TryGetValue"));
            ILGenerator.Emit(OpCodes.Brtrue, mainBodyLabel);

            ILGenerator.MarkLabel(throwExceptionLabel);

            ILGenerator.Emit(OpCodes.Newobj, typeof(ArgumentException).GetConstructor(new Type[0]));
            ILGenerator.Emit(OpCodes.Throw);

            ILGenerator.MarkLabel(mainBodyLabel);

            foreach (var kvp in propertyInfoToIndexMap.Where(kvp => kvp.Key.GetGetMethod() != null && kvp.Key.GetGetMethod().IsPublic))
            {
                var beforeDecoratorType = kvp.Key.GetGetMethod().GetMethodArgumentsTypes().MakeGenericDelegateAction();
                var afterDecoratorType = kvp.Key.GetGetMethod().GetMethodArgumentsTypes().Concat(kvp.Key.GetGetMethod().ReturnType).MakeGenericDelegateAction();

                var decoratorsPairType = typeof(ValueTuple<,>).MakeGenericType(beforeDecoratorType, afterDecoratorType);


                var jumpOverLabel = ILGenerator.DefineLabel();
                var foreachBodyLabel = ILGenerator.DefineLabel();
                var foreachStatementLabel = ILGenerator.DefineLabel();

                ILGenerator.Emit(OpCodes.Ldloc_0);
                ILGenerator.Emit(OpCodes.Ldc_I4, kvp.Value);
                ILGenerator.Emit(OpCodes.Bne_Un, jumpOverLabel);

                ILGenerator.BeginExceptionBlock();

                ILGenerator.Emit(OpCodes.Ldarg_2);
                ILGenerator.Emit(OpCodes.Callvirt, typeof(IEnumerable<ValueTuple<object, object>>).GetMethod("GetEnumerator"));
                ILGenerator.Emit(OpCodes.Stloc_2);
                ILGenerator.Emit(OpCodes.Br, foreachStatementLabel);

                ILGenerator.MarkLabel(foreachBodyLabel);
                ILGenerator.Emit(OpCodes.Ldloc_1);
                ILGenerator.Emit(OpCodes.Ldarg_0);
                ILGenerator.Emit(OpCodes.Ldfld, targetInstanceFieldInfo);
                ILGenerator.Emit(OpCodes.Callvirt, typeof(FieldInfo).GetMethod("GetValue"));
                ILGenerator.Emit(OpCodes.Ldloc_2);
                ILGenerator.Emit(OpCodes.Callvirt, typeof(IEnumerator<ValueTuple<object, object>>).GetProperty("Current").GetGetMethod());
                ILGenerator.Emit(OpCodes.Stloc_3);
                ILGenerator.Emit(OpCodes.Ldloc_3);
                ILGenerator.Emit(OpCodes.Ldfld, typeof(ValueTuple<object, object>).GetField("Item1"));
                ILGenerator.Emit(OpCodes.Ldloc_3);
                ILGenerator.Emit(OpCodes.Ldfld, typeof(ValueTuple<object, object>).GetField("Item2"));
                ILGenerator.Emit(OpCodes.Newobj, decoratorsPairType.GetConstructor(new Type[] { beforeDecoratorType, afterDecoratorType }));
                ILGenerator.Emit(OpCodes.Callvirt, propertyInfoToGetterDecoratorsFieldInfoMap[kvp.Key].FieldType.GetMethod("AddLast", new Type[] { decoratorsPairType }));
                ILGenerator.Emit(OpCodes.Pop);

                ILGenerator.MarkLabel(foreachStatementLabel);
                ILGenerator.Emit(OpCodes.Ldloc_2);
                ILGenerator.Emit(OpCodes.Callvirt, typeof(IEnumerator).GetMethod("MoveNext"));
                ILGenerator.Emit(OpCodes.Brtrue, foreachBodyLabel);
                ILGenerator.Emit(OpCodes.Leave, jumpOverLabel);

                ILGenerator.BeginFinallyBlock();
                ILGenerator.Emit(OpCodes.Ldloc_2);
                ILGenerator.Emit(OpCodes.Callvirt, typeof(IDisposable).GetMethod("Dispose"));
                ILGenerator.Emit(OpCodes.Endfinally);

                ILGenerator.EndExceptionBlock();

                ILGenerator.MarkLabel(jumpOverLabel);
            }

            ILGenerator.Emit(OpCodes.Ret);

            typeBuilder.DefineMethodOverride(methodBuilder, methodInfo);
        }

        private void DefineSetPropertySetterProxyMethod()
        {
            var methodInfo = typeof(IProxyConfigurator).GetMethod("SetPropertySetterProxy");

            var methodBuilder =
                typeBuilder.DefineMethod(
                    methodInfo.Name,
                    MethodAttributes.Public | MethodAttributes.Virtual,
                    methodInfo.ReturnType,
                    methodInfo.GetMethodArgumentsTypes());

            var ILGenerator = methodBuilder.GetILGenerator();

            var bodyLabel = ILGenerator.DefineLabel();

            ILGenerator.DeclareLocal(typeof(FieldInfo)); // Local 0
            ILGenerator.Emit(OpCodes.Ldsfld, propertyInfoToSetterProxyFieldInfoMapStaticFieldInfo);
            ILGenerator.Emit(OpCodes.Ldarg_1);
            ILGenerator.Emit(OpCodes.Ldloca_S, (byte)0);
            ILGenerator.Emit(OpCodes.Callvirt, typeof(Dictionary<PropertyInfo, FieldInfo>).GetMethod("TryGetValue"));
            ILGenerator.Emit(OpCodes.Brtrue_S, bodyLabel);

            ILGenerator.Emit(OpCodes.Newobj, typeof(ArgumentException).GetConstructor(new Type[0]));
            ILGenerator.Emit(OpCodes.Throw);

            ILGenerator.MarkLabel(bodyLabel);

            ILGenerator.Emit(OpCodes.Ldloc_0);
            ILGenerator.Emit(OpCodes.Ldarg_0);
            ILGenerator.Emit(OpCodes.Ldfld, targetInstanceFieldInfo);
            ILGenerator.Emit(OpCodes.Ldarg_2);
            ILGenerator.Emit(OpCodes.Callvirt, typeof(FieldInfo).GetMethod("SetValue", new Type[] { typeof(object), typeof(object) }));

            ILGenerator.Emit(OpCodes.Ret);

            typeBuilder.DefineMethodOverride(methodBuilder, methodInfo);
        }

        private void DefineSetPropertySetterDecoratorsMethod()
        {
            var methodInfo = typeof(IProxyConfigurator).GetMethod("SetPropertySetterDecorators");

            var methodBuilder =
                typeBuilder.DefineMethod(
                    methodInfo.Name,
                    MethodAttributes.Public | MethodAttributes.Virtual,
                    methodInfo.ReturnType,
                    methodInfo.GetMethodArgumentsTypes());

            var ILGenerator = methodBuilder.GetILGenerator();

            var mainBodyLabel = ILGenerator.DefineLabel();
            var throwExceptionLabel = ILGenerator.DefineLabel();

            ILGenerator.DeclareLocal(typeof(int)); // Local 0
            ILGenerator.DeclareLocal(typeof(FieldInfo)); // Local 1
            ILGenerator.DeclareLocal(typeof(IEnumerator<ValueTuple<object, object>>)); // Local 2
            ILGenerator.DeclareLocal(typeof(ValueTuple<object, object>)); // Local 3

            ILGenerator.Emit(OpCodes.Ldsfld, propertyInfoToIndexMapStaticFieldInfo);
            ILGenerator.Emit(OpCodes.Ldarg_1);
            ILGenerator.Emit(OpCodes.Ldloca_S, (byte)0);
            ILGenerator.Emit(OpCodes.Callvirt, typeof(Dictionary<PropertyInfo, int>).GetMethod("TryGetValue"));
            ILGenerator.Emit(OpCodes.Brfalse, throwExceptionLabel);

            ILGenerator.Emit(OpCodes.Ldsfld, propertyInfoToSetterDecoratorsFieldInfoMapStaticFieldInfo);
            ILGenerator.Emit(OpCodes.Ldarg_1);
            ILGenerator.Emit(OpCodes.Ldloca_S, (byte)1);
            ILGenerator.Emit(OpCodes.Callvirt, typeof(Dictionary<PropertyInfo, FieldInfo>).GetMethod("TryGetValue"));
            ILGenerator.Emit(OpCodes.Brtrue, mainBodyLabel);

            ILGenerator.MarkLabel(throwExceptionLabel);

            ILGenerator.Emit(OpCodes.Newobj, typeof(ArgumentException).GetConstructor(new Type[0]));
            ILGenerator.Emit(OpCodes.Throw);

            ILGenerator.MarkLabel(mainBodyLabel);

            foreach (var kvp in propertyInfoToIndexMap.Where(kvp => kvp.Key.GetSetMethod() != null && kvp.Key.GetSetMethod().IsPublic))
            {
                var beforeDecoratorType = kvp.Key.GetSetMethod().GetMethodArgumentsTypes().MakeGenericDelegateAction();
                var afterDecoratorType = kvp.Key.GetSetMethod().GetMethodArgumentsTypes().MakeGenericDelegateAction();

                var decoratorsPairType = typeof(ValueTuple<,>).MakeGenericType(beforeDecoratorType, afterDecoratorType);


                var jumpOverLabel = ILGenerator.DefineLabel();
                var foreachBodyLabel = ILGenerator.DefineLabel();
                var foreachStatementLabel = ILGenerator.DefineLabel();

                ILGenerator.Emit(OpCodes.Ldloc_0);
                ILGenerator.Emit(OpCodes.Ldc_I4, kvp.Value);
                ILGenerator.Emit(OpCodes.Bne_Un, jumpOverLabel);

                ILGenerator.BeginExceptionBlock();

                ILGenerator.Emit(OpCodes.Ldarg_2);
                ILGenerator.Emit(OpCodes.Callvirt, typeof(IEnumerable<ValueTuple<object, object>>).GetMethod("GetEnumerator"));
                ILGenerator.Emit(OpCodes.Stloc_2);
                ILGenerator.Emit(OpCodes.Br, foreachStatementLabel);

                ILGenerator.MarkLabel(foreachBodyLabel);
                ILGenerator.Emit(OpCodes.Ldloc_1);
                ILGenerator.Emit(OpCodes.Ldarg_0);
                ILGenerator.Emit(OpCodes.Ldfld, targetInstanceFieldInfo);
                ILGenerator.Emit(OpCodes.Callvirt, typeof(FieldInfo).GetMethod("GetValue"));
                ILGenerator.Emit(OpCodes.Ldloc_2);
                ILGenerator.Emit(OpCodes.Callvirt, typeof(IEnumerator<ValueTuple<object, object>>).GetProperty("Current").GetGetMethod());
                ILGenerator.Emit(OpCodes.Stloc_3);
                ILGenerator.Emit(OpCodes.Ldloc_3);
                ILGenerator.Emit(OpCodes.Ldfld, typeof(ValueTuple<object, object>).GetField("Item1"));
                ILGenerator.Emit(OpCodes.Ldloc_3);
                ILGenerator.Emit(OpCodes.Ldfld, typeof(ValueTuple<object, object>).GetField("Item2"));
                ILGenerator.Emit(OpCodes.Newobj, decoratorsPairType.GetConstructor(new Type[] { beforeDecoratorType, afterDecoratorType }));
                ILGenerator.Emit(OpCodes.Callvirt, propertyInfoToSetterDecoratorsFieldInfoMap[kvp.Key].FieldType.GetMethod("AddLast", new Type[] { decoratorsPairType }));
                ILGenerator.Emit(OpCodes.Pop);

                ILGenerator.MarkLabel(foreachStatementLabel);
                ILGenerator.Emit(OpCodes.Ldloc_2);
                ILGenerator.Emit(OpCodes.Callvirt, typeof(IEnumerator).GetMethod("MoveNext"));
                ILGenerator.Emit(OpCodes.Brtrue, foreachBodyLabel);
                ILGenerator.Emit(OpCodes.Leave, jumpOverLabel);

                ILGenerator.BeginFinallyBlock();
                ILGenerator.Emit(OpCodes.Ldloc_2);
                ILGenerator.Emit(OpCodes.Callvirt, typeof(IDisposable).GetMethod("Dispose"));
                ILGenerator.Emit(OpCodes.Endfinally);

                ILGenerator.EndExceptionBlock();

                ILGenerator.MarkLabel(jumpOverLabel);
            }

            ILGenerator.Emit(OpCodes.Ret);

            typeBuilder.DefineMethodOverride(methodBuilder, methodInfo);
        }

        private void SetStaticFields(Type type)
        {
            type.GetField(eventInfoToIndexMapStaticFieldInfo.Name, BindingFlags.NonPublic | BindingFlags.Static).SetValue(null, eventInfoToIndexMap);
            type.GetField(methodInfoToIndexMapStaticFieldInfo.Name, BindingFlags.NonPublic | BindingFlags.Static).SetValue(null, methodInfoToIndexMap);
            type.GetField(propertyInfoToIndexMapStaticFieldInfo.Name, BindingFlags.NonPublic | BindingFlags.Static).SetValue(null, propertyInfoToIndexMap);

            type.GetField(eventInfoToProxyFieldInfoMapStaticFieldInfo.Name, BindingFlags.NonPublic | BindingFlags.Static).SetValue(null, eventInfoToProxyFieldInfoMap);
            type.GetField(eventInfoToDecoratorsFieldInfoMapStaticFieldInfo.Name, BindingFlags.NonPublic | BindingFlags.Static).SetValue(null, eventInfoToDecoratorsFieldInfoMap);
            type.GetField(methodInfoToProxyFieldInfoMapStaticFieldInfo.Name, BindingFlags.NonPublic | BindingFlags.Static).SetValue(null, methodInfoToProxyFieldInfoMap);
            type.GetField(methodInfoToDecoratorsFieldInfoMapStaticFieldInfo.Name, BindingFlags.NonPublic | BindingFlags.Static).SetValue(null, methodInfoToDecoratorsFieldInfoMap);
            type.GetField(propertyInfoToGetterProxyFieldInfoMapStaticFieldInfo.Name, BindingFlags.NonPublic | BindingFlags.Static).SetValue(null, propertyInfoToGetterProxyFieldInfoMap);
            type.GetField(propertyInfoToGetterDecoratorsFieldInfoMapStaticFieldInfo.Name, BindingFlags.NonPublic | BindingFlags.Static).SetValue(null, propertyInfoToGetterDecoratorsFieldInfoMap);
            type.GetField(propertyInfoToSetterProxyFieldInfoMapStaticFieldInfo.Name, BindingFlags.NonPublic | BindingFlags.Static).SetValue(null, propertyInfoToSetterProxyFieldInfoMap);
            type.GetField(propertyInfoToSetterDecoratorsFieldInfoMapStaticFieldInfo.Name, BindingFlags.NonPublic | BindingFlags.Static).SetValue(null, propertyInfoToSetterDecoratorsFieldInfoMap);
        }
    }
}
