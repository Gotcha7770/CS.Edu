using System.Collections.Generic;
using Xunit;

namespace CS.Edu.Tests.TestCases;

public class UniquePriorityQueueCases : TheoryData<IEnumerable<int>, IEnumerable<int>>
{
    public UniquePriorityQueueCases()
    {
        Add(
            [],
            []);

        Add(
            [1, 2, 3],
            [1, 2, 3]);

        Add(
            [3, 1, 2],
            [3, 1, 2]);

        Add(
            [1, 1, 2, 2, 3],
            [1, 2, 3]);

        Add(
            [3, 3, 1, 1, 2],
            [1, 2, 3]);
    }
}