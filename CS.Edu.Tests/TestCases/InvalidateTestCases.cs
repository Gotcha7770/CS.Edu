using System.Collections.Generic;
using System.Linq;
using CS.Edu.Tests.Utils;
using Xunit;

namespace CS.Edu.Tests.TestCases;

public class InvalidateTestCases : TheoryData<IList<KeyValue>, IEnumerable<KeyValue>, IEnumerable<KeyValue>>
{
    public InvalidateTestCases()
    {
        Add(new List<KeyValue>(0), [new KeyValue(0, "Zero"), new KeyValue(1, "One")], [new KeyValue(0, "Zero"), new KeyValue(1, "One")]);

        Add(new List<KeyValue>
            {
                new KeyValue(1, "One"),
                new KeyValue(2, "Two")
            },
            [new KeyValue(0, "Zero"), new KeyValue(1, "Changed")],
            [new KeyValue(0, "Zero"), new KeyValue(1, "One/Changed")]);
        Add(new List<KeyValue>
            {
                new KeyValue(1, "One"),
                new KeyValue(2, "Two")
            },
            Enumerable.Empty<KeyValue>(),
            Enumerable.Empty<KeyValue>());
    }
}