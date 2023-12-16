using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions.TestingHelpers;
using FluentAssertions;
using Xunit;

namespace CS.Edu.Tests.IO;

public class PathExistedApiTests
{
    private readonly MockFileSystem _fileSystem = new MockFileSystem(
        new Dictionary<string, MockFileData>
        {
            ["user"] = new MockDirectoryData(),
            ["user/dir1"] = new MockDirectoryData(),
            ["user/dir2"] = new MockDirectoryData(),
            ["user/dir2/subdir"] = new MockDirectoryData()
        },
        @"C:\user\dir2");

    [Theory]
    [InlineData("1", "/v1/items/1")]
    [InlineData("../cart", "/v1/cart")]
    [InlineData("/v2/cart", "/v2/cart")]
    public void Path_Join(string relative, string expected)
    {
        const string current = "/v1/items";
        Path.Join(current, relative)
            .Should()
            .Be(expected);
    }

    [Theory]
    [InlineData("1", "/v1/items/1")]
    [InlineData("../cart", "/v1/cart")]
    [InlineData("/v2/cart", "/v2/cart")]
    public void Path_Combine(string relative, string expected)
    {
        const string current = "/v1/items";
        Path.Combine(current, relative)
            .Should()
            .Be(expected);
    }

    [Theory]
    [InlineData("1", @"C:\v1\items\1")]
    [InlineData("../cart", @"C:\v1\cart")]
    [InlineData("/v2/cart", @"C:\v2\cart")]
    public void Path_GetFullPath(string relative, string expected)
    {
        // works based on OS parameters
        const string current = @"C:\v1\items";
        //const string current = "/v1/items";
        _fileSystem.Path.GetFullPath(relative, current)
            .Should()
            .Be(expected);
    }

    [Theory]
    [InlineData("subdir")]
    [InlineData("../dir1")]
    [InlineData(@"C:\user\dir1")]
    public void GetFile(string path)
    {
        _fileSystem.Directory.Exists(path)
            .Should()
            .BeTrue();
    }
}