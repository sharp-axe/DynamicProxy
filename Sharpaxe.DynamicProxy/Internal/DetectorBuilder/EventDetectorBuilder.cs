using Sharpaxe.DynamicProxy.Internal.Detector;
using System;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace Sharpaxe.DynamicProxy.Internal.DetectorBuilder
{
    internal class EventDetectorBuilder : CommonDetectorBuilder
    {
        private FieldInfo typeEventsStaticField;
        private FieldInfo detectedEventInstanceField;

        public EventDetectorBuilder(Type targetType, ModuleBuilder moduleBuilder) 
            : base(targetType, moduleBuilder)
        {
        }

        protected override Type DetectorInterfaceType => typeof(IEventDetector);
        protected override string NotSupportedMemberExceptionMessage => "Only an event add method can be invoked on this instance";

        protected override void DefineCustomStaticFields()
        {
            typeEventsStaticField = TypeBuilder.DefineField("typeEvents", typeof(EventInfo[]), FieldAttributes.Private | FieldAttributes.Static);
        }

        protected override void SetCustomStaticFields(Type detectorType)
        {
            detectorType.GetField(typeEventsStaticField.Name, BindingFlags.Static | BindingFlags.NonPublic).SetValue(null, EventsInfo);
        }

        protected override void DefineCustomInstanceFields()
        {
            detectedEventInstanceField = TypeBuilder.DefineField("detectedEvent", typeof(EventInfo), FieldAttributes.Private);
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
            ILGenerator.Emit(OpCodes.Ldfld, detectedEventInstanceField);
            ILGenerator.Emit(OpCodes.Brtrue_S, returnLabel);

            // Throw an invalid operation exception if detectedPropertyGetter field value is null
            ILGenerator.Emit(OpCodes.Ldstr, "No event has been detected");
            ILGenerator.Emit(OpCodes.Newobj, typeof(InvalidOperationException).GetConstructor(new Type[] { typeof(string) }));
            ILGenerator.Emit(OpCodes.Throw);

            // Return the value of the detectedPropertyGetter field
            ILGenerator.MarkLabel(returnLabel);
            ILGenerator.Emit(OpCodes.Ldarg_0);
            ILGenerator.Emit(OpCodes.Ldfld, detectedEventInstanceField);
            ILGenerator.Emit(OpCodes.Ret);

            TypeBuilder.DefineMethodOverride(methodBuilder, methodInfo);
        }

        protected override void DefineTargetTypeEventAddMethod(MethodInfo addMethod, int indexInTypeDefinition)
        {
            var methodBuilder =
                TypeBuilder.DefineMethod(
                    addMethod.Name,
                    MethodAttributes.Public | MethodAttributes.Virtual,
                    addMethod.ReturnType,
                    addMethod.GetParameters().Select(p => p.ParameterType).ToArray());

            var ILGenerator = methodBuilder.GetILGenerator();

            var setFieldLabel = ILGenerator.DefineLabel();

            // Go to 'set field' label if the detectedPropertyGetter field value is null
            ILGenerator.Emit(OpCodes.Ldarg_0);
            ILGenerator.Emit(OpCodes.Ldfld, detectedEventInstanceField);
            ILGenerator.Emit(OpCodes.Brfalse_S, setFieldLabel);

            // Throw an invalid operation exception if the detectedPropertyGetter field is NOT null
            ILGenerator.Emit(OpCodes.Ldstr, "The following event has been already detected: {0}");
            ILGenerator.Emit(OpCodes.Ldarg_0);
            ILGenerator.Emit(OpCodes.Ldfld, detectedEventInstanceField);
            ILGenerator.Emit(OpCodes.Callvirt, typeof(MemberInfo).GetProperty("Name").GetGetMethod());
            ILGenerator.Emit(OpCodes.Call, typeof(string).GetMethod("Format", new Type[] { typeof(string), typeof(object) }));
            ILGenerator.Emit(OpCodes.Newobj, typeof(InvalidOperationException).GetConstructor(new Type[] { typeof(string) }));
            ILGenerator.Emit(OpCodes.Throw);

            ILGenerator.MarkLabel(setFieldLabel);

            // Set the detectedPropertyGetter field value based on it's index
            ILGenerator.Emit(OpCodes.Ldarg_0);
            ILGenerator.Emit(OpCodes.Ldsfld, typeEventsStaticField);
            ILGenerator.Emit(OpCodes.Ldc_I4, indexInTypeDefinition);
            ILGenerator.Emit(OpCodes.Ldelem_Ref);
            ILGenerator.Emit(OpCodes.Stfld, detectedEventInstanceField);

            // Return the default value
            ILGenerator.Emit(OpCodes.Ret);

            TypeBuilder.DefineMethodOverride(methodBuilder, addMethod);
        }
    }
}
