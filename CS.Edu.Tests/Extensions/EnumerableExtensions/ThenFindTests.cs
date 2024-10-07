using CS.Edu.Core.Extensions;
using CS.Edu.Tests.TestCases;
using DynamicData.Kernel;
using FluentAssertions;
using Xunit;

namespace CS.Edu.Tests.Extensions.EnumerableExtensions;

public class ThenFindTests
{
    [Theory]
    [ClassData(typeof(ThenFindTestCases))]
    public void FluentTwoLayersFind(int[] source, int expected)
    {
        Optional<int> result = source.FirstOrOptional(x => x == 10)
            .ValueOr(() => source.FirstOrOptional(x => x == 5))
            .ValueOr(() => source.FirstOrOptional(x => x % 3 == 0))
            .ValueOr(() => source.FirstOrOptional(x => x % 2 == 0));

        result.Should().Be(Optional.Some(expected));
    }
}