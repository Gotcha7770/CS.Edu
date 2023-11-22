using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using System.Linq;
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
        int index = _fileSystem.GetNextDirectoryIndex2(".");

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
        int index = _fileSystem.GetNextDirectoryIndex2(".");

        index.Should()
            .Be(expected);
    }

    [Theory]
    [InlineData("Новая папка1")]
    [InlineData("Новая папка (a)")]
    public void GetIndexForNewFolder_ContainsFolderInWrongFormat_ReturnsNextInt(string directory)
    {
        _fileSystem.AddDirectory(directory);
        int index = _fileSystem.GetNextDirectoryIndex2(".");

        index.Should()
            .Be(0);
    }
}

internal static class FileSystemExtensions
{
    public static int GetNextDirectoryIndex(this IFileSystem fileSystem, string directory)
    {
        return fileSystem.Directory.GetDirectories(directory)
            .Select(s => s[s.LastIndexOf('\\')..][1..])
            .Where(s => s.Contains("Новая папка")
                        && !s.Equals("Новая папка")
                        && int.TryParse(s[(s.IndexOf('(') + 1)..s.IndexOf(')')], out _))
            .Select(s => int.Parse(s[(s.IndexOf('(') + 1)..s.IndexOf(')')]))?.Max() ?? 1;
    }

    public static int GetNextDirectoryIndex2(this IFileSystem fileSystem, string directory)
    {
        int index = fileSystem.Directory.Exists("Новая папка") ? 1 : 0;
        return index + fileSystem.Directory.EnumerateDirectories(directory, "Новая папка (*)")
            .Count();
    }
}