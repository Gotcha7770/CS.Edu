using System.Collections.Generic;
using CS.Edu.Tests.Utils;
using CS.Edu.Tests.Utils.Models;
using Xunit;

namespace CS.Edu.Tests.TestCases;

public class AddOrUpdateTestCases : TheoryData<IList<KeyValue>, KeyValue, IEnumerable<KeyValue>>
{
    public AddOrUpdateTestCases()
    {
        Add(new List<KeyValue>
            {
                new KeyValue(1, "One"),
                new KeyValue(2, "Two")
            },
            new KeyValue(0, "Zero"),
            [new KeyValue(0, "Zero"),  new KeyValue(1, "One"), new KeyValue(2, "Two")]);

        Add(new List<KeyValue>
            {
                new KeyValue(1, "One"),
                new KeyValue(2, "Two")
            },
            new KeyValue(1, "Changed"),
            [ new KeyValue(1, "One/Changed"), new KeyValue(2, "Two")]);
    }
}