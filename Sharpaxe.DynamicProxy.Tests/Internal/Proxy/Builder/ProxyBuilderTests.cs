using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Sharpaxe.DynamicProxy.Internal.Proxy.Builder;
using Sharpaxe.DynamicProxy.Tests.TestHelper;
using Sharpaxe.DynamicProxy.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Collections.Concurrent;
using Sharpaxe.DynamicProxy.Internal.Proxy.NameProvider;

namespace Sharpaxe.DynamicProxy.Tests.Internal.Proxy.Builder
{
    [TestClass]
    public class ProxyBuilderTests
    {
        #region Create Type

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
                .Concat(ExpectedIEventSubscribersFields)
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
                .Concat(ExpectedIEventSubscribersFields)
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

        #endregion Create Type

        #region Create Instance

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

        #endregion Create Instance

        #region Transparent Call

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

        #endregion Transparent Call

        #region Call with Before Decorator

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
            AddBeforeDecorator(proxy, "ActionMethodDecorators0", beforeDecorator);

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
            AddBeforeDecorator(proxy, "ActionWithValueArgumentMethodDecorators0", beforeDecorator);

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
            AddBeforeDecorator(proxy, "ActionWithValueArgumentMethodDecorators1", beforeDecorator);

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
            AddBeforeDecorator(proxy, "ActionWithReferenceArgumentMethodDecorators0", beforeDecorator);

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
            AddBeforeDecorator(proxy, "ActionWithReferenceArgumentMethodDecorators1", beforeDecorator);

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
            AddBeforeDecorator<Action, Action<int>>(proxy, "FunctionWithValueReturnTypeMethodDecorators0", beforeDecorator);

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
            AddBeforeDecorator<Action<int>, Action<int, int>>(proxy, "FunctionWithValueArgumentAndValueReturnTypeMethodDecorators0", beforeDecorator);

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
            AddBeforeDecorator<Action<uint>, Action<uint, int>>(proxy, "FunctionWithValueArgumentAndValueReturnTypeMethodDecorators1", beforeDecorator);

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
            AddBeforeDecorator<Action<object>, Action<object, int>>(proxy, "FunctionWithReferenceArgumentAndValueReturnTypeMethodDecorators0", beforeDecorator);

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
            AddBeforeDecorator<Action<string>, Action<string, int>>(proxy, "FunctionWithReferenceArgumentAndValueReturnTypeMethodDecorators1", beforeDecorator);

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
            AddBeforeDecorator<Action, Action<object>>(proxy, "FunctionWithReferenceReturnTypeMethodDecorators0", beforeDecorator);

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
            AddBeforeDecorator<Action<int>, Action<int, object>>(proxy, "FunctionWithValueArgumentAndReferenceReturnTypeMethodDecorators0", beforeDecorator);

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
            AddBeforeDecorator<Action<uint>, Action<uint, object>>(proxy, "FunctionWithValueArgumentAndReferenceReturnTypeMethodDecorators1", beforeDecorator);

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
            AddBeforeDecorator<Action<object>, Action<object, object>>(proxy, "FunctionWithReferenceArgumentAndReferenceReturnTypeMethodDecorators0", beforeDecorator);

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
            AddBeforeDecorator<Action<string>, Action<string, object>>(proxy, "FunctionWithReferenceArgumentAndReferenceReturnTypeMethodDecorators1", beforeDecorator);

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
            AddBeforeDecorator<Action, Action<ValueTuple<int, object>>>(proxy, "FunctionWithTupleReturnTypeMethodDecorators0", beforeDecorator);

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
            AddBeforeDecorator<Action<int>, Action<int, ValueTuple<int, object>>>(proxy, "FunctionWithValueArgumentAndTupleReturnTypeMethodDecorators0", beforeDecorator);

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
            AddBeforeDecorator<Action<uint>, Action<uint, ValueTuple<int, object>>>(proxy, "FunctionWithValueArgumentAndTupleReturnTypeMethodDecorators1", beforeDecorator);

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
            AddBeforeDecorator<Action<object>, Action<object, ValueTuple<int, object>>>(proxy, "FunctionWithReferenceArgumentAndTupleReturnTypeMethodDecorators0", beforeDecorator);

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
            AddBeforeDecorator<Action<string>, Action<string, ValueTuple<int, object>>>(proxy, "FunctionWithReferenceArgumentAndTupleReturnTypeMethodDecorators1", beforeDecorator);

            proxy.FunctionWithReferenceArgumentAndTupleReturnType(arg);

            Assert.IsTrue(decoratorCalled);
            mock.Verify(m => m.FunctionWithReferenceArgumentAndTupleReturnType(arg), Times.Once());
            mock.VerifyNoOtherCalls();
        }

        #endregion Call with Before Decorator

        #region Call with After Decorator

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
            AddAfterDecorator(proxy, "ActionMethodDecorators0", afterDecorator);

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
            AddAfterDecorator(proxy, "ActionWithValueArgumentMethodDecorators0", afterDecorator);

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
            AddAfterDecorator(proxy, "ActionWithValueArgumentMethodDecorators1", afterDecorator);

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
            AddAfterDecorator(proxy, "ActionWithReferenceArgumentMethodDecorators0", afterDecorator);

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
            AddAfterDecorator(proxy, "ActionWithReferenceArgumentMethodDecorators1", afterDecorator);

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
            AddAfterDecorator<Action, Action<int>>(proxy, "FunctionWithValueReturnTypeMethodDecorators0", afterDecorator);

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
            AddAfterDecorator<Action<int>, Action<int, int>>(proxy, "FunctionWithValueArgumentAndValueReturnTypeMethodDecorators0", afterDecorator);

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
            AddAfterDecorator<Action<uint>, Action<uint, int>>(proxy, "FunctionWithValueArgumentAndValueReturnTypeMethodDecorators1", afterDecorator);

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
            AddAfterDecorator<Action<object>, Action<object, int>>(proxy, "FunctionWithReferenceArgumentAndValueReturnTypeMethodDecorators0", afterDecorator);

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
            AddAfterDecorator<Action<string>, Action<string, int>>(proxy, "FunctionWithReferenceArgumentAndValueReturnTypeMethodDecorators1", afterDecorator);

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
            AddAfterDecorator<Action, Action<object>>(proxy, "FunctionWithReferenceReturnTypeMethodDecorators0", afterDecorator);

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
            AddAfterDecorator<Action<int>, Action<int, object>>(proxy, "FunctionWithValueArgumentAndReferenceReturnTypeMethodDecorators0", afterDecorator);

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
            AddAfterDecorator<Action<uint>, Action<uint, object>>(proxy, "FunctionWithValueArgumentAndReferenceReturnTypeMethodDecorators1", afterDecorator);

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
            AddAfterDecorator<Action<object>, Action<object, object>>(proxy, "FunctionWithReferenceArgumentAndReferenceReturnTypeMethodDecorators0", afterDecorator);

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
            AddAfterDecorator<Action<string>, Action<string, object>>(proxy, "FunctionWithReferenceArgumentAndReferenceReturnTypeMethodDecorators1", afterDecorator);

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
            AddAfterDecorator<Action, Action<ValueTuple<int, object>>>(proxy, "FunctionWithTupleReturnTypeMethodDecorators0", afterDecorator);

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
            AddAfterDecorator<Action<int>, Action<int, ValueTuple<int, object>>>(proxy, "FunctionWithValueArgumentAndTupleReturnTypeMethodDecorators0", afterDecorator);

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
            AddAfterDecorator<Action<uint>, Action<uint, ValueTuple<int, object>>>(proxy, "FunctionWithValueArgumentAndTupleReturnTypeMethodDecorators1", afterDecorator);

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
            AddAfterDecorator<Action<object>, Action<object, ValueTuple<int, object>>>(proxy, "FunctionWithReferenceArgumentAndTupleReturnTypeMethodDecorators0", afterDecorator);

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
            AddAfterDecorator<Action<string>, Action<string, ValueTuple<int, object>>>(proxy, "FunctionWithReferenceArgumentAndTupleReturnTypeMethodDecorators1", afterDecorator);

            proxy.FunctionWithReferenceArgumentAndTupleReturnType(arg);

            Assert.IsTrue(decoratorCalled);
            mock.Verify(m => m.FunctionWithReferenceArgumentAndTupleReturnType(arg), Times.Once());
            mock.VerifyNoOtherCalls();
        }

        #endregion Call with After Decorator

        #region Call with Proxy

        [TestMethod]
        public void InvokeAction_IMethodProxyWithProxyDelegateCallingTargetAction_CallsRequestedMethodAndProxy()
        {
            var mock = new Mock<IMethod>();
            var proxy = CreateProxyInstance(mock.Object);
            bool proxyCalled = false;
            Action<Action> proxyDelegate = a =>
            {
                mock.VerifyNoOtherCalls();
                a.Invoke();
                mock.Verify(m => m.Action(), Times.Once());
                mock.VerifyNoOtherCalls();
                proxyCalled = true;
            };
            SetProxy(proxy, "ActionMethodProxy0", proxyDelegate);

            proxy.Action();

            Assert.IsTrue(proxyCalled);
        }

        [TestMethod]
        public void InvokeAction_IMethodProxyWithProxyDelegateNotCallingTargetAction_CallsRequestedMethodAndProxy()
        {
            var mock = new Mock<IMethod>();
            var proxy = CreateProxyInstance(mock.Object);
            bool proxyCalled = false;
            Action<Action> proxyDelegate = a =>
            {
                proxyCalled = true;
            };
            SetProxy(proxy, "ActionMethodProxy0", proxyDelegate);

            proxy.Action();

            Assert.IsTrue(proxyCalled);
            mock.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void InvokeActionWithValueArgument_IMethodProxyWithProxyDelegateCallingTargetAction_CallsRequestedMethodAndProxy()
        {
            var mock = new Mock<IMethod>();
            var proxy = CreateProxyInstance(mock.Object);
            int arg = 123;
            bool proxyCalled = false;
            Action<Action<int>, int> proxyDelegate = (a, i) =>
            {
                Assert.AreEqual(i, arg);
                mock.VerifyNoOtherCalls();
                a.Invoke(i);
                mock.Verify(m => m.ActionWithValueArgument(arg), Times.Once());
                mock.VerifyNoOtherCalls();
                proxyCalled = true;
            };
            SetProxy(proxy, "ActionWithValueArgumentMethodProxy0", proxyDelegate);

            proxy.ActionWithValueArgument(arg);

            Assert.IsTrue(proxyCalled);
        }

        [TestMethod]
        public void InvokeActionWithValueArgument_IMethodProxyWithProxyDelegateNotCallingTargetAction_CallsRequestedMethodAndProxy()
        {
            var mock = new Mock<IMethod>();
            var proxy = CreateProxyInstance(mock.Object);
            int arg = 123;
            bool proxyCalled = false;
            Action<Action<int>, int> proxyDelegate = (a, i) =>
            {
                Assert.AreEqual(arg, i);
                proxyCalled = true;
            };
            SetProxy(proxy, "ActionWithValueArgumentMethodProxy0", proxyDelegate);

            proxy.ActionWithValueArgument(arg);

            Assert.IsTrue(proxyCalled);
            mock.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void InvokeActionWithValueArgumentOverloaded_IMethodProxyWithProxyDelegateCallingTargetAction_CallsRequestedMethodAndProxy()
        {
            var mock = new Mock<IMethod>();
            var proxy = CreateProxyInstance(mock.Object);
            uint arg = 123;
            bool proxyCalled = false;
            Action<Action<uint>, uint> proxyDelegate = (a, ui) =>
            {
                Assert.AreEqual(ui, arg);
                mock.VerifyNoOtherCalls();
                a.Invoke(ui);
                mock.Verify(m => m.ActionWithValueArgument(arg), Times.Once());
                mock.VerifyNoOtherCalls();
                proxyCalled = true;
            };
            SetProxy(proxy, "ActionWithValueArgumentMethodProxy1", proxyDelegate);

            proxy.ActionWithValueArgument(arg);

            Assert.IsTrue(proxyCalled);
        }

        [TestMethod]
        public void InvokeActionWithValueArgumentOverloaded_IMethodProxyWithProxyDelegateNotCallingTargetAction_CallsRequestedMethodAndProxy()
        {
            var mock = new Mock<IMethod>();
            var proxy = CreateProxyInstance(mock.Object);
            uint arg = 123;
            bool proxyCalled = false;
            Action<Action<uint>, uint> proxyDelegate = (a, ui) =>
            {
                Assert.AreEqual(arg, ui);
                proxyCalled = true;
            };
            SetProxy(proxy, "ActionWithValueArgumentMethodProxy1", proxyDelegate);

            proxy.ActionWithValueArgument(arg);

            Assert.IsTrue(proxyCalled);
            mock.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void InvokeActionWithReferenceArgument_IMethodProxyWithProxyDelegateCallingTargetAction_CallsRequestedMethodAndProxy()
        {
            var mock = new Mock<IMethod>();
            var proxy = CreateProxyInstance(mock.Object);
            object arg = new object();
            bool proxyCalled = false;
            Action<Action<object>, object> proxyDelegate = (a, o) =>
            {
                Assert.AreEqual(o, arg);
                mock.VerifyNoOtherCalls();
                a.Invoke(o);
                mock.Verify(m => m.ActionWithReferenceArgument(arg), Times.Once());
                mock.VerifyNoOtherCalls();
                proxyCalled = true;
            };
            SetProxy(proxy, "ActionWithReferenceArgumentMethodProxy0", proxyDelegate);

            proxy.ActionWithReferenceArgument(arg);

            Assert.IsTrue(proxyCalled);
        }

        [TestMethod]
        public void InvokeActionWithReferenceArgument_IMethodProxyWithProxyDelegateNotCallingTargetAction_CallsRequestedMethodAndProxy()
        {
            var mock = new Mock<IMethod>();
            var proxy = CreateProxyInstance(mock.Object);
            object arg = new object();
            bool proxyCalled = false;
            Action<Action<object>, object> proxyDelegate = (a, o) =>
            {
                Assert.AreEqual(arg, o);
                proxyCalled = true;
            };
            SetProxy(proxy, "ActionWithReferenceArgumentMethodProxy0", proxyDelegate);

            proxy.ActionWithReferenceArgument(arg);

            Assert.IsTrue(proxyCalled);
            mock.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void InvokeActionWithReferenceArgumentOverloaded_IMethodProxyWithProxyDelegateCallingTargetAction_CallsRequestedMethodAndProxy()
        {
            var mock = new Mock<IMethod>();
            var proxy = CreateProxyInstance(mock.Object);
            string arg = "TestString";
            bool proxyCalled = false;
            Action<Action<string>, string> proxyDelegate = (a, s) =>
            {
                Assert.AreEqual(s, arg);
                mock.VerifyNoOtherCalls();
                a.Invoke(s);
                mock.Verify(m => m.ActionWithReferenceArgument(arg), Times.Once());
                mock.VerifyNoOtherCalls();
                proxyCalled = true;
            };
            SetProxy(proxy, "ActionWithReferenceArgumentMethodProxy1", proxyDelegate);

            proxy.ActionWithReferenceArgument(arg);

            Assert.IsTrue(proxyCalled);
        }

        [TestMethod]
        public void InvokeActionWithReferenceArgumentOverloaded_IMethodProxyWithProxyDelegateNotCallingTargetAction_CallsRequestedMethodAndProxy()
        {
            var mock = new Mock<IMethod>();
            var proxy = CreateProxyInstance(mock.Object);
            string arg = "TestString";
            bool proxyCalled = false;
            Action<Action<string>, string> proxyDelegate = (a, ui) =>
            {
                Assert.AreEqual(arg, ui);
                proxyCalled = true;
            };
            SetProxy(proxy, "ActionWithReferenceArgumentMethodProxy1", proxyDelegate);

            proxy.ActionWithReferenceArgument(arg);

            Assert.IsTrue(proxyCalled);
            mock.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void InvokeFunctionWithValueReturnType_IMethodProxyWithProxyDelegateCallingTargetAction_CallsRequestedMethodAndProxy()
        {
            int result = 321;
            var mock = new Mock<IMethod>();
            mock.Setup(m => m.FunctionWithValueReturnType()).Returns(result);
            var proxy = CreateProxyInstance(mock.Object);
            bool proxyCalled = false;
            Func<Func<int>, int> proxyDelegate = a =>
            {
                mock.VerifyNoOtherCalls();
                var targetResult = a.Invoke();
                mock.Verify(m => m.FunctionWithValueReturnType(), Times.Once());
                mock.VerifyNoOtherCalls();
                proxyCalled = true;
                return targetResult;
            };
            SetProxy(proxy, "FunctionWithValueReturnTypeMethodProxy0", proxyDelegate);

            Assert.AreEqual(result, proxy.FunctionWithValueReturnType());

            Assert.IsTrue(proxyCalled);
        }

        [TestMethod]
        public void InvokeFunctionWithValueReturnType_IMethodProxyWithProxyDelegateNotCallingTargetAction_CallsRequestedMethodAndProxy()
        {
            int result = 321;
            var mock = new Mock<IMethod>();
            var proxy = CreateProxyInstance(mock.Object);
            bool proxyCalled = false;
            Func<Func<int>, int> proxyDelegate = a =>
            {
                proxyCalled = true;
                return result;
            };
            SetProxy(proxy, "FunctionWithValueReturnTypeMethodProxy0", proxyDelegate);

            Assert.AreEqual(result, proxy.FunctionWithValueReturnType());

            Assert.IsTrue(proxyCalled);
            mock.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void InvokeFunctionWithValueArgumentAndValueReturnType_IMethodProxyWithProxyDelegateCallingTargetAction_CallsRequestedMethodAndProxy()
        {
            int result = 321;   
            var mock = new Mock<IMethod>();
            mock.Setup(m => m.FunctionWithValueArgumentAndValueReturnType(It.IsAny<int>())).Returns(result);
            var proxy = CreateProxyInstance(mock.Object);
            int arg = 123;
            bool proxyCalled = false;
            Func<Func<int, int>, int, int> proxyDelegate = (a, i) =>
            {
                Assert.AreEqual(arg, i);
                mock.VerifyNoOtherCalls();
                var targetResult = a.Invoke(i);
                mock.Verify(m => m.FunctionWithValueArgumentAndValueReturnType(arg), Times.Once());
                mock.VerifyNoOtherCalls();
                proxyCalled = true;
                return targetResult;
            };
            SetProxy(proxy, "FunctionWithValueArgumentAndValueReturnTypeMethodProxy0", proxyDelegate);

            Assert.AreEqual(result, proxy.FunctionWithValueArgumentAndValueReturnType(arg));

            Assert.IsTrue(proxyCalled);
        }

        [TestMethod]
        public void InvokeFunctionWithValueArgumentAndValueReturnType_IMethodProxyWithProxyDelegateNotCallingTargetAction_CallsRequestedMethodAndProxy()
        {
            var mock = new Mock<IMethod>();
            var proxy = CreateProxyInstance(mock.Object);
            int arg = 123;
            int result = 321;
            bool proxyCalled = false;
            Func<Func<int, int>, int, int> proxyDelegate = (a, i) =>
            {
                Assert.AreEqual(arg, i);
                proxyCalled = true;
                return result;
            };
            SetProxy(proxy, "FunctionWithValueArgumentAndValueReturnTypeMethodProxy0", proxyDelegate);

            Assert.AreEqual(result, proxy.FunctionWithValueArgumentAndValueReturnType(arg));

            Assert.IsTrue(proxyCalled);
            mock.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void InvokeFunctionWithValueArgumentAndValueReturnTypeOverloaded_IMethodProxyWithProxyDelegateCallingTargetAction_CallsRequestedMethodAndProxy()
        {
            int result = 321;
            var mock = new Mock<IMethod>();
            mock.Setup(m => m.FunctionWithValueArgumentAndValueReturnType(It.IsAny<uint>())).Returns(result);
            var proxy = CreateProxyInstance(mock.Object);
            uint arg = 123;
            bool proxyCalled = false;
            Func<Func<uint, int>, uint, int> proxyDelegate = (a, ui) =>
            {
                Assert.AreEqual(arg, ui);
                mock.VerifyNoOtherCalls();
                var targetResult = a.Invoke(ui);
                mock.Verify(m => m.FunctionWithValueArgumentAndValueReturnType(arg), Times.Once());
                mock.VerifyNoOtherCalls();
                proxyCalled = true;
                return targetResult;
            };
            SetProxy(proxy, "FunctionWithValueArgumentAndValueReturnTypeMethodProxy1", proxyDelegate);

            Assert.AreEqual(result, proxy.FunctionWithValueArgumentAndValueReturnType(arg));

            Assert.IsTrue(proxyCalled);
        }

        [TestMethod]
        public void InvokeFunctionWithValueArgumentAndValueReturnTypeOverloaded_IMethodProxyWithProxyDelegateNotCallingTargetAction_CallsRequestedMethodAndProxy()
        {
            int result = 321;
            var mock = new Mock<IMethod>();
            var proxy = CreateProxyInstance(mock.Object);
            uint arg = 123;
            bool proxyCalled = false;
            Func<Func<uint, int>, uint, int> proxyDelegate = (a, ui) =>
            {
                proxyCalled = true;
                return result;
            };
            SetProxy(proxy, "FunctionWithValueArgumentAndValueReturnTypeMethodProxy1", proxyDelegate);

            Assert.AreEqual(result, proxy.FunctionWithValueArgumentAndValueReturnType(arg));

            Assert.IsTrue(proxyCalled);
            mock.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void InvokeFunctionWithReferenceArgumentAndValueReturnType_IMethodProxyWithProxyDelegateCallingTargetAction_CallsRequestedMethodAndProxy()
        {
            int result = 321;
            var mock = new Mock<IMethod>();
            mock.Setup(m => m.FunctionWithReferenceArgumentAndValueReturnType(It.IsAny<object>())).Returns(result);
            var proxy = CreateProxyInstance(mock.Object);
            object arg = new object();
            bool proxyCalled = false;
            Func<Func<object, int>, object, int> proxyDelegate = (a, o) =>
            {
                Assert.AreEqual(arg, o);
                mock.VerifyNoOtherCalls();
                var targetResult = a.Invoke(o);
                mock.Verify(m => m.FunctionWithReferenceArgumentAndValueReturnType(arg), Times.Once());
                mock.VerifyNoOtherCalls();
                proxyCalled = true;
                return targetResult;
            };
            SetProxy(proxy, "FunctionWithReferenceArgumentAndValueReturnTypeMethodProxy0", proxyDelegate);

            Assert.AreEqual(result, proxy.FunctionWithReferenceArgumentAndValueReturnType(arg));

            Assert.IsTrue(proxyCalled);
        }

        [TestMethod]
        public void InvokeFunctionWithReferenceArgumentAndValueReturnType_IMethodProxyWithProxyDelegateNotCallingTargetAction_CallsRequestedMethodAndProxy()
        {
            var mock = new Mock<IMethod>();
            var proxy = CreateProxyInstance(mock.Object);
            object arg = new object();
            int result = 321;
            bool proxyCalled = false;
            Func<Func<object, int>, object, int> proxyDelegate = (a, i) =>
            {
                Assert.AreEqual(arg, i);
                proxyCalled = true;
                return result;
            };
            SetProxy(proxy, "FunctionWithReferenceArgumentAndValueReturnTypeMethodProxy0", proxyDelegate);

            Assert.AreEqual(result, proxy.FunctionWithReferenceArgumentAndValueReturnType(arg));

            Assert.IsTrue(proxyCalled);
            mock.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void InvokeFunctionWithReferenceArgumentAndValueReturnTypeOverloaded_IMethodProxyWithProxyDelegateCallingTargetAction_CallsRequestedMethodAndProxy()
        {
            int result = 321;
            var mock = new Mock<IMethod>();
            mock.Setup(m => m.FunctionWithReferenceArgumentAndValueReturnType(It.IsAny<string>())).Returns(result);
            var proxy = CreateProxyInstance(mock.Object);
            string arg = "TestString";
            bool proxyCalled = false;
            Func<Func<string, int>, string, int> proxyDelegate = (a, s) =>
            {
                Assert.AreEqual(arg, s);
                mock.VerifyNoOtherCalls();
                var targetResult = a.Invoke(s);
                mock.Verify(m => m.FunctionWithReferenceArgumentAndValueReturnType(arg), Times.Once());
                mock.VerifyNoOtherCalls();
                proxyCalled = true;
                return targetResult;
            };
            SetProxy(proxy, "FunctionWithReferenceArgumentAndValueReturnTypeMethodProxy1", proxyDelegate);

            Assert.AreEqual(result, proxy.FunctionWithReferenceArgumentAndValueReturnType(arg));

            Assert.IsTrue(proxyCalled);
        }

        [TestMethod]
        public void InvokeFunctionWithReferenceArgumentAndValueReturnTypeOverloaded_IMethodProxyWithProxyDelegateNonCallingTargetAction_CallsRequestedMethodAndProxy()
        {
            int result = 321;
            var mock = new Mock<IMethod>();
            var proxy = CreateProxyInstance(mock.Object);
            string arg = "TestString";
            bool proxyCalled = false;
            Func<Func<string, int>, string, int> proxyDelegate = (a, s) =>
            {
                proxyCalled = true;
                return result;
            };
            SetProxy(proxy, "FunctionWithReferenceArgumentAndValueReturnTypeMethodProxy1", proxyDelegate);

            Assert.AreEqual(result, proxy.FunctionWithReferenceArgumentAndValueReturnType(arg));

            Assert.IsTrue(proxyCalled);
            mock.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void InvokeFunctionWithReferenceReturnType_IMethodProxyWithProxyDelegateCallingTargetAction_CallsRequestedMethodAndProxy()
        {
            object result = new object();
            var mock = new Mock<IMethod>();
            mock.Setup(m => m.FunctionWithReferenceReturnType()).Returns(result);
            var proxy = CreateProxyInstance(mock.Object);
            bool proxyCalled = false;
            Func<Func<object>, object> proxyDelegate = a =>
            {
                mock.VerifyNoOtherCalls();
                var targetResult = a.Invoke();
                mock.Verify(m => m.FunctionWithReferenceReturnType(), Times.Once());
                mock.VerifyNoOtherCalls();
                proxyCalled = true;
                return targetResult;
            };
            SetProxy(proxy, "FunctionWithReferenceReturnTypeMethodProxy0", proxyDelegate);

            Assert.AreEqual(result, proxy.FunctionWithReferenceReturnType());

            Assert.IsTrue(proxyCalled);
        }

        [TestMethod]
        public void InvokeFunctionWithReferenceReturnType_IMethodProxyWithProxyDelegateNotCallingTargetAction_CallsRequestedMethodAndProxy()
        {
            object result = new object();
            var mock = new Mock<IMethod>();
            var proxy = CreateProxyInstance(mock.Object);
            bool proxyCalled = false;
            Func<Func<object>, object> proxyDelegate = a =>
            {
                proxyCalled = true;
                return result;
            };
            SetProxy(proxy, "FunctionWithReferenceReturnTypeMethodProxy0", proxyDelegate);

            Assert.AreEqual(result, proxy.FunctionWithReferenceReturnType());

            Assert.IsTrue(proxyCalled);
            mock.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void InvokeFunctionWithValueArgumentAndReferenceReturnType_IMethodProxyWithProxyDelegateCallingTargetAction_CallsRequestedMethodAndProxy()
        {
            object result = new object();
            var mock = new Mock<IMethod>();
            mock.Setup(m => m.FunctionWithValueArgumentAndReferenceReturnType(It.IsAny<int>())).Returns(result);
            var proxy = CreateProxyInstance(mock.Object);
            int arg = 123;
            bool proxyCalled = false;
            Func<Func<int, object>, int, object> proxyDelegate = (a, i) =>
            {
                Assert.AreEqual(arg, i);
                mock.VerifyNoOtherCalls();
                var targetResult = a.Invoke(i);
                mock.Verify(m => m.FunctionWithValueArgumentAndReferenceReturnType(arg), Times.Once());
                mock.VerifyNoOtherCalls();
                proxyCalled = true;
                return targetResult;
            };
            SetProxy(proxy, "FunctionWithValueArgumentAndReferenceReturnTypeMethodProxy0", proxyDelegate);

            Assert.AreEqual(result, proxy.FunctionWithValueArgumentAndReferenceReturnType(arg));

            Assert.IsTrue(proxyCalled);
        }

        [TestMethod]
        public void InvokeFunctionWithValueArgumentAndReferenceReturnType_IMethodProxyWithProxyDelegateNotCallingTargetAction_CallsRequestedMethodAndProxy()
        {
            object result = new object();
            var mock = new Mock<IMethod>();
            var proxy = CreateProxyInstance(mock.Object);
            int arg = 123;
            bool proxyCalled = false;
            Func<Func<int, object>, int, object> proxyDelegate = (a, i) =>
            {
                Assert.AreEqual(arg, i);
                proxyCalled = true;
                return result;
            };
            SetProxy(proxy, "FunctionWithValueArgumentAndReferenceReturnTypeMethodProxy0", proxyDelegate);

            Assert.AreEqual(result, proxy.FunctionWithValueArgumentAndReferenceReturnType(arg));

            Assert.IsTrue(proxyCalled);
            mock.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void InvokeFunctionWithValueArgumentAndReferenceReturnTypeOverloaded_IMethodProxyWithProxyDelegateCallingTargetAction_CallsRequestedMethodAndProxy()
        {
            object result = new object();
            var mock = new Mock<IMethod>();
            mock.Setup(m => m.FunctionWithValueArgumentAndReferenceReturnType(It.IsAny<uint>())).Returns(result);
            var proxy = CreateProxyInstance(mock.Object);
            uint arg = 123;
            bool proxyCalled = false;
            Func<Func<uint, object>, uint, object> proxyDelegate = (a, ui) =>
            {
                Assert.AreEqual(arg, ui);
                mock.VerifyNoOtherCalls();
                var targetResult = a.Invoke(ui);
                mock.Verify(m => m.FunctionWithValueArgumentAndReferenceReturnType(arg), Times.Once());
                mock.VerifyNoOtherCalls();
                proxyCalled = true;
                return targetResult;
            };
            SetProxy(proxy, "FunctionWithValueArgumentAndReferenceReturnTypeMethodProxy1", proxyDelegate);

            Assert.AreEqual(result, proxy.FunctionWithValueArgumentAndReferenceReturnType(arg));

            Assert.IsTrue(proxyCalled);
        }

        [TestMethod]
        public void InvokeFunctionWithValueArgumentAndReferenceReturnTypeOverloaded_IMethodProxyWithProxyDelegateNotCallingTargetAction_CallsRequestedMethodAndProxy()
        {
            object result = new object();
            var mock = new Mock<IMethod>();
            var proxy = CreateProxyInstance(mock.Object);
            uint arg = 123;
            bool proxyCalled = false;
            Func<Func<uint, object>, uint, object> proxyDelegate = (a, ui) =>
            {
                proxyCalled = true;
                return result;
            };
            SetProxy(proxy, "FunctionWithValueArgumentAndReferenceReturnTypeMethodProxy1", proxyDelegate);

            Assert.AreEqual(result, proxy.FunctionWithValueArgumentAndReferenceReturnType(arg));

            Assert.IsTrue(proxyCalled);
            mock.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void InvokeFunctionWithReferenceArgumentAndReferenceReturnType_IMethodProxyWithProxyDelegateCallingTargetAction_CallsRequestedMethodAndProxy()
        {
            object result = new object();
            var mock = new Mock<IMethod>();
            mock.Setup(m => m.FunctionWithReferenceArgumentAndReferenceReturnType(It.IsAny<object>())).Returns(result);
            var proxy = CreateProxyInstance(mock.Object);
            object arg = new object();
            bool proxyCalled = false;
            Func<Func<object, object>, object, object> proxyDelegate = (a, o) =>
            {
                Assert.AreEqual(arg, o);
                mock.VerifyNoOtherCalls();
                var targetResult = a.Invoke(o);
                mock.Verify(m => m.FunctionWithReferenceArgumentAndReferenceReturnType(arg), Times.Once());
                mock.VerifyNoOtherCalls();
                proxyCalled = true;
                return targetResult;
            };
            SetProxy(proxy, "FunctionWithReferenceArgumentAndReferenceReturnTypeMethodProxy0", proxyDelegate);

            Assert.AreEqual(result, proxy.FunctionWithReferenceArgumentAndReferenceReturnType(arg));

            Assert.IsTrue(proxyCalled);
        }

        [TestMethod]
        public void InvokeFunctionWithReferenceArgumentAndReferenceReturnType_IMethodProxyWithProxyDelegateNotCallingTargetAction_CallsRequestedMethodAndProxy()
        {
            object result = new object();
            var mock = new Mock<IMethod>();
            var proxy = CreateProxyInstance(mock.Object);
            object arg = new object();
            bool proxyCalled = false;
            Func<Func<object, object>, object, object> proxyDelegate = (a, i) =>
            {
                Assert.AreEqual(arg, i);
                proxyCalled = true;
                return result;
            };
            SetProxy(proxy, "FunctionWithReferenceArgumentAndReferenceReturnTypeMethodProxy0", proxyDelegate);

            Assert.AreEqual(result, proxy.FunctionWithReferenceArgumentAndReferenceReturnType(arg));

            Assert.IsTrue(proxyCalled);
            mock.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void InvokeFunctionWithReferenceArgumentAndReferenceReturnTypeOverloaded_IMethodProxyWithProxyDelegateCallingTargetAction_CallsRequestedMethodAndProxy()
        {
            object result = new object();
            var mock = new Mock<IMethod>();
            mock.Setup(m => m.FunctionWithReferenceArgumentAndReferenceReturnType(It.IsAny<string>())).Returns(result);
            var proxy = CreateProxyInstance(mock.Object);
            string arg = "TestString";
            bool proxyCalled = false;
            Func<Func<string, object>, string, object> proxyDelegate = (a, s) =>
            {
                Assert.AreEqual(arg, s);
                mock.VerifyNoOtherCalls();
                var targetResult = a.Invoke(s);
                mock.Verify(m => m.FunctionWithReferenceArgumentAndReferenceReturnType(arg), Times.Once());
                mock.VerifyNoOtherCalls();
                proxyCalled = true;
                return targetResult;
            };
            SetProxy(proxy, "FunctionWithReferenceArgumentAndReferenceReturnTypeMethodProxy1", proxyDelegate);

            Assert.AreEqual(result, proxy.FunctionWithReferenceArgumentAndReferenceReturnType(arg));

            Assert.IsTrue(proxyCalled);
        }

        [TestMethod]
        public void InvokeFunctionWithReferenceArgumentAndReferenceReturnTypeOverloaded_IMethodProxyWithProxyDelegateNonCallingTargetAction_CallsRequestedMethodAndProxy()
        {
            object result = new object();
            var mock = new Mock<IMethod>();
            var proxy = CreateProxyInstance(mock.Object);
            string arg = "TestString";
            bool proxyCalled = false;
            Func<Func<string, object>, string, object> proxyDelegate = (a, s) =>
            {
                proxyCalled = true;
                return result;
            };
            SetProxy(proxy, "FunctionWithReferenceArgumentAndReferenceReturnTypeMethodProxy1", proxyDelegate);

            Assert.AreEqual(result, proxy.FunctionWithReferenceArgumentAndReferenceReturnType(arg));

            Assert.IsTrue(proxyCalled);
            mock.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void InvokeFunctionWithTupleReturnType_IMethodProxyWithProxyDelegateCallingTargetAction_CallsRequestedMethodAndProxy()
        {
            var result = new ValueTuple<int, object>(321, new object());
            var mock = new Mock<IMethod>();
            mock.Setup(m => m.FunctionWithTupleReturnType()).Returns(result);
            var proxy = CreateProxyInstance(mock.Object);
            bool proxyCalled = false;
            Func<Func<ValueTuple<int, object>>, ValueTuple<int, object>> proxyDelegate = a =>
            {
                mock.VerifyNoOtherCalls();
                var targetResult = a.Invoke();
                mock.Verify(m => m.FunctionWithTupleReturnType(), Times.Once());
                mock.VerifyNoOtherCalls();
                proxyCalled = true;
                return targetResult;
            };
            SetProxy(proxy, "FunctionWithTupleReturnTypeMethodProxy0", proxyDelegate);

            Assert.AreEqual(result, proxy.FunctionWithTupleReturnType());

            Assert.IsTrue(proxyCalled);
        }

        [TestMethod]
        public void InvokeFunctionWithTupleReturnType_IMethodProxyWithProxyDelegateNotCallingTargetAction_CallsRequestedMethodAndProxy()
        {
            var result = new ValueTuple<int, object>(321, new object());
            var mock = new Mock<IMethod>();
            var proxy = CreateProxyInstance(mock.Object);
            bool proxyCalled = false;
            Func<Func<ValueTuple<int, object>>, ValueTuple<int, object>> proxyDelegate = a =>
            {
                proxyCalled = true;
                return result;
            };
            SetProxy(proxy, "FunctionWithTupleReturnTypeMethodProxy0", proxyDelegate);

            Assert.AreEqual(result, proxy.FunctionWithTupleReturnType());

            Assert.IsTrue(proxyCalled);
            mock.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void InvokeFunctionWithValueArgumentAndTupleReturnType_IMethodProxyWithProxyDelegateCallingTargetAction_CallsRequestedMethodAndProxy()
        {
            var result = new ValueTuple<int, object>(321, new object());
            var mock = new Mock<IMethod>();
            mock.Setup(m => m.FunctionWithValueArgumentAndTupleReturnType(It.IsAny<int>())).Returns(result);
            var proxy = CreateProxyInstance(mock.Object);
            int arg = 123;
            bool proxyCalled = false;
            Func<Func<int, ValueTuple<int, object>>, int, ValueTuple<int, object>> proxyDelegate = (a, i) =>
            {
                Assert.AreEqual(arg, i);
                mock.VerifyNoOtherCalls();
                var targetResult = a.Invoke(i);
                mock.Verify(m => m.FunctionWithValueArgumentAndTupleReturnType(arg), Times.Once());
                mock.VerifyNoOtherCalls();
                proxyCalled = true;
                return targetResult;
            };
            SetProxy(proxy, "FunctionWithValueArgumentAndTupleReturnTypeMethodProxy0", proxyDelegate);

            Assert.AreEqual(result, proxy.FunctionWithValueArgumentAndTupleReturnType(arg));

            Assert.IsTrue(proxyCalled);
        }

        [TestMethod]
        public void InvokeFunctionWithValueArgumentAndTupleReturnType_IMethodProxyWithProxyDelegateNotCallingTargetAction_CallsRequestedMethodAndProxy()
        {
            var result = new ValueTuple<int, object>(321, new object());
            var mock = new Mock<IMethod>();
            var proxy = CreateProxyInstance(mock.Object);
            int arg = 123;
            bool proxyCalled = false;
            Func<Func<int, ValueTuple<int, object>>, int, ValueTuple<int, object>> proxyDelegate = (a, i) =>
            {
                Assert.AreEqual(arg, i);
                proxyCalled = true;
                return result;
            };
            SetProxy(proxy, "FunctionWithValueArgumentAndTupleReturnTypeMethodProxy0", proxyDelegate);

            Assert.AreEqual(result, proxy.FunctionWithValueArgumentAndTupleReturnType(arg));

            Assert.IsTrue(proxyCalled);
            mock.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void InvokeFunctionWithValueArgumentAndTupleReturnTypeOverloaded_IMethodProxyWithProxyDelegateCallingTargetAction_CallsRequestedMethodAndProxy()
        {
            var result = new ValueTuple<int, object>(321, new object());
            var mock = new Mock<IMethod>();
            mock.Setup(m => m.FunctionWithValueArgumentAndTupleReturnType(It.IsAny<uint>())).Returns(result);
            var proxy = CreateProxyInstance(mock.Object);
            uint arg = 123;
            bool proxyCalled = false;
            Func<Func<uint, ValueTuple<int, object>>, uint, ValueTuple<int, object>> proxyDelegate = (a, ui) =>
            {
                Assert.AreEqual(arg, ui);
                mock.VerifyNoOtherCalls();
                var targetResult = a.Invoke(ui);
                mock.Verify(m => m.FunctionWithValueArgumentAndTupleReturnType(arg), Times.Once());
                mock.VerifyNoOtherCalls();
                proxyCalled = true;
                return targetResult;
            };
            SetProxy(proxy, "FunctionWithValueArgumentAndTupleReturnTypeMethodProxy1", proxyDelegate);

            Assert.AreEqual(result, proxy.FunctionWithValueArgumentAndTupleReturnType(arg));

            Assert.IsTrue(proxyCalled);
        }

        [TestMethod]
        public void InvokeFunctionWithValueArgumentAndTupleReturnTypeOverloaded_IMethodProxyWithProxyDelegateNotCallingTargetAction_CallsRequestedMethodAndProxy()
        {
            var result = new ValueTuple<int, object>(321, new object());
            var mock = new Mock<IMethod>();
            var proxy = CreateProxyInstance(mock.Object);
            uint arg = 123;
            bool proxyCalled = false;
            Func<Func<uint, ValueTuple<int, object>>, uint, ValueTuple<int, object>> proxyDelegate = (a, ui) =>
            {
                proxyCalled = true;
                return result;
            };
            SetProxy(proxy, "FunctionWithValueArgumentAndTupleReturnTypeMethodProxy1", proxyDelegate);

            Assert.AreEqual(result, proxy.FunctionWithValueArgumentAndTupleReturnType(arg));

            Assert.IsTrue(proxyCalled);
            mock.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void InvokeFunctionWithReferenceArgumentAndTupleReturnType_IMethodProxyWithProxyDelegateCallingTargetAction_CallsRequestedMethodAndProxy()
        {
            var result = new ValueTuple<int, object>(321, new object());
            var mock = new Mock<IMethod>();
            mock.Setup(m => m.FunctionWithReferenceArgumentAndTupleReturnType(It.IsAny<object>())).Returns(result);
            var proxy = CreateProxyInstance(mock.Object);
            object arg = new object();
            bool proxyCalled = false;
            Func<Func<object, ValueTuple<int, object>>, object, ValueTuple<int, object>> proxyDelegate = (a, o) =>
            {
                Assert.AreEqual(arg, o);
                mock.VerifyNoOtherCalls();
                var targetResult = a.Invoke(o);
                mock.Verify(m => m.FunctionWithReferenceArgumentAndTupleReturnType(arg), Times.Once());
                mock.VerifyNoOtherCalls();
                proxyCalled = true;
                return targetResult;
            };
            SetProxy(proxy, "FunctionWithReferenceArgumentAndTupleReturnTypeMethodProxy0", proxyDelegate);

            Assert.AreEqual(result, proxy.FunctionWithReferenceArgumentAndTupleReturnType(arg));

            Assert.IsTrue(proxyCalled);
        }

        [TestMethod]
        public void InvokeFunctionWithReferenceArgumentAndTupleReturnType_IMethodProxyWithProxyDelegateNotCallingTargetAction_CallsRequestedMethodAndProxy()
        {
            var result = new ValueTuple<int, object>(321, new object());
            var mock = new Mock<IMethod>();
            var proxy = CreateProxyInstance(mock.Object);
            object arg = new object();
            bool proxyCalled = false;
            Func<Func<object, ValueTuple<int, object>>, object, ValueTuple<int, object>> proxyDelegate = (a, i) =>
            {
                Assert.AreEqual(arg, i);
                proxyCalled = true;
                return result;
            };
            SetProxy(proxy, "FunctionWithReferenceArgumentAndTupleReturnTypeMethodProxy0", proxyDelegate);

            Assert.AreEqual(result, proxy.FunctionWithReferenceArgumentAndTupleReturnType(arg));

            Assert.IsTrue(proxyCalled);
            mock.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void InvokeFunctionWithReferenceArgumentAndTupleReturnTypeOverloaded_IMethodProxyWithProxyDelegateCallingTargetAction_CallsRequestedMethodAndProxy()
        {
            var result = new ValueTuple<int, object>(321, new object());
            var mock = new Mock<IMethod>();
            mock.Setup(m => m.FunctionWithReferenceArgumentAndTupleReturnType(It.IsAny<string>())).Returns(result);
            var proxy = CreateProxyInstance(mock.Object);
            string arg = "TestString";
            bool proxyCalled = false;
            Func<Func<string, ValueTuple<int, object>>, string, ValueTuple<int, object>> proxyDelegate = (a, s) =>
            {
                Assert.AreEqual(arg, s);
                mock.VerifyNoOtherCalls();
                var targetResult = a.Invoke(s);
                mock.Verify(m => m.FunctionWithReferenceArgumentAndTupleReturnType(arg), Times.Once());
                mock.VerifyNoOtherCalls();
                proxyCalled = true;
                return targetResult;
            };
            SetProxy(proxy, "FunctionWithReferenceArgumentAndTupleReturnTypeMethodProxy1", proxyDelegate);

            Assert.AreEqual(result, proxy.FunctionWithReferenceArgumentAndTupleReturnType(arg));

            Assert.IsTrue(proxyCalled);
        }

        [TestMethod]
        public void InvokeFunctionWithReferenceArgumentAndTupleReturnTypeOverloaded_IMethodProxyWithProxyDelegateNonCallingTargetAction_CallsRequestedMethodAndProxy()
        {
            var result = new ValueTuple<int, object>(321, new object());
            var mock = new Mock<IMethod>();
            var proxy = CreateProxyInstance(mock.Object);
            string arg = "TestString";
            bool proxyCalled = false;
            Func<Func<string, ValueTuple<int, object>>, string, ValueTuple<int, object>> proxyDelegate = (a, s) =>
            {
                proxyCalled = true;
                return result;
            };
            SetProxy(proxy, "FunctionWithReferenceArgumentAndTupleReturnTypeMethodProxy1", proxyDelegate);

            Assert.AreEqual(result, proxy.FunctionWithReferenceArgumentAndTupleReturnType(arg));

            Assert.IsTrue(proxyCalled);
            mock.VerifyNoOtherCalls();
        }

        #endregion Call with Proxy

        #region Call with Throwing Exception

        [TestMethod]
        [ExpectedException(typeof(TestException))]
        public void InvokeAction_IMethodProxyDecoratorWithBeforeDecoratorThrowingException_ThrowsExpectedException()
        {
            var mock = new Mock<IMethod>();
            var proxy = CreateProxyInstance(mock.Object);
            Action beforeDecorator = () =>
            {
                throw new TestException();
            };
            AddBeforeDecorator(proxy, "ActionMethodDecorators0", beforeDecorator);

            proxy.Action();
        }

        [TestMethod]
        [ExpectedException(typeof(TestException))]
        public void InvokeAction_IMethodProxyWithProxyDelegateThrowingException_ThrowsExpectedException()
        {
            var mock = new Mock<IMethod>();
            var proxy = CreateProxyInstance(mock.Object);
            Action<Action> proxyDelegate = a =>
            {
                throw new TestException();
            };
            SetProxy(proxy, "ActionMethodProxy0", proxyDelegate);

            proxy.Action();
        }

        [TestMethod]
        [ExpectedException(typeof(TestException))]
        public void InvokeAction_IMethodProxyDecoratorWithTargetThrowingException_ThrowsExpectedException()
        {
            var mock = new Mock<IMethod>();
            mock.Setup(m => m.Action()).Throws<TestException>();
            var proxy = CreateProxyInstance(mock.Object);

            proxy.Action();
        }

        [TestMethod]
        [ExpectedException(typeof(TestException))]
        public void InvokeAction_IMethodProxyDecoratorWithAfterDecoratorThrowingException_ThrowsExpectedException()
        {
            var mock = new Mock<IMethod>();
            var proxy = CreateProxyInstance(mock.Object);
            Action afterDecorator = () =>
            {
                throw new TestException();
            };
            AddAfterDecorator(proxy, "ActionMethodDecorators0", afterDecorator);

            proxy.Action();
        }

        [TestMethod]
        [ExpectedException(typeof(TestException))]
        public void InvokeFunctionWithReferenceArgumentAndTupleReturnType_IMethodProxyDecoratorWithBeforeDecoratorThrowingException_ThrowsExpectedException()
        {
            var mock = new Mock<IMethod>();
            var proxy = CreateProxyInstance(mock.Object);
            Action<string> beforeDecorator = o =>
            {
                throw new TestException();
            };
            AddBeforeDecorator<Action<string>, Action<string, ValueTuple<int, object>>>(proxy, "FunctionWithReferenceArgumentAndTupleReturnTypeMethodDecorators1", beforeDecorator);

            proxy.FunctionWithReferenceArgumentAndTupleReturnType(default);
        }

        [TestMethod]
        [ExpectedException(typeof(TestException))]
        public void InvokeFunctionWithReferenceArgumentAndTupleReturnType_IMethodProxyWithProxyDelegateThrowingException_ThrowsExpectedException()
        {
            var mock = new Mock<IMethod>();
            var proxy = CreateProxyInstance(mock.Object);
            Func<Func<string, ValueTuple<int, object>>, string, ValueTuple<int, object>> proxyDelegate = (a, o) =>
            {
                throw new TestException();
            };
            SetProxy(proxy, "FunctionWithReferenceArgumentAndTupleReturnTypeMethodProxy1", proxyDelegate);

            proxy.FunctionWithReferenceArgumentAndTupleReturnType(default);
        }

        [TestMethod]
        [ExpectedException(typeof(TestException))]
        public void InvokeFunctionWithReferenceArgumentAndTupleReturnType_IMethodProxyDecoratorWithTargetThrowingException_ThrowsExpectedException()
        {
            var mock = new Mock<IMethod>();
            mock.Setup(m => m.FunctionWithReferenceArgumentAndTupleReturnType(It.IsAny<string>())).Throws<TestException>();
            var proxy = CreateProxyInstance(mock.Object);

            proxy.FunctionWithReferenceArgumentAndTupleReturnType(default);
        }

        [TestMethod]
        [ExpectedException(typeof(TestException))]
        public void InvokeFunctionWithReferenceArgumentAndTupleReturnType_IMethodProxyDecoratorWithAfterDecoratorThrowingException_ThrowsExpectedException()
        {
            var mock = new Mock<IMethod>();
            var proxy = CreateProxyInstance(mock.Object);
            Action<string, ValueTuple<int, object>> afterDecorator = (o, r) =>
            {
                throw new TestException();
            };
            AddAfterDecorator<Action<string>, Action<string, ValueTuple<int, object>>>(proxy, "FunctionWithReferenceArgumentAndTupleReturnTypeMethodDecorators1", afterDecorator);

            proxy.FunctionWithReferenceArgumentAndTupleReturnType(default);
        }

        [TestMethod]
        [ExpectedException(typeof(TestException))]
        public void InvokeAction_IMethodProxyPreviousBeforeDecoratorThrowsException_NextPairedAfterDecoratorNotExecuted()
        {
            var mock = new Mock<IMethod>();
            var proxy = CreateProxyInstance(mock.Object);
            Action beforeDecoratorThrowingException = () =>
            {
                throw new TestException();
            };
            AddBeforeDecorator(proxy, "ActionMethodDecorators0", beforeDecoratorThrowingException);
            Action beforePairedDecorator = () =>
            {

            };
            bool afterDecoratorExecuted = false;
            Action afterPairedDecorator = () =>
            {
                afterDecoratorExecuted = true;
            };
            AddDecorators<Action>(proxy, "ActionMethodDecorators0", beforePairedDecorator, afterPairedDecorator);

            try
            {
                proxy.Action();
            }
            finally
            {
                Assert.IsFalse(afterDecoratorExecuted);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(TestException))]
        public void InvokeAction_IMethodProxyPreviousBeforeDecoratorThrowsException_NextNotPairedAfterDecoratorNotExecuted()
        {
            var mock = new Mock<IMethod>();
            var proxy = CreateProxyInstance(mock.Object);
            Action beforeDecoratorThrowingException = () =>
            {
                throw new TestException();
            };
            AddBeforeDecorator(proxy, "ActionMethodDecorators0", beforeDecoratorThrowingException);
            bool afterDecoratorExecuted = false;
            Action afterNotPairedDecorator = () =>
            {
                afterDecoratorExecuted = true;
            };
            AddAfterDecorator<Action>(proxy, "ActionMethodDecorators0", afterNotPairedDecorator);

            try
            {
                proxy.Action();
            }
            finally
            {
                Assert.IsFalse(afterDecoratorExecuted);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(TestException))]
        public void InvokeAction_IMethodProxyPairDecoratorThrowsException_PairAfterDecoratorExecuted()
        {
            var mock = new Mock<IMethod>();
            var proxy = CreateProxyInstance(mock.Object);
            Action beforePairedDecorator = () =>
            {
                throw new TestException();
            };
            bool afterDecoratorExecuted = false;
            Action afterPairedDecorator = () =>
            {
                afterDecoratorExecuted = true;
            };
            AddDecorators<Action>(proxy, "ActionMethodDecorators0", beforePairedDecorator, afterPairedDecorator);

            try
            {
                proxy.Action();
            }
            finally
            {
                Assert.IsTrue(afterDecoratorExecuted);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(TestException))]
        public void InvokeAction_IMethodProxyNextBeforeDecoratorThrowsException_PreviousPairedAfterDecoratorExecuted()
        {
            var mock = new Mock<IMethod>();
            var proxy = CreateProxyInstance(mock.Object);
            Action beforePairedDecorator = () =>
            {

            };
            bool afterDecoratorExecuted = false;
            Action afterPairDecorator = () =>
            {
                afterDecoratorExecuted = true;
            };
            AddDecorators<Action>(proxy, "ActionMethodDecorators0", beforePairedDecorator, afterPairDecorator);
            Action beforeDecoratorThrowingException = () =>
            {
                throw new TestException();
            };
            AddBeforeDecorator(proxy, "ActionMethodDecorators0", beforeDecoratorThrowingException);

            try
            {
                proxy.Action();
            }
            finally
            {
                Assert.IsTrue(afterDecoratorExecuted);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(TestException))]
        public void InvokeAction_IMethodProxyNextBeforeDecoratorThrowsException_PreviousNotPairedAfterDecoratorNotExecuted()
        {
            var mock = new Mock<IMethod>();
            var proxy = CreateProxyInstance(mock.Object);
            bool afterDecoratorExecuted = false;
            Action afterNotPairedDecorator = () =>
            {
                afterDecoratorExecuted = true;
            };
            AddAfterDecorator(proxy, "ActionMethodDecorators0", afterNotPairedDecorator);
            Action beforeDecoratorThrowingException = () =>
            {
                throw new TestException();
            };
            AddBeforeDecorator(proxy, "ActionMethodDecorators0", beforeDecoratorThrowingException);

            try
            {
                proxy.Action();
            }
            finally
            {
                Assert.IsFalse(afterDecoratorExecuted);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(TestException))]
        public void InvokeAction_IMethodProxyWhenProxyDelegateThrowsException_PairedAfterDecoratorExecuted()
        {
            var mock = new Mock<IMethod>();
            var proxy = CreateProxyInstance(mock.Object);
            bool afterDecoratorExecuted = false;
            Action beforePairedDecorator = () =>
            {

            };
            Action afterPairedDecorator = () =>
            {
                afterDecoratorExecuted = true;
            };
            AddDecorators<Action, Action>(proxy, "ActionMethodDecorators0", beforePairedDecorator, afterPairedDecorator);
            Action<Action> proxyDelegatedThrowingException = a =>
            {
                throw new TestException();
            };
            SetProxy(proxy, "ActionMethodProxy0", proxyDelegatedThrowingException);

            try
            {
                proxy.Action();
            }
            finally
            {
                Assert.IsTrue(afterDecoratorExecuted);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(TestException))]
        public void InvokeAction_IMethodProxyWhenProxyDelegateThrowsException_NotPairedAfterDecoratorNotExecuted()
        {
            var mock = new Mock<IMethod>();
            var proxy = CreateProxyInstance(mock.Object);
            bool afterDecoratorExecuted = false;
            Action afterNotPairedDecorator = () =>
            {
                afterDecoratorExecuted = true;
            };
            AddAfterDecorator(proxy, "ActionMethodDecorators0", afterNotPairedDecorator);
            Action<Action> proxyDelegatedThrowingException = a =>
            {
                throw new TestException();
            };
            SetProxy(proxy, "ActionMethodProxy0", proxyDelegatedThrowingException);

            try
            {
                proxy.Action();
            }
            finally
            {
                Assert.IsFalse(afterDecoratorExecuted);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(TestException))]
        public void InvokeAction_IMethodProxyTargetThrowingException_PairedAfterDecoratorExecuted()
        {
            var mock = new Mock<IMethod>();
            mock.Setup(m => m.Action()).Throws<TestException>();
            var proxy = CreateProxyInstance(mock.Object);
            bool afterDecoratorExecuted = false;
            Action beforePairedDecorator = () =>
            {

            };
            Action afterPairedDecorator = () =>
            {
                afterDecoratorExecuted = true;
            };
            AddDecorators<Action, Action>(proxy, "ActionMethodDecorators0", beforePairedDecorator, afterPairedDecorator);

            try
            {
                proxy.Action();
            }
            finally
            {
                Assert.IsTrue(afterDecoratorExecuted);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(TestException))]
        public void InvokeAction_IMethodProxyTargetThrowingException_NotPairedAfterDecoratorNotExecuted()
        {
            var mock = new Mock<IMethod>();
            mock.Setup(m => m.Action()).Throws<TestException>();
            var proxy = CreateProxyInstance(mock.Object);
            bool afterDecoratorExecuted = false;
            Action afterNotPairedDecorator = () =>
            {
                afterDecoratorExecuted = true;
            };
            AddAfterDecorator<Action, Action>(proxy, "ActionMethodDecorators0", afterNotPairedDecorator);

            try
            {
                proxy.Action();
            }
            finally
            {
                Assert.IsFalse(afterDecoratorExecuted);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(TestException))]
        public void InvokeAction_IMethodProxyPreviousAfterDecoratorThrowingException_PairedAfterDecoratorExecuted()
        {
            var mock = new Mock<IMethod>();
            var proxy = CreateProxyInstance(mock.Object);
            bool afterDecoratorExecuted = false;
            Action beforePairedDecorator = () =>
            {

            };
            Action afterPairedDecorator = () =>
            {
                afterDecoratorExecuted = true;
            };
            AddDecorators<Action, Action>(proxy, "ActionMethodDecorators0", beforePairedDecorator, afterPairedDecorator);
            Action afterDecoratorThrowingException = () =>
            {
                throw new TestException();
            };
            AddAfterDecorator(proxy, "ActionMethodDecorators0", afterDecoratorThrowingException);

            try
            {
                proxy.Action();
            }
            finally
            {
                Assert.IsTrue(afterDecoratorExecuted);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(TestException))]
        public void InvokeAction_IMethodProxyPreviousAfterDecoratorThrowingException_NotPairedAfterDecoratorNotExecuted()
        {
            var mock = new Mock<IMethod>();
            var proxy = CreateProxyInstance(mock.Object);
            bool afterDecoratorExecuted = false;
            Action afterPairedDecorator = () =>
            {
                afterDecoratorExecuted = true;
            };
            AddAfterDecorator(proxy, "ActionMethodDecorators0", afterPairedDecorator);
            Action afterDecoratorThrowingException = () =>
            {
                throw new TestException();
            };
            AddAfterDecorator(proxy, "ActionMethodDecorators0", afterDecoratorThrowingException);

            try
            {
                proxy.Action();
            }
            finally
            {
                Assert.IsFalse(afterDecoratorExecuted);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(AggregateException))]
        public void InvokeAction_IMethodProxyTargetAndAfterDecoratorThrowingException_ThrowsExpectedException()
        {
            var mock = new Mock<IMethod>();
            mock.Setup(m => m.Action()).Throws(new TestException(1));
            var proxy = CreateProxyInstance(mock.Object);
            Action beforePairedDecorator = () =>
            {

            };
            Action afterPairedDecorator = () =>
            {
                throw new TestException(2);
            };
            AddDecorators<Action, Action>(proxy, "ActionMethodDecorators0", beforePairedDecorator, afterPairedDecorator);

            try
            {
                proxy.Action();
            }
            catch (AggregateException ex)
            {
                Assert.AreEqual(2, ex.InnerExceptions.Count);
                Assert.IsInstanceOfType(ex.InnerExceptions[0], typeof(TestException));
                Assert.AreEqual(1, ((TestException)ex.InnerExceptions[0]).Id);
                Assert.IsInstanceOfType(ex.InnerExceptions[1], typeof(TestException));
                Assert.AreEqual(2, ((TestException)ex.InnerExceptions[1]).Id);
                throw;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(AggregateException))]
        public void InvokeAction_IMethodProxyTargetAndPreviousAfterDecoratorThrowingException_PairedAfterDecoratorExecuted()
        {
            var mock = new Mock<IMethod>();
            mock.Setup(m => m.Action()).Throws(new TestException(1));
            var proxy = CreateProxyInstance(mock.Object);
            bool afterDecoratorExecuted = false;
            Action beforePairedDecorator = () =>
            {

            };
            Action afterPairedDecorator = () =>
            {
                afterDecoratorExecuted = true;
            };
            AddDecorators<Action, Action>(proxy, "ActionMethodDecorators0", beforePairedDecorator, afterPairedDecorator);
            Action beforeDecoratorPairedWithThrowingException = () =>
            {

            };
            Action afterDecoratorThrowingException = () =>
            {
                throw new TestException(2);
            };
            AddDecorators<Action, Action>(proxy, "ActionMethodDecorators0", beforeDecoratorPairedWithThrowingException, afterDecoratorThrowingException);

            try
            {
                proxy.Action();
            }
            catch (AggregateException ex)
            {
                Assert.AreEqual(2, ex.InnerExceptions.Count);
                Assert.IsInstanceOfType(ex.InnerExceptions[0], typeof(TestException));
                Assert.AreEqual(1, ((TestException)ex.InnerExceptions[0]).Id);
                Assert.IsInstanceOfType(ex.InnerExceptions[1], typeof(TestException));
                Assert.AreEqual(2, ((TestException)ex.InnerExceptions[1]).Id);
                throw;
            }
            finally
            {
                Assert.IsTrue(afterDecoratorExecuted);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(TestException))]
        public void InvokeFunctionWithValueArgumentAndValueReturnType_IMethodProxyPreviousBeforeDecoratorThrowsException_NextPairedAfterDecoratorNotExecuted()
        {
            var mock = new Mock<IMethod>();
            var proxy = CreateProxyInstance(mock.Object);
            Action<int> beforeDecoratorThrowingException = i =>
            {
                throw new TestException();
            };
            AddBeforeDecorator<Action<int>, Action<int, int>>(proxy, "FunctionWithValueArgumentAndValueReturnTypeMethodDecorators0", beforeDecoratorThrowingException);
            Action<int> beforePairedDecorator = i =>
            {

            };
            bool afterDecoratorExecuted = false;
            Action<int, int> afterPairedDecorator = (i, r) =>
            {
                afterDecoratorExecuted = true;
            };
            AddDecorators(proxy, "FunctionWithValueArgumentAndValueReturnTypeMethodDecorators0", beforePairedDecorator, afterPairedDecorator);

            try
            {
                proxy.FunctionWithValueArgumentAndValueReturnType(default);
            }
            finally
            {
                Assert.IsFalse(afterDecoratorExecuted);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(TestException))]
        public void InvokeFunctionWithValueArgumentAndValueReturnType_IMethodProxyPreviousBeforeDecoratorThrowsException_NextNotPairedAfterDecoratorNotExecuted()
        {
            var mock = new Mock<IMethod>();
            var proxy = CreateProxyInstance(mock.Object);
            Action<int> beforeDecoratorThrowingException = i =>
            {
                throw new TestException();
            };
            AddBeforeDecorator<Action<int>, Action<int, int>>(proxy, "FunctionWithValueArgumentAndValueReturnTypeMethodDecorators0", beforeDecoratorThrowingException);
            bool afterDecoratorExecuted = false;
            Action<int, int> afterNotPairedDecorator = (i, r)=>
            {
                afterDecoratorExecuted = true;
            };
            AddAfterDecorator<Action<int>, Action<int, int>>(proxy, "FunctionWithValueArgumentAndValueReturnTypeMethodDecorators0", afterNotPairedDecorator);

            try
            {
                proxy.FunctionWithValueArgumentAndValueReturnType(default);
            }
            finally
            {
                Assert.IsFalse(afterDecoratorExecuted);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(TestException))]
        public void InvokeFunctionWithValueArgumentAndValueReturnType_IMethodProxyPairDecoratorThrowsException_PairAfterDecoratorExecuted()
        {
            var mock = new Mock<IMethod>();
            var proxy = CreateProxyInstance(mock.Object);
            Action<int> beforePairedDecorator = i =>
            {
                throw new TestException();
            };
            bool afterDecoratorExecuted = false;
            Action<int, int> afterPairedDecorator = (i, r) =>
            {
                afterDecoratorExecuted = true;
            };
            AddDecorators(proxy, "FunctionWithValueArgumentAndValueReturnTypeMethodDecorators0", beforePairedDecorator, afterPairedDecorator);

            try
            {
                proxy.FunctionWithValueArgumentAndValueReturnType(default);
            }
            finally
            {
                Assert.IsTrue(afterDecoratorExecuted);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(TestException))]
        public void InvokeFunctionWithValueArgumentAndValueReturnType_IMethodProxyNextBeforeDecoratorThrowsException_PreviousPairedAfterDecoratorExecuted()
        {
            var mock = new Mock<IMethod>();
            var proxy = CreateProxyInstance(mock.Object);
            Action<int> beforePairedDecorator = i =>
            {

            };
            bool afterDecoratorExecuted = false;
            Action<int, int> afterPairDecorator = (i, r) =>
            {
                afterDecoratorExecuted = true;
            };
            AddDecorators(proxy, "FunctionWithValueArgumentAndValueReturnTypeMethodDecorators0", beforePairedDecorator, afterPairDecorator);
            Action<int> beforeDecoratorThrowingException = i =>
            {
                throw new TestException();
            };
            AddBeforeDecorator<Action<int>, Action<int, int>>(proxy, "FunctionWithValueArgumentAndValueReturnTypeMethodDecorators0", beforeDecoratorThrowingException);

            try
            {
                proxy.FunctionWithValueArgumentAndValueReturnType(default);
            }
            finally
            {
                Assert.IsTrue(afterDecoratorExecuted);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(TestException))]
        public void InvokeFunctionWithValueArgumentAndValueReturnType_IMethodProxyNextBeforeDecoratorThrowsException_PreviousNotPairedAfterDecoratorNotExecuted()
        {
            var mock = new Mock<IMethod>();
            var proxy = CreateProxyInstance(mock.Object);
            bool afterDecoratorExecuted = false;
            Action<int, int> afterNotPairedDecorator = (i, r) =>
            {
                afterDecoratorExecuted = true;
            };
            AddAfterDecorator<Action<int>, Action<int, int>>(proxy, "FunctionWithValueArgumentAndValueReturnTypeMethodDecorators0", afterNotPairedDecorator);
            Action<int> beforeDecoratorThrowingException = i =>
            {
                throw new TestException();
            };
            AddBeforeDecorator<Action<int>, Action<int, int>>(proxy, "FunctionWithValueArgumentAndValueReturnTypeMethodDecorators0", beforeDecoratorThrowingException);

            try
            {
                proxy.FunctionWithValueArgumentAndValueReturnType(default);
            }
            finally
            {
                Assert.IsFalse(afterDecoratorExecuted);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(TestException))]
        public void InvokeFunctionWithValueArgumentAndValueReturnType_IMethodProxyWhenProxyDelegateThrowsException_PairedAfterDecoratorExecuted()
        {
            var mock = new Mock<IMethod>();
            var proxy = CreateProxyInstance(mock.Object);
            bool afterDecoratorExecuted = false;
            Action<int> beforePairedDecorator = i =>
            {

            };
            Action<int, int> afterPairedDecorator = (i, r) =>
            {
                afterDecoratorExecuted = true;
            };
            AddDecorators(proxy, "FunctionWithValueArgumentAndValueReturnTypeMethodDecorators0", beforePairedDecorator, afterPairedDecorator);
            Func<Func<int, int>, int, int> proxyDelegatedThrowingException = (f, i) =>
            {
                throw new TestException();
            };
            SetProxy(proxy, "FunctionWithValueArgumentAndValueReturnTypeMethodProxy0", proxyDelegatedThrowingException);

            try
            {
                proxy.FunctionWithValueArgumentAndValueReturnType(default);
            }
            finally
            {
                Assert.IsTrue(afterDecoratorExecuted);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(TestException))]
        public void InvokeFunctionWithValueArgumentAndValueReturnType_IMethodProxyWhenProxyDelegateThrowsException_NotPairedAfterDecoratorNotExecuted()
        {
            var mock = new Mock<IMethod>();
            var proxy = CreateProxyInstance(mock.Object);
            bool afterDecoratorExecuted = false;
            Action<int, int> afterNotPairedDecorator = (i, r) =>
            {
                afterDecoratorExecuted = true;
            };
            AddAfterDecorator<Action<int>, Action<int, int>>(proxy, "FunctionWithValueArgumentAndValueReturnTypeMethodDecorators0", afterNotPairedDecorator);
            Func<Func<int, int>, int, int> proxyDelegatedThrowingException = (f, i) =>
            {
                throw new TestException();
            };
            SetProxy(proxy, "FunctionWithValueArgumentAndValueReturnTypeMethodProxy0", proxyDelegatedThrowingException);

            try
            {
                proxy.FunctionWithValueArgumentAndValueReturnType(default);
            }
            finally
            {
                Assert.IsFalse(afterDecoratorExecuted);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(TestException))]
        public void InvokeFunctionWithValueArgumentAndValueReturnType_IMethodProxyTargetThrowingException_PairedAfterDecoratorExecuted()
        {
            var mock = new Mock<IMethod>();
            mock.Setup(m => m.FunctionWithValueArgumentAndValueReturnType(It.IsAny<int>())).Throws<TestException>();
            var proxy = CreateProxyInstance(mock.Object);
            bool afterDecoratorExecuted = false;
            Action<int> beforePairedDecorator = i =>
            {

            };
            Action<int, int> afterPairedDecorator = (i, r) =>
            {
                afterDecoratorExecuted = true;
            };
            AddDecorators(proxy, "FunctionWithValueArgumentAndValueReturnTypeMethodDecorators0", beforePairedDecorator, afterPairedDecorator);

            try
            {
                proxy.FunctionWithValueArgumentAndValueReturnType(default);
            }
            finally
            {
                Assert.IsTrue(afterDecoratorExecuted);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(TestException))]
        public void InvokeFunctionWithValueArgumentAndValueReturnType_IMethodProxyTargetThrowingException_NotPairedAfterDecoratorNotExecuted()
        {
            var mock = new Mock<IMethod>();
            mock.Setup(m => m.FunctionWithValueArgumentAndValueReturnType(It.IsAny<int>())).Throws<TestException>();
            var proxy = CreateProxyInstance(mock.Object);
            bool afterDecoratorExecuted = false;
            Action<int, int> afterNotPairedDecorator = (i, r) =>
            {
                afterDecoratorExecuted = true;
            };
            AddAfterDecorator<Action<int>, Action<int, int>>(proxy, "FunctionWithValueArgumentAndValueReturnTypeMethodDecorators0", afterNotPairedDecorator);

            try
            {
                proxy.FunctionWithValueArgumentAndValueReturnType(default);
            }
            finally
            {
                Assert.IsFalse(afterDecoratorExecuted);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(TestException))]
        public void InvokeFunctionWithValueArgumentAndValueReturnType_IMethodProxyPreviousAfterDecoratorThrowingException_PairedAfterDecoratorExecuted()
        {
            var mock = new Mock<IMethod>();
            var proxy = CreateProxyInstance(mock.Object);
            bool afterDecoratorExecuted = false;
            Action<int> beforePairedDecorator = i =>
            {

            };
            Action<int, int> afterPairedDecorator = (i, r) =>
            {
                afterDecoratorExecuted = true;
            };
            AddDecorators(proxy, "FunctionWithValueArgumentAndValueReturnTypeMethodDecorators0", beforePairedDecorator, afterPairedDecorator);
            Action<int, int> afterDecoratorThrowingException = (i, r) =>
            {
                throw new TestException();
            };
            AddAfterDecorator<Action<int>, Action<int, int>>(proxy, "FunctionWithValueArgumentAndValueReturnTypeMethodDecorators0", afterDecoratorThrowingException);

            try
            {
                proxy.FunctionWithValueArgumentAndValueReturnType(default);
            }
            finally
            {
                Assert.IsTrue(afterDecoratorExecuted);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(TestException))]
        public void InvokeFunctionWithValueArgumentAndValueReturnType_IMethodProxyPreviousAfterDecoratorThrowingException_NotPairedAfterDecoratorNotExecuted()
        {
            var mock = new Mock<IMethod>();
            var proxy = CreateProxyInstance(mock.Object);
            bool afterDecoratorExecuted = false;
            Action<int, int> afterPairedDecorator = (i, r) =>
            {
                afterDecoratorExecuted = true;
            };
            AddAfterDecorator<Action<int>, Action<int, int>>(proxy, "FunctionWithValueArgumentAndValueReturnTypeMethodDecorators0", afterPairedDecorator);
            Action<int, int> afterDecoratorThrowingException = (i, r) =>
            {
                throw new TestException();
            };
            AddAfterDecorator<Action<int>, Action<int, int>>(proxy, "FunctionWithValueArgumentAndValueReturnTypeMethodDecorators0", afterDecoratorThrowingException);

            try
            {
                proxy.FunctionWithValueArgumentAndValueReturnType(default);
            }
            finally
            {
                Assert.IsFalse(afterDecoratorExecuted);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(AggregateException))]
        public void InvokeFunctionWithValueArgumentAndValueReturnType_IMethodProxyTargetAndAfterDecoratorThrowingException_ThrowsExpectedException()
        {
            var mock = new Mock<IMethod>();
            mock.Setup(m => m.FunctionWithValueArgumentAndValueReturnType(It.IsAny<int>())).Throws(new TestException(1));
            var proxy = CreateProxyInstance(mock.Object);
            Action<int> beforePairedDecorator = i =>
            {

            };
            Action<int, int> afterPairedDecorator = (i, r) =>
            {
                throw new TestException(2);
            };
            AddDecorators(proxy, "FunctionWithValueArgumentAndValueReturnTypeMethodDecorators0", beforePairedDecorator, afterPairedDecorator);

            try
            {
                proxy.FunctionWithValueArgumentAndValueReturnType(default);
            }
            catch (AggregateException ex)
            {
                Assert.AreEqual(2, ex.InnerExceptions.Count);
                Assert.IsInstanceOfType(ex.InnerExceptions[0], typeof(TestException));
                Assert.AreEqual(1, ((TestException)ex.InnerExceptions[0]).Id);
                Assert.IsInstanceOfType(ex.InnerExceptions[1], typeof(TestException));
                Assert.AreEqual(2, ((TestException)ex.InnerExceptions[1]).Id);
                throw;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(AggregateException))]
        public void InvokeFunctionWithValueArgumentAndValueReturnType_IMethodProxyTargetAndPreviousAfterDecoratorThrowingException_PairedAfterDecoratorExecuted()
        {
            var mock = new Mock<IMethod>();
            mock.Setup(m => m.FunctionWithValueArgumentAndValueReturnType(It.IsAny<int>())).Throws(new TestException(1));
            var proxy = CreateProxyInstance(mock.Object);
            bool afterDecoratorExecuted = false;
            Action<int> beforePairedDecorator = i =>
            {

            };
            Action<int, int> afterPairedDecorator = (i, r) =>
            {
                afterDecoratorExecuted = true;
            };
            AddDecorators(proxy, "FunctionWithValueArgumentAndValueReturnTypeMethodDecorators0", beforePairedDecorator, afterPairedDecorator);
            Action<int> beforeDecoratorPairedWithThrowingException = i =>
            {

            };
            Action<int, int> afterDecoratorThrowingException = (i, r) =>
            {
                throw new TestException(2);
            };
            AddDecorators(proxy, "FunctionWithValueArgumentAndValueReturnTypeMethodDecorators0", beforeDecoratorPairedWithThrowingException, afterDecoratorThrowingException);

            try
            {
                proxy.FunctionWithValueArgumentAndValueReturnType(default);
            }
            catch (AggregateException ex)
            {
                Assert.AreEqual(2, ex.InnerExceptions.Count);
                Assert.IsInstanceOfType(ex.InnerExceptions[0], typeof(TestException));
                Assert.AreEqual(1, ((TestException)ex.InnerExceptions[0]).Id);
                Assert.IsInstanceOfType(ex.InnerExceptions[1], typeof(TestException));
                Assert.AreEqual(2, ((TestException)ex.InnerExceptions[1]).Id);
                throw;
            }
            finally
            {
                Assert.IsTrue(afterDecoratorExecuted);
            }
        }

        #endregion Call with Throwing Exception

        #region Event

        [TestMethod]
        public void TriggerEventOnTarget_IEventProxySubscribed_CallHandler()
        {
            var mock = new Mock<IEvent>();
            var proxy = CreateProxyInstance(mock.Object);
            var handlerInvoked = false;
            EventHandler handler = (o, a) =>
            {
                handlerInvoked = true;
            };
            proxy.EventEmptyArgs += handler;

            mock.Raise(m => m.EventEmptyArgs += null, EventArgs.Empty);

            Assert.IsTrue(handlerInvoked);
        }

        [TestMethod]
        public void TriggerEventOnTarget_IEventProxyUnsubscribed_CallHandler()
        {
            var mock = new Mock<IEvent>();
            var proxy = CreateProxyInstance(mock.Object);
            var handlerInvoked = false;
            EventHandler handler = (o, a) =>
            {
                handlerInvoked = true;
            };
            proxy.EventEmptyArgs += handler;
            proxy.EventEmptyArgs -= handler;

            mock.Raise(m => m.EventEmptyArgs += null, EventArgs.Empty);

            Assert.IsFalse(handlerInvoked);
        }

        [TestMethod]
        public void TriggerEventOnTarget_IEventProxyWithBeforeDecorator_DecoratorInvoked()
        {
            var mock = new Mock<IEvent>();
            var proxy = CreateProxyInstance(mock.Object);
            bool decoratorInvoked = false;
            Action<object, EventArgs> beforeDecorator = (o, a) =>
            {
                decoratorInvoked = true;
            };
            AddBeforeDecorator(proxy, "EventEmptyArgsEventDecorators0", beforeDecorator);
            EventHandler handler = (o, a) =>
            {
                Assert.IsTrue(decoratorInvoked);
            };
            proxy.EventEmptyArgs += handler;

            mock.Raise(m => m.EventEmptyArgs += null, EventArgs.Empty);

            Assert.IsTrue(decoratorInvoked);
        }

        [TestMethod]
        public void TriggerEventOnTarget_IEventProxyWithProxyDelegateCallingHandler_ProxyAndHandlerInvoked()
        {
            var mock = new Mock<IEvent>();
            var proxy = CreateProxyInstance(mock.Object);
            bool proxyInvoked = false;
            Action<Action<object, EventArgs>, object, EventArgs> proxyDelegate = (h, o, a) =>
            {
                proxyInvoked = true;
                h.Invoke(o, a);
            };
            SetProxy(proxy, "EventEmptyArgsEventProxy0", proxyDelegate);
            bool handlerInvoked = false;
            EventHandler handler = (o, a) =>
            {
                Assert.IsTrue(proxyInvoked);
                handlerInvoked = true;
            };
            proxy.EventEmptyArgs += handler;

            mock.Raise(m => m.EventEmptyArgs += null, EventArgs.Empty);

            Assert.IsTrue(proxyInvoked);
            Assert.IsTrue(handlerInvoked);
        }

        [TestMethod]
        public void TriggerEventOnTarget_IEventProxyWithProxyDelegateNotCallingHandler_ProxyInvokedAndHandlerNotInvoked()
        {
            var mock = new Mock<IEvent>();
            var proxy = CreateProxyInstance(mock.Object);
            bool proxyInvoked = false;
            Action<Action<object, EventArgs>, object, EventArgs> proxyDelegate = (h, o, a) =>
            {
                proxyInvoked = true;
            };
            SetProxy(proxy, "EventEmptyArgsEventProxy0", proxyDelegate);
            bool handlerInvoked = false;
            EventHandler handler = (o, a) =>
            {
                handlerInvoked = true;
            };
            proxy.EventEmptyArgs += handler;

            mock.Raise(m => m.EventEmptyArgs += null, EventArgs.Empty);

            Assert.IsTrue(proxyInvoked);
            Assert.IsFalse(handlerInvoked);
        }

        [TestMethod]
        public void TriggerEventOnTarget_IEventProxyWithAfterDecorator_DecoratorInvoked()
        {
            var mock = new Mock<IEvent>();
            var proxy = CreateProxyInstance(mock.Object);
            bool decoratorInvoked = false;
            Action<object, EventArgs> beforeDecorator = (o, a) =>
            {
                decoratorInvoked = true;
            };
            AddAfterDecorator(proxy, "EventEmptyArgsEventDecorators0", beforeDecorator);
            EventHandler handler = (o, a) =>
            {
                Assert.IsFalse(decoratorInvoked);
            };
            proxy.EventEmptyArgs += handler;

            mock.Raise(m => m.EventEmptyArgs += null, EventArgs.Empty);

            Assert.IsTrue(decoratorInvoked);
        }

        #endregion


        internal static T CreateProxyInstance<T>(T target)
        {
            return (T)Activator.CreateInstance(CreateTargetType(typeof(T)), target);
        }

        internal static Type CreateTargetType(Type targetType)
        {
            return TypeRepository.GetOrAdd(targetType, t => new ProxyBuilder(t, GetMemberNamesProvider(t), Static.ModuleBinder.Value).CreateProxyType());
        }

        internal static IMemberNamesProvider GetMemberNamesProvider(Type targetType)
        {
            return MemberNamesProvidersRepository.GetOrAdd(targetType, t => new MemberNamesProviderCacheProxy(new MemberNamesProviderCore()));
        }

        internal static ConcurrentDictionary<Type, Type> TypeRepository { get; } = new ConcurrentDictionary<Type, Type>();
        internal static ConcurrentDictionary<Type, IMemberNamesProvider> MemberNamesProvidersRepository { get; } = new ConcurrentDictionary<Type, IMemberNamesProvider>();

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

        internal static void AddBeforeDecorator<TDecorator>(object instance, string fieldName, TDecorator decorator)
            where TDecorator : Delegate
        {
            ((LinkedList<ValueTuple<TDecorator, TDecorator>>)instance.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic).GetValue(instance))
                .AddLast(new ValueTuple<TDecorator, TDecorator>(decorator, null));
        }

        internal static void AddBeforeDecorator<TBeforeDecorator, TAfterDecorator>(object instance, string fieldName, TBeforeDecorator decorator)
            where TBeforeDecorator : Delegate
            where TAfterDecorator : Delegate
        {
            ((LinkedList<ValueTuple<TBeforeDecorator, TAfterDecorator>>)instance.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic).GetValue(instance))
                .AddLast(new ValueTuple<TBeforeDecorator, TAfterDecorator>(decorator, null));
        }

        internal static void AddAfterDecorator<TDecorator>(object instance, string fieldName, TDecorator decorator)
            where TDecorator : Delegate
        {
            ((LinkedList<ValueTuple<TDecorator, TDecorator>>)instance.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic).GetValue(instance))
                .AddLast(new ValueTuple<TDecorator, TDecorator>(null, decorator));
        }

        internal static void AddAfterDecorator<TBeforeDecorator, TAfterDecorator>(object instance, string fieldName, TAfterDecorator decorator)
            where TBeforeDecorator : Delegate
            where TAfterDecorator : Delegate
        {
            ((LinkedList<ValueTuple<TBeforeDecorator, TAfterDecorator>>)instance.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic).GetValue(instance))
                .AddLast(new ValueTuple<TBeforeDecorator, TAfterDecorator>(null, decorator));
        }

        internal static void AddDecorators<TDecorator>(object instance, string fieldName, TDecorator beforeDecorator, TDecorator afterDecorator)
        {
            ((LinkedList<ValueTuple<TDecorator, TDecorator>>)instance.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic).GetValue(instance))
                .AddLast(new ValueTuple<TDecorator, TDecorator>(beforeDecorator, afterDecorator));
        }

        internal static void AddDecorators<TBeforeDecorator, TAfterDecorator>(object instance, string fieldName, TBeforeDecorator beforeDecorator, TAfterDecorator afterDecorator)
        {
            ((LinkedList<ValueTuple<TBeforeDecorator, TAfterDecorator>>)instance.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic).GetValue(instance))
                .AddLast(new ValueTuple<TBeforeDecorator, TAfterDecorator>(beforeDecorator, afterDecorator));
        }

        internal static void SetProxy<TProxy>(object instance, string fieldName, TProxy proxy)
        {
            instance.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic).SetValue(instance, proxy);
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
