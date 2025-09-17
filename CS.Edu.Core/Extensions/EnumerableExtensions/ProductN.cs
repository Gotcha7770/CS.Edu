using System;
using System.Collections.Generic;
using System.Linq;

// ReSharper disable once CheckNamespace
namespace CS.Edu.Core.Extensions;

public static partial class EnumerableExtensions
{
    // https://ericlippert.com/2010/06/28/computing-a-cartesian-product-with-linq/
    // https://www.interact-sw.co.uk/iangblog/2010/07/28/linq-cartesian-1

    public static IEnumerable<IEnumerable<T>> ProductN<T>(this IEnumerable<IEnumerable<T>> source)
    {
        ArgumentNullException.ThrowIfNull(source);

        return source.Aggregate<IEnumerable<T>, IEnumerable<IEnumerable<T>>>(
            [[]],
            (acc, cur) =>
                from prevProductItem in acc
                from item in cur
                select prevProductItem.Append(item));
    }
}