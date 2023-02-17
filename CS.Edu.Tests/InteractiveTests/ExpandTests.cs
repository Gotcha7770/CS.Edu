using System.Linq;
using System.Xml.Linq;
using FluentAssertions;
using Xunit;

namespace CS.Edu.Tests.InteractiveTests;

public class ExpandTests
{
    private readonly XElement _root = new XElement("root",
        new XElement("firstLevel",
            new XElement("secondLevel"),
            new XElement("secondLevel")),
        new XElement("firstLevel",
            new XElement("secondLevel"),
            new XElement("secondLevel")));

    [Fact]
    public void TreeTest()
    {
        var flatElements = _root.Elements()
            .Expand(x => x.Elements())
            .ToArray();

        flatElements.Select(x => x.Name.LocalName)
            .Should()
            .BeEquivalentTo(new[]
            {
                "firstLevel",
                "firstLevel",
                "secondLevel",
                "secondLevel",
                "secondLevel",
                "secondLevel"
            });
    }
}