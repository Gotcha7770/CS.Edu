using System.Collections.Generic;
using System.Linq;
using CS.Edu.Core.Comparers;
using CS.Edu.Core.Interfaces;
using FluentAssertions;
using Xunit;

namespace CS.Edu.Tests;

struct IntType : IMonoid<int>
{
    public int Append(int x, int y) => x + y;

    public int Empty() => 0;
}

struct StringType : IMonoid<string>
{
    public string Append(string one, string other) => one + other;

    public string Empty() => string.Empty;
}

struct SeqType<T> : IMonoid<IEnumerable<T>>
{
    public IEnumerable<T> Append(IEnumerable<T> x, IEnumerable<T> y) => x.Concat(y);

    public IEnumerable<T> Empty() => Enumerable.Empty<T>();
}

public class MonoidTests
{
    [Fact]
    public void IsIntTypeMonoid()
    {
        MonoidLaws.HasIdentity<IntType, int>(1)
            .Should().BeTrue();
        MonoidLaws.IsAssociative<IntType, int>(1, 11, 111)
            .Should().BeTrue();
    }

    [Fact]
    public void IsStringTypeMonoid()
    {
        MonoidLaws.HasIdentity<StringType, string>("test")
            .Should().BeTrue();
        MonoidLaws.IsAssociative<StringType, string>("a", "b", "c")
            .Should().BeTrue();
    }

    [Fact]
    public void IsSeqTypeMonoid()
    {
        var comparer = EnumerableEqualityComparer<int>.Instance;
        MonoidLaws.HasIdentity<SeqType<int>, IEnumerable<int>>(Enumerable.Range(0, 2), comparer)
            .Should().BeTrue();
        MonoidLaws.IsAssociative<SeqType<int>, IEnumerable<int>>([1, 2], [3], [4, 5], comparer)
            .Should().BeTrue();
    }
}