using Sharpaxe.DynamicProxy.Internal.Detector;
using System;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace Sharpaxe.DynamicProxy.Internal.DetectorBuilder
{
    internal sealed class MethodDetectorBuilder : CommonDetectorBuilder
    {
        private FieldInfo interfaceMappingStaticField;

        public MethodDetectorBuilder(Type targetType, ModuleBuilder moduleBuilder) 
            : base(targetType, moduleBuilder)
        {
        }

        protected override Type DetectorInterfaceType => typeof(IMethodDetector);
        protected override string NotSupportedMemberExceptionMessage => "No members can be invoked on this instance";

        protected override void DefineCustomStaticFields()
        {
            interfaceMappingStaticField = TypeBuilder.DefineField("interfaceMapping", typeof(InterfaceMapping), FieldAttributes.Private | FieldAttributes.Static);
        }

        protected override void SetCustomStaticFields(Type detectorType)
        {
            detectorType.GetField(interfaceMappingStaticField.Name, BindingFlags.Static | BindingFlags.NonPublic).SetValue(null, detectorType.GetInterfaceMap(TargetType));
        }

        protected override void DefineCustomInstanceFields()
        {
        }

        protected override void DefineCustomPublicMethods()
        {
            var methodInfo = DetectorInterfaceType.GetMethods(BindingFlags.Public | BindingFlags.Instance).First();

            var methodBuilder =
                TypeBuilder.DefineMethod(
                    methodInfo.Name,
                    MethodAttributes.Public | MethodAttributes.Virtual,
                    methodInfo.ReturnType,
                    methodInfo.GetParameters().Select(p => p.ParameterType).ToArray());

            var ILGenerator = methodBuilder.GetILGenerator();

            var checkIndexLabel = ILGenerator.DefineLabel();
            var returnLabel = ILGenerator.DefineLabel();
            var indexOfMethodLocalVariable = ILGenerator.DeclareLocal(typeof(MethodInfo));

            ILGenerator.Emit(OpCodes.Ldarg_0);
            ILGenerator.Emit(OpCodes.Ldflda, interfaceMappingStaticField);
            ILGenerator.Emit(OpCodes.Ldfld, typeof(InterfaceMapping).GetField("TargetMethods"));
            ILGenerator.Emit(OpCodes.Ldarg_1);
            ILGenerator.Emit(OpCodes.Callvirt, typeof(Delegate).GetProperty("Method").GetGetMethod());
            ILGenerator.Emit(OpCodes.Call, typeof(Array).GetMethod("IndexOf", new Type[] { typeof(Array), typeof(object) }));
            ILGenerator.Emit(OpCodes.Stloc_0, indexOfMethodLocalVariable);

            ILGenerator.Emit(OpCodes.Ldloc_0, indexOfMethodLocalVariable);
            ILGenerator.Emit(OpCodes.Ldc_I4_0);
            ILGenerator.Emit(OpCodes.Clt);
            ILGenerator.Emit(OpCodes.Brfalse_S, checkIndexLabel);

            ILGenerator.Emit(OpCodes.Ldstr, "The passed token must be a method pointer of target interface.");
            ILGenerator.Emit(OpCodes.Ldstr, "methodToken");
            ILGenerator.Emit(OpCodes.Newobj, typeof(ArgumentException).GetConstructor(new Type[] { typeof(string), typeof(string) }));
            ILGenerator.Emit(OpCodes.Throw);

            ILGenerator.MarkLabel(checkIndexLabel);

            ILGenerator.Emit(OpCodes.Ldloc_0, indexOfMethodLocalVariable);
            ILGenerator.Emit(OpCodes.Ldarg_0);
            ILGenerator.Emit(OpCodes.Ldflda, interfaceMappingStaticField);
            ILGenerator.Emit(OpCodes.Ldfld, typeof(InterfaceMapping).GetField("InterfaceMethods"));
            ILGenerator.Emit(OpCodes.Ldlen);
            ILGenerator.Emit(OpCodes.Conv_I4);
            ILGenerator.Emit(OpCodes.Ldc_I4_1);
            ILGenerator.Emit(OpCodes.Sub);
            ILGenerator.Emit(OpCodes.Cgt);
            ILGenerator.Emit(OpCodes.Brfalse_S, returnLabel);

            ILGenerator.Emit(OpCodes.Ldstr, "The passed token points on a method which index in the TargetMethods array ({0}) is out of range of the InterfaceMethods array length ({1}).");
            ILGenerator.Emit(OpCodes.Ldloc_0, indexOfMethodLocalVariable);
            ILGenerator.Emit(OpCodes.Box, typeof(object));
            ILGenerator.Emit(OpCodes.Ldarg_0);
            ILGenerator.Emit(OpCodes.Ldflda, interfaceMappingStaticField);
            ILGenerator.Emit(OpCodes.Ldfld, typeof(InterfaceMapping).GetField("InterfaceMethods"));
            ILGenerator.Emit(OpCodes.Ldlen);
            ILGenerator.Emit(OpCodes.Conv_I4);
            ILGenerator.Emit(OpCodes.Box, typeof(object));
            ILGenerator.Emit(OpCodes.Call, typeof(String).GetMethod("Format", new Type[] { typeof(string), typeof(object), typeof(object) }));
            ILGenerator.Emit(OpCodes.Newobj, typeof(IndexOutOfRangeException).GetConstructor(new Type[] { typeof(string) }));
            ILGenerator.Emit(OpCodes.Throw);

            ILGenerator.MarkLabel(returnLabel);

            ILGenerator.Emit(OpCodes.Ldarg_0);
            ILGenerator.Emit(OpCodes.Ldflda, interfaceMappingStaticField);
            ILGenerator.Emit(OpCodes.Ldfld, typeof(InterfaceMapping).GetField("InterfaceMethods"));
            ILGenerator.Emit(OpCodes.Ldloc_0, indexOfMethodLocalVariable);
            ILGenerator.Emit(OpCodes.Ldelem_Ref);
            ILGenerator.Emit(OpCodes.Ret);
        }
    }
}
