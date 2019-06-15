using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace Sharpaxe.DynamicProxy.Internal.DetectorBuilder
{
    internal abstract class CommonDetectorBuilder
    {
        protected abstract Type DetectorInterfaceType { get; }
        protected abstract string NotSupportedMemberExceptionMessage { get; }

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
            (EventsInfo, MethodsInfo, PropertiesInfo) = TargetType.GetAllInterfaceMembers();
        }

        private void InitializeTypeBuilder()
        {
            TypeBuilder =
                ModuleBuilder.DefineType(
                GetTypeName(),
                TypeAttributes.Class | TypeAttributes.Public,
                typeof(object),
                new[] { TargetType, DetectorInterfaceType });
        }

        private string GetTypeName()
        {
            return $"{TargetType.Name}``_{DetectorInterfaceType.Name}";
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

        private void DefineTargetTypeMethods()
        {
            for (int i = 0; i < MethodsInfo.Length; i++)
            {
                DefineTargetTypeMethod(MethodsInfo[i], i);
            }
        }

        private void DefineTargetTypeEvents()
        {
            for (int i = 0; i < EventsInfo.Length; i++)
            {
                DefineTargetTypeEventAddMethod(EventsInfo[i].GetAddMethod(), i);
                DefineTargetTypeEventRemoveMethod(EventsInfo[i].GetRemoveMethod(), i);
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
                    DefineTargetTypePropertySetMethod(setMethod, i);
                }
            }
        }

        protected virtual void DefineTargetTypeMethod(MethodInfo methodInfo, int indexInTypeDefenition)
        {
            DefineMethodThrowingNotSupportedException(methodInfo);
        }

        protected virtual void DefineTargetTypeEventAddMethod(MethodInfo addMethod, int indexInTypeDefenition)
        {
            DefineMethodThrowingNotSupportedException(addMethod);
        }

        protected virtual void DefineTargetTypeEventRemoveMethod(MethodInfo removeMethod, int indexInTypeDefenition)
        {
            DefineMethodThrowingNotSupportedException(removeMethod);
        }

        protected virtual void DefineTargetTypePropertyGetMethod(MethodInfo getMethod, int indexInTypeDefenition)
        {
            DefineMethodThrowingNotSupportedException(getMethod);
        }

        protected virtual void DefineTargetTypePropertySetMethod(MethodInfo setMethod, int indexInTypeDefinition)
        {
            DefineMethodThrowingNotSupportedException(setMethod);
        }

        private void DefineMethodThrowingNotSupportedException(MethodInfo methodInfo)
        {
            var methodBuilder =
                TypeBuilder.DefineMethod(
                    methodInfo.Name,
                    MethodAttributes.Public | MethodAttributes.Virtual,
                    methodInfo.ReturnType,
                    methodInfo.GetParameters().Select(p => p.ParameterType).ToArray());

            methodBuilder.GetILGenerator().EmitThrowNotSupportedException(NotSupportedMemberExceptionMessage);

            TypeBuilder.DefineMethodOverride(methodBuilder, methodInfo);
        }
    }
}
