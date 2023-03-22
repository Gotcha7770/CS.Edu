using CS.Edu.Core.Extensions;
using FluentAssertions;
using Xunit;

namespace CS.Edu.Tests;

public class MemoizationTests
{
    class TestClass
    {
        public int Counter { get; private set; }

        public int Add(int one, int other)
        {
            Counter++;
            return one + other;
        }
    }

    [Fact]
    public void OneInvocation()
    {
        var fact = new TestClass();
        var memoized = Functions.Memoize<int, int, int>((x,y) => fact.Add(x, y));
        //var memoized = Functions.Memoize<(int, int), int>(x => Fact.Add(x.Item1, x.Item2));
        //var memoized = Functions.Memoize<(int, int), int>(((int a, int b) x) => Fact.Add(x.a, x.b));
        //var memoized = Functions.Memoize<int, Func<int, int>>(x => y => Fact.Add(x, y));
        var result = memoized(2, 3);

        result.Should().Be(5);
        fact.Counter.Should().Be(1);
    }

    [Fact]
    public void TwoInvocations_SameValue()
    {
        var fact = new TestClass();
        var memoized = Functions.Memoize<int, int, int>(fact.Add);
        var first = memoized(2, 3);
        var second = memoized(2, 3);

        first.Should().Be(5);
        second.Should().Be(5);
        fact.Counter.Should().Be(1);
    }

    [Fact]
    public void TwoInvocations_DifferentValues()
    {
        var fact = new TestClass();
        var memoized = Functions.Memoize<int, int, int>(fact.Add);
        var first = memoized(3, 2); //тут вопрос как считать, параметры то одинаковые
        var second = memoized(2, 3);

        first.Should().Be(5);
        second.Should().Be(5);
        fact.Counter.Should().Be(2);
    }
}