using System;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace Sharpaxe.DynamicProxy.Internal
{
#warning Use MembersInfo instances instead of strings
    internal static class TypeFactory
    {
        public static Type CreateDetectorType(Type type, ModuleBuilder moduleBuilder)
        {
            var typeBuilder = moduleBuilder.DefineType(
                type.Name + DetectorTypeNameEnd,
                TypeAttributes.Class | TypeAttributes.Public,
                typeof(System.Object),
                new Type[] { type, typeof(IPropertyDetector), typeof(IEventDetector), typeof(IMethodDetector) });

            AddDetectorTypeStaticFields(typeBuilder);
            AddDetectorTypeFields(typeBuilder);
            AddDetectorTypeConstructor(typeBuilder);
            AddDetectorTypeMethods(typeBuilder);

            var properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            var events = type.GetEvents(BindingFlags.Instance | BindingFlags.Public);
            var methods = type.GetMethods(BindingFlags.Instance | BindingFlags.Public).Where(m => !m.IsSpecialName).ToArray();

            typeBuilder.AddInterfaceImplementation(type);

            AddPropertiesDetector(typeBuilder, properties);
            AddEventsDetector(typeBuilder, events);
            AddMethodsDetector(typeBuilder, methods);

            var detectorType = typeBuilder.CreateType();

            SetDetectorTypeStaticFields(detectorType, properties, events, methods);

            return detectorType;
        }

        private static void AddDetectorTypeFields(TypeBuilder typeBuilder)
        {
            typeBuilder.DefineField(DetectedPropertyGetterFieldName, typeof(PropertyInfo), FieldAttributes.Private);
            typeBuilder.DefineField(DetectedPropertySetterFieldName, typeof(PropertyInfo), FieldAttributes.Private);
            typeBuilder.DefineField(DetectedEventFieldName, typeof(EventInfo), FieldAttributes.Private);
        }

        private static void AddDetectorTypeConstructor(TypeBuilder typeBuilder)
        {
            var constructor = typeBuilder.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, new Type[0]);
            var ILGenerator = constructor.GetILGenerator();

            // Call the System.Object empty constructor
            ILGenerator.Emit(OpCodes.Ldarg_0);
            ILGenerator.Emit(OpCodes.Call, typeof(Object).GetConstructor(new Type[0]));

            // Return
            ILGenerator.Emit(OpCodes.Ret);
        }

        private static void AddDetectorTypeMethods(TypeBuilder typeBuilder)
        {

            AddPropertyDetectorMethods(typeBuilder);
            AddEventDetectorMethods(typeBuilder);
            AddMethodDetectorMethods(typeBuilder);

            AddThrowIfDetectedMethod(typeBuilder);
        }

        private static void AddPropertyDetectorMethods(TypeBuilder typeBuilder)
        {
            typeBuilder.AddInterfaceImplementation(typeof(IPropertyDetector));

            var methods = typeof(IPropertyDetector).GetMethods(BindingFlags.Public | BindingFlags.Instance);
            if (methods.Length != 1)
            {
                throw new InvalidOperationException($"Expecting one method is declared in the '{nameof(IPropertyDetector)}' interface, but it's '{methods.Length}' method(s) declared.");
            }

            var getDetectedPropertyMethod = methods[0];
            if (getDetectedPropertyMethod.GetParameters().Length != 0)
            {
                throw new InvalidOperationException($"Expecting no parameters are declared in the '{getDetectedPropertyMethod.Name}' method, but it's '{getDetectedPropertyMethod.GetParameters().Length}' parameter(s) declared.");
            }
            if (getDetectedPropertyMethod.ReturnType != typeof(PropertyInfo))
            {
                throw new InvalidOperationException($"Expecting '{nameof(PropertyInfo)}' is a return type of the '{getDetectedPropertyMethod.Name}' method, but it's '{getDetectedPropertyMethod.ReturnType.Name}'.");
            }

            AddGetDetectedPropetyMethod(typeBuilder, getDetectedPropertyMethod);
        }

        private static void AddGetDetectedPropetyMethod(TypeBuilder typeBuilder, MethodInfo method)
        {
            var getDetectedPropertyMethod = typeBuilder.DefineMethod(
                method.Name,
                MethodAttributes.Public | MethodAttributes.Virtual,
                method.ReturnType,
                method.GetParameters().Select(p => p.ParameterType).ToArray());
            var ILGenerator = getDetectedPropertyMethod.GetILGenerator();

            var returnLabel = ILGenerator.DefineLabel();

            // Jump to return if the detectedPropertyGetter field is not null
            ILGenerator.Emit(OpCodes.Ldarg_0);
            ILGenerator.Emit(OpCodes.Ldfld, $"{typeBuilder.FullName}.{DetectedPropertyGetterFieldName}");
            ILGenerator.Emit(OpCodes.Dup);
            ILGenerator.Emit(OpCodes.Brtrue_S, returnLabel);

            // Jump to return if the detectedPropertySetter field is not null
            ILGenerator.Emit(OpCodes.Ldarg_0);
            ILGenerator.Emit(OpCodes.Ldfld, $"{typeBuilder.FullName}.{DetectedPropertySetterFieldName}");
            ILGenerator.Emit(OpCodes.Dup);
            ILGenerator.Emit(OpCodes.Brtrue_S, returnLabel);

            // Throw an invalid operation exception if both fields are null
            ILGenerator.Emit(OpCodes.Pop);
            ILGenerator.Emit(OpCodes.Ldstr, "No properties have been detected");
            ILGenerator.Emit(OpCodes.Newobj, "System.InvalidOperationException..ctor");
            ILGenerator.Emit(OpCodes.Throw);

            // Return label
            ILGenerator.MarkLabel(returnLabel);

            // Return the first not-null field
            ILGenerator.Emit(OpCodes.Stloc_0);
            ILGenerator.Emit(OpCodes.Ldloc_0);
            ILGenerator.Emit(OpCodes.Ret);

            typeBuilder.DefineMethodOverride(getDetectedPropertyMethod, method);
        }

        private static void AddEventDetectorMethods(TypeBuilder typeBuilder)
        {
            typeBuilder.AddInterfaceImplementation(typeof(IEventDetector));


            var methods = typeof(IEventDetector).GetMethods(BindingFlags.Public | BindingFlags.Instance);
            if (methods.Length != 1)
            {
                throw new InvalidOperationException($"Expecting one method is declared in the '{typeof(IEventDetector).FullName}' interface, but it's '{methods.Length}' method(s) declared.");
            }

            var getDetectedEventMethod = methods[0];
            if (getDetectedEventMethod.GetParameters().Length != 0)
            {
                throw new InvalidOperationException($"Expecting no parameters are declared in the '{getDetectedEventMethod.Name}' method, but it's '{getDetectedEventMethod.GetParameters().Length}' parameter(s) declared.");
            }
            if (getDetectedEventMethod.ReturnType != typeof(EventInfo))
            {
                throw new InvalidOperationException($"Expecting '{nameof(EventInfo)}' is a return type of the '{getDetectedEventMethod.Name}' method, but it's '{getDetectedEventMethod.ReturnType.Name}'.");
            }

            AddGetDetectedEventMethod(typeBuilder, getDetectedEventMethod);
        }

        private static void AddGetDetectedEventMethod(TypeBuilder typeBuilder, MethodInfo method)
        {
            var getDetectedEventMethod = typeBuilder.DefineMethod(
                method.Name,
                MethodAttributes.Public | MethodAttributes.Virtual,
                method.ReturnType,
                method.GetParameters().Select(p => p.ParameterType).ToArray());
            var ILGenerator = getDetectedEventMethod.GetILGenerator();

            // Throw a not implemented exception
            ILGenerator.Emit(OpCodes.Newobj, "System.NotImplementedException..ctor");
            ILGenerator.Emit(OpCodes.Throw);

            typeBuilder.DefineMethodOverride(getDetectedEventMethod, method);
        }

        private static void AddMethodDetectorMethods(TypeBuilder typeBuilder)
        {
            typeBuilder.AddInterfaceImplementation(typeof(IMethodDetector));

            var methods = typeof(IMethodDetector).GetMethods(BindingFlags.Public | BindingFlags.Instance);
            if (methods.Length != 1)
            {
                throw new InvalidOperationException($"Expecting one method is declared in the '{typeof(IMethodDetector).FullName}' interface, but it's '{methods.Length}' method(s) declared.");
            }

            var getDetectedMethodMethod = methods[0];
            if (getDetectedMethodMethod.GetParameters().Length != 1)
            {
                throw new InvalidOperationException($"Expecting one parameter is declared in the '{getDetectedMethodMethod.Name}' method, but it's '{getDetectedMethodMethod.GetParameters().Length}' parameter(s) declared.");
            }
            if (getDetectedMethodMethod.GetParameters()[0].ParameterType != typeof(object))
            {
                throw new InvalidOperationException($"Expecting an object type parameter is declared in the '{getDetectedMethodMethod.Name}' method, but it's '{getDetectedMethodMethod.GetParameters()[0].ParameterType}'.");
            }
            if (getDetectedMethodMethod.ReturnType != typeof(MethodInfo))
            {
                throw new InvalidOperationException($"Expecting '{nameof(MethodInfo)}' is a return type of the '{getDetectedMethodMethod.Name}' method, but it's '{getDetectedMethodMethod.ReturnType.Name}'.");
            }

            AddGetDetectedMethodMethod(typeBuilder, getDetectedMethodMethod);
        }

        private static void AddGetDetectedMethodMethod(TypeBuilder typeBuilder, MethodInfo method)
        {
            var getDetectedMethodMethod = typeBuilder.DefineMethod(
                method.Name,
                MethodAttributes.Public | MethodAttributes.Virtual,
                method.ReturnType,
                method.GetParameters().Select(p => p.ParameterType).ToArray());
            var ILGenerator = getDetectedMethodMethod.GetILGenerator();

            // Throw a not implemented exception
            ILGenerator.Emit(OpCodes.Newobj, "System.NotImplementedException..ctor");
            ILGenerator.Emit(OpCodes.Throw);

            typeBuilder.DefineMethodOverride(getDetectedMethodMethod, method);

        }

        private static void AddThrowIfDetectedMethod(TypeBuilder typeBuilder)
        {
            var throwIfCheckedMethod = typeBuilder.DefineMethod(
                ThrowIfDetectedMethodName, 
                MethodAttributes.Private, 
                typeof(void), 
                new Type[0]);
            var ILGenerator = throwIfCheckedMethod.GetILGenerator();

            var propertySetterLabel = ILGenerator.DefineLabel();
            var eventLabel = ILGenerator.DefineLabel();
            var returnLabel = ILGenerator.DefineLabel();

            // Check if the detectedPropertyGetter field is null
            ILGenerator.Emit(OpCodes.Ldarg_0);
            ILGenerator.Emit(OpCodes.Ldfld, $"{typeBuilder.FullName}.{DetectedPropertyGetterFieldName}");
            ILGenerator.Emit(OpCodes.Stloc_0);
            ILGenerator.Emit(OpCodes.Ldloc_0);
            ILGenerator.Emit(OpCodes.Brfalse_S, propertySetterLabel);

            // Throw if the detectedPropertyGetter field is not null
            ILGenerator.Emit(OpCodes.Ldstr, "Property getter has been already detected: {0}");
            ILGenerator.Emit(OpCodes.Ldarg_0);
            ILGenerator.Emit(OpCodes.Ldfld, $"{typeBuilder.FullName}.{DetectedPropertyGetterFieldName}");
            ILGenerator.Emit(OpCodes.Callvirt, "System.Reflection.MemberInfo.get_Name");
            ILGenerator.Emit(OpCodes.Call, "System.String.Format");
            ILGenerator.Emit(OpCodes.Newobj, "System.InvalidOperationException..ctor");
            ILGenerator.Emit(OpCodes.Throw);

            // Label for check the detectedPropertySetter field
            ILGenerator.MarkLabel(propertySetterLabel);

            // Check if the detectedPropertySetter field is null
            ILGenerator.Emit(OpCodes.Ldarg_0);
            ILGenerator.Emit(OpCodes.Ldfld, $"{typeBuilder.FullName}.{DetectedPropertySetterFieldName}");
            ILGenerator.Emit(OpCodes.Stloc_0);
            ILGenerator.Emit(OpCodes.Ldloc_0);
            ILGenerator.Emit(OpCodes.Brfalse_S, eventLabel);

            // Throw if the detectedPropertySetter field is not null
            ILGenerator.Emit(OpCodes.Ldstr, "Property setter has been already detected: {0}");
            ILGenerator.Emit(OpCodes.Ldarg_0);
            ILGenerator.Emit(OpCodes.Ldfld, $"{typeBuilder.FullName}.{DetectedPropertySetterFieldName}");
            ILGenerator.Emit(OpCodes.Callvirt, "System.Reflection.MemberInfo.get_Name");
            ILGenerator.Emit(OpCodes.Call, "System.String.Format");
            ILGenerator.Emit(OpCodes.Newobj, "System.InvalidOperationException..ctor");
            ILGenerator.Emit(OpCodes.Throw);

            // Label for check the detectedEvent field
            ILGenerator.MarkLabel(eventLabel);

            // Check if the detectedEvent field is null
            ILGenerator.Emit(OpCodes.Ldarg_0);
            ILGenerator.Emit(OpCodes.Ldfld, $"{typeBuilder.FullName}.{DetectedEventFieldName}");
            ILGenerator.Emit(OpCodes.Stloc_0);
            ILGenerator.Emit(OpCodes.Ldloc_0);
            ILGenerator.Emit(OpCodes.Brfalse_S, returnLabel);

            // Throw if the detectedEvent field is not null
            ILGenerator.Emit(OpCodes.Ldstr, "Event has been already detected: {0}");
            ILGenerator.Emit(OpCodes.Ldarg_0);
            ILGenerator.Emit(OpCodes.Ldfld, $"{typeBuilder.FullName}.{DetectedEventFieldName}");
            ILGenerator.Emit(OpCodes.Callvirt, "System.Reflection.MemberInfo.get_Name");
            ILGenerator.Emit(OpCodes.Call, "System.String.Format");
            ILGenerator.Emit(OpCodes.Newobj, "System.InvalidOperationException..ctor");
            ILGenerator.Emit(OpCodes.Throw);

            //Label for return
            ILGenerator.MarkLabel(returnLabel);
            ILGenerator.Emit(OpCodes.Ret);
        }

        private static void AddDetectorTypeStaticFields(TypeBuilder type)
        {
            type.DefineField(PropertiesStaticFieldName, typeof(PropertyInfo[]), FieldAttributes.Static | FieldAttributes.Private);
            type.DefineField(EventsStaticFieldName, typeof(EventInfo[]), FieldAttributes.Static | FieldAttributes.Private);
            type.DefineField(MethodsStaticFieldName, typeof(MethodInfo[]), FieldAttributes.Static | FieldAttributes.Private);
        }

        private static void AddPropertiesDetector(TypeBuilder typeBuilder, PropertyInfo[] properties)
        {
            for (int i = 0; i < properties.Length; i++)
            {
                var getMethod = properties[i].GetGetMethod();
                if (getMethod != null && getMethod.IsPublic)
                {
                    AddPropertyGetterDetectorMethod(typeBuilder, getMethod, i);
                }

                var setMethod = properties[i].GetSetMethod();
                if (setMethod != null && setMethod.IsPublic)
                {
                    AddPropertySetterDetectorMethod(typeBuilder, setMethod, i);
                }
            }
        }

        private static void AddPropertyGetterDetectorMethod(TypeBuilder typeBuilder, MethodInfo getMethod, int index)
        {
            var getterDetectorMethod = typeBuilder.DefineMethod(
                getMethod.Name,
                MethodAttributes.Public | MethodAttributes.Virtual, 
                getMethod.ReturnType, 
                getMethod.GetParameters().Select(p => p.ParameterType).ToArray());
            var ILGenerator = getterDetectorMethod.GetILGenerator();

            // Throw if a member has been detected already
            ILGenerator.Emit(OpCodes.Ldarg_0);
            ILGenerator.Emit(OpCodes.Call, $"{typeBuilder.FullName}.{ThrowIfDetectedMethodName}");

            // Get the property by index and store it into the detectedPropertyGetter field
            ILGenerator.Emit(OpCodes.Ldarg_0);
            ILGenerator.Emit(OpCodes.Ldsfld, $"{typeBuilder.FullName}.{PropertiesStaticFieldName}");
            ILGenerator.Emit(OpCodes.Ldc_I4_S, index);
            ILGenerator.Emit(OpCodes.Ldelem_Ref);
            ILGenerator.Emit(OpCodes.Stfld, $"{typeBuilder.FullName}.{DetectedPropertyGetterFieldName}");

            // Return the default value
            ILGenerator.Emit(OpCodes.Ldloca_S, 0);
            ILGenerator.Emit(OpCodes.Initobj, getMethod.ReturnType.Name);
            ILGenerator.Emit(OpCodes.Ldloc_0);
            ILGenerator.Emit(OpCodes.Stloc_1);
            ILGenerator.Emit(OpCodes.Ldloc_1);
            ILGenerator.Emit(OpCodes.Ret);

            typeBuilder.DefineMethodOverride(getterDetectorMethod, getMethod);
        }

        private static void AddPropertySetterDetectorMethod(TypeBuilder typeBuilder, MethodInfo setMethod, int index)
        {
            var setterDetectorMethod = typeBuilder.DefineMethod(
                setMethod.Name,
                MethodAttributes.Public | MethodAttributes.Virtual, 
                setMethod.ReturnType, 
                setMethod.GetParameters().Select(p => p.ParameterType).ToArray());
            var ILGenerator = setterDetectorMethod.GetILGenerator();

            // Throw if a member has been detected already
            ILGenerator.Emit(OpCodes.Ldarg_0);
            ILGenerator.Emit(OpCodes.Call, $"{typeBuilder.FullName}.{ThrowIfDetectedMethodName}");

            // Get the property by index and store it into the detectedPropertySetter field
            ILGenerator.Emit(OpCodes.Ldarg_0);
            ILGenerator.Emit(OpCodes.Ldsfld, $"{typeBuilder.FullName}.{PropertiesStaticFieldName}");
            ILGenerator.Emit(OpCodes.Ldc_I4_S, index);
            ILGenerator.Emit(OpCodes.Ldelem_Ref);
            ILGenerator.Emit(OpCodes.Stfld, $"{typeBuilder.FullName}.{DetectedPropertyGetterFieldName}");

            // Return
            ILGenerator.Emit(OpCodes.Ret);

            typeBuilder.DefineMethodOverride(setterDetectorMethod, setMethod);
        }

        private static void AddEventsDetector(TypeBuilder typeBuilder, EventInfo[] events)
        {
            for (int i = 0; i < events.Length; i++)
            {
                AddEventDetector(typeBuilder, events[i], i);
            }
        }

        private static void AddEventDetector(TypeBuilder typeBuilder, EventInfo eventInfo, int index)
        {
            throw new NotImplementedException();
        }

        private static void AddMethodsDetector(TypeBuilder typeBuilder, MethodInfo[] methods)
        {
            for (int i = 0; i < methods.Length; i++)
            {
                AddMethodDetector(typeBuilder, methods[i], i);
            }
        }

        private static void AddMethodDetector(TypeBuilder typeBuilder, MethodInfo method, int index)
        {
            throw new NotImplementedException();
        }

        private static void SetDetectorTypeStaticFields(Type type, PropertyInfo[] properties, EventInfo[] events, MethodInfo[] methods)
        {
            type.GetField(PropertiesStaticFieldName, BindingFlags.Static | BindingFlags.NonPublic).SetValue(null, properties);
            type.GetField(EventsStaticFieldName, BindingFlags.Static | BindingFlags.NonPublic).SetValue(null, events);
            type.GetField(MethodsStaticFieldName, BindingFlags.Static | BindingFlags.NonPublic).SetValue(null, methods);
        }

        private static Type CreateProxyType(Type type, ModuleBuilder moduleBuilder)
        {
            throw new NotImplementedException();
        }

        private static Type CreateConfiguratorType(Type type, ModuleBuilder moduleBuilder)
        {
            throw new NotImplementedException();
        }

        private const string ProxyTypeNameEnd = "__Sharpaxe.DynamicProxy";
        private const string DetectorTypeNameEnd = "__Sharpaxe.DynamicDetector";
        private const string ConfiguratorTypeNameEnd = "__Sharpaxe.DynamicConfigurator";

        private const string DetectedPropertyGetterFieldName = "detectedPropertyGetter";
        private const string DetectedPropertySetterFieldName = "detectedPropertySetter";
        private const string DetectedEventFieldName = "detectedEvent";
        private const string PropertiesStaticFieldName = "properties";
        private const string EventsStaticFieldName = "events";
        private const string MethodsStaticFieldName = "methods";

        private const string ThrowIfDetectedMethodName = "ThrowIfDetected";
    }
}
