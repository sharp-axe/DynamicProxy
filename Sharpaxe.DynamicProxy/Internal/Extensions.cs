using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace Sharpaxe.DynamicProxy.Internal
{
    public static class Extensions
    {
        public static bool IsEmpty<T>(this ICollection<T> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }
            return source.Count <= 0;
        }

        public static ReadOnlyDictionary<TKey, TElement> ToReadOnlyDictionary<TSource, TKey, TElement>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> valueSelector)
        {
            return new ReadOnlyDictionary<TKey, TElement>(source.ToDictionary(keySelector, valueSelector));
        }

        public static void EmitCreateTypeDefaultValueOnStack(this ILGenerator ILGenerator, Type type)
        {
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
    }
}
