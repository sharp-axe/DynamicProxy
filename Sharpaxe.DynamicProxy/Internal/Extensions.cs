using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace Sharpaxe.DynamicProxy.Internal
{
    internal static class Extensions
    {
        public static bool IsEmpty<T>(this ICollection<T> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return source.Count <= 0;
        }

        public static IEnumerable<T> ConcatInstances<T>(this T instance, params T[] anotherInstances)
        {
            return instance.ToEnumerable().Concat(anotherInstances);
        }

        public static IEnumerable<T> Concat<T>(this T instance, IEnumerable<T> enumerable)
        {
            return instance.ToEnumerable().Concat(enumerable);
        }

        public static IEnumerable<T> Concat<T>(this IEnumerable<T> source, T instance)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return source.Concat(instance.ToEnumerable());
        }

        public static IEnumerable<T> ToEnumerable<T>(this T instance)
        {
            yield return instance;
        }

        public static ReadOnlyDictionary<TKey, TValue> ToReadOnlyDictionary<TKey, TValue>(this IEnumerable<TKey> source, Func<TKey, TValue> valueSelector)
        {
            if (source == null)
            {
                throw new ArgumentNullException();
            }

            return new ReadOnlyDictionary<TKey, TValue>(source.ToDictionary(s => s, valueSelector));
        }

        public static ReadOnlyDictionary<TKey, TValue> ToReadOnlyDictionary<TSource, TKey, TValue>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TValue> valueSelector)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return new ReadOnlyDictionary<TKey, TValue>(source.ToDictionary(keySelector, valueSelector));
        }

        public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, Func<TKey, TValue> valueCreator)
        {
            if (dictionary == null)
            {
                throw new ArgumentNullException(nameof(dictionary));
            }

            if (valueCreator == null)
            {
                throw new ArgumentNullException(nameof(valueCreator));
            }

            if (!dictionary.TryGetValue(key, out TValue value))
            {
                dictionary.Add(key, value = valueCreator.Invoke(key));
            }
            return value;
        }

        public static Type[] GetMethodArgumentsTypes(this MethodInfo methodInfo)
        {
            if (methodInfo == null)
            {
                throw new ArgumentNullException(nameof(methodInfo));
            }

            return methodInfo.GetParameters().Select(p => p.ParameterType).ToArray();
        }

        public static void EmitCreateTypeDefaultValueOnStack(this ILGenerator ILGenerator, Type type)
        {
            if (ILGenerator == null)
            {
                throw new ArgumentNullException(nameof(ILGenerator));
            }

            switch (type)
            {
                case var _ when type == typeof(bool) || type == typeof(byte) || type == typeof(sbyte) || type == typeof(char) ||
                                type == typeof(int) || type == typeof(uint) || type == typeof(short) || type == typeof(ushort):
                    ILGenerator.Emit(OpCodes.Ldc_I4_0); break;

                case var _ when type == typeof(long) || type == typeof(ulong):
                    ILGenerator.Emit(OpCodes.Ldc_I4_0);
                    ILGenerator.Emit(OpCodes.Conv_I8); break;

                case var _ when type == typeof(float):
                    ILGenerator.Emit(OpCodes.Ldc_R4, 0f); break;

                case var _ when type == typeof(double):
                    ILGenerator.Emit(OpCodes.Ldc_R8, 0d); break;

                case var _ when type == typeof(decimal):
                    ILGenerator.Emit(OpCodes.Ldsfld, typeof(decimal).GetField("Zero", BindingFlags.Static | BindingFlags.Public)); break;

                case var _ when type.IsValueType:
                    var varialble = ILGenerator.DeclareLocal(type);
                    ILGenerator.Emit(OpCodes.Ldloca_S, varialble);
                    ILGenerator.Emit(OpCodes.Initobj, type);
                    ILGenerator.Emit(OpCodes.Ldloc_0); break;

                default:
                    ILGenerator.Emit(OpCodes.Ldnull); break;
            }
        }

        public static void EmitLoadArgumentsRange(this ILGenerator ILGenerator, int startArgumentIndex, int count)
        {
            if (ILGenerator == null)
            {
                throw new ArgumentNullException(nameof(ILGenerator));
            }

            for (short i = (short)startArgumentIndex; i < startArgumentIndex + count; i++)
            {
                ILGenerator.Emit(OpCodes.Ldarg, i);
            }
        }

        public static void EmitThrowNotSupportedException(this ILGenerator ILGenerator, string message)
        {
            if (ILGenerator == null)
            {
                throw new ArgumentNullException(nameof(ILGenerator));
            }

            ILGenerator.Emit(OpCodes.Ldstr, message);
            ILGenerator.Emit(OpCodes.Newobj, typeof(NotSupportedException).GetConstructor(new Type[] { typeof(string) }));
            ILGenerator.Emit(OpCodes.Throw);
        }

        public static (EventInfo[], MethodInfo[], PropertyInfo[]) GetAllInterfaceMembers(this Type interfaceType)
        {
            if (interfaceType == null)
            {
                throw new ArgumentNullException(nameof(interfaceType));
            }

            if (!interfaceType.IsInterface)
            {
                throw new ArgumentException("Type is not an interface", nameof(interfaceType));
            }

            var eventsInfo = new List<EventInfo>();
            var methodsInfo = new List<MethodInfo>();
            var propertyInfo = new List<PropertyInfo>();
            var typesToProcessMap = new Dictionary<Type, bool>() { { interfaceType, false } };

            while (typesToProcessMap.Any(kvp => kvp.Value == false))
            {
                foreach (var type in typesToProcessMap.Where(kvp => kvp.Value == false).Select(kvp => kvp.Key).ToList())
                {
                    eventsInfo.AddRange(type.GetEvents(BindingFlags.Public | BindingFlags.Instance));
                    methodsInfo.AddRange(type.GetMethods(BindingFlags.Public | BindingFlags.Instance).Where(m => !m.IsSpecialName));
                    propertyInfo.AddRange(type.GetProperties(BindingFlags.Public | BindingFlags.Instance));

                    foreach (var baseInterface in type.GetInterfaces().Where(t => !typesToProcessMap.ContainsKey(t)))
                    {
                        typesToProcessMap.Add(baseInterface, false);
                    }

                    typesToProcessMap[type] = true;
                }
            }

            return (eventsInfo.ToArray(), methodsInfo.ToArray(), propertyInfo.ToArray());
        }

        public static bool HasOutParameters(this MethodInfo methodInfo)
        {
            if (methodInfo == null)
            {
                throw new ArgumentNullException(nameof(methodInfo));
            }

            return methodInfo.GetParameters().Any(p => p.IsOut);
        }

        public static Type MakeGenericDelegateType(this MethodInfo methodInfo)
        {
            if (methodInfo == null)
            {
                throw new ArgumentNullException(nameof(methodInfo));
            }
               
            switch (methodInfo.ReturnType)
            {
                case Type voidType when voidType == typeof(void):
                    return methodInfo.GetMethodArgumentsTypes().MakeGenericDelegateAction();

                case Type nonVoidType:
                    return methodInfo.GetMethodArgumentsTypes().Concat(nonVoidType).MakeGenericDelegateFunction();

                case null:
                    throw new ArgumentException("Method info return type is null", $"{nameof(methodInfo)}.{nameof(methodInfo.ReturnType)}");
            }
        }

        public static Type MakeGenericDelegateFunction(this IEnumerable<Type> argumentsWithReturnType)
        {
            if (argumentsWithReturnType == null)
            {
                throw new ArgumentNullException(nameof(argumentsWithReturnType));
            }

            var argumentsWithReturnTypeArray = argumentsWithReturnType.ToArray();
            switch (argumentsWithReturnTypeArray.Length)
            {
                case 0:
                    throw new ArgumentException("Array is empty. At least one type must be provided as a return type");

                case 1:
                    return typeof(Func<>).MakeGenericType(argumentsWithReturnTypeArray);

                case 2:
                    return typeof(Func<,>).MakeGenericType(argumentsWithReturnTypeArray);

                case 3:
                    return typeof(Func<,,>).MakeGenericType(argumentsWithReturnTypeArray);

                case 4:
                    return typeof(Func<,,,>).MakeGenericType(argumentsWithReturnTypeArray);

                case 5:
                    return typeof(Func<,,,,>).MakeGenericType(argumentsWithReturnTypeArray);

                case 6:
                    return typeof(Func<,,,,,>).MakeGenericType(argumentsWithReturnTypeArray);

                case 7:
                    return typeof(Func<,,,,,,>).MakeGenericType(argumentsWithReturnTypeArray);

                case 8:
                    return typeof(Func<,,,,,,,>).MakeGenericType(argumentsWithReturnTypeArray);

                case 9:
                    return typeof(Func<,,,,,,,,>).MakeGenericType(argumentsWithReturnTypeArray);

                case 10:
                    return typeof(Func<,,,,,,,,,>).MakeGenericType(argumentsWithReturnTypeArray);

                case 11:
                    return typeof(Func<,,,,,,,,,,>).MakeGenericType(argumentsWithReturnTypeArray);

                case 12:
                    return typeof(Func<,,,,,,,,,,,>).MakeGenericType(argumentsWithReturnTypeArray);

                case 13:
                    return typeof(Func<,,,,,,,,,,,,>).MakeGenericType(argumentsWithReturnTypeArray);

                case 14:
                    return typeof(Func<,,,,,,,,,,,,,>).MakeGenericType(argumentsWithReturnTypeArray);

                case 15:
                    return typeof(Func<,,,,,,,,,,,,,,,>).MakeGenericType(argumentsWithReturnTypeArray);

                case 16:
                    return typeof(Func<,,,,,,,,,,,,,,,,>).MakeGenericType(argumentsWithReturnTypeArray);

                default:
                    throw new ArgumentException($"Arguments with return type count '{argumentsWithReturnTypeArray.Length}' greater than 17", nameof(argumentsWithReturnType));
            }
        }

        public static Type MakeGenericDelegateAction(this IEnumerable<Type> arguments)
        {
            if (arguments == null)
            {
                throw new ArgumentNullException(nameof(arguments));
            }

            var argumentsArray = arguments.ToArray();
            switch (argumentsArray.Length)
            {
                case 0:
                    return typeof(Action);

                case 1:
                    return typeof(Action<>).MakeGenericType(argumentsArray);

                case 2:
                    return typeof(Action<,>).MakeGenericType(argumentsArray);

                case 3:
                    return typeof(Action<,,>).MakeGenericType(argumentsArray);

                case 4:
                    return typeof(Action<,,,>).MakeGenericType(argumentsArray);

                case 5:
                    return typeof(Action<,,,,>).MakeGenericType(argumentsArray);

                case 6:
                    return typeof(Action<,,,,,>).MakeGenericType(argumentsArray);

                case 7:
                    return typeof(Action<,,,,,,>).MakeGenericType(argumentsArray);

                case 8:
                    return typeof(Action<,,,,,,,>).MakeGenericType(argumentsArray);

                case 9:
                    return typeof(Action<,,,,,,,,>).MakeGenericType(argumentsArray);

                case 10:
                    return typeof(Action<,,,,,,,,,>).MakeGenericType(argumentsArray);

                case 11:
                    return typeof(Action<,,,,,,,,,,>).MakeGenericType(argumentsArray);

                case 12:
                    return typeof(Action<,,,,,,,,,,,>).MakeGenericType(argumentsArray);

                case 13:
                    return typeof(Action<,,,,,,,,,,,,>).MakeGenericType(argumentsArray);

                case 14:
                    return typeof(Action<,,,,,,,,,,,,,>).MakeGenericType(argumentsArray);

                case 15:
                    return typeof(Action<,,,,,,,,,,,,,,>).MakeGenericType(argumentsArray);

                case 16:
                    return typeof(Action<,,,,,,,,,,,,,,,>).MakeGenericType(argumentsArray);

                default:
                    throw new ArgumentException($"Arguments count '{argumentsArray.Length}' greater than 16", nameof(arguments));
            }
        }
    }
}
