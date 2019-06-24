using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Sharpaxe.DynamicProxy.Internal.Proxy;
using Sharpaxe.DynamicProxy.Tests.TestHelper;
using Sharpaxe.DynamicProxy.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Collections.Concurrent;

namespace Sharpaxe.DynamicProxy.Tests.Internal.Proxy.Builder
{
    [TestClass]
    public class ProxyBuilderTests
    {
        [TestMethod]
        public void CreateType_IPropertyGetter_HasExpectedFields()
        {
            var type = CreateTargetType(typeof(IPropertyGetter));

            var expectedFields =
                ExpectedIPropertyGetterProxyFields
                .Concat(ExpectedIPropertyGetterDecoratorsFields)
                .Concat(new KeyValuePair<string, Type>("target", typeof(IPropertyGetter)))
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

            AssertTypeHasPassedPrivateInstanceFields(type, expectedFields);
        }

        [TestMethod]
        public void CreateType_IPropertGetter_HasExpectedMethods()
        {
            var type = CreateTargetType(typeof(IPropertyGetter));

            var expectedMethods =
                ExpectedIPropertyGetterFunctions
                .Concat(ExpectedObjectFunctions)
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

            AssertTypeHasPassedPrivateInstanceMethods(type, expectedMethods);
        }

        [TestMethod]
        public void CreateType_IPropertySetter_HasExpectedFields()
        {
            var type = CreateTargetType(typeof(IPropertySetter));

            var expectedFields =
                ExpectedIPropertySetterProxyFields
                .Concat(ExpectedIPropertySetterDecoratorsFields)
                .Concat(new KeyValuePair<string, Type>("target", typeof(IPropertySetter)))
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

            AssertTypeHasPassedPrivateInstanceFields(type, expectedFields);
        }

        [TestMethod]
        public void CreateType_IPropertSetter_HasExpectedMethods()
        {
            var type = CreateTargetType(typeof(IPropertySetter));

            var expectedMethods =
                ExpectedIPropertySetterFunctions
                .Concat(ExpectedObjectFunctions)
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

            AssertTypeHasPassedPrivateInstanceMethods(type, expectedMethods);
        }

        [TestMethod]
        public void CreateType_IEvent_HasExpectedFields()
        {
            var type = CreateTargetType(typeof(IEvent));

            var expectedFields =
                ExpectedIEventProxyFields
                .Concat(ExpectedIEventDecoratorsFields)
                .Concat(new KeyValuePair<string, Type>("target", typeof(IEvent)))
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

            AssertTypeHasPassedPrivateInstanceFields(type, expectedFields);
        }

        [TestMethod]
        public void CreateType_IEvent_HasExpectedMethods()
        {
            var type = CreateTargetType(typeof(IEvent));

            var expectedFunctions =
                ExpectedIEventMethods
                .Concat(ExpectedObjectFunctions)
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

            AssertTypeHasPassedPrivateInstanceMethods(type, expectedFunctions);
        }

        [TestMethod]
        public void CreateType_IMethod_HasExpectedFields()
        {
            var type = CreateTargetType(typeof(IMethod));

            var expectedFields =
                ExpectedIMethodProxyFields
                .Concat(ExpectedIMethodDecoratorsFields)
                .Concat(new KeyValuePair<string, Type>("target", typeof(IMethod)))
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

            AssertTypeHasPassedPrivateInstanceFields(type, expectedFields);
        }

        [TestMethod]
        public void CreateType_IMethod_HasExpectedFunctions()
        {
            var type = CreateTargetType(typeof(IMethod));

            var expectedFunctions =
                ExpectedIMethodFunctions
                .Concat(ExpectedObjectFunctions)
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

            AssertTypeHasPassedPrivateInstanceMethods(type, expectedFunctions);
        }

        [TestMethod]
        public void CreateType_IInterface_HasExpectedFields()
        {
            var type = CreateTargetType(typeof(IInterface));

            var expectedFields =
                ExpectedIEventProxyFields
                .Concat(ExpectedIEventDecoratorsFields)
                .Concat(ExpectedIMethodProxyFields)
                .Concat(ExpectedIMethodDecoratorsFields)
                .Concat(ExpectedIPropertyGetterProxyFields)
                .Concat(ExpectedIPropertyGetterDecoratorsFields)
                .Concat(ExpectedIPropertySetterProxyFields)
                .Concat(ExpectedIPropertySetterDecoratorsFields)
                .Concat(new KeyValuePair<string, Type>("target", typeof(IInterface)))
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

            AssertTypeHasPassedPrivateInstanceFields(type, expectedFields);
        }

        [TestMethod]
        public void CreateType_IInterface_HasExpectedMethods()
        {
            var type = CreateTargetType(typeof(IInterface));

            var expectedFunctions =
                ExpectedIEventMethods
                .Concat(ExpectedIMethodFunctions)
                .Concat(ExpectedIPropertyGetterFunctions)
                .Concat(ExpectedIPropertySetterFunctions)
                .Concat(ExpectedObjectFunctions)
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

            AssertTypeHasPassedPrivateInstanceMethods(type, expectedFunctions);
        }

        [TestMethod]
        public void CreateInstance_IPropertyGetterProxy_ThrowsNoException()
        {
            var type = CreateTargetType(typeof(IPropertyGetter));
            var mock = new Mock<IPropertyGetter>();

            Activator.CreateInstance(type, mock.Object);
        }

        [TestMethod]
        public void CreateInstance_IPropertyGetterProxy_FieldsAreNotNull()
        {
            AssertInstanceHasNotNullFieldsValues(CreateTargetType(typeof(IPropertyGetter)), CreateProxyInstance<IPropertyGetter>(null), ExpectedIPropertyGetterDecoratorsFields.Keys);
        }

        [TestMethod]
        public void CreateInstance_IPropertySetterProxy_ThrowsNoException()
        {
            var type = CreateTargetType(typeof(IPropertySetter));
            var mock = new Mock<IPropertySetter>();

            Activator.CreateInstance(type, mock.Object);
        }

        [TestMethod]
        public void CreateInstance_IPropertySetterProxy_FieldsAreNotNull()
        {
            AssertInstanceHasNotNullFieldsValues(CreateTargetType(typeof(IPropertySetter)), CreateProxyInstance<IPropertySetter>(null), ExpectedIPropertySetterDecoratorsFields.Keys);
        }

        [TestMethod]
        public void CreateInstance_IEventProxy_ThrowsNoException()
        {
            var type = CreateTargetType(typeof(IEvent));
            var mock = new Mock<IEvent>();

            Activator.CreateInstance(type, mock.Object);
        }

        [TestMethod]
        public void CreateInstance_IEventProxy_FieldsAreNotNull()
        {
            AssertInstanceHasNotNullFieldsValues(CreateTargetType(typeof(IEvent)), CreateProxyInstance<IEvent>(null),
                ExpectedIEventDecoratorsFields.Keys.Concat(ExpectedIEventSubscribersFields.Keys));
        }

        [TestMethod]
        public void CreateInstance_IMethodProxy_ThrowsNoException()
        {
            var type = CreateTargetType(typeof(IMethod));
            var mock = new Mock<IMethod>();

            Activator.CreateInstance(type, mock.Object);
        }

        [TestMethod]
        public void CreateInstance_IMethodProxy_FieldsAreNotNull()
        {
            AssertInstanceHasNotNullFieldsValues(CreateTargetType(typeof(IMethod)), CreateProxyInstance<IMethod>(null), ExpectedIMethodDecoratorsFields.Keys);
        }

        [TestMethod]
        public void CreateInstance_IInterfaceProxy_ThrowsNoException()
        {
            var type = CreateTargetType(typeof(IInterface));
            var mock = new Mock<IInterface>();

            Activator.CreateInstance(type, mock.Object);
        }

        [TestMethod]
        public void CreateInstance_IInterfaceProxy_FieldsAreNotNull()
        {
            AssertInstanceHasNotNullFieldsValues(CreateTargetType(typeof(IInterface)), CreateProxyInstance<IInterface>(null),
                ExpectedIMethodDecoratorsFields.Keys
                .Concat(ExpectedIEventDecoratorsFields.Keys)
                .Concat(ExpectedIEventSubscribersFields.Keys)
                .Concat(ExpectedIPropertyGetterDecoratorsFields.Keys)
                .Concat(ExpectedIPropertySetterDecoratorsFields.Keys));
        }

        [TestMethod]
        public void InvokeAction_IMethodProxy_CallsRequestedMethod()
        {
            var mock = new Mock<IMethod>();
            var proxy = CreateProxyInstance(mock.Object);

            proxy.Action();

            mock.Verify(m => m.Action(), Times.Once());
            mock.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void InvokeActionWithValueArgument_IMethodProxy_CallsRequestedMethod()
        {
            var mock = new Mock<IMethod>();
            var proxy = CreateProxyInstance(mock.Object);

            proxy.ActionWithValueArgument(123);

            mock.Verify(m => m.ActionWithValueArgument(123), Times.Once());
            mock.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void InvokeActionWithValueArgumentOverload_IMethodProxy_CallsRequestedMethod()
        {
            var mock = new Mock<IMethod>();
            var proxy = CreateProxyInstance(mock.Object);

            proxy.ActionWithValueArgument(123u);

            mock.Verify(m => m.ActionWithValueArgument(123u), Times.Once());
            mock.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void InvokeActionWithValueReference_IMethodProxy_CallsRequestedMethod()
        {
            var mock = new Mock<IMethod>();
            var proxy = CreateProxyInstance(mock.Object);

            var obj = new object();
            proxy.ActionWithReferenceArgument(obj);

            mock.Verify(m => m.ActionWithReferenceArgument(obj), Times.Once());
            mock.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void InvokeActionWithValueReferenceOverload_IMethodProxy_CallsRequestedMethod()
        {
            var mock = new Mock<IMethod>();
            var proxy = CreateProxyInstance(mock.Object);

            var str = "TestString";
            proxy.ActionWithReferenceArgument(str);

            mock.Verify(m => m.ActionWithReferenceArgument(str), Times.Once());
            mock.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void InvokeFunctionWithValueReturnType_IMethodProxy_CallsRequestedMethod()
        {
            var mock = new Mock<IMethod>();
            var proxy = CreateProxyInstance(mock.Object);

            proxy.FunctionWithValueReturnType();

            mock.Verify(m => m.FunctionWithValueReturnType(), Times.Once());
            mock.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void InvokeFunctionWithValueArgumentAndValueReturnType_IMethodProxy_CallsRequestedMethod()
        {
            var mock = new Mock<IMethod>();
            var proxy = CreateProxyInstance(mock.Object);

            proxy.FunctionWithValueArgumentAndValueReturnType(123);

            mock.Verify(m => m.FunctionWithValueArgumentAndValueReturnType(123), Times.Once());
            mock.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void InvokeFunctionWithValueArgumentAndValueReturnTypeOverload_IMethodProxy_CallsRequestedMethod()
        {
            var mock = new Mock<IMethod>();
            var proxy = CreateProxyInstance(mock.Object);

            proxy.FunctionWithValueArgumentAndValueReturnType(123u);

            mock.Verify(m => m.FunctionWithValueArgumentAndValueReturnType(123u), Times.Once());
            mock.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void InvokeFunctionWithReferenceArgumentAndValueReturnType_IMethodProxy_CallsRequestedMethod()
        {
            var mock = new Mock<IMethod>();
            var proxy = CreateProxyInstance(mock.Object);

            var obj = new object();
            proxy.FunctionWithReferenceArgumentAndValueReturnType(obj);

            mock.Verify(m => m.FunctionWithReferenceArgumentAndValueReturnType(obj), Times.Once());
            mock.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void InvokeFunctionWithReferenceArgumentAndValueReturnTypeOverload_IMethodProxy_CallsRequestedMethod()
        {
            var mock = new Mock<IMethod>();
            var proxy = CreateProxyInstance(mock.Object);

            var str = "TestString";
            proxy.FunctionWithReferenceArgumentAndValueReturnType(str);

            mock.Verify(m => m.FunctionWithReferenceArgumentAndValueReturnType(str), Times.Once());
            mock.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void InvokeFunctionWithReferenceReturnType_IMethodProxy_CallsRequestedMethod()
        {
            var mock = new Mock<IMethod>();
            var proxy = CreateProxyInstance(mock.Object);

            proxy.FunctionWithReferenceReturnType();

            mock.Verify(m => m.FunctionWithReferenceReturnType(), Times.Once());
            mock.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void InvokeFunctionWithValueArgumentAndReferenceReturnType_IMethodProxy_CallsRequestedMethod()
        {
            var mock = new Mock<IMethod>();
            var proxy = CreateProxyInstance(mock.Object);

            proxy.FunctionWithValueArgumentAndReferenceReturnType(123);

            mock.Verify(m => m.FunctionWithValueArgumentAndReferenceReturnType(123), Times.Once());
            mock.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void InvokeFunctionWithValueArgumentAndReferenceReturnTypeOverload_IMethodProxy_CallsRequestedMethod()
        {
            var mock = new Mock<IMethod>();
            var proxy = CreateProxyInstance(mock.Object);

            proxy.FunctionWithValueArgumentAndReferenceReturnType(123u);

            mock.Verify(m => m.FunctionWithValueArgumentAndReferenceReturnType(123u), Times.Once());
            mock.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void InvokeFunctionWithReferenceArgumentAndReferenceReturnType_IMethodProxy_CallsRequestedMethod()
        {
            var mock = new Mock<IMethod>();
            var proxy = CreateProxyInstance(mock.Object);

            var obj = new object();
            proxy.FunctionWithReferenceArgumentAndReferenceReturnType(obj);

            mock.Verify(m => m.FunctionWithReferenceArgumentAndReferenceReturnType(obj), Times.Once());
            mock.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void InvokeFunctionWithReferenceArgumentAndReferenceReturnTypeOverload_IMethodProxy_CallsRequestedMethod()
        {
            var mock = new Mock<IMethod>();
            var proxy = CreateProxyInstance(mock.Object);

            var str = "TestString";
            proxy.FunctionWithReferenceArgumentAndReferenceReturnType(str);

            mock.Verify(m => m.FunctionWithReferenceArgumentAndReferenceReturnType(str), Times.Once());
            mock.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void InvokeFunctionWithTupleReturnType_IMethodProxy_CallsRequestedMethod()
        {
            var mock = new Mock<IMethod>();
            var proxy = CreateProxyInstance(mock.Object);

            proxy.FunctionWithTupleReturnType();

            mock.Verify(m => m.FunctionWithTupleReturnType(), Times.Once());
            mock.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void InvokeFunctionWithValueArgumentAndTupleReturnType_IMethodProxy_CallsRequestedMethod()
        {
            var mock = new Mock<IMethod>();
            var proxy = CreateProxyInstance(mock.Object);

            proxy.FunctionWithValueArgumentAndTupleReturnType(123);

            mock.Verify(m => m.FunctionWithValueArgumentAndTupleReturnType(123), Times.Once());
            mock.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void InvokeFunctionWithValueArgumentAndTupleReturnTypeOverload_IMethodProxy_CallsRequestedMethod()
        {
            var mock = new Mock<IMethod>();
            var proxy = CreateProxyInstance(mock.Object);

            proxy.FunctionWithValueArgumentAndTupleReturnType(123u);

            mock.Verify(m => m.FunctionWithValueArgumentAndTupleReturnType(123u), Times.Once());
            mock.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void InvokeFunctionWithReferenceArgumentAndTupleReturnType_IMethodProxy_CallsRequestedMethod()
        {
            var mock = new Mock<IMethod>();
            var proxy = CreateProxyInstance(mock.Object);

            var obj = new object();
            proxy.FunctionWithReferenceArgumentAndTupleReturnType(obj);

            mock.Verify(m => m.FunctionWithReferenceArgumentAndTupleReturnType(obj), Times.Once());
            mock.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void InvokeFunctionWithReferenceArgumentAndTupleReturnTypeOverload_IMethodProxy_CallsRequestedMethod()
        {
            var mock = new Mock<IMethod>();
            var proxy = CreateProxyInstance(mock.Object);

            var str = "TestString";
            proxy.FunctionWithReferenceArgumentAndTupleReturnType(str);

            mock.Verify(m => m.FunctionWithReferenceArgumentAndTupleReturnType(str), Times.Once());
            mock.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void InvokeAction_IMethodProxyWithBeforeDecorator_CallsRequestedMethodAndDecorator()
        {
            var mock = new Mock<IMethod>();
            var proxy = CreateProxyInstance(mock.Object);
            bool decoratorCalled = false;
            Action beforeDecorator = () => 
            {
                mock.VerifyNoOtherCalls();
                decoratorCalled = true;
            };
            SetBeforeDecorator(proxy, "ActionMethodDecorators0", beforeDecorator);

            proxy.Action();

            Assert.IsTrue(decoratorCalled);
            mock.Verify(m => m.Action(), Times.Once());
            mock.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void InvokeActionWithValueArgument_IMethodProxyWithBeforeDecorator_CallsRequestedMethodAndDecorator()
        {
            var mock = new Mock<IMethod>();
            var proxy = CreateProxyInstance(mock.Object);
            int arg = 123;
            bool decoratorCalled = false;
            Action<int> beforeDecorator = a =>
            {
                Assert.AreEqual(arg, a);
                mock.VerifyNoOtherCalls();
                decoratorCalled = true;
            };
            SetBeforeDecorator(proxy, "ActionWithValueArgumentMethodDecorators0", beforeDecorator);

            proxy.ActionWithValueArgument(arg);

            Assert.IsTrue(decoratorCalled);
            mock.Verify(m => m.ActionWithValueArgument(arg), Times.Once());
            mock.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void InvokeActionWithValueArgumentOverload_IMethodProxyWithBeforeDecorator_CallsRequestedMethodAndDecorator()
        {
            var mock = new Mock<IMethod>();
            var proxy = CreateProxyInstance(mock.Object);
            uint arg = 123u;
            bool decoratorCalled = false;
            Action<uint> beforeDecorator = a =>
            {
                Assert.AreEqual(arg, a);
                mock.VerifyNoOtherCalls();
                decoratorCalled = true;
            };
            SetBeforeDecorator(proxy, "ActionWithValueArgumentMethodDecorators1", beforeDecorator);

            proxy.ActionWithValueArgument(arg);

            Assert.IsTrue(decoratorCalled);
            mock.Verify(m => m.ActionWithValueArgument(arg), Times.Once());
            mock.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void InvokeActionWithReferenceArgument_IMethodProxyWithBeforeDecorator_CallsRequestedMethodAndDecorator()
        {
            var mock = new Mock<IMethod>();
            var proxy = CreateProxyInstance(mock.Object);
            var arg = new object();
            bool decoratorCalled = false;
            Action<object> beforeDecorator = a =>
            {
                Assert.AreEqual(arg, a);
                mock.VerifyNoOtherCalls();
                decoratorCalled = true;
            };
            SetBeforeDecorator(proxy, "ActionWithReferenceArgumentMethodDecorators0", beforeDecorator);

            proxy.ActionWithReferenceArgument(arg);

            Assert.IsTrue(decoratorCalled);
            mock.Verify(m => m.ActionWithReferenceArgument(arg), Times.Once());
            mock.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void InvokeActionWithReferenceArgumentOverload_IMethodProxyWithBeforeDecorator_CallsRequestedMethodAndDecorator()
        {
            var mock = new Mock<IMethod>();
            var proxy = CreateProxyInstance(mock.Object);
            string arg = "TestString";
            bool decoratorCalled = false;
            Action<string> beforeDecorator = a =>
            {
                Assert.AreEqual(arg, a);
                mock.VerifyNoOtherCalls();
                decoratorCalled = true;
            };
            SetBeforeDecorator(proxy, "ActionWithReferenceArgumentMethodDecorators1", beforeDecorator);

            proxy.ActionWithReferenceArgument(arg);

            Assert.IsTrue(decoratorCalled);
            mock.Verify(m => m.ActionWithReferenceArgument(arg), Times.Once());
            mock.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void InvokeFunctionWithValueReturnType_IMethodProxyWithBeforeDecorator_CallsRequestedMethodAndDecorator()
        {
            var mock = new Mock<IMethod>();
            var proxy = CreateProxyInstance(mock.Object);
            bool decoratorCalled = false;
            Action beforeDecorator = () =>
            {
                mock.VerifyNoOtherCalls();
                decoratorCalled = true;
            };
            SetBeforeDecorator<Action, Action<int>>(proxy, "FunctionWithValueReturnTypeMethodDecorators0", beforeDecorator);

            proxy.FunctionWithValueReturnType();

            Assert.IsTrue(decoratorCalled);
            mock.Verify(m => m.FunctionWithValueReturnType(), Times.Once());
            mock.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void InvokeFunctionWithValueArgumentAndValueReturnTypeWithValueArgument_IMethodProxyWithBeforeDecorator_CallsRequestedMethodAndDecorator()
        {
            var mock = new Mock<IMethod>();
            var proxy = CreateProxyInstance(mock.Object);
            int arg = 123;
            bool decoratorCalled = false;
            Action<int> beforeDecorator = a =>
            {
                Assert.AreEqual(arg, a);
                mock.VerifyNoOtherCalls();
                decoratorCalled = true;
            };
            SetBeforeDecorator<Action<int>, Action<int, int>>(proxy, "FunctionWithValueArgumentAndValueReturnTypeMethodDecorators0", beforeDecorator);

            proxy.FunctionWithValueArgumentAndValueReturnType(arg);

            Assert.IsTrue(decoratorCalled);
            mock.Verify(m => m.FunctionWithValueArgumentAndValueReturnType(arg), Times.Once());
            mock.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void InvokeFunctionWithValueArgumentAndValueReturnTypeOverload_IMethodProxyWithBeforeDecorator_CallsRequestedMethodAndDecorator()
        {
            var mock = new Mock<IMethod>();
            var proxy = CreateProxyInstance(mock.Object);
            uint arg = 123u;
            bool decoratorCalled = false;
            Action<uint> beforeDecorator = a =>
            {
                Assert.AreEqual(arg, a);
                mock.VerifyNoOtherCalls();
                decoratorCalled = true;
            };
            SetBeforeDecorator<Action<uint>, Action<uint, int>>(proxy, "FunctionWithValueArgumentAndValueReturnTypeMethodDecorators1", beforeDecorator);

            proxy.FunctionWithValueArgumentAndValueReturnType(arg);

            Assert.IsTrue(decoratorCalled);
            mock.Verify(m => m.FunctionWithValueArgumentAndValueReturnType(arg), Times.Once());
            mock.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void InvokeFunctionWithReferenceArgumentAndValueReturnType_IMethodProxyWithBeforeDecorator_CallsRequestedMethodAndDecorator()
        {
            var mock = new Mock<IMethod>();
            var proxy = CreateProxyInstance(mock.Object);
            var arg = new object();
            bool decoratorCalled = false;
            Action<object> beforeDecorator = a =>
            {
                Assert.AreEqual(arg, a);
                mock.VerifyNoOtherCalls();
                decoratorCalled = true;
            };
            SetBeforeDecorator<Action<object>, Action<object, int>>(proxy, "FunctionWithReferenceArgumentAndValueReturnTypeMethodDecorators0", beforeDecorator);

            proxy.FunctionWithReferenceArgumentAndValueReturnType(arg);

            Assert.IsTrue(decoratorCalled);
            mock.Verify(m => m.FunctionWithReferenceArgumentAndValueReturnType(arg), Times.Once());
            mock.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void InvokeFunctionWithReferenceArgumentAndValueReturnTypeOverload_IMethodProxyWithBeforeDecorator_CallsRequestedMethodAndDecorator()
        {
            var mock = new Mock<IMethod>();
            var proxy = CreateProxyInstance(mock.Object);
            string arg = "TestString";
            bool decoratorCalled = false;
            Action<string> beforeDecorator = a =>
            {
                Assert.AreEqual(arg, a);
                mock.VerifyNoOtherCalls();
                decoratorCalled = true;
            };
            SetBeforeDecorator<Action<string>, Action<string, int>>(proxy, "FunctionWithReferenceArgumentAndValueReturnTypeMethodDecorators1", beforeDecorator);

            proxy.FunctionWithReferenceArgumentAndValueReturnType(arg);

            Assert.IsTrue(decoratorCalled);
            mock.Verify(m => m.FunctionWithReferenceArgumentAndValueReturnType(arg), Times.Once());
            mock.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void InvokeFunctionWithReferenceReturnType_IMethodProxyWithBeforeDecorator_CallsRequestedMethodAndDecorator()
        {
            var mock = new Mock<IMethod>();
            var proxy = CreateProxyInstance(mock.Object);
            bool decoratorCalled = false;
            Action beforeDecorator = () =>
            {
                mock.VerifyNoOtherCalls();
                decoratorCalled = true;
            };
            SetBeforeDecorator<Action, Action<object>>(proxy, "FunctionWithReferenceReturnTypeMethodDecorators0", beforeDecorator);

            proxy.FunctionWithReferenceReturnType();

            Assert.IsTrue(decoratorCalled);
            mock.Verify(m => m.FunctionWithReferenceReturnType(), Times.Once());
            mock.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void InvokeFunctionWithValueArgumentAndReferenceReturnTypeWithValueArgument_IMethodProxyWithBeforeDecorator_CallsRequestedMethodAndDecorator()
        {
            var mock = new Mock<IMethod>();
            var proxy = CreateProxyInstance(mock.Object);
            int arg = 123;
            bool decoratorCalled = false;
            Action<int> beforeDecorator = a =>
            {
                Assert.AreEqual(arg, a);
                mock.VerifyNoOtherCalls();
                decoratorCalled = true;
            };
            SetBeforeDecorator<Action<int>, Action<int, object>>(proxy, "FunctionWithValueArgumentAndReferenceReturnTypeMethodDecorators0", beforeDecorator);

            proxy.FunctionWithValueArgumentAndReferenceReturnType(arg);

            Assert.IsTrue(decoratorCalled);
            mock.Verify(m => m.FunctionWithValueArgumentAndReferenceReturnType(arg), Times.Once());
            mock.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void InvokeFunctionWithValueArgumentAndReferenceReturnTypeOverload_IMethodProxyWithBeforeDecorator_CallsRequestedMethodAndDecorator()
        {
            var mock = new Mock<IMethod>();
            var proxy = CreateProxyInstance(mock.Object);
            uint arg = 123u;
            bool decoratorCalled = false;
            Action<uint> beforeDecorator = a =>
            {
                Assert.AreEqual(arg, a);
                mock.VerifyNoOtherCalls();
                decoratorCalled = true;
            };
            SetBeforeDecorator<Action<uint>, Action<uint, object>>(proxy, "FunctionWithValueArgumentAndReferenceReturnTypeMethodDecorators1", beforeDecorator);

            proxy.FunctionWithValueArgumentAndReferenceReturnType(arg);

            Assert.IsTrue(decoratorCalled);
            mock.Verify(m => m.FunctionWithValueArgumentAndReferenceReturnType(arg), Times.Once());
            mock.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void InvokeFunctionWithReferenceArgumentAndReferenceReturnType_IMethodProxyWithBeforeDecorator_CallsRequestedMethodAndDecorator()
        {
            var mock = new Mock<IMethod>();
            var proxy = CreateProxyInstance(mock.Object);
            var arg = new object();
            bool decoratorCalled = false;
            Action<object> beforeDecorator = a =>
            {
                Assert.AreEqual(arg, a);
                mock.VerifyNoOtherCalls();
                decoratorCalled = true;
            };
            SetBeforeDecorator<Action<object>, Action<object, object>>(proxy, "FunctionWithReferenceArgumentAndReferenceReturnTypeMethodDecorators0", beforeDecorator);

            proxy.FunctionWithReferenceArgumentAndReferenceReturnType(arg);

            Assert.IsTrue(decoratorCalled);
            mock.Verify(m => m.FunctionWithReferenceArgumentAndReferenceReturnType(arg), Times.Once());
            mock.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void InvokeFunctionWithReferenceArgumentAndReferenceReturnTypeOverload_IMethodProxyWithBeforeDecorator_CallsRequestedMethodAndDecorator()
        {
            var mock = new Mock<IMethod>();
            var proxy = CreateProxyInstance(mock.Object);
            string arg = "TestString";
            bool decoratorCalled = false;
            Action<string> beforeDecorator = a =>
            {
                Assert.AreEqual(arg, a);
                mock.VerifyNoOtherCalls();
                decoratorCalled = true;
            };
            SetBeforeDecorator<Action<string>, Action<string, object>>(proxy, "FunctionWithReferenceArgumentAndReferenceReturnTypeMethodDecorators1", beforeDecorator);

            proxy.FunctionWithReferenceArgumentAndReferenceReturnType(arg);

            Assert.IsTrue(decoratorCalled);
            mock.Verify(m => m.FunctionWithReferenceArgumentAndReferenceReturnType(arg), Times.Once());
            mock.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void InvokeFunctionWithTupleReturnType_IMethodProxyWithBeforeDecorator_CallsRequestedMethodAndDecorator()
        {
            var mock = new Mock<IMethod>();
            var proxy = CreateProxyInstance(mock.Object);
            bool decoratorCalled = false;
            Action beforeDecorator = () =>
            {
                mock.VerifyNoOtherCalls();
                decoratorCalled = true;
            };
            SetBeforeDecorator<Action, Action<ValueTuple<int, object>>>(proxy, "FunctionWithTupleReturnTypeMethodDecorators0", beforeDecorator);

            proxy.FunctionWithTupleReturnType();

            Assert.IsTrue(decoratorCalled);
            mock.Verify(m => m.FunctionWithTupleReturnType(), Times.Once());
            mock.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void InvokeFunctionWithValueArgumentAndTupleReturnTypeWithValueArgument_IMethodProxyWithBeforeDecorator_CallsRequestedMethodAndDecorator()
        {
            var mock = new Mock<IMethod>();
            var proxy = CreateProxyInstance(mock.Object);
            int arg = 123;
            bool decoratorCalled = false;
            Action<int> beforeDecorator = a =>
            {
                Assert.AreEqual(arg, a);
                mock.VerifyNoOtherCalls();
                decoratorCalled = true;
            };
            SetBeforeDecorator<Action<int>, Action<int, ValueTuple<int, object>>>(proxy, "FunctionWithValueArgumentAndTupleReturnTypeMethodDecorators0", beforeDecorator);

            proxy.FunctionWithValueArgumentAndTupleReturnType(arg);

            Assert.IsTrue(decoratorCalled);
            mock.Verify(m => m.FunctionWithValueArgumentAndTupleReturnType(arg), Times.Once());
            mock.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void InvokeFunctionWithValueArgumentAndTupleReturnTypeOverload_IMethodProxyWithBeforeDecorator_CallsRequestedMethodAndDecorator()
        {
            var mock = new Mock<IMethod>();
            var proxy = CreateProxyInstance(mock.Object);
            uint arg = 123u;
            bool decoratorCalled = false;
            Action<uint> beforeDecorator = a =>
            {
                Assert.AreEqual(arg, a);
                mock.VerifyNoOtherCalls();
                decoratorCalled = true;
            };
            SetBeforeDecorator<Action<uint>, Action<uint, ValueTuple<int, object>>>(proxy, "FunctionWithValueArgumentAndTupleReturnTypeMethodDecorators1", beforeDecorator);

            proxy.FunctionWithValueArgumentAndTupleReturnType(arg);

            Assert.IsTrue(decoratorCalled);
            mock.Verify(m => m.FunctionWithValueArgumentAndTupleReturnType(arg), Times.Once());
            mock.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void InvokeFunctionWithReferenceArgumentAndTupleReturnType_IMethodProxyWithBeforeDecorator_CallsRequestedMethodAndDecorator()
        {
            var mock = new Mock<IMethod>();
            var proxy = CreateProxyInstance(mock.Object);
            var arg = new object();
            bool decoratorCalled = false;
            Action<object> beforeDecorator = a =>
            {
                Assert.AreEqual(arg, a);
                mock.VerifyNoOtherCalls();
                decoratorCalled = true;
            };
            SetBeforeDecorator<Action<object>, Action<object, ValueTuple<int, object>>>(proxy, "FunctionWithReferenceArgumentAndTupleReturnTypeMethodDecorators0", beforeDecorator);

            proxy.FunctionWithReferenceArgumentAndTupleReturnType(arg);

            Assert.IsTrue(decoratorCalled);
            mock.Verify(m => m.FunctionWithReferenceArgumentAndTupleReturnType(arg), Times.Once());
            mock.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void InvokeFunctionWithReferenceArgumentAndTupleReturnTypeOverload_IMethodProxyWithBeforeDecorator_CallsRequestedMethodAndDecorator()
        {
            var mock = new Mock<IMethod>();
            var proxy = CreateProxyInstance(mock.Object);
            string arg = "TestString";
            bool decoratorCalled = false;
            Action<string> beforeDecorator = a =>
            {
                Assert.AreEqual(arg, a);
                mock.VerifyNoOtherCalls();
                decoratorCalled = true;
            };
            SetBeforeDecorator<Action<string>, Action<string, ValueTuple<int, object>>>(proxy, "FunctionWithReferenceArgumentAndTupleReturnTypeMethodDecorators1", beforeDecorator);

            proxy.FunctionWithReferenceArgumentAndTupleReturnType(arg);

            Assert.IsTrue(decoratorCalled);
            mock.Verify(m => m.FunctionWithReferenceArgumentAndTupleReturnType(arg), Times.Once());
            mock.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void InvokeAction_IMethodProxyWithAfterDecorator_CallsRequestedMethodAndDecorator()
        {
            var mock = new Mock<IMethod>();
            var proxy = CreateProxyInstance(mock.Object);
            bool decoratorCalled = false;
            Action afterDecorator = () =>
            {
                mock.Verify(m => m.Action(), Times.Once());
                decoratorCalled = true;
            };
            SetAfterDecorator(proxy, "ActionMethodDecorators0", afterDecorator);

            proxy.Action();

            Assert.IsTrue(decoratorCalled);
            mock.Verify(m => m.Action(), Times.Once());
            mock.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void InvokeActionWithValueArgument_IMethodProxyWithAfterDecorator_CallsRequestedMethodAndDecorator()
        {
            var mock = new Mock<IMethod>();
            var proxy = CreateProxyInstance(mock.Object);
            int arg = 123;
            bool decoratorCalled = false;
            Action<int> afterDecorator = a =>
            {
                Assert.AreEqual(arg, a);
                mock.Verify(m => m.ActionWithValueArgument(arg), Times.Once());
                decoratorCalled = true;
            };
            SetAfterDecorator(proxy, "ActionWithValueArgumentMethodDecorators0", afterDecorator);

            proxy.ActionWithValueArgument(arg);

            Assert.IsTrue(decoratorCalled);
            mock.Verify(m => m.ActionWithValueArgument(arg), Times.Once());
            mock.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void InvokeActionWithValueArgumentOverload_IMethodProxyWithAfterDecorator_CallsRequestedMethodAndDecorator()
        {
            var mock = new Mock<IMethod>();
            var proxy = CreateProxyInstance(mock.Object);
            uint arg = 123u;
            bool decoratorCalled = false;
            Action<uint> afterDecorator = a =>
            {
                Assert.AreEqual(arg, a);
                mock.Verify(m => m.ActionWithValueArgument(arg), Times.Once());
                decoratorCalled = true;
            };
            SetAfterDecorator(proxy, "ActionWithValueArgumentMethodDecorators1", afterDecorator);

            proxy.ActionWithValueArgument(arg);

            Assert.IsTrue(decoratorCalled);
            mock.Verify(m => m.ActionWithValueArgument(arg), Times.Once());
            mock.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void InvokeActionWithReferenceArgument_IMethodProxyWithAfterDecorator_CallsRequestedMethodAndDecorator()
        {
            var mock = new Mock<IMethod>();
            var proxy = CreateProxyInstance(mock.Object);
            var arg = new object();
            bool decoratorCalled = false;
            Action<object> afterDecorator = a =>
            {
                Assert.AreEqual(arg, a);
                mock.Verify(m => m.ActionWithReferenceArgument(arg), Times.Once());
                decoratorCalled = true;
            };
            SetAfterDecorator(proxy, "ActionWithReferenceArgumentMethodDecorators0", afterDecorator);

            proxy.ActionWithReferenceArgument(arg);

            Assert.IsTrue(decoratorCalled);
            mock.Verify(m => m.ActionWithReferenceArgument(arg), Times.Once());
            mock.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void InvokeActionWithReferenceArgumentOverload_IMethodProxyWithAfterDecorator_CallsRequestedMethodAndDecorator()
        {
            var mock = new Mock<IMethod>();
            var proxy = CreateProxyInstance(mock.Object);
            string arg = "TestString";
            bool decoratorCalled = false;
            Action<string> afterDecorator = a =>
            {
                Assert.AreEqual(arg, a);
                mock.Verify(m => m.ActionWithReferenceArgument(arg), Times.Once());
                decoratorCalled = true;
            };
            SetAfterDecorator(proxy, "ActionWithReferenceArgumentMethodDecorators1", afterDecorator);

            proxy.ActionWithReferenceArgument(arg);

            Assert.IsTrue(decoratorCalled);
            mock.Verify(m => m.ActionWithReferenceArgument(arg), Times.Once());
            mock.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void InvokeFunctionWithValueReturnType_IMethodProxyWithAfterDecorator_CallsRequestedMethodAndDecorator()
        {
            var mock = new Mock<IMethod>();
            var proxy = CreateProxyInstance(mock.Object);
            bool decoratorCalled = false;
            Action<int> afterDecorator = r =>
            {
                mock.Verify(m => m.FunctionWithValueReturnType(), Times.Once());
                decoratorCalled = true;
            };
            SetAfterDecorator<Action, Action<int>>(proxy, "FunctionWithValueReturnTypeMethodDecorators0", afterDecorator);

            proxy.FunctionWithValueReturnType();

            Assert.IsTrue(decoratorCalled);
            mock.Verify(m => m.FunctionWithValueReturnType(), Times.Once());
            mock.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void InvokeFunctionWithValueArgumentAndValueReturnTypeWithValueArgument_IMethodProxyWithAfterDecorator_CallsRequestedMethodAndDecorator()
        {
            var mock = new Mock<IMethod>();
            var proxy = CreateProxyInstance(mock.Object);
            int arg = 123;
            bool decoratorCalled = false;
            Action<int, int> afterDecorator = (a, r) =>
            {
                Assert.AreEqual(arg, a);
                mock.Verify(m => m.FunctionWithValueArgumentAndValueReturnType(arg), Times.Once());
                decoratorCalled = true;
            };
            SetAfterDecorator<Action<int>, Action<int, int>>(proxy, "FunctionWithValueArgumentAndValueReturnTypeMethodDecorators0", afterDecorator);

            proxy.FunctionWithValueArgumentAndValueReturnType(arg);

            Assert.IsTrue(decoratorCalled);
            mock.Verify(m => m.FunctionWithValueArgumentAndValueReturnType(arg), Times.Once());
            mock.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void InvokeFunctionWithValueArgumentAndValueReturnTypeOverload_IMethodProxyWithAfterDecorator_CallsRequestedMethodAndDecorator()
        {
            var mock = new Mock<IMethod>();
            var proxy = CreateProxyInstance(mock.Object);
            uint arg = 123u;
            bool decoratorCalled = false;
            Action<uint, int> afterDecorator = (a, r) =>
            {
                Assert.AreEqual(arg, a);
                mock.Verify(m => m.FunctionWithValueArgumentAndValueReturnType(arg), Times.Once());
                decoratorCalled = true;
            };
            SetAfterDecorator<Action<uint>, Action<uint, int>>(proxy, "FunctionWithValueArgumentAndValueReturnTypeMethodDecorators1", afterDecorator);

            proxy.FunctionWithValueArgumentAndValueReturnType(arg);

            Assert.IsTrue(decoratorCalled);
            mock.Verify(m => m.FunctionWithValueArgumentAndValueReturnType(arg), Times.Once());
            mock.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void InvokeFunctionWithReferenceArgumentAndValueReturnType_IMethodProxyWithAfterDecorator_CallsRequestedMethodAndDecorator()
        {
            var mock = new Mock<IMethod>();
            var proxy = CreateProxyInstance(mock.Object);
            var arg = new object();
            bool decoratorCalled = false;
            Action<object, int> afterDecorator = (a, r) =>
            {
                Assert.AreEqual(arg, a);
                mock.Verify(m => m.FunctionWithReferenceArgumentAndValueReturnType(arg), Times.Once());
                decoratorCalled = true;
            };
            SetAfterDecorator<Action<object>, Action<object, int>>(proxy, "FunctionWithReferenceArgumentAndValueReturnTypeMethodDecorators0", afterDecorator);

            proxy.FunctionWithReferenceArgumentAndValueReturnType(arg);

            Assert.IsTrue(decoratorCalled);
            mock.Verify(m => m.FunctionWithReferenceArgumentAndValueReturnType(arg), Times.Once());
            mock.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void InvokeFunctionWithReferenceArgumentAndValueReturnTypeOverload_IMethodProxyWithAfterDecorator_CallsRequestedMethodAndDecorator()
        {
            var mock = new Mock<IMethod>();
            var proxy = CreateProxyInstance(mock.Object);
            string arg = "TestString";
            bool decoratorCalled = false;
            Action<string, int> afterDecorator = (a, r) =>
            {
                Assert.AreEqual(arg, a);
                mock.Verify(m => m.FunctionWithReferenceArgumentAndValueReturnType(arg), Times.Once());
                decoratorCalled = true;
            };
            SetAfterDecorator<Action<string>, Action<string, int>>(proxy, "FunctionWithReferenceArgumentAndValueReturnTypeMethodDecorators1", afterDecorator);

            proxy.FunctionWithReferenceArgumentAndValueReturnType(arg);

            Assert.IsTrue(decoratorCalled);
            mock.Verify(m => m.FunctionWithReferenceArgumentAndValueReturnType(arg), Times.Once());
            mock.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void InvokeFunctionWithReferenceReturnType_IMethodProxyWithAfterDecorator_CallsRequestedMethodAndDecorator()
        {
            var mock = new Mock<IMethod>();
            var proxy = CreateProxyInstance(mock.Object);
            bool decoratorCalled = false;
            Action<object> afterDecorator = r =>
            {
                mock.Verify(m => m.FunctionWithReferenceReturnType(), Times.Once());
                decoratorCalled = true;
            };
            SetAfterDecorator<Action, Action<object>>(proxy, "FunctionWithReferenceReturnTypeMethodDecorators0", afterDecorator);

            proxy.FunctionWithReferenceReturnType();

            Assert.IsTrue(decoratorCalled);
            mock.Verify(m => m.FunctionWithReferenceReturnType(), Times.Once());
            mock.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void InvokeFunctionWithValueArgumentAndReferenceReturnTypeWithValueArgument_IMethodProxyWithAfterDecorator_CallsRequestedMethodAndDecorator()
        {
            var mock = new Mock<IMethod>();
            var proxy = CreateProxyInstance(mock.Object);
            int arg = 123;
            bool decoratorCalled = false;
            Action<int, object> afterDecorator = (a, r) =>
            {
                Assert.AreEqual(arg, a);
                mock.Verify(m => m.FunctionWithValueArgumentAndReferenceReturnType(arg), Times.Once());
                decoratorCalled = true;
            };
            SetAfterDecorator<Action<int>, Action<int, object>>(proxy, "FunctionWithValueArgumentAndReferenceReturnTypeMethodDecorators0", afterDecorator);

            proxy.FunctionWithValueArgumentAndReferenceReturnType(arg);

            Assert.IsTrue(decoratorCalled);
            mock.Verify(m => m.FunctionWithValueArgumentAndReferenceReturnType(arg), Times.Once());
            mock.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void InvokeFunctionWithValueArgumentAndReferenceReturnTypeOverload_IMethodProxyWithAfterDecorator_CallsRequestedMethodAndDecorator()
        {
            var mock = new Mock<IMethod>();
            var proxy = CreateProxyInstance(mock.Object);
            uint arg = 123u;
            bool decoratorCalled = false;
            Action<uint, object> afterDecorator = (a, r) =>
            {
                Assert.AreEqual(arg, a);
                mock.Verify(m => m.FunctionWithValueArgumentAndReferenceReturnType(arg), Times.Once());
                decoratorCalled = true;
            };
            SetAfterDecorator<Action<uint>, Action<uint, object>>(proxy, "FunctionWithValueArgumentAndReferenceReturnTypeMethodDecorators1", afterDecorator);

            proxy.FunctionWithValueArgumentAndReferenceReturnType(arg);

            Assert.IsTrue(decoratorCalled);
            mock.Verify(m => m.FunctionWithValueArgumentAndReferenceReturnType(arg), Times.Once());
            mock.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void InvokeFunctionWithReferenceArgumentAndReferenceReturnType_IMethodProxyWithAfterDecorator_CallsRequestedMethodAndDecorator()
        {
            var mock = new Mock<IMethod>();
            var proxy = CreateProxyInstance(mock.Object);
            var arg = new object();
            bool decoratorCalled = false;
            Action<object, object> afterDecorator = (a, r) =>
            {
                Assert.AreEqual(arg, a);
                mock.Verify(m => m.FunctionWithReferenceArgumentAndReferenceReturnType(arg), Times.Once());
                decoratorCalled = true;
            };
            SetAfterDecorator<Action<object>, Action<object, object>>(proxy, "FunctionWithReferenceArgumentAndReferenceReturnTypeMethodDecorators0", afterDecorator);

            proxy.FunctionWithReferenceArgumentAndReferenceReturnType(arg);

            Assert.IsTrue(decoratorCalled);
            mock.Verify(m => m.FunctionWithReferenceArgumentAndReferenceReturnType(arg), Times.Once());
            mock.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void InvokeFunctionWithReferenceArgumentAndReferenceReturnTypeOverload_IMethodProxyWithAfterDecorator_CallsRequestedMethodAndDecorator()
        {
            var mock = new Mock<IMethod>();
            var proxy = CreateProxyInstance(mock.Object);
            string arg = "TestString";
            bool decoratorCalled = false;
            Action<string, object> afterDecorator = (a, r) =>
            {
                Assert.AreEqual(arg, a);
                mock.Verify(m => m.FunctionWithReferenceArgumentAndReferenceReturnType(arg), Times.Once());
                decoratorCalled = true;
            };
            SetAfterDecorator<Action<string>, Action<string, object>>(proxy, "FunctionWithReferenceArgumentAndReferenceReturnTypeMethodDecorators1", afterDecorator);

            proxy.FunctionWithReferenceArgumentAndReferenceReturnType(arg);

            Assert.IsTrue(decoratorCalled);
            mock.Verify(m => m.FunctionWithReferenceArgumentAndReferenceReturnType(arg), Times.Once());
            mock.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void InvokeFunctionWithTupleReturnType_IMethodProxyWithAfterDecorator_CallsRequestedMethodAndDecorator()
        {
            var mock = new Mock<IMethod>();
            var proxy = CreateProxyInstance(mock.Object);
            bool decoratorCalled = false;
            Action<ValueTuple<int, object>> afterDecorator = r =>
            {
                mock.Verify(m => m.FunctionWithTupleReturnType(), Times.Once());
                decoratorCalled = true;
            };
            SetAfterDecorator<Action, Action<ValueTuple<int, object>>>(proxy, "FunctionWithTupleReturnTypeMethodDecorators0", afterDecorator);

            proxy.FunctionWithTupleReturnType();

            Assert.IsTrue(decoratorCalled);
            mock.Verify(m => m.FunctionWithTupleReturnType(), Times.Once());
            mock.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void InvokeFunctionWithValueArgumentAndTupleReturnTypeWithValueArgument_IMethodProxyWithAfterDecorator_CallsRequestedMethodAndDecorator()
        {
            var mock = new Mock<IMethod>();
            var proxy = CreateProxyInstance(mock.Object);
            int arg = 123;
            bool decoratorCalled = false;
            Action<int, ValueTuple<int, object>> afterDecorator = (a, r) =>
            {
                Assert.AreEqual(arg, a);
                mock.Verify(m => m.FunctionWithValueArgumentAndTupleReturnType(arg), Times.Once());
                decoratorCalled = true;
            };
            SetAfterDecorator<Action<int>, Action<int, ValueTuple<int, object>>>(proxy, "FunctionWithValueArgumentAndTupleReturnTypeMethodDecorators0", afterDecorator);

            proxy.FunctionWithValueArgumentAndTupleReturnType(arg);

            Assert.IsTrue(decoratorCalled);
            mock.Verify(m => m.FunctionWithValueArgumentAndTupleReturnType(arg), Times.Once());
            mock.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void InvokeFunctionWithValueArgumentAndTupleReturnTypeOverload_IMethodProxyWithAfterDecorator_CallsRequestedMethodAndDecorator()
        {
            var mock = new Mock<IMethod>();
            var proxy = CreateProxyInstance(mock.Object);
            uint arg = 123u;
            bool decoratorCalled = false;
            Action<uint, ValueTuple<int, object>> afterDecorator = (a, r) =>
            {
                Assert.AreEqual(arg, a);
                mock.Verify(m => m.FunctionWithValueArgumentAndTupleReturnType(arg), Times.Once());
                decoratorCalled = true;
            };
            SetAfterDecorator<Action<uint>, Action<uint, ValueTuple<int, object>>>(proxy, "FunctionWithValueArgumentAndTupleReturnTypeMethodDecorators1", afterDecorator);

            proxy.FunctionWithValueArgumentAndTupleReturnType(arg);

            Assert.IsTrue(decoratorCalled);
            mock.Verify(m => m.FunctionWithValueArgumentAndTupleReturnType(arg), Times.Once());
            mock.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void InvokeFunctionWithReferenceArgumentAndTupleReturnType_IMethodProxyWithAfterDecorator_CallsRequestedMethodAndDecorator()
        {
            var mock = new Mock<IMethod>();
            var proxy = CreateProxyInstance(mock.Object);
            var arg = new object();
            bool decoratorCalled = false;
            Action<object, ValueTuple<int, object>> afterDecorator = (a, r) =>
            {
                Assert.AreEqual(arg, a);
                mock.Verify(m => m.FunctionWithReferenceArgumentAndTupleReturnType(arg), Times.Once());
                decoratorCalled = true;
            };
            SetAfterDecorator<Action<object>, Action<object, ValueTuple<int, object>>>(proxy, "FunctionWithReferenceArgumentAndTupleReturnTypeMethodDecorators0", afterDecorator);

            proxy.FunctionWithReferenceArgumentAndTupleReturnType(arg);

            Assert.IsTrue(decoratorCalled);
            mock.Verify(m => m.FunctionWithReferenceArgumentAndTupleReturnType(arg), Times.Once());
            mock.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void InvokeFunctionWithReferenceArgumentAndTupleReturnTypeOverload_IMethodProxyWithAfterDecorator_CallsRequestedMethodAndDecorator()
        {
            var mock = new Mock<IMethod>();
            var proxy = CreateProxyInstance(mock.Object);
            string arg = "TestString";
            bool decoratorCalled = false;
            Action<string, ValueTuple<int, object>> afterDecorator = (a, r) =>
            {
                Assert.AreEqual(arg, a);
                mock.Verify(m => m.FunctionWithReferenceArgumentAndTupleReturnType(arg), Times.Once());
                decoratorCalled = true;
            };
            SetAfterDecorator<Action<string>, Action<string, ValueTuple<int, object>>>(proxy, "FunctionWithReferenceArgumentAndTupleReturnTypeMethodDecorators1", afterDecorator);

            proxy.FunctionWithReferenceArgumentAndTupleReturnType(arg);

            Assert.IsTrue(decoratorCalled);
            mock.Verify(m => m.FunctionWithReferenceArgumentAndTupleReturnType(arg), Times.Once());
            mock.VerifyNoOtherCalls();
        }

        internal static T CreateProxyInstance<T>(T target)
        {
            return (T)Activator.CreateInstance(CreateTargetType(typeof(T)), target);
        }

        internal static Type CreateTargetType(Type targetType)
        {
            return TypeRepository.GetOrAdd(targetType, t => new ProxyBuilder(t, Static.ModuleBinder.Value).CreateProxyType());
        }

        internal static ConcurrentDictionary<Type, Type> TypeRepository { get; } = new ConcurrentDictionary<Type, Type>();

        internal static void AssertTypeHasPassedPrivateInstanceFields(Type type, Dictionary<string, Type> expectedFields)
        {
            var fields = type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance);

            Assert.AreEqual(expectedFields.Count, fields.Length);

            foreach (var kvp in expectedFields)
            {
                Assert.AreEqual(kvp.Value, fields.FirstOrDefault(f => f.Name == kvp.Key)?.FieldType,
                    $"Expected field '{kvp.Key}' with type '{kvp.Value}' has not been found");
            }
        }

        internal static void AssertTypeHasPassedPrivateInstanceMethods(Type type, Dictionary<string, Type> expectedFunctions)
        {
            var methods = type.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance);

            Assert.AreEqual(expectedFunctions.Count, methods.Length);

            foreach (var kvp in expectedFunctions)
            {
                Assert.AreEqual(kvp.Value, methods.FirstOrDefault(m => m.Name == kvp.Key)?.MakeGenericDelegateType(),
                    $"Expected method '{kvp.Key}' with type '{kvp.Value}' has not been found");
            }
        }

        internal static void AssertInstanceHasNotNullFieldsValues(Type type, object instance, IEnumerable<string> fields)
        {
            foreach (var f in fields)
            {
                Assert.IsNotNull(type.GetField(f, BindingFlags.NonPublic | BindingFlags.Instance).GetValue(instance));
            }
        }

        internal static void SetBeforeDecorator<TDecorator>(object instance, string fieldName, TDecorator decorator)
            where TDecorator : Delegate
        {
            ((LinkedList<ValueTuple<TDecorator, TDecorator>>)instance.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic).GetValue(instance))
                .AddFirst(new ValueTuple<TDecorator, TDecorator>(decorator, null));
        }

        internal static void SetBeforeDecorator<TBeforeDecorator, TAfterDecorator>(object instance, string fieldName, TBeforeDecorator decorator)
            where TBeforeDecorator : Delegate
            where TAfterDecorator : Delegate
        {
            ((LinkedList<ValueTuple<TBeforeDecorator, TAfterDecorator>>)instance.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic).GetValue(instance))
                .AddFirst(new ValueTuple<TBeforeDecorator, TAfterDecorator>(decorator, null));
        }

        internal static void SetAfterDecorator<TDecorator>(object instance, string fieldName, TDecorator decorator)
            where TDecorator : Delegate
        {
            ((LinkedList<ValueTuple<TDecorator, TDecorator>>)instance.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic).GetValue(instance))
                .AddFirst(new ValueTuple<TDecorator, TDecorator>(null, decorator));
        }

        internal static void SetAfterDecorator<TBeforeDecorator, TAfterDecorator>(object instance, string fieldName, TAfterDecorator decorator)
            where TBeforeDecorator : Delegate
            where TAfterDecorator : Delegate
        {
            ((LinkedList<ValueTuple<TBeforeDecorator, TAfterDecorator>>)instance.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic).GetValue(instance))
                .AddFirst(new ValueTuple<TBeforeDecorator, TAfterDecorator>(null, decorator));
        }

        internal static Dictionary<string, Type> ExpectedIPropertyGetterProxyFields = new Dictionary<string, Type>()
        {
            ["BooleanGetterProxy0"] = typeof(Func<Func<bool>, bool>),
            ["ByteGetterProxy0"] = typeof(Func<Func<byte>, byte>),
            ["SByteGetterProxy0"] = typeof(Func<Func<sbyte>, sbyte>),
            ["CharGetterProxy0"] = typeof(Func<Func<char>, char>),
            ["DecimalGetterProxy0"] = typeof(Func<Func<decimal>, decimal>),
            ["DoubleGetterProxy0"] = typeof(Func<Func<double>, double>),
            ["FloatGetterProxy0"] = typeof(Func<Func<float>, float>),
            ["IntGetterProxy0"] = typeof(Func<Func<int>, int>),
            ["UIntGetterProxy0"] = typeof(Func<Func<uint>, uint>),
            ["LongGetterProxy0"] = typeof(Func<Func<long>, long>),
            ["ULongGetterProxy0"] = typeof(Func<Func<ulong>, ulong>),
            ["ShortGetterProxy0"] = typeof(Func<Func<short>, short>),
            ["UShortGetterProxy0"] = typeof(Func<Func<ushort>, ushort>),
            ["StructGetterProxy0"] = typeof(Func<Func<TestStruct>, TestStruct>),
            ["ClassGetterProxy0"] = typeof(Func<Func<string>, string>),
        };

        internal static Dictionary<string, Type> ExpectedIPropertyGetterDecoratorsFields = new Dictionary<string, Type>()
        {
            ["BooleanGetterDecorators0"] = typeof(LinkedList<ValueTuple<Action, Action<bool>>>),
            ["ByteGetterDecorators0"] = typeof(LinkedList<ValueTuple<Action, Action<byte>>>),
            ["SByteGetterDecorators0"] = typeof(LinkedList<ValueTuple<Action, Action<sbyte>>>),
            ["CharGetterDecorators0"] = typeof(LinkedList<ValueTuple<Action, Action<char>>>),
            ["DecimalGetterDecorators0"] = typeof(LinkedList<ValueTuple<Action, Action<decimal>>>),
            ["DoubleGetterDecorators0"] = typeof(LinkedList<ValueTuple<Action, Action<double>>>),
            ["FloatGetterDecorators0"] = typeof(LinkedList<ValueTuple<Action, Action<float>>>),
            ["IntGetterDecorators0"] = typeof(LinkedList<ValueTuple<Action, Action<int>>>),
            ["UIntGetterDecorators0"] = typeof(LinkedList<ValueTuple<Action, Action<uint>>>),
            ["LongGetterDecorators0"] = typeof(LinkedList<ValueTuple<Action, Action<long>>>),
            ["ULongGetterDecorators0"] = typeof(LinkedList<ValueTuple<Action, Action<ulong>>>),
            ["ShortGetterDecorators0"] = typeof(LinkedList<ValueTuple<Action, Action<short>>>),
            ["UShortGetterDecorators0"] = typeof(LinkedList<ValueTuple<Action, Action<ushort>>>),
            ["StructGetterDecorators0"] = typeof(LinkedList<ValueTuple<Action, Action<TestStruct>>>),
            ["ClassGetterDecorators0"] = typeof(LinkedList<ValueTuple<Action, Action<string>>>),
        };

        internal static Dictionary<string, Type> ExpectedIPropertyGetterFunctions = new Dictionary<string, Type>()
        {
            ["BooleanGetterWrapper0"] = typeof(Func<Func<bool>, bool>),
            ["ByteGetterWrapper0"] = typeof(Func<Func<byte>, byte>),
            ["SByteGetterWrapper0"] = typeof(Func<Func<sbyte>, sbyte>),
            ["CharGetterWrapper0"] = typeof(Func<Func<char>, char>),
            ["DecimalGetterWrapper0"] = typeof(Func<Func<decimal>, decimal>),
            ["DoubleGetterWrapper0"] = typeof(Func<Func<double>, double>),
            ["FloatGetterWrapper0"] = typeof(Func<Func<float>, float>),
            ["IntGetterWrapper0"] = typeof(Func<Func<int>, int>),
            ["UIntGetterWrapper0"] = typeof(Func<Func<uint>, uint>),
            ["LongGetterWrapper0"] = typeof(Func<Func<long>, long>),
            ["ULongGetterWrapper0"] = typeof(Func<Func<ulong>, ulong>),
            ["ShortGetterWrapper0"] = typeof(Func<Func<short>, short>),
            ["UShortGetterWrapper0"] = typeof(Func<Func<ushort>, ushort>),
            ["StructGetterWrapper0"] = typeof(Func<Func<TestStruct>, TestStruct>),
            ["ClassGetterWrapper0"] = typeof(Func<Func<string>, string>),
        };

        internal static Dictionary<string, Type> ExpectedIPropertySetterProxyFields = new Dictionary<string, Type>()
        {
            ["BooleanSetterProxy0"] = typeof(Action<Action<bool>, bool>),
            ["ByteSetterProxy0"] = typeof(Action<Action<byte>, byte>),
            ["SByteSetterProxy0"] = typeof(Action<Action<sbyte>, sbyte>),
            ["CharSetterProxy0"] = typeof(Action<Action<char>, char>),
            ["DecimalSetterProxy0"] = typeof(Action<Action<decimal>, decimal>),
            ["DoubleSetterProxy0"] = typeof(Action<Action<double>, double>),
            ["FloatSetterProxy0"] = typeof(Action<Action<float>, float>),
            ["IntSetterProxy0"] = typeof(Action<Action<int>, int>),
            ["UIntSetterProxy0"] = typeof(Action<Action<uint>, uint>),
            ["LongSetterProxy0"] = typeof(Action<Action<long>, long>),
            ["ULongSetterProxy0"] = typeof(Action<Action<ulong>, ulong>),
            ["ShortSetterProxy0"] = typeof(Action<Action<short>, short>),
            ["UShortSetterProxy0"] = typeof(Action<Action<ushort>, ushort>),
            ["StructSetterProxy0"] = typeof(Action<Action<TestStruct>, TestStruct>),
            ["ClassSetterProxy0"] = typeof(Action<Action<string>, string>),
        };

        internal static Dictionary<string, Type> ExpectedIPropertySetterDecoratorsFields = new Dictionary<string, Type>()
        {
            ["BooleanSetterDecorators0"] = typeof(LinkedList<ValueTuple<Action<bool>, Action<bool>>>),
            ["ByteSetterDecorators0"] = typeof(LinkedList<ValueTuple<Action<byte>, Action<byte>>>),
            ["SByteSetterDecorators0"] = typeof(LinkedList<ValueTuple<Action<sbyte>, Action<sbyte>>>),
            ["CharSetterDecorators0"] = typeof(LinkedList<ValueTuple<Action<char>, Action<char>>>),
            ["DecimalSetterDecorators0"] = typeof(LinkedList<ValueTuple<Action<decimal>, Action<decimal>>>),
            ["DoubleSetterDecorators0"] = typeof(LinkedList<ValueTuple<Action<double>, Action<double>>>),
            ["FloatSetterDecorators0"] = typeof(LinkedList<ValueTuple<Action<float>, Action<float>>>),
            ["IntSetterDecorators0"] = typeof(LinkedList<ValueTuple<Action<int>, Action<int>>>),
            ["UIntSetterDecorators0"] = typeof(LinkedList<ValueTuple<Action<uint>, Action<uint>>>),
            ["LongSetterDecorators0"] = typeof(LinkedList<ValueTuple<Action<long>, Action<long>>>),
            ["ULongSetterDecorators0"] = typeof(LinkedList<ValueTuple<Action<ulong>, Action<ulong>>>),
            ["ShortSetterDecorators0"] = typeof(LinkedList<ValueTuple<Action<short>, Action<short>>>),
            ["UShortSetterDecorators0"] = typeof(LinkedList<ValueTuple<Action<ushort>, Action<ushort>>>),
            ["StructSetterDecorators0"] = typeof(LinkedList<ValueTuple<Action<TestStruct>, Action<TestStruct>>>),
            ["ClassSetterDecorators0"] = typeof(LinkedList<ValueTuple<Action<string>, Action<string>>>),
        };

        internal static Dictionary<string, Type> ExpectedIPropertySetterFunctions = new Dictionary<string, Type>()
        {
            ["BooleanSetterWrapper0"] = typeof(Action<Action<bool>, bool>),
            ["ByteSetterWrapper0"] = typeof(Action<Action<byte>, byte>),
            ["SByteSetterWrapper0"] = typeof(Action<Action<sbyte>, sbyte>),
            ["CharSetterWrapper0"] = typeof(Action<Action<char>, char>),
            ["DecimalSetterWrapper0"] = typeof(Action<Action<decimal>, decimal>),
            ["DoubleSetterWrapper0"] = typeof(Action<Action<double>, double>),
            ["FloatSetterWrapper0"] = typeof(Action<Action<float>, float>),
            ["IntSetterWrapper0"] = typeof(Action<Action<int>, int>),
            ["UIntSetterWrapper0"] = typeof(Action<Action<uint>, uint>),
            ["LongSetterWrapper0"] = typeof(Action<Action<long>, long>),
            ["ULongSetterWrapper0"] = typeof(Action<Action<ulong>, ulong>),
            ["ShortSetterWrapper0"] = typeof(Action<Action<short>, short>),
            ["UShortSetterWrapper0"] = typeof(Action<Action<ushort>, ushort>),
            ["StructSetterWrapper0"] = typeof(Action<Action<TestStruct>, TestStruct>),
            ["ClassSetterWrapper0"] = typeof(Action<Action<string>, string>)
        };

        internal static Dictionary<string, Type> ExpectedIEventProxyFields = new Dictionary<string, Type>()
        {
            ["EventEmptyArgsEventProxy0"] = typeof(Action<Action<object, EventArgs>, object, EventArgs>),
            ["EventIntArgsEventProxy0"] = typeof(Action<Action<object, EventArgs<int>>, object, EventArgs<int>>),
            ["EventStringArgsEventProxy0"] = typeof(Action<Action<object, EventArgs<string>>, object, EventArgs<string>>)
        };

        internal static Dictionary<string, Type> ExpectedIEventDecoratorsFields = new Dictionary<string, Type>()
        {
            ["EventEmptyArgsEventDecorators0"] = typeof(LinkedList<ValueTuple<Action<object, EventArgs>, Action<object, EventArgs>>>),
            ["EventIntArgsEventDecorators0"] = typeof(LinkedList<ValueTuple<Action<object, EventArgs<int>>, Action<object, EventArgs<int>>>>),
            ["EventStringArgsEventDecorators0"] = typeof(LinkedList<ValueTuple<Action<object, EventArgs<string>>, Action<object, EventArgs<string>>>>),
        };

        internal static Dictionary<string, Type> ExpectedIEventSubscribersFields = new Dictionary<string, Type>()
        {
            ["EventEmptyArgsEventSubscribers0"] = typeof(Dictionary<Action<object, EventArgs>, Action<object, EventArgs>>),
            ["EventIntArgsEventSubscribers0"] = typeof(Dictionary<Action<object, EventArgs<int>>, Action<object, EventArgs<int>>>),
            ["EventStringArgsEventSubscribers0"] = typeof(Dictionary<Action<object, EventArgs<string>>, Action<object, EventArgs<string>>>),
        };

        internal static Dictionary<string, Type> ExpectedIEventMethods = new Dictionary<string, Type>()
        {
            ["EventEmptyArgsEventWrapper0"] = typeof(Action<Action<object, EventArgs>, object, EventArgs>),
            ["EventIntArgsEventWrapper0"] = typeof(Action<Action<object, EventArgs<int>>, object, EventArgs<int>>),
            ["EventStringArgsEventWrapper0"] = typeof(Action<Action<object, EventArgs<string>>, object, EventArgs<string>>)
        };

        internal static Dictionary<string, Type> ExpectedIMethodProxyFields = new Dictionary<string, Type>()
        {
            ["ActionMethodProxy0"] = typeof(Action<Action>),
            ["ActionWithValueArgumentMethodProxy0"] = typeof(Action<Action<int>, int>),
            ["ActionWithValueArgumentMethodProxy1"] = typeof(Action<Action<uint>, uint>),
            ["ActionWithReferenceArgumentMethodProxy0"] = typeof(Action<Action<object>, object>),
            ["ActionWithReferenceArgumentMethodProxy1"] = typeof(Action<Action<string>, string>),
            ["FunctionWithValueReturnTypeMethodProxy0"] = typeof(Func<Func<int>, int>),
            ["FunctionWithValueArgumentAndValueReturnTypeMethodProxy0"] = typeof(Func<Func<int, int>, int, int>),
            ["FunctionWithValueArgumentAndValueReturnTypeMethodProxy1"] = typeof(Func<Func<uint, int>, uint, int>),
            ["FunctionWithReferenceArgumentAndValueReturnTypeMethodProxy0"] = typeof(Func<Func<object, int>, object, int>),
            ["FunctionWithReferenceArgumentAndValueReturnTypeMethodProxy1"] = typeof(Func<Func<string, int>, string, int>),
            ["FunctionWithReferenceReturnTypeMethodProxy0"] = typeof(Func<Func<object>, object>),
            ["FunctionWithValueArgumentAndReferenceReturnTypeMethodProxy0"] = typeof(Func<Func<int, object>, int, object>),
            ["FunctionWithValueArgumentAndReferenceReturnTypeMethodProxy1"] = typeof(Func<Func<uint, object>, uint, object>),
            ["FunctionWithReferenceArgumentAndReferenceReturnTypeMethodProxy0"] = typeof(Func<Func<object, object>, object, object>),
            ["FunctionWithReferenceArgumentAndReferenceReturnTypeMethodProxy1"] = typeof(Func<Func<string, object>, string, object>),
            ["FunctionWithTupleReturnTypeMethodProxy0"] = typeof(Func<Func<ValueTuple<int, object>>, ValueTuple<int, object>>),
            ["FunctionWithValueArgumentAndTupleReturnTypeMethodProxy0"] = typeof(Func<Func<int, ValueTuple<int, object>>, int, ValueTuple<int, object>>),
            ["FunctionWithValueArgumentAndTupleReturnTypeMethodProxy1"] = typeof(Func<Func<uint, ValueTuple<int, object>>, uint, ValueTuple<int, object>>),
            ["FunctionWithReferenceArgumentAndTupleReturnTypeMethodProxy0"] = typeof(Func<Func<object, ValueTuple<int, object>>, object, ValueTuple<int, object>>),
            ["FunctionWithReferenceArgumentAndTupleReturnTypeMethodProxy1"] = typeof(Func<Func<string, ValueTuple<int, object>>, string, ValueTuple<int, object>>),
        };

        internal static Dictionary<string, Type> ExpectedIMethodDecoratorsFields = new Dictionary<string, Type>()
        {
            ["ActionMethodDecorators0"] = typeof(LinkedList<ValueTuple<Action, Action>>),
            ["ActionWithValueArgumentMethodDecorators0"] = typeof(LinkedList<ValueTuple<Action<int>, Action<int>>>),
            ["ActionWithValueArgumentMethodDecorators1"] = typeof(LinkedList<ValueTuple<Action<uint>, Action<uint>>>),
            ["ActionWithReferenceArgumentMethodDecorators0"] = typeof(LinkedList<ValueTuple<Action<object>, Action<object>>>),
            ["ActionWithReferenceArgumentMethodDecorators1"] = typeof(LinkedList<ValueTuple<Action<string>, Action<string>>>),
            ["FunctionWithValueReturnTypeMethodDecorators0"] = typeof(LinkedList<ValueTuple<Action, Action<int>>>),
            ["FunctionWithValueArgumentAndValueReturnTypeMethodDecorators0"] = typeof(LinkedList<ValueTuple<Action<int>, Action<int, int>>>),
            ["FunctionWithValueArgumentAndValueReturnTypeMethodDecorators1"] = typeof(LinkedList<ValueTuple<Action<uint>, Action<uint, int>>>),
            ["FunctionWithReferenceArgumentAndValueReturnTypeMethodDecorators0"] = typeof(LinkedList<ValueTuple<Action<object>, Action<object, int>>>),
            ["FunctionWithReferenceArgumentAndValueReturnTypeMethodDecorators1"] = typeof(LinkedList<ValueTuple<Action<string>, Action<string, int>>>),
            ["FunctionWithReferenceReturnTypeMethodDecorators0"] = typeof(LinkedList<ValueTuple<Action, Action<object>>>),
            ["FunctionWithValueArgumentAndReferenceReturnTypeMethodDecorators0"] = typeof(LinkedList<ValueTuple<Action<int>, Action<int, object>>>),
            ["FunctionWithValueArgumentAndReferenceReturnTypeMethodDecorators1"] = typeof(LinkedList<ValueTuple<Action<uint>, Action<uint, object>>>),
            ["FunctionWithReferenceArgumentAndReferenceReturnTypeMethodDecorators0"] = typeof(LinkedList<ValueTuple<Action<object>, Action<object, object>>>),
            ["FunctionWithReferenceArgumentAndReferenceReturnTypeMethodDecorators1"] = typeof(LinkedList<ValueTuple<Action<string>, Action<string, object>>>),
            ["FunctionWithTupleReturnTypeMethodDecorators0"] = typeof(LinkedList<ValueTuple<Action, Action<ValueTuple<int, object>>>>),
            ["FunctionWithValueArgumentAndTupleReturnTypeMethodDecorators0"] = typeof(LinkedList<ValueTuple<Action<int>, Action<int, ValueTuple<int, object>>>>),
            ["FunctionWithValueArgumentAndTupleReturnTypeMethodDecorators1"] = typeof(LinkedList<ValueTuple<Action<uint>, Action<uint, ValueTuple<int, object>>>>),
            ["FunctionWithReferenceArgumentAndTupleReturnTypeMethodDecorators0"] = typeof(LinkedList<ValueTuple<Action<object>, Action<object, ValueTuple<int, object>>>>),
            ["FunctionWithReferenceArgumentAndTupleReturnTypeMethodDecorators1"] = typeof(LinkedList<ValueTuple<Action<string>, Action<string, ValueTuple<int, object>>>>)
        };

        internal static Dictionary<string, Type> ExpectedIMethodFunctions = new Dictionary<string, Type>()
        {
            ["ActionMethodWrapper0"]                                              = typeof(Action<Action>),
            ["ActionWithValueArgumentMethodWrapper0"]                             = typeof(Action<Action<int>, int>),
            ["ActionWithValueArgumentMethodWrapper1"]                             = typeof(Action<Action<uint>, uint>),
            ["ActionWithReferenceArgumentMethodWrapper0"]                         = typeof(Action<Action<object>, object>),
            ["ActionWithReferenceArgumentMethodWrapper1"]                         = typeof(Action<Action<string>, string>),
            ["FunctionWithValueReturnTypeMethodWrapper0"]                         = typeof(Func<Func<int>, int>),
            ["FunctionWithValueArgumentAndValueReturnTypeMethodWrapper0"]         = typeof(Func<Func<int, int>, int, int>),
            ["FunctionWithValueArgumentAndValueReturnTypeMethodWrapper1"]         = typeof(Func<Func<uint, int>, uint, int>),
            ["FunctionWithReferenceArgumentAndValueReturnTypeMethodWrapper0"]     = typeof(Func<Func<object, int>, object, int>),
            ["FunctionWithReferenceArgumentAndValueReturnTypeMethodWrapper1"]     = typeof(Func<Func<string, int>, string, int>),
            ["FunctionWithReferenceReturnTypeMethodWrapper0"]                     = typeof(Func<Func<object>, object>),
            ["FunctionWithValueArgumentAndReferenceReturnTypeMethodWrapper0"]     = typeof(Func<Func<int, object>, int, object>),
            ["FunctionWithValueArgumentAndReferenceReturnTypeMethodWrapper1"]     = typeof(Func<Func<uint, object>, uint, object>),
            ["FunctionWithReferenceArgumentAndReferenceReturnTypeMethodWrapper0"] = typeof(Func<Func<object, object>, object, object>),
            ["FunctionWithReferenceArgumentAndReferenceReturnTypeMethodWrapper1"] = typeof(Func<Func<string, object>, string, object>),
            ["FunctionWithTupleReturnTypeMethodWrapper0"]                         = typeof(Func<Func<ValueTuple<int, object>>, ValueTuple<int, object>>),
            ["FunctionWithValueArgumentAndTupleReturnTypeMethodWrapper0"]         = typeof(Func<Func<int, ValueTuple<int, object>>, int, ValueTuple<int, object>>),
            ["FunctionWithValueArgumentAndTupleReturnTypeMethodWrapper1"]         = typeof(Func<Func<uint, ValueTuple<int, object>>, uint, ValueTuple<int, object>>),
            ["FunctionWithReferenceArgumentAndTupleReturnTypeMethodWrapper0"]     = typeof(Func<Func<object, ValueTuple<int, object>>, object, ValueTuple<int, object>>),
            ["FunctionWithReferenceArgumentAndTupleReturnTypeMethodWrapper1"]     = typeof(Func<Func<string, ValueTuple<int, object>>, string, ValueTuple<int, object>>)
        };

        internal static Dictionary<string, Type> ExpectedObjectFunctions = new Dictionary<string, Type>()
        {
            ["Finalize"] = typeof(Action),
            ["MemberwiseClone"] = typeof(Func<object>)
        };
    }
}
