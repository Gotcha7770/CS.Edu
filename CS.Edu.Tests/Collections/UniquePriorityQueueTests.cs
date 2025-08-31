using System.Collections.Generic;
using System.Linq;
using CS.Edu.Core.Collections;
using CS.Edu.Tests.TestCases;
using FluentAssertions;
using Xunit;

namespace CS.Edu.Tests.Collections;

public class UniquePriorityQueueTests
{
    [Theory]
    [ClassData(typeof(UniquePriorityQueueCases))]
    public void UniquePriorityQueue(IEnumerable<int> input, IEnumerable<int> expected)
    {
        var queue = new UniquePriorityQueue<int>(input);
        queue.ToArray()
            .Should()
            .BeEquivalentTo(expected);
    }

    [Theory]
    [ClassData(typeof(UniquePriorityQueueCases))]
    public void CollapsingPriorityQueue(IEnumerable<int> input, IEnumerable<int> expected)
    {
        var queue = new CollapsingPriorityQueue<int>(input);
        queue.ToArray()
            .Should()
            .BeEquivalentTo(expected);
    }
}