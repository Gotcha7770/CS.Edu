using System;
using System.Collections.Generic;

// ReSharper disable once CheckNamespace
namespace CS.Edu.Core.Extensions;

public static partial class Enumerables
{
    private static readonly Func<Random, Func<Range<int>, int>> Rand = rand => range => rand.Next(range.Min, range.Max);

    public static IEnumerable<int> Random(Range<int> range, int count) => Random(range, count, (int)DateTime.Now.Ticks);

    public static IEnumerable<int> Random(Range<int> range, int count, int seed)
    {
        var randomizer = Rand(new Random(seed));

        for (int i = 0; i < count; i++)
        {
            yield return randomizer(range);
        }
    }
}