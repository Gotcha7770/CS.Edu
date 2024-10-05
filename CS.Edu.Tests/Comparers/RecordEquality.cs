using FluentAssertions;
using Xunit;

namespace CS.Edu.Tests.Comparers;

public class RecordEquality
{
    record Entity(int Id, string Tag, Entity[] Children);

    [Fact]
    public void EntityEquality()
    {
        var one = new Entity(1, "First", []);
        var other = new Entity(1, "First", []);

        one.Equals(other)
            .Should()
            .BeTrue();

        one = one with { Children = [new Entity(2, "Second", [])] };
        other = other with { Children = [new Entity(2, "Second", [])] };

        one.Equals(other)
            .Should()
            .BeFalse();

        one.Children.GetHashCode().Should().NotBe(other.GetHashCode());
    }
}