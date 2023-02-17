using System;
using System.Collections.Generic;

// ReSharper disable once CheckNamespace
namespace CS.Edu.Core.Extensions;

public static partial class EnumerableExtensions
{
    private static readonly Func<Random, Func<Range, int>> Rand = rand => range => rand.Next(range.Start.Value, range.End.Value);

    public static IEnumerable<int> Random(Range range, int count) => Random(range, count, (int)DateTime.Now.Ticks);

    public static IEnumerable<int> Random(Range range, int count, int seed)
    {
        var randomizer = Rand(new Random(seed));

        for (int i = 0; i < count; i++)
        {
            yield return randomizer(range);
        }
    }
}