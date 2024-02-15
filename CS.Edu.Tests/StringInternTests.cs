using System.Collections.Generic;
using FluentAssertions;
using Xunit;

namespace CS.Edu.Tests;

public class StringInternTests
{
    // Среда CLR поддерживает таблицу называемую пул интернирования.
    // Эта таблица содержит одну уникальную ссылку на каждую строку,
    // которая либо объявлена, либо создана программно во время выполнения вашей программы.

    [Theory]
    [MemberData(nameof(StringEqualityCases))]
    public void StringEquality(string one, string other, bool expected)
    {
        one.Equals(other)
            .Should()
            .BeTrue();
        ReferenceEquals(one, other)
            .Should()
            .Be(expected);
    }

    public static IEnumerable<object[]> StringEqualityCases
    {
        get
        {
            const string constant = "hello";
            string variable = "hello";

            yield return ["hello world", "hello" + " world", true];
            yield return ["hello world", constant + " world", true];
            yield return ["hello world", variable + " world", false];
            yield return ["hello world", string.Intern(variable + " world"), true];
        }
    }
}