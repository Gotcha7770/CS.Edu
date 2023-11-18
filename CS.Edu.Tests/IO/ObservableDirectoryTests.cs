using System.IO.Abstractions;
using System.Threading.Tasks;
using CS.Edu.Core.IO;
using CS.Edu.Tests.Utils.IO;
using DynamicData;
using DynamicData.Tests;
using FluentAssertions;
using Xunit;

namespace CS.Edu.Tests.IO;

public class ObservableDirectoryTests : IClassFixture<IOTestFixture>
{
    private readonly IFileSystem _fileSystem = new FileSystem();
    private readonly IOTestFixture _fixture;

    public ObservableDirectoryTests(IOTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public void ObservableDirectory_EmptyDirectory_NoneInitialEntries()
    {
        using var scope = _fixture.CreateTestScope("IOTests", _fileSystem);
        using var aggregate = scope.Directory
            .ToObservable()
            .AsAggregator();

        aggregate.Messages.Should()
            .BeEmpty();
    }

    [Fact]
    public void ObservableDirectory_HasEntries_HasInitialEntries()
    {
        using var scope = _fixture.CreateTestScope("IOTests", _fileSystem);
        scope.CreateDirectory("Subdir");
        using var aggregate = scope.Directory
            .ToObservable()
            .AsAggregator();

        aggregate.Messages.Should()
            .ContainSingle()
            .Which.Should()
            .BeEquivalentTo(new[]
            {
                new Change<string, string>(
                    ChangeReason.Add,
                    @"C:\Users\gbaka\AppData\Local\Temp\IOTests\Subdir",
                    "Subdir")
            });
    }

    [Fact]
    public async Task FileCreated_NewItemAddedToDirectory()
    {
        using var scope = _fixture.CreateTestScope("IOTests", _fileSystem);
        using var aggregate = scope.Directory
            .ToObservable()
            .AsAggregator();

        await scope.CreateFile("file.txt", out _).DisposeAsync();
        await Task.Delay(150); //??? how to avoid delay

        aggregate.Messages.Should()
            .ContainSingle()
            .Which.Should()
            .BeEquivalentTo(new[]
            {
                new Change<string, string>(
                    ChangeReason.Add,
                    @"C:\Users\gbaka\AppData\Local\Temp\IOTests\file.txt",
                    "file.txt")
            });
    }

    //TODO: what should we do when file renamed?
    // [Fact]
    // public async Task FileRenamed()
    // {
    //     using var scope = _fixture.CreateTestScope("IOTests");
    //     await using (var _ = scope.CreateFile("file.txt")) { }
    //     using var aggregate = scope.Directory
    //         .ToObservable()
    //         .SkipInitial()
    //         .AsAggregator();
    //
    //     scope.MoveFile("file.txt", "new file.txt");
    //     await Task.Delay(150); //??? how to avoid delay
    //
    //     aggregate.Messages.Should().ContainSingle();
    // }

    [Fact]
    public async Task FileDeleted_ItemRemovedFromDirectory()
    {
        using var scope = _fixture.CreateTestScope("IOTests", _fileSystem);
        await scope.CreateFile("file.txt", out _).DisposeAsync();
        using var aggregate = scope.Directory
            .ToObservable()
            .SkipInitial()
            .AsAggregator();

        scope.DeleteFile("file.txt");
        await Task.Delay(150); //??? how to avoid delay

        aggregate.Messages.Should()
            .ContainSingle()
            .Which.Should()
            .BeEquivalentTo(new[]
            {
                new Change<string, string>(
                    ChangeReason.Remove,
                    @"C:\Users\gbaka\AppData\Local\Temp\IOTests\file.txt",
                    "file.txt")
            });
    }
}