using System.Collections.Generic;
using System.Linq;
using CS.Edu.Core.Comparers;
using CS.Edu.Core.Interfaces;
using NUnit.Framework;

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

[TestFixture]
public class MonoidTests
{
    [Test]
    public void IsIntTypeMonoid()
    {
        Assert.True(MonoidLaws.HasIdentity<IntType, int>(1));
        Assert.True(MonoidLaws.IsAssociative<IntType, int>(1, 11, 111));
    }

    [Test]
    public void IsStringTypeMonoid()
    {
        Assert.True(MonoidLaws.HasIdentity<StringType, string>("test"));
        Assert.True(MonoidLaws.IsAssociative<StringType, string>("a", "b", "c"));
    }

    [Test]
    public void IsSeqTypeMonoid()
    {
        var comparer = EnumerableEqualityComparer<int>.Instance;
        Assert.True(MonoidLaws.HasIdentity<SeqType<int>, IEnumerable<int>>(Enumerable.Range(0, 2), comparer));
        Assert.True(MonoidLaws.IsAssociative<SeqType<int>, IEnumerable<int>>(new[] { 1, 2 }, new[] { 3 }, new[] { 4, 5 }, comparer));
    }
}