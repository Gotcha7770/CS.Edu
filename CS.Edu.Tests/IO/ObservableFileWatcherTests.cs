using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CS.Edu.Core.IO;
using CS.Edu.Tests.Utils.IO;
using DynamicData;
using DynamicData.Tests;
using FluentAssertions;
using Xunit;

namespace CS.Edu.Tests.IO;

public class ObservableFileWatcherTests : IClassFixture<IOTestFixture>
{
    private readonly IOTestFixture _fixture;

    public ObservableFileWatcherTests(IOTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task FileSystemWatcher_FileCreated()
    {
        FileSystemEventArgs args = null;
        using var scope = _fixture.CreateTestScope("IOTests");
        scope.Watcher.Created += (_, e) => args = e;
        scope.Watcher.EnableRaisingEvents = true;

        await using (var _ = scope.CreateFile("file.txt")) { }
        await Task.Yield(); //??? how to avoid yield

        args.Should()
            .BeEquivalentTo(new
            {
                ChangeType = WatcherChangeTypes.Created,
                FullPath = scope.Directory + "\\file.txt",
                Name = "file.txt"
            });
    }

    [Fact]
    public async Task FileSystemWatcher_FileDeleted()
    {
        FileSystemEventArgs args = null;
        using var scope = _fixture.CreateTestScope("IOTests");
        await using (var _ = scope.CreateFile("file.txt")) { }

        scope.Watcher.Deleted += (_, e) => args = e;
        scope.Watcher.EnableRaisingEvents = true;

        scope.DeleteFile("file.txt");
        await Task.Yield(); //??? how to avoid yield

        args.Should()
            .BeEquivalentTo(new
            {
                ChangeType = WatcherChangeTypes.Deleted,
                FullPath = scope.Directory + "\\file.txt",
                Name = "file.txt"
            });
    }

    [Fact]
    public void ObservableDirectory_EmptyDirectory_NoneInitialEntries()
    {
        using var scope = _fixture.CreateTestScope("IOTests");
        using var aggregate = scope.Directory
            .ToObservable()
            .AsAggregator();

        aggregate.Messages.Should()
            .BeEmpty();
    }

    [Fact]
    public void ObservableDirectory_HasEntries_HasInitialEntries()
    {
        using var scope = _fixture.CreateTestScope("IOTests");
        scope.CreateDirectory("Subdir");
        using var aggregate = scope.Directory
            .ToObservable()
            .AsAggregator();

        aggregate.Messages.Should().ContainSingle();
        aggregate.Messages[0].Should()
            .BeEquivalentTo(new[]
            {
                new Change<string, string>(
                    ChangeReason.Add,
                    @"C:\Users\gbaka\AppData\Local\Temp\IOTests\Subdir",
                    @"C:\Users\gbaka\AppData\Local\Temp\IOTests\Subdir")
            });
    }

    [Fact]
    public async Task FileCreated_NewItemAddedToDirectory()
    {
        using var scope = _fixture.CreateTestScope("IOTests");
        using var aggregate = scope.Directory
            .ToObservable()
            .AsAggregator();

        await using (var _ = scope.CreateFile("file.txt")) { }
        await Task.Delay(150); //??? how to avoid delay

        aggregate.Messages.Should().ContainSingle();
        aggregate.Messages[0].Should()
            .BeEquivalentTo(new[]
            {
                new Change<string, string>(
                    ChangeReason.Add,
                    @"C:\Users\gbaka\AppData\Local\Temp\IOTests\file.txt",
                    "file.txt")
            });
    }

    [Fact]
    public async Task FileDeleted_ItemRemovedFromDirectory()
    {
        using var scope = _fixture.CreateTestScope("IOTests");
        await using (var _ = scope.CreateFile("file.txt")) { }
        using var aggregate = scope.Directory
            .ToObservable()
            .SkipInitial()
            .AsAggregator();

        scope.DeleteFile("file.txt");
        await Task.Delay(150); //??? how to avoid delay

        aggregate.Messages.Should().ContainSingle();
        aggregate.Messages[0].Should()
            .BeEquivalentTo(new[]
            {
                new Change<string, string>(
                    ChangeReason.Remove,
                    @"C:\Users\gbaka\AppData\Local\Temp\IOTests\file.txt",
                    "file.txt")
            });
    }

    [Fact]
    public async Task FileRenamed_FileNameChanged()
    {
        string name = null;
        using var scope = _fixture.CreateTestScope("IOTests");
        await using (var _ = scope.CreateFile("file.txt")) { }

        var file = scope.Directory.EnumerateFiles().First().ToObservable();
        using (var _ = file.Name.Subscribe(x => name = x))
        {
            scope.MoveFile("file.txt", "new file.txt");
            await Task.Delay(150); //??? how to avoid delay
        }

        name.Should()
            .Be("new file.txt");
    }
}