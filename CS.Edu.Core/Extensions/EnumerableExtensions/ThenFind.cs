using System;
using System.Collections.Generic;
using CS.Edu.Core.Monads;
using DynamicData.Kernel;

// ReSharper disable once CheckNamespace
namespace CS.Edu.Core.Extensions
{
    public static partial class Enumerables
    {
        public static OneFrom<T> Find<T>(this IEnumerable<T> source, Predicate<T> predicate)
        {
            return source.FirstOrOptional(x => predicate(x))
                .ConvertOr(x => new OneFrom<T>(x), () => new OneFrom<T>(source));
        }

        public static OneFrom<T> ThenFind<T>(this OneFrom<T> result, Predicate<T> predicate)
        {
            return result.Match(l => l.Find(predicate), _ => result);
        }

        public static Optional<T> Result<T>(this OneFrom<T> result)
        {
            return result.Match(l => Optional<T>.None, Optional<T>.Create);
        }
    }
}