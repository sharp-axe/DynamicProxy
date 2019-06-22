using Microsoft.VisualStudio.TestTools.UnitTesting;
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
                ExpectedIPropertyGetterFields
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
                .Concat(new KeyValuePair<string, Type>("Finalize", typeof(Action)))
                .Concat(new KeyValuePair<string, Type>("MemberwiseClone", typeof(Func<object>)))
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

            AssertTypeHasPassedPrivateInstanceMethods(type, expectedMethods);
        }

        [TestMethod]
        public void CreateType_IPropertySetter_HasExpectedFields()
        {
            var type = CreateTargetType(typeof(IPropertySetter));

            var expectedFields =
                ExpectedIPropertySetterFields
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
                .Concat(new KeyValuePair<string, Type>("Finalize", typeof(Action)))
                .Concat(new KeyValuePair<string, Type>("MemberwiseClone", typeof(Func<object>)))
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

            AssertTypeHasPassedPrivateInstanceMethods(type, expectedMethods);
        }

        [TestMethod]
        public void CreateType_IEvent_HasExpectedFields()
        {
            var type = CreateTargetType(typeof(IEvent));

            var expectedFields =
                ExpectedIEventFields
                .Concat(new KeyValuePair<string, Type>("target", typeof(IEvent)))
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

            AssertTypeHasPassedPrivateInstanceFields(type, expectedFields);
        }

        [TestMethod]
        public void CreateType_IMethod_HasExpectedFields()
        {
            var type = CreateTargetType(typeof(IMethod));

            var expectedFields =
                ExpectedIMethodFields
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
                .Concat(new KeyValuePair<string, Type>("Finalize", typeof(Action)))
                .Concat(new KeyValuePair<string, Type>("MemberwiseClone", typeof(Func<object>)))
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

            AssertTypeHasPassedPrivateInstanceMethods(type, expectedFunctions);
        }

        [TestMethod]
        public void CreateType_IInterface_HasExpectedFields()
        {
            var type = CreateTargetType(typeof(IInteface));

            var expectedFields =
                ExpectedIEventFields
                .Concat(ExpectedIMethodFields)
                .Concat(ExpectedIPropertyGetterFields)
                .Concat(ExpectedIPropertySetterFields)
                .Concat(new KeyValuePair<string, Type>("target", typeof(IInteface)))
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

            AssertTypeHasPassedPrivateInstanceFields(type, expectedFields);
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

        internal static Dictionary<string, Type> ExpectedIPropertyGetterFields = new Dictionary<string, Type>()
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

        internal static Dictionary<string, Type> ExpectedIPropertySetterFields = new Dictionary<string, Type>()
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

        internal static Dictionary<string, Type> ExpectedIEventFields = new Dictionary<string, Type>()
        {
            ["EventEmptyArgsEventProxy0"] = typeof(Action<Action<object, EventArgs>, object, EventArgs>),
            ["EventIntArgsEventProxy0"] = typeof(Action<Action<object, EventArgs<int>>, object, EventArgs<int>>),
            ["EventStringArgsEventProxy0"] = typeof(Action<Action<object, EventArgs<string>>, object, EventArgs<string>>),

            ["EventEmptyArgsEventDecorators0"] = typeof(LinkedList<ValueTuple<Action<object, EventArgs>, Action<object, EventArgs>>>),
            ["EventIntArgsEventDecorators0"] = typeof(LinkedList<ValueTuple<Action<object, EventArgs<int>>, Action<object, EventArgs<int>>>>),
            ["EventStringArgsEventDecorators0"] = typeof(LinkedList<ValueTuple<Action<object, EventArgs<string>>, Action<object, EventArgs<string>>>>),
        };

        internal static Dictionary<string, Type> ExpectedIMethodFields = new Dictionary<string, Type>()
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
    }
}
