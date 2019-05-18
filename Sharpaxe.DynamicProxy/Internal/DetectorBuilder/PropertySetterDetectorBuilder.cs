using Sharpaxe.DynamicProxy.Internal.Detector;
using System;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace Sharpaxe.DynamicProxy.Internal.DetectorBuilder
{
    internal sealed class PropertySetterDetectorBuilder : CommonDetectorBuilder
    {
        private FieldInfo typePropertiesStaticField;
        private FieldInfo detectedPropertySetterInstanceField;

        public PropertySetterDetectorBuilder(Type targetType, ModuleBuilder moduleBuilder) 
            : base(targetType, moduleBuilder)
        {
        }

        protected override Type DetectorInterfaceType => typeof(IPropertySetterDetector);
        protected override string NotSupportedMemberExceptionMessage => "Only a property setter can be invoked on this instance";

        protected override void DefineCustomStaticFields()
        {
            typePropertiesStaticField = TypeBuilder.DefineField("typeProperties", typeof(PropertyInfo[]), FieldAttributes.Private | FieldAttributes.Static);
        }

        protected override void SetCustomStaticFields(Type detectorType)
        {
            detectorType.GetField(typePropertiesStaticField.Name, BindingFlags.Static | BindingFlags.NonPublic).SetValue(null, PropertiesInfo);
        }

        protected override void DefineCustomInstanceFields()
        {
            detectedPropertySetterInstanceField = TypeBuilder.DefineField("detectedPropertySetter", typeof(PropertyInfo), FieldAttributes.Private);
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

            var returnLabel = ILGenerator.DefineLabel();

            // Go to the 'return' label if the detectedPropertyGetter field value is NOT null
            ILGenerator.Emit(OpCodes.Ldarg_0);
            ILGenerator.Emit(OpCodes.Ldfld, detectedPropertySetterInstanceField);
            ILGenerator.Emit(OpCodes.Brtrue_S, returnLabel);

            // Throw an invalid operation exception if detectedPropertyGetter field value is null
            ILGenerator.Emit(OpCodes.Ldstr, "No property has been detected");
            ILGenerator.Emit(OpCodes.Newobj, typeof(InvalidOperationException).GetConstructor(new Type[] { typeof(string) }));
            ILGenerator.Emit(OpCodes.Throw);

            // Return
            ILGenerator.MarkLabel(returnLabel);
            ILGenerator.Emit(OpCodes.Ldarg_0);
            ILGenerator.Emit(OpCodes.Ldfld, detectedPropertySetterInstanceField);
            ILGenerator.Emit(OpCodes.Ret);

            TypeBuilder.DefineMethodOverride(methodBuilder, methodInfo);
        }

        protected override void DefineTargetTypePropertySetMethod(MethodInfo setMethod, int indexInTypeDefinition)
        {
            var methodBuilder =
                TypeBuilder.DefineMethod(
                    setMethod.Name,
                    MethodAttributes.Public | MethodAttributes.Virtual,
                    setMethod.ReturnType,
                    setMethod.GetParameters().Select(p => p.ParameterType).ToArray());

            var ILGenerator = methodBuilder.GetILGenerator();

            var setFieldLabel = ILGenerator.DefineLabel();

            // Go to 'set field' label if the detectedPropertySetter field value is null
            ILGenerator.Emit(OpCodes.Ldarg_0);
            ILGenerator.Emit(OpCodes.Ldfld, detectedPropertySetterInstanceField);
            ILGenerator.Emit(OpCodes.Brfalse_S, setFieldLabel);

            // Throw an invalid operation exception if the detectedPropertySetter field is NOT null
            ILGenerator.Emit(OpCodes.Ldstr, "The setter of the following property has been already detected: {0}");
            ILGenerator.Emit(OpCodes.Ldarg_0);
            ILGenerator.Emit(OpCodes.Ldfld, detectedPropertySetterInstanceField);
            ILGenerator.Emit(OpCodes.Callvirt, typeof(MemberInfo).GetProperty("Name").GetGetMethod());
            ILGenerator.Emit(OpCodes.Call, typeof(string).GetMethod("Format", new Type[] { typeof(string), typeof(object) }));
            ILGenerator.Emit(OpCodes.Newobj, typeof(InvalidOperationException).GetConstructor(new Type[] { typeof(string) }));
            ILGenerator.Emit(OpCodes.Throw);

            ILGenerator.MarkLabel(setFieldLabel);

            // Set the detectedPropertySetter field value based on it's index
            ILGenerator.Emit(OpCodes.Ldarg_0);
            ILGenerator.Emit(OpCodes.Ldsfld, typePropertiesStaticField);
            ILGenerator.Emit(OpCodes.Ldc_I4, indexInTypeDefinition);
            ILGenerator.Emit(OpCodes.Ldelem_Ref);
            ILGenerator.Emit(OpCodes.Stfld, detectedPropertySetterInstanceField);

            // Return
            ILGenerator.Emit(OpCodes.Ret);

            TypeBuilder.DefineMethodOverride(methodBuilder, setMethod);
        }
    }
}
