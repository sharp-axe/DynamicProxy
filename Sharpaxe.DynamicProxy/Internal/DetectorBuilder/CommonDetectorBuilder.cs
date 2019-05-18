using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace Sharpaxe.DynamicProxy.Internal.DetectorBuilder
{
    internal abstract class CommonDetectorBuilder
    {
        protected abstract string GetTypeName();
        protected abstract IEnumerable<Type> GetCustomInterfacesToBeImplemented();
        protected abstract void DefineCustomStaticFields();
        protected abstract void DefineCustomInstanceFields();
        protected abstract void DefineCustomPublicMethods();
        protected abstract void SetCustomStaticFields(Type detectorType);

        protected Type TargetType { get; }
        protected ModuleBuilder ModuleBuilder { get; }

        protected TypeBuilder TypeBuilder { get; private set; }

        protected EventInfo[] EventsInfo { get; private set; }
        protected MethodInfo[] MethodsInfo { get; private set; }
        protected PropertyInfo[] PropertiesInfo { get; private set; }

        public CommonDetectorBuilder(Type targetType, ModuleBuilder moduleBuilder)
        {
            TargetType = targetType ?? throw new NullReferenceException(nameof(targetType));
            ModuleBuilder = moduleBuilder ?? throw new NullReferenceException(nameof(moduleBuilder));
        }

        public Type CreateDetectorType()
        {
            InitializeInfo();
            InitializeTypeBuilder();

            DefineCustomStaticFields();
            DefineCustomInstanceFields();
            DefineConstructor();

            DefineTargetTypeEvents();
            DefineTargetTypeMethods();
            DefineTargetTypeProperties();

            DefineCustomPublicMethods();

            var detectorType = TypeBuilder.CreateType();
            SetCustomStaticFields(detectorType);
            return detectorType;
        }

        private void InitializeInfo()
        {
            EventsInfo = TargetType.GetEvents(BindingFlags.Public | BindingFlags.Instance);
            MethodsInfo = TargetType.GetMethods(BindingFlags.Public | BindingFlags.Instance).Where(m => !m.IsSpecialName).ToArray();
            PropertiesInfo = TargetType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
        }

        private void InitializeTypeBuilder()
        {
            var baseTypes = GetInterfacesToBeImplemented().ToArray();

            TypeBuilder =
                ModuleBuilder.DefineType(
                GetTypeName(),
                TypeAttributes.Class | TypeAttributes.Public,
                typeof(object),
                baseTypes);

            foreach (var bt in baseTypes)
            {
                TypeBuilder.AddInterfaceImplementation(bt);
            }
        }

        private IEnumerable<Type> GetInterfacesToBeImplemented()
        {
            yield return TargetType;

            foreach (var abt in GetCustomInterfacesToBeImplemented())
            {
                yield return abt;
            }
        }

        private void DefineConstructor()
        {
            var constructor = TypeBuilder.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, new Type[0]);
            var ILGenerator = constructor.GetILGenerator();

            // Call System.Object empty constructor
            ILGenerator.Emit(OpCodes.Ldarg_0);
            ILGenerator.Emit(OpCodes.Call, typeof(object).GetConstructor(new Type[0]));

            // Return
            ILGenerator.Emit(OpCodes.Ret);
        }

        private void DefineTargetTypeEvents()
        {
            for (int i = 0; i < EventsInfo.Length; i++)
            {
                DefineTargetTypeEvent(EventsInfo[i], i);
            }
        }

        private void DefineTargetTypeMethods()
        {
            for (int i = 0; i < MethodsInfo.Length; i++)
            {
                DefineTargetTypeMethod(MethodsInfo[i], i);
            }
        }

        private void DefineTargetTypeProperties()
        {
            for (int i = 0; i < PropertiesInfo.Length; i++)
            {
                var getMethod = PropertiesInfo[i].GetGetMethod();
                if (getMethod != null && getMethod.IsPublic)
                {
                    DefineTargetTypePropertyGetMethod(getMethod, i);
                }

                var setMethod = PropertiesInfo[i].GetSetMethod();
                if (setMethod != null && setMethod.IsPublic)
                {
                    DefineTargetTypePropertySetMethod(getMethod, i);
                }
            }
        }

        protected virtual void DefineTargetTypeEvent(EventInfo eventInfo, int indexInTypeDefinition)
        {
            throw new NotImplementedException();
        }

        protected virtual void DefineTargetTypeMethod(MethodInfo methodInfo, int indexInTypeDefinition)
        {
            throw new NotImplementedException();
        }

        protected virtual void DefineTargetTypePropertyGetMethod(MethodInfo getMethod, int indexInTypeDefinition)
        {
            var methodBuilder =
                TypeBuilder.DefineMethod(
                    getMethod.Name,
                    MethodAttributes.Public | MethodAttributes.Virtual,
                    getMethod.ReturnType,
                    getMethod.GetParameters().Select(p => p.ParameterType).ToArray());

            var ILGenerator = methodBuilder.GetILGenerator();

            // Throw a not supported exception
            ILGenerator.Emit(OpCodes.Newobj, typeof(NotSupportedException).GetConstructor(new Type[0]));
            ILGenerator.Emit(OpCodes.Throw);

            TypeBuilder.DefineMethodOverride(methodBuilder, getMethod);
        }

        protected virtual void DefineTargetTypePropertySetMethod(MethodInfo setMethod, int indexInTypeDefinition)
        {
            var methodBuilder =
                TypeBuilder.DefineMethod(
                    setMethod.Name,
                    MethodAttributes.Public | MethodAttributes.Virtual,
                    setMethod.ReturnType,
                    setMethod.GetParameters().Select(p => p.ParameterType).ToArray());

            var ILGenerator = methodBuilder.GetILGenerator();

            // Throw a not supported exception
            ILGenerator.Emit(OpCodes.Newobj, typeof(NotSupportedException).GetConstructor(new Type[0]));
            ILGenerator.Emit(OpCodes.Throw);

            TypeBuilder.DefineMethodOverride(methodBuilder, setMethod);
        }
    }
}
