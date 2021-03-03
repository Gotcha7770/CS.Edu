using System;
using System.Collections.Generic;
using CS.Edu.Core.Monads;
using DynamicData.Kernel;

namespace CS.Edu.Core.Extensions.EnumerableExtensions
{
    public static partial class EnumerableExt
    {
        public static FindResult<T> Find<T>(this IEnumerable<T> source, Predicate<T> predicate)
        {
            var lookup = source.FirstOrOptional(x => predicate(x));
            return lookup.HasValue ? new FindResult<T>(lookup.Value) : new FindResult<T>(source);
        }

        public static FindResult<T> ThenFind<T>(this FindResult<T> result, Predicate<T> predicate)
        {
            return result.Match(l => l.Find(predicate), r => result);
        }

        public static Optional<T> Result<T>(this FindResult<T> result)
        {
            return result.Match(l => Optional<T>.None, r => Optional<T>.Create(r));
        }
    }
}