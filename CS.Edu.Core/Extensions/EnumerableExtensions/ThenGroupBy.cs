using System;
using System.Collections.Generic;
using System.Linq;
using CS.Edu.Core.Monads;

// ReSharper disable once CheckNamespace
namespace CS.Edu.Core.Extensions
{
    public static partial class Enumerables
    {
        public static Group<TKey, T> ToGroup<TKey, T>(this IGrouping<TKey, T> grouping)
        {
            return new Group<TKey, T>(grouping.Key, grouping);
        }

        public static Group<TKey, T> ToGroup<TKey, T>(this IGrouping<TKey, T> grouping, Func<T, TKey> keySelector)
        {
            return new Group<TKey, T>(grouping.Key, grouping.GroupBy(keySelector).Select(x => x.ToGroup()));
        }

        public static IEnumerable<Group<TKey, T>> ThenBy<TKey, T>(this IEnumerable<IGrouping<TKey, T>> source, Func<T, TKey> keySelector)
        {
            return source.Select(x => x.ToGroup(keySelector));
        }

        public static IEnumerable<Group<TKey, T>> ThenBy<TKey, T>(this IEnumerable<Group<TKey, T>> source, Func<T, TKey> keySelector)
        {
            return source.Select(x => x.Match(
                l => new Group<TKey, T>(x.Key, l.ThenBy(keySelector)),
                r => new Group<TKey, T>(x.Key, r.GroupBy(keySelector).Select(y => y.ToGroup()))));
        }
    }
}