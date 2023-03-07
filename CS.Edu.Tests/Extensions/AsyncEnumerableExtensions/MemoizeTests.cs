using System.Linq;
using System.Threading.Tasks;
using CS.Edu.Core.Extensions;
using FluentAssertions;
using Xunit;

namespace CS.Edu.Tests.Extensions.AsyncEnumerableExtensions;

public class MemoizeTests
{
    [Fact]
    public async Task EmptyIn_EmptyOut()
    {
        var memoized = AsyncEnumerable.Empty<int>().Memoize();

        var result = await memoized.ToArrayAsync();
        result.Should().BeEmpty();
    }
}