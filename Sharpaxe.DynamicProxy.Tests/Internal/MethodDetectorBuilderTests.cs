using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sharpaxe.DynamicProxy.Internal.Detector;
using Sharpaxe.DynamicProxy.Internal.DetectorBuilder;
using Sharpaxe.DynamicProxy.Tests.TestHelper;
using System;
using System.Collections.Concurrent;

namespace Sharpaxe.DynamicProxy.Tests.Internal
{
    [TestClass]
    public class MethodDetectorBuilderTests
    {

        #region IMethodInterface

        [TestMethod]
        public void CreateDetectorType_MethodInterface_ReturnsNotNull()
        {
            Assert.IsNotNull(GetMethodDetectorType(typeof(IMethod)));
        }

        [TestMethod]
        public void CreateDetectorTypeInstance_MethodInterface_ThrowsNoExceptions()
        {
            var type = GetMethodDetectorType(typeof(IMethod));
            Activator.CreateInstance(type);
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void CallAction_MethodInterfaceDetectorTypeInstance_ThrowsExpectedException()
        {
            try
            {
                var type = GetMethodDetectorType(typeof(IMethod));
                var instance = Activator.CreateInstance(type);
                var methodInstacne = (IMethod)instance;
                methodInstacne.Action();
            }
            catch (NotSupportedException ex)
            {
                Assert.IsTrue(ex.Message.Contains("No members"));
                throw;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void CallActionWithValueArgument_MethodInterfaceDetectorTypeInstance_ThrowsExpectedException()
        {
            try
            {
                var type = GetMethodDetectorType(typeof(IMethod));
                var instance = Activator.CreateInstance(type);
                var methodInstacne = (IMethod)instance;
                methodInstacne.ActionWithValueArgument(default);
            }
            catch (NotSupportedException ex)
            {
                Assert.IsTrue(ex.Message.Contains("No members"));
                throw;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void CallActionWithReferenceArgument_MethodInterfaceDetectorTypeInstance_ThrowsExpectedException()
        {
            try
            {
                var type = GetMethodDetectorType(typeof(IMethod));
                var instance = Activator.CreateInstance(type);
                var methodInstacne = (IMethod)instance;
                methodInstacne.ActionWithReferenceArgument(default);
            }
            catch (NotSupportedException ex)
            {
                Assert.IsTrue(ex.Message.Contains("No members"));
                throw;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void CallFunctionWithValueReturnType_MethodInterfaceDetectorTypeInstance_ThrowsExpectedException()
        {
            try
            {
                var type = GetMethodDetectorType(typeof(IMethod));
                var instance = Activator.CreateInstance(type);
                var methodInstacne = (IMethod)instance;
                methodInstacne.FunctionWithValueReturnType();
            }
            catch (NotSupportedException ex)
            {
                Assert.IsTrue(ex.Message.Contains("No members"));
                throw;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void CallFunctionWithValueArgumentAndValueReturnType_MethodInterfaceDetectorTypeInstance_ThrowsExpectedException()
        {
            try
            {
                var type = GetMethodDetectorType(typeof(IMethod));
                var instance = Activator.CreateInstance(type);
                var methodInstacne = (IMethod)instance;
                methodInstacne.FunctionWithValueArgumentAndValueReturnType(default);
            }
            catch (NotSupportedException ex)
            {
                Assert.IsTrue(ex.Message.Contains("No members"));
                throw;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void CallFunctionWithReferenceArgumentAndValueReturnType_MethodInterfaceDetectorTypeInstance_ThrowsExpectedException()
        {
            try
            {
                var type = GetMethodDetectorType(typeof(IMethod));
                var instance = Activator.CreateInstance(type);
                var methodInstacne = (IMethod)instance;
                methodInstacne.FunctionWithReferenceArgumentAndValueReturnType(default);
            }
            catch (NotSupportedException ex)
            {
                Assert.IsTrue(ex.Message.Contains("No members"));
                throw;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void CallFunctionWithTupleReturnType_MethodInterfaceDetectorTypeInstance_ThrowsExpectedException()
        {
            try
            {
                var type = GetMethodDetectorType(typeof(IMethod));
                var instance = Activator.CreateInstance(type);
                var methodInstacne = (IMethod)instance;
                methodInstacne.FunctionWithTupleReturnType();
            }
            catch (NotSupportedException ex)
            {
                Assert.IsTrue(ex.Message.Contains("No members"));
                throw;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void CallFunctionWithValueArgumentAndTupleReturnType_MethodInterfaceDetectorTypeInstance_ThrowsExpectedException()
        {
            try
            {
                var type = GetMethodDetectorType(typeof(IMethod));
                var instance = Activator.CreateInstance(type);
                var methodInstacne = (IMethod)instance;
                methodInstacne.FunctionWithValueArgumentAndTupleReturnType(default);
            }
            catch (NotSupportedException ex)
            {
                Assert.IsTrue(ex.Message.Contains("No members"));
                throw;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void CallFunctionWithReferenceArgumentAndTupleReturnType_MethodInterfaceDetectorTypeInstance_ThrowsExpectedException()
        {
            try
            {
                var type = GetMethodDetectorType(typeof(IMethod));
                var instance = Activator.CreateInstance(type);
                var methodInstacne = (IMethod)instance;
                methodInstacne.FunctionWithReferenceArgumentAndTupleReturnType(default);
            }
            catch (NotSupportedException ex)
            {
                Assert.IsTrue(ex.Message.Contains("No members"));
                throw;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void DetectFunction_ConsoleWriteLine_ThrowsExpectedException()
        {
            try
            {
                var type = GetMethodDetectorType(typeof(IMethod));
                var instance = Activator.CreateInstance(type);
                var detectorInstance = (IMethodDetector)instance;
                detectorInstance.GetDetectedMethod((Action)Console.WriteLine);
            }
            catch (ArgumentException ex)
            {
                Assert.IsTrue(ex.Message.Contains("method pointer"));
                Assert.IsTrue(ex.Message.Contains("target interface"));
                throw;
            }
        }

        [TestMethod]
        public void DetectFunction_MethodInterfaceDetectorTypeInstance_ReturnsExpectedMethod()
        {
            var type = GetMethodDetectorType(typeof(IMethod));
            var instance = Activator.CreateInstance(type);
            var methodInstance = (IMethod)instance;
            var detectorInstance = (IMethodDetector)instance;
            Assert.AreEqual(typeof(IMethod).GetMethod("Action"), detectorInstance.GetDetectedMethod((Action)methodInstance.Action));
        }

        [TestMethod]
        public void DetectActionWithValueArgument_MethodInterfaceDetectorTypeInstance_ReturnsExpectedMethod()
        {
            var type = GetMethodDetectorType(typeof(IMethod));
            var instance = Activator.CreateInstance(type);
            var methodInstance = (IMethod)instance;
            var detectorInstance = (IMethodDetector)instance;
            Assert.AreEqual(typeof(IMethod).GetMethod("ActionWithValueArgument"), detectorInstance.GetDetectedMethod((Action<int>)methodInstance.ActionWithValueArgument));
        }

        [TestMethod]
        public void DetectActionWithReferenceArgument_MethodInterfaceDetectorTypeInstance_ReturnsExpectedMethod()
        {
            var type = GetMethodDetectorType(typeof(IMethod));
            var instance = Activator.CreateInstance(type);
            var methodInstance = (IMethod)instance;
            var detectorInstance = (IMethodDetector)instance;
            Assert.AreEqual(typeof(IMethod).GetMethod("ActionWithReferenceArgument"), detectorInstance.GetDetectedMethod((Action<object>)methodInstance.ActionWithReferenceArgument));
        }

        [TestMethod]
        public void DetectFunctionWithValueReturnType_MethodInterfaceDetectorTypeInstance_ReturnsExpectedMethod()
        {
            var type = GetMethodDetectorType(typeof(IMethod));
            var instance = Activator.CreateInstance(type);
            var methodInstance = (IMethod)instance;
            var detectorInstance = (IMethodDetector)instance;
            Assert.AreEqual(typeof(IMethod).GetMethod("FunctionWithValueReturnType"), detectorInstance.GetDetectedMethod((Func<int>)methodInstance.FunctionWithValueReturnType));
        }

        [TestMethod]
        public void DetectFunctionWithValueArgumentAndValueReturnType_MethodInterfaceDetectorTypeInstance_ReturnsExpectedMethod()
        {
            var type = GetMethodDetectorType(typeof(IMethod));
            var instance = Activator.CreateInstance(type);
            var methodInstance = (IMethod)instance;
            var detectorInstance = (IMethodDetector)instance;
            Assert.AreEqual(typeof(IMethod).GetMethod("FunctionWithValueArgumentAndValueReturnType"), detectorInstance.GetDetectedMethod((Func<int, int>)methodInstance.FunctionWithValueArgumentAndValueReturnType));
        }

        [TestMethod]
        public void DetectFunctionWithReferenceArgumentAndValueReturnType_MethodInterfaceDetectorTypeInstance_ReturnsExpectedMethod()
        {
            var type = GetMethodDetectorType(typeof(IMethod));
            var instance = Activator.CreateInstance(type);
            var methodInstance = (IMethod)instance;
            var detectorInstance = (IMethodDetector)instance;
            Assert.AreEqual(typeof(IMethod).GetMethod("FunctionWithReferenceArgumentAndValueReturnType"), detectorInstance.GetDetectedMethod((Func<object, int>)methodInstance.FunctionWithReferenceArgumentAndValueReturnType));
        }   

        [TestMethod]
        public void DetectFunctionWithReferenceReturnType_MethodInterfaceDetectorTypeInstance_ReturnsExpectedMethod()
        {
            var type = GetMethodDetectorType(typeof(IMethod));
            var instance = Activator.CreateInstance(type);
            var methodInstance = (IMethod)instance;
            var detectorInstance = (IMethodDetector)instance;
            Assert.AreEqual(typeof(IMethod).GetMethod("FunctionWithReferenceReturnType"), detectorInstance.GetDetectedMethod((Func<object>)methodInstance.FunctionWithReferenceReturnType));
        }

        [TestMethod]
        public void DetectFunctionWithReferenceArgumentAndReferenceReturnType_MethodInterfaceDetectorTypeInstance_ReturnsExpectedMethod()
        {
            var type = GetMethodDetectorType(typeof(IMethod));
            var instance = Activator.CreateInstance(type);
            var methodInstance = (IMethod)instance;
            var detectorInstance = (IMethodDetector)instance;
            Assert.AreEqual(typeof(IMethod).GetMethod("FunctionWithReferenceArgumentAndReferenceReturnType"), detectorInstance.GetDetectedMethod((Func<object, object>)methodInstance.FunctionWithReferenceArgumentAndReferenceReturnType));
        }

        [TestMethod]
        public void DetectFunctionWithTupleReturnType_MethodInterfaceDetectorTypeInstance_ReturnsExpectedMethod()
        {
            var type = GetMethodDetectorType(typeof(IMethod));
            var instance = Activator.CreateInstance(type);
            var methodInstance = (IMethod)instance;
            var detectorInstance = (IMethodDetector)instance;
            Assert.AreEqual(typeof(IMethod).GetMethod("FunctionWithTupleReturnType"), detectorInstance.GetDetectedMethod((Func<ValueTuple<int, object>>)methodInstance.FunctionWithTupleReturnType));
        }

        [TestMethod]
        public void DetectFunctionWithValueArgumentAndTupleReturnType_MethodInterfaceDetectorTypeInstance_ReturnsExpectedMethod()
        {
            var type = GetMethodDetectorType(typeof(IMethod));
            var instance = Activator.CreateInstance(type);
            var methodInstance = (IMethod)instance;
            var detectorInstance = (IMethodDetector)instance;
            Assert.AreEqual(typeof(IMethod).GetMethod("FunctionWithValueArgumentAndTupleReturnType"), detectorInstance.GetDetectedMethod((Func<int, ValueTuple<int, object>>)methodInstance.FunctionWithValueArgumentAndTupleReturnType));
        }

        [TestMethod]
        public void DetectFunctionWithReferenceArgumentAndTupleReturnType_MethodInterfaceDetectorTypeInstance_ReturnsExpectedMethod()
        {
            var type = GetMethodDetectorType(typeof(IMethod));
            var instance = Activator.CreateInstance(type);
            var methodInstance = (IMethod)instance;
            var detectorInstance = (IMethodDetector)instance;
            Assert.AreEqual(typeof(IMethod).GetMethod("FunctionWithReferenceArgumentAndTupleReturnType"), detectorInstance.GetDetectedMethod((Func<object, ValueTuple<int, object>>)methodInstance.FunctionWithReferenceArgumentAndTupleReturnType));
        }

        #endregion IMethodInterface

        #region Other interfaces

        [TestMethod]
        public void Create_IPropretyGetterInterface_IsNotNull()
        {
            Assert.IsNotNull(GetMethodDetectorType(typeof(IPropertyGetter)));
        }

        [TestMethod]
        public void Create_IPropretySetterInterface_IsNotNull()
        {
            Assert.IsNotNull(GetMethodDetectorType(typeof(IPropertySetter)));
        }

        [TestMethod]
        public void Create_IEvent_IsNotNull()
        {
            Assert.IsNotNull(GetMethodDetectorType(typeof(IEvent)));
        }

        [TestMethod]
        public void Create_IInteface_IsNotNull()
        {
            Assert.IsNotNull(GetMethodDetectorType(typeof(IInteface)));
        }

        [TestMethod]
        public void CreateInstance_IPropertyGetterInterface_ThrowsNoException()
        {
            var type = GetMethodDetectorType(typeof(IPropertyGetter));
            Activator.CreateInstance(type);
        }

        [TestMethod]
        public void CreateInstance_IPropertySetterInterface_ThrowsNoException()
        {
            var type = GetMethodDetectorType(typeof(IPropertySetter));
            Activator.CreateInstance(type);
        }

        [TestMethod]
        public void CreateInstance_IEventInterface_ThrowsNoException()
        {
            var type = GetMethodDetectorType(typeof(IEvent));
            Activator.CreateInstance(type);
        }

        [TestMethod]
        public void CreateInstance_IInterface_ThrowsNoException()
        {
            var type = GetMethodDetectorType(typeof(IInteface));
            Activator.CreateInstance(type);
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void CallGetter_IPropertyGetterInterface_ThrowsExpectedException()
        {
            try
            {
                var type = GetMethodDetectorType(typeof(IPropertyGetter));
                var instance = (IPropertyGetter)Activator.CreateInstance(type);
                var boolean = instance.Boolean;
            }
            catch (NotSupportedException ex)
            {
                Assert.IsTrue(ex.Message.Contains("No members"));
                throw;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void CallSetter_IPropertySetterInterface_ThrowsExpectedException()
        {
            try
            {
                var type = GetMethodDetectorType(typeof(IPropertySetter));
                var instance = (IPropertySetter)Activator.CreateInstance(type);
                instance.Boolean = default;
            }
            catch (NotSupportedException ex)
            {
                Assert.IsTrue(ex.Message.Contains("No members"));
                throw;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void SubscriveEvent_IEvent_ThrowsExpectedException()
        {
            try
            {
                var type = GetMethodDetectorType(typeof(IEvent));
                var instance = (IEvent)Activator.CreateInstance(type);
                instance.EventEmptyArgs += (o, a) => { };
            }
            catch (NotSupportedException ex)
            {
                Assert.IsTrue(ex.Message.Contains("No members"));
                throw;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void UnsubscriveEvent_IEvent_ThrowsExpectedException()
        {
            try
            {
                var type = GetMethodDetectorType(typeof(IEvent));
                var instance = (IEvent)Activator.CreateInstance(type);
                instance.EventEmptyArgs -= (o, a) => { };
            }
            catch (NotSupportedException ex)
            {
                Assert.IsTrue(ex.Message.Contains("No members"));
                throw;
            }
        }

        #endregion Other interfaces

        public static Type GetMethodDetectorType(Type targetType)
        {
            return targetTypeToMethodDetectorTypeMap.GetOrAdd(targetType, t => new MethodDetectorBuilder(t, Static.ModuleBinder.Value).CreateDetectorType());
        }

        private static ConcurrentDictionary<Type, Type> targetTypeToMethodDetectorTypeMap = new ConcurrentDictionary<Type, Type>();
    }
}
