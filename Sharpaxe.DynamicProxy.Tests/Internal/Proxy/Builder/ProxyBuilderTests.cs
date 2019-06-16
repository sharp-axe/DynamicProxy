using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sharpaxe.DynamicProxy.Internal.Proxy;
using Sharpaxe.DynamicProxy.Tests.TestHelper;
using Sharpaxe.DynamicProxy.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Sharpaxe.DynamicProxy.Tests.Internal.Proxy.Builder
{
    [TestClass]
    public class ProxyBuilderTests
    {
        [TestMethod]
        public void CreateType_IPropertyGetter_HasExpectedFields()
        {
            var target = CreateTarget(typeof(IPropertyGetter));
            var type = target.CreateProxyType();

            var expectedFields =
                ExpectedIPropertyGetterFields
                .Concat<KeyValuePair<string, Type>>(new KeyValuePair<string, Type>("target", typeof(IPropertyGetter)))
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

            AssertTypeHasPassedPrivateInstanceFields(type, expectedFields);
        }

        [TestMethod]
        public void CreateType_IPropertySetter_HasExpectedFields()
        {
            var target = CreateTarget(typeof(IPropertySetter));
            var type = target.CreateProxyType();

            var expectedFields =
                ExpectedIPropertySetterFields
                .Concat(new KeyValuePair<string, Type>("target", typeof(IPropertySetter)))
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

            AssertTypeHasPassedPrivateInstanceFields(type, expectedFields);
        }

        [TestMethod]
        public void CreateType_IEvent_HasExpectedFields()
        {
            var target = CreateTarget(typeof(IEvent));
            var type = target.CreateProxyType();

            var expectedFields =
                ExpectedIEventFields
                .Concat(new KeyValuePair<string, Type>("target", typeof(IEvent)))
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

            AssertTypeHasPassedPrivateInstanceFields(type, expectedFields);
        }

        [TestMethod]
        public void CreateType_IMethod_HasExpectedFields()
        {
            var target = CreateTarget(typeof(IMethod));
            var type = target.CreateProxyType();

            var expectedFields =
                ExpectedIMethodFields
                .Concat(new KeyValuePair<string, Type>("target", typeof(IMethod)))
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

            AssertTypeHasPassedPrivateInstanceFields(type, expectedFields);
        }

        [TestMethod]
        public void CreateType_IInterface_HasExpectedFields()
        {
            var target = CreateTarget(typeof(IInteface));
            var type = target.CreateProxyType();

            var expectedFields =
                ExpectedIEventFields
                .Concat(ExpectedIMethodFields)
                .Concat(ExpectedIPropertyGetterFields)
                .Concat(ExpectedIPropertySetterFields)
                .Concat(new KeyValuePair<string, Type>("target", typeof(IInteface)))
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

            AssertTypeHasPassedPrivateInstanceFields(type, expectedFields);
        }
        
        internal static ProxyBuilder CreateTarget(Type targetType)
        {
            return new ProxyBuilder(targetType, Static.ModuleBinder.Value);
        }

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

        internal static Dictionary<string, Type> ExpectedIPropertyGetterFields = new Dictionary<string, Type>()
        {
            ["BooleanGetterProxy"] = typeof(Func<Func<bool>, bool>),
            ["ByteGetterProxy"]    = typeof(Func<Func<byte>, byte>),
            ["SByteGetterProxy"]   = typeof(Func<Func<sbyte>, sbyte>),
            ["CharGetterProxy"]    = typeof(Func<Func<char>, char>),
            ["DecimalGetterProxy"] = typeof(Func<Func<decimal>, decimal>),
            ["DoubleGetterProxy"]  = typeof(Func<Func<double>, double>),
            ["FloatGetterProxy"]   = typeof(Func<Func<float>, float>),
            ["IntGetterProxy"]     = typeof(Func<Func<int>, int>),
            ["UIntGetterProxy"]    = typeof(Func<Func<uint>, uint>),
            ["LongGetterProxy"]    = typeof(Func<Func<long>, long>),
            ["ULongGetterProxy"]   = typeof(Func<Func<ulong>, ulong>),
            ["ShortGetterProxy"]   = typeof(Func<Func<short>, short>),
            ["UShortGetterProxy"]  = typeof(Func<Func<ushort>, ushort>),
            ["StructGetterProxy"]  = typeof(Func<Func<TestStruct>, TestStruct>),
            ["ClassGetterProxy"]   = typeof(Func<Func<string>, string>),

            ["BooleanGetterDecorators"] = typeof(LinkedList<ValueTuple<Action, Action<bool>>>),
            ["ByteGetterDecorators"]    = typeof(LinkedList<ValueTuple<Action, Action<byte>>>),
            ["SByteGetterDecorators"]   = typeof(LinkedList<ValueTuple<Action, Action<sbyte>>>),
            ["CharGetterDecorators"]    = typeof(LinkedList<ValueTuple<Action, Action<char>>>),
            ["DecimalGetterDecorators"] = typeof(LinkedList<ValueTuple<Action, Action<decimal>>>),
            ["DoubleGetterDecorators"]  = typeof(LinkedList<ValueTuple<Action, Action<double>>>),
            ["FloatGetterDecorators"]   = typeof(LinkedList<ValueTuple<Action, Action<float>>>),
            ["IntGetterDecorators"]     = typeof(LinkedList<ValueTuple<Action, Action<int>>>),
            ["UIntGetterDecorators"]    = typeof(LinkedList<ValueTuple<Action, Action<uint>>>),
            ["LongGetterDecorators"]    = typeof(LinkedList<ValueTuple<Action, Action<long>>>),
            ["ULongGetterDecorators"]   = typeof(LinkedList<ValueTuple<Action, Action<ulong>>>),
            ["ShortGetterDecorators"]   = typeof(LinkedList<ValueTuple<Action, Action<short>>>),
            ["UShortGetterDecorators"]  = typeof(LinkedList<ValueTuple<Action, Action<ushort>>>),
            ["StructGetterDecorators"]  = typeof(LinkedList<ValueTuple<Action, Action<TestStruct>>>),
            ["ClassGetterDecorators"]   = typeof(LinkedList<ValueTuple<Action, Action<string>>>),
        };

        internal static Dictionary<string, Type> ExpectedIPropertySetterFields = new Dictionary<string, Type>()
        {
            ["BooleanSetterProxy"] = typeof(Action<Action<bool>, bool>),
            ["ByteSetterProxy"]    = typeof(Action<Action<byte>, byte>),
            ["SByteSetterProxy"]   = typeof(Action<Action<sbyte>, sbyte>),
            ["CharSetterProxy"]    = typeof(Action<Action<char>, char>),
            ["DecimalSetterProxy"] = typeof(Action<Action<decimal>, decimal>),
            ["DoubleSetterProxy"]  = typeof(Action<Action<double>, double>),
            ["FloatSetterProxy"]   = typeof(Action<Action<float>, float>),
            ["IntSetterProxy"]     = typeof(Action<Action<int>, int>),
            ["UIntSetterProxy"]    = typeof(Action<Action<uint>, uint>),
            ["LongSetterProxy"]    = typeof(Action<Action<long>, long>),
            ["ULongSetterProxy"]   = typeof(Action<Action<ulong>, ulong>),
            ["ShortSetterProxy"]   = typeof(Action<Action<short>, short>),
            ["UShortSetterProxy"]  = typeof(Action<Action<ushort>, ushort>),
            ["StructSetterProxy"]  = typeof(Action<Action<TestStruct>, TestStruct>),
            ["ClassSetterProxy"]   = typeof(Action<Action<string>, string>),

            ["BooleanSetterDecorators"] = typeof(LinkedList<ValueTuple<Action<bool>, Action<bool>>>),
            ["ByteSetterDecorators"]    = typeof(LinkedList<ValueTuple<Action<byte>, Action<byte>>>),
            ["SByteSetterDecorators"]   = typeof(LinkedList<ValueTuple<Action<sbyte>, Action<sbyte>>>),
            ["CharSetterDecorators"]    = typeof(LinkedList<ValueTuple<Action<char>, Action<char>>>),
            ["DecimalSetterDecorators"] = typeof(LinkedList<ValueTuple<Action<decimal>, Action<decimal>>>),
            ["DoubleSetterDecorators"]  = typeof(LinkedList<ValueTuple<Action<double>, Action<double>>>),
            ["FloatSetterDecorators"]   = typeof(LinkedList<ValueTuple<Action<float>, Action<float>>>),
            ["IntSetterDecorators"]     = typeof(LinkedList<ValueTuple<Action<int>, Action<int>>>),
            ["UIntSetterDecorators"]    = typeof(LinkedList<ValueTuple<Action<uint>, Action<uint>>>),
            ["LongSetterDecorators"]    = typeof(LinkedList<ValueTuple<Action<long>, Action<long>>>),
            ["ULongSetterDecorators"]   = typeof(LinkedList<ValueTuple<Action<ulong>, Action<ulong>>>),
            ["ShortSetterDecorators"]   = typeof(LinkedList<ValueTuple<Action<short>, Action<short>>>),
            ["UShortSetterDecorators"]  = typeof(LinkedList<ValueTuple<Action<ushort>, Action<ushort>>>),
            ["StructSetterDecorators"]  = typeof(LinkedList<ValueTuple<Action<TestStruct>, Action<TestStruct>>>),
            ["ClassSetterDecorators"]   = typeof(LinkedList<ValueTuple<Action<string>, Action<string>>>),
        };

        internal static Dictionary<string, Type> ExpectedIEventFields = new Dictionary<string, Type>()
        {
            ["EventEmptyArgsEventProxy"]  = typeof(Action<Action<object, EventArgs>, object, EventArgs>),
            ["EventIntArgsEventProxy"]    = typeof(Action<Action<object, EventArgs<int>>, object, EventArgs<int>>),
            ["EventStringArgsEventProxy"] = typeof(Action<Action<object, EventArgs<string>>, object, EventArgs<string>>),

            ["EventEmptyArgsEventDecorators"]  = typeof(LinkedList<ValueTuple<Action<object, EventArgs>, Action<object, EventArgs>>>),
            ["EventIntArgsEventDecorators"]    = typeof(LinkedList<ValueTuple<Action<object, EventArgs<int>>, Action<object, EventArgs<int>>>>),
            ["EventStringArgsEventDecorators"] = typeof(LinkedList<ValueTuple<Action<object, EventArgs<string>>, Action<object, EventArgs<string>>>>),
        };

        internal static Dictionary<string, Type> ExpectedIMethodFields = new Dictionary<string, Type>()
        {
            ["ActionMethodProxy0"]                                              = typeof(Action<Action>),
            ["ActionWithValueArgumentMethodProxy0"]                             = typeof(Action<Action<int>, int>),
            ["ActionWithValueArgumentMethodProxy1"]                             = typeof(Action<Action<uint>, uint>),
            ["ActionWithReferenceArgumentMethodProxy0"]                         = typeof(Action<Action<object>, object>),
            ["ActionWithReferenceArgumentMethodProxy1"]                         = typeof(Action<Action<string>, string>),
            ["FunctionWithValueReturnTypeMethodProxy0"]                         = typeof(Func<Func<int>, int>),
            ["FunctionWithValueArgumentAndValueReturnTypeMethodProxy0"]         = typeof(Func<Func<int, int>, int, int>),
            ["FunctionWithValueArgumentAndValueReturnTypeMethodProxy1"]         = typeof(Func<Func<uint, int>, uint, int>),
            ["FunctionWithReferenceArgumentAndValueReturnTypeMethodProxy0"]     = typeof(Func<Func<object, int>, object, int>),
            ["FunctionWithReferenceArgumentAndValueReturnTypeMethodProxy1"]     = typeof(Func<Func<string, int>, string, int>),
            ["FunctionWithReferenceReturnTypeMethodProxy0"]                     = typeof(Func<Func<object>, object>),
            ["FunctionWithValueArgumentAndReferenceReturnTypeMethodProxy0"]     = typeof(Func<Func<int, object>, int, object>),
            ["FunctionWithValueArgumentAndReferenceReturnTypeMethodProxy1"]     = typeof(Func<Func<uint, object>, uint, object>),
            ["FunctionWithReferenceArgumentAndReferenceReturnTypeMethodProxy0"] = typeof(Func<Func<object, object>, object, object>),
            ["FunctionWithReferenceArgumentAndReferenceReturnTypeMethodProxy1"] = typeof(Func<Func<string, object>, string, object>),
            ["FunctionWithTupleReturnTypeMethodProxy0"]                         = typeof(Func<Func<ValueTuple<int, object>>, ValueTuple<int, object>>),
            ["FunctionWithValueArgumentAndTupleReturnTypeMethodProxy0"]         = typeof(Func<Func<int, ValueTuple<int, object>>, int, ValueTuple<int, object>>),
            ["FunctionWithValueArgumentAndTupleReturnTypeMethodProxy1"]         = typeof(Func<Func<uint, ValueTuple<int, object>>, uint, ValueTuple<int, object>>),
            ["FunctionWithReferenceArgumentAndTupleReturnTypeMethodProxy0"]     = typeof(Func<Func<object, ValueTuple<int, object>>, object, ValueTuple<int, object>>),
            ["FunctionWithReferenceArgumentAndTupleReturnTypeMethodProxy1"]     = typeof(Func<Func<string, ValueTuple<int, object>>, string, ValueTuple<int, object>>),

            ["ActionMethodDecorators0"]                                              = typeof(LinkedList<ValueTuple<Action, Action>>),
            ["ActionWithValueArgumentMethodDecorators0"]                             = typeof(LinkedList<ValueTuple<Action<int>, Action<int>>>),
            ["ActionWithValueArgumentMethodDecorators1"]                             = typeof(LinkedList<ValueTuple<Action<uint>, Action<uint>>>),
            ["ActionWithReferenceArgumentMethodDecorators0"]                         = typeof(LinkedList<ValueTuple<Action<object>, Action<object>>>),
            ["ActionWithReferenceArgumentMethodDecorators1"]                         = typeof(LinkedList<ValueTuple<Action<string>, Action<string>>>),
            ["FunctionWithValueReturnTypeMethodDecorators0"]                         = typeof(LinkedList<ValueTuple<Action, Action<int>>>),
            ["FunctionWithValueArgumentAndValueReturnTypeMethodDecorators0"]         = typeof(LinkedList<ValueTuple<Action<int>, Action<int, int>>>),
            ["FunctionWithValueArgumentAndValueReturnTypeMethodDecorators1"]         = typeof(LinkedList<ValueTuple<Action<uint>, Action<uint, int>>>),
            ["FunctionWithReferenceArgumentAndValueReturnTypeMethodDecorators0"]     = typeof(LinkedList<ValueTuple<Action<object>, Action<object, int>>>),
            ["FunctionWithReferenceArgumentAndValueReturnTypeMethodDecorators1"]     = typeof(LinkedList<ValueTuple<Action<string>, Action<string, int>>>),
            ["FunctionWithReferenceReturnTypeMethodDecorators0"]                     = typeof(LinkedList<ValueTuple<Action, Action<object>>>),
            ["FunctionWithValueArgumentAndReferenceReturnTypeMethodDecorators0"]     = typeof(LinkedList<ValueTuple<Action<int>, Action<int, object>>>),
            ["FunctionWithValueArgumentAndReferenceReturnTypeMethodDecorators1"]     = typeof(LinkedList<ValueTuple<Action<uint>, Action<uint, object>>>),
            ["FunctionWithReferenceArgumentAndReferenceReturnTypeMethodDecorators0"] = typeof(LinkedList<ValueTuple<Action<object>, Action<object, object>>>),
            ["FunctionWithReferenceArgumentAndReferenceReturnTypeMethodDecorators1"] = typeof(LinkedList<ValueTuple<Action<string>, Action<string, object>>>),
            ["FunctionWithTupleReturnTypeMethodDecorators0"]                         = typeof(LinkedList<ValueTuple<Action, Action<ValueTuple<int, object>>>>),
            ["FunctionWithValueArgumentAndTupleReturnTypeMethodDecorators0"]         = typeof(LinkedList<ValueTuple<Action<int>, Action<int, ValueTuple<int, object>>>>),
            ["FunctionWithValueArgumentAndTupleReturnTypeMethodDecorators1"]         = typeof(LinkedList<ValueTuple<Action<uint>, Action<uint, ValueTuple<int, object>>>>),
            ["FunctionWithReferenceArgumentAndTupleReturnTypeMethodDecorators0"]     = typeof(LinkedList<ValueTuple<Action<object>, Action<object, ValueTuple<int, object>>>>),
            ["FunctionWithReferenceArgumentAndTupleReturnTypeMethodDecorators1"]     = typeof(LinkedList<ValueTuple<Action<string>, Action<string, ValueTuple<int, object>>>>)
        };
    }
}
