using System.Collections.Generic;
using System.Linq;
using CS.Edu.Core.Extensions;
using DynamicData.Kernel;
using Xunit;

namespace CS.Edu.Tests.Extensions.EnumerableExtensions;

public class ThenFindTests
{
    private readonly IEnumerable<int> _source = Enumerable.Range(1, 99);

    [Fact]
    public void FluentOneLayerFind()
    {
        //Optional<int> result = _source.Find(x => x.IsEven()).Result();
        Optional<int> result = _source.FirstOrOptional(x => x.IsEven());
    }

    [Fact]
    public void FluentTwoLayersFind()
    {
        //Optional<int> result = _source.Find(x => x == 101).ThenFind(x => x % 3 == 0).Result();
        Optional<int> result = _source.Where(x => x == 101)
            .DefaultIfEmpty(() => _source.FirstOrDefault(x => x % 3 == 0))
            .FirstOrDefault();
    }
}