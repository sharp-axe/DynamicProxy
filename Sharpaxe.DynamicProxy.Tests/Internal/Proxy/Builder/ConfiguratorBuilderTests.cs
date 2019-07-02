using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Sharpaxe.DynamicProxy.Internal.Proxy;
using Sharpaxe.DynamicProxy.Internal.Proxy.Builder;
using Sharpaxe.DynamicProxy.Tests.TestHelper;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Sharpaxe.DynamicProxy.Tests.Internal.Proxy.Builder
{
    [TestClass]
    public class ConfiguratorBuilderTests
    {
        [TestMethod]
        public void CreateConfiguratorType_IMethod_ReturnsNotNull()
        {
            Assert.IsNotNull(GetConfiguratorType(typeof(IMethod)));
        }

        [TestMethod]
        public void CreateConfiguratorInstance_IMethod_DoesNotThrowException()
        {
            var proxy = ProxyBuilderTests.CreateProxyInstance<IMethod>(null);
            CreateConfiguratorInstance(proxy);
        }

        [TestMethod]
        public void SetEventProxy_IEventConfigurator_SettedFieldOfProxy()
        {
            var proxy = ProxyBuilderTests.CreateProxyInstance<IEvent>(null);
            var configurator = CreateConfiguratorInstance(proxy);
            Action<Action<object, EventArgs>, object, EventArgs> proxyDelegate = (h, o, a)  =>
            {

            };

            configurator.SetEventProxy(typeof(IEvent).GetEvent("EventEmptyArgs"), proxyDelegate);

            Assert.AreEqual(proxy.GetType().GetField("EventEmptyArgsEventProxy0", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(proxy), proxyDelegate);
        }

        [TestMethod]
        public void SetEventDecorators_IEventConfigurator_SettedFieldOfProxy()
        {
            var proxy = ProxyBuilderTests.CreateProxyInstance<IEvent>(null);
            var configurator = CreateConfiguratorInstance(proxy);
            Action<object, EventArgs> beforeDecorator = (o, a) =>
            {

            };
            Action<object, EventArgs> beforePairedDecorator = (o, a) =>
            {

            };
            Action<object, EventArgs> afterPairedDecorator = (o, a) =>
            {

            };
            Action<object, EventArgs> afterDecorator = (o, a) =>
            {

            };

            var decorators = new List<ValueTuple<object, object>>()
            {
                new ValueTuple<object, object>(beforeDecorator, null),
                new ValueTuple<object, object>(beforePairedDecorator, afterPairedDecorator),
                new ValueTuple<object, object>(null, afterDecorator)
            };

            configurator.SetEventDecorators(typeof(IEvent).GetEvent("EventEmptyArgs"), decorators);

            var decoratorPair =
                ((LinkedList<ValueTuple<Action<object, EventArgs>, Action<object, EventArgs>>>)
                 proxy.GetType().GetField("EventEmptyArgsEventDecorators0", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(proxy))
                 .First;
            Assert.AreEqual(decoratorPair.Value.Item1, beforeDecorator);
            Assert.IsNull(decoratorPair.Value.Item2);
            decoratorPair = decoratorPair.Next;
            Assert.AreEqual(decoratorPair.Value.Item1, beforePairedDecorator);
            Assert.AreEqual(decoratorPair.Value.Item2, afterPairedDecorator);
            decoratorPair = decoratorPair.Next;
            Assert.IsNull(decoratorPair.Value.Item1);
            Assert.AreEqual(decoratorPair.Value.Item2, afterDecorator);
            Assert.IsNull(decoratorPair.Next);
        }

        [TestMethod]
        public void SetActionProxby_IMethodConfigurator_SettedFieldOfProxy()
        {
            var proxy = ProxyBuilderTests.CreateProxyInstance<IMethod>(null);
            var configurator = CreateConfiguratorInstance(proxy);
            Action<Action> proxyDelegate = a =>
            {

            };

            configurator.SetMethodProxy(typeof(IMethod).GetMethod("Action"), proxyDelegate);

            Assert.AreEqual(proxy.GetType().GetField("ActionMethodProxy0", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(proxy), proxyDelegate);
        }

        [TestMethod]
        public void SetFunctionWithReferenceArgumentAndValueReturnTypeProxy_IMethodConfigurator_SettedFieldOfProxy()
        {
            var proxy = ProxyBuilderTests.CreateProxyInstance<IMethod>(null);
            var configurator = CreateConfiguratorInstance(proxy);
            Func<Func<object, int>, object, int> proxyDelegate = (f, o) =>
            {
                return default(int);
            };

            configurator.SetMethodProxy(typeof(IMethod).GetMethod("FunctionWithReferenceArgumentAndValueReturnType", new Type[] { typeof(object) }), proxyDelegate);

            Assert.AreEqual(proxy.GetType().GetField("FunctionWithReferenceArgumentAndValueReturnTypeMethodProxy0", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(proxy), proxyDelegate);
        }

        [TestMethod]
        public void SetActionDecorators_IMethodConfigurator_SettedFieldOfDecorators()
        {
            var proxy = ProxyBuilderTests.CreateProxyInstance<IMethod>(null);
            var configurator = CreateConfiguratorInstance(proxy);
            Action beforeDecorator = () =>
            {

            };
            Action beforePairedDecorator = () =>
            {

            };
            Action afterPairedDecorator = () =>
            {

            };
            Action afterDecorator = () =>
            {

            };
            var decorators = new List<ValueTuple<object, object>>()
            {
                new ValueTuple<object, object>(beforeDecorator, null),
                new ValueTuple<object, object>(beforePairedDecorator, afterPairedDecorator),
                new ValueTuple<object, object>(null, afterDecorator)
            };

            configurator.SetMethodDecorators(typeof(IMethod).GetMethod("Action"), decorators);

            var decoratorPair = 
                ((LinkedList<ValueTuple<Action, Action>>)
                 proxy.GetType().GetField("ActionMethodDecorators0", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(proxy))
                 .First;
            Assert.AreEqual(decoratorPair.Value.Item1, beforeDecorator);
            Assert.IsNull(decoratorPair.Value.Item2);
            decoratorPair = decoratorPair.Next;
            Assert.AreEqual(decoratorPair.Value.Item1, beforePairedDecorator);
            Assert.AreEqual(decoratorPair.Value.Item2, afterPairedDecorator);
            decoratorPair = decoratorPair.Next;
            Assert.IsNull(decoratorPair.Value.Item1);
            Assert.AreEqual(decoratorPair.Value.Item2, afterDecorator);
            Assert.IsNull(decoratorPair.Next);
        }

        [TestMethod]
        public void SetFunctionWithReferenceArgumentAndValueReturnTypeDecorators_IMethodConfigurator_SettedFieldOfDecorators()
        {
            var proxy = ProxyBuilderTests.CreateProxyInstance<IMethod>(null);
            var configurator = CreateConfiguratorInstance(proxy);
            Action<object> beforeDecorator = o =>
            {

            };
            Action<object> beforePairedDecorator = o =>
            {

            };
            Action<object, int> afterPairedDecorator = (o, a) =>
            {

            };
            Action<object, int> afterDecorator = (o, a) =>
            {

            };
            var decorators = new List<ValueTuple<object, object>>()
            {
                new ValueTuple<object, object>(beforeDecorator, null),
                new ValueTuple<object, object>(beforePairedDecorator, afterPairedDecorator),
                new ValueTuple<object, object>(null, afterDecorator)
            };

            configurator.SetMethodDecorators(typeof(IMethod).GetMethod("FunctionWithReferenceArgumentAndValueReturnType", new Type[] { typeof(object) }), decorators);

            var decoratorPair =
                ((LinkedList<ValueTuple<Action<object>, Action<object, int>>>)
                 proxy.GetType().GetField("FunctionWithReferenceArgumentAndValueReturnTypeMethodDecorators0", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(proxy))
                 .First;
            Assert.AreEqual(decoratorPair.Value.Item1, beforeDecorator);
            Assert.IsNull(decoratorPair.Value.Item2);
            decoratorPair = decoratorPair.Next;
            Assert.AreEqual(decoratorPair.Value.Item1, beforePairedDecorator);
            Assert.AreEqual(decoratorPair.Value.Item2, afterPairedDecorator);
            decoratorPair = decoratorPair.Next;
            Assert.IsNull(decoratorPair.Value.Item1);
            Assert.AreEqual(decoratorPair.Value.Item2, afterDecorator);
            Assert.IsNull(decoratorPair.Next);
        }

        [TestMethod]
        public void SetPropertySetterProxy_IPropertySetterConfigurator_SettedFieldOfProxy()
        {
            var proxy = ProxyBuilderTests.CreateProxyInstance<IPropertySetter>(null);
            var configurator = CreateConfiguratorInstance(proxy);
            Action<Action<bool>, bool> proxyDelegate = (h, a) =>
            {

            };

            configurator.SetPropertySetterProxy(typeof(IPropertySetter).GetProperty("Boolean"), proxyDelegate);

            Assert.AreEqual(proxy.GetType().GetField("BooleanSetterProxy0", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(proxy), proxyDelegate);
        }

        [TestMethod]
        public void SetPropertyDecorators_IPropertySetterConfigurator_SettedFieldOfProxy()
        {
            var proxy = ProxyBuilderTests.CreateProxyInstance<IPropertySetter>(null);
            var configurator = CreateConfiguratorInstance(proxy);
            Action<bool> beforeDecorator = b =>
            {

            };
            Action<bool> beforePairedDecorator = b =>
            {

            };
            Action<bool> afterPairedDecorator = b =>
            {

            };
            Action<bool> afterDecorator = b =>
            {

            };
            var decorators = new List<ValueTuple<object, object>>()
            {
                new ValueTuple<object, object>(beforeDecorator, null),
                new ValueTuple<object, object>(beforePairedDecorator, afterPairedDecorator),
                new ValueTuple<object, object>(null, afterDecorator)
            };

            configurator.SetPropertySetterDecorators(typeof(IPropertySetter).GetProperty("Boolean"), decorators);

            var decoratorPair =
                ((LinkedList<ValueTuple<Action<bool>, Action<bool>>>)
                 proxy.GetType().GetField("BooleanSetterDecorators0", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(proxy))
                 .First;
            Assert.AreEqual(decoratorPair.Value.Item1, beforeDecorator);
            Assert.IsNull(decoratorPair.Value.Item2);
            decoratorPair = decoratorPair.Next;
            Assert.AreEqual(decoratorPair.Value.Item1, beforePairedDecorator);
            Assert.AreEqual(decoratorPair.Value.Item2, afterPairedDecorator);
            decoratorPair = decoratorPair.Next;
            Assert.IsNull(decoratorPair.Value.Item1);
            Assert.AreEqual(decoratorPair.Value.Item2, afterDecorator);
            Assert.IsNull(decoratorPair.Next);
        }

        [TestMethod]
        public void SetPropertyGetterProxy_IPropertyGetterConfigurator_SettedFieldOfProxy()
        {
            var proxy = ProxyBuilderTests.CreateProxyInstance<IPropertyGetter>(null);
            var configurator = CreateConfiguratorInstance(proxy);
            Func<Func<bool>, bool> proxyDelegate = (h) =>
            {
                return default(bool);
            };

            configurator.SetPropertyGetterProxy(typeof(IPropertyGetter).GetProperty("Boolean"), proxyDelegate);

            Assert.AreEqual(proxy.GetType().GetField("BooleanGetterProxy0", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(proxy), proxyDelegate);
        }

        [TestMethod]
        public void SetPropertyDecorators_IPropertyGetterConfigurator_SettedFieldOfProxy()
        {
            var proxy = ProxyBuilderTests.CreateProxyInstance<IPropertyGetter>(null);
            var configurator = CreateConfiguratorInstance(proxy);
            Action beforeDecorator = () =>
            {

            };
            Action beforePairedDecorator = () =>
            {

            };
            Action<bool> afterPairedDecorator = b =>
            {

            };
            Action<bool> afterDecorator = b =>
            {

            };
            var decorators = new List<ValueTuple<object, object>>()
            {
                new ValueTuple<object, object>(beforeDecorator, null),
                new ValueTuple<object, object>(beforePairedDecorator, afterPairedDecorator),
                new ValueTuple<object, object>(null, afterDecorator)
            };

            configurator.SetPropertyGetterDecorators(typeof(IPropertyGetter).GetProperty("Boolean"), decorators);

            var decoratorPair =
                ((LinkedList<ValueTuple<Action, Action<bool>>>)
                 proxy.GetType().GetField("BooleanGetterDecorators0", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(proxy))
                 .First;
            Assert.AreEqual(decoratorPair.Value.Item1, beforeDecorator);
            Assert.IsNull(decoratorPair.Value.Item2);
            decoratorPair = decoratorPair.Next;
            Assert.AreEqual(decoratorPair.Value.Item1, beforePairedDecorator);
            Assert.AreEqual(decoratorPair.Value.Item2, afterPairedDecorator);
            decoratorPair = decoratorPair.Next;
            Assert.IsNull(decoratorPair.Value.Item1);
            Assert.AreEqual(decoratorPair.Value.Item2, afterDecorator);
            Assert.IsNull(decoratorPair.Next);
        }

        internal static IProxyConfigurator CreateConfiguratorInstance<T>(T proxy)
        {
            return (IProxyConfigurator)Activator.CreateInstance(GetConfiguratorType(typeof(T)), proxy);
        }

        internal static Type GetConfiguratorType(Type targetType)
        {
            return TypeRepository.GetOrAdd(targetType,
                t => new ConfiguratorBuilder(
                        t,
                        ProxyBuilderTests.CreateTargetType(t),
                        ProxyBuilderTests.GetMemberNamesProvider(t),
                        Static.ModuleBinder.Value)
                    .CreateConfiguratorType());
        }

        internal static ConcurrentDictionary<Type, Type> TypeRepository { get; } = new ConcurrentDictionary<Type, Type>();
    }
}
