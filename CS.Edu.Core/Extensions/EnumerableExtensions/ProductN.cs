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

        return source.Aggregate(
            EnumerableEx.Return(Enumerable.Empty<T>()),
            (acc, cur) =>
                from prevProductItem in acc
                from item in cur
                select prevProductItem.Append(item));
    }

    private static IEnumerable<IEnumerable<T>> Iterator<T>(IEnumerable<IEnumerable<T>> source)
    {
        using var enumerator = source.GetEnumerator();
        if(!enumerator.MoveNext())
            return Enumerable.Empty<IEnumerable<T>>();

        var product = enumerator.Current.Select(EnumerableEx.Return);
        while (enumerator.MoveNext())
        {
            var current = enumerator.Current;
            product = from prevProductItem in product
                      from item in current
                      select prevProductItem.Append(item);
        }

        return product;
    }
}