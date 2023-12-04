using System.IO.Abstractions.TestingHelpers;
using System.Linq;
using System.Text.RegularExpressions;
using CS.Edu.Core.IO;
using FluentAssertions;
using Xunit;

namespace CS.Edu.Tests.IO;

public class GetNextDirectoryIndexTests
{
    private const string NewDirectoryName = "Новая папка";
    private readonly MockFileSystem _fileSystem = new MockFileSystem();

    [Fact]
    public void GetIndexForNewFolder_FirstDirectory_ReturnsZero()
    {
        int index = _fileSystem.GetNextDirectoryIndex(".");

        index.Should()
            .Be(0);
    }

    [Theory]
    [InlineData(new[] {1}, 2)]
    [InlineData(new[] {1, 2}, 3)]
    [InlineData(new[] {1, 2, 3, 4, 5}, 6)]
    public void GetIndexForNextFolder_ReturnsNextInt(int[] indices, int expected)
    {
        _fileSystem.AddDirectory(NewDirectoryName);

        indices.ForEach(x => _fileSystem.AddDirectory($"{NewDirectoryName} ({x})"));
        int index = _fileSystem.GetNextDirectoryIndex(".");

        index.Should()
            .Be(expected);
    }

    [Theory]
    [InlineData("Новаяпапка", false)]
    [InlineData("Новая папка1", false)]
    [InlineData("Новая папка 1", false)]
    [InlineData("Новая папка (1", false)]
    [InlineData("Новая папка 1)", false)]
    [InlineData("Новая папка (a)", false)]
    [InlineData("Новая папка (a1)", false)]
    [InlineData("яНовая папка (1)", false)]
    [InlineData("Новая папка ((1)", false)]
    [InlineData("Новая папка (1)a", false)]
    [InlineData("Новая папка", true)]
    [InlineData("Новая папка (1)", true)]
    [InlineData("Новая папка (12)", true)]
    public void MatchNewDirectoryPattern(string path, bool expected)
    {
        var regex = new Regex(@"^Новая папка( \(\d+\))?$");

        regex.IsMatch(path)
            .Should()
            .Be(expected);
    }
}