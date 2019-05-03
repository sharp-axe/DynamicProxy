using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

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
    }
}
