using System;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
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
    public async Task FileSystemWatcher_FileRenamed()
    {
        RenamedEventArgs args = null;
        using var scope = _fixture.CreateTestScope("IOTests");
        await using (var _ = scope.CreateFile("file.txt")) { }
        scope.Watcher.Renamed += (_, e) => args = e;
        scope.Watcher.EnableRaisingEvents = true;

        scope.MoveFile("file.txt", "new file.txt");
        await Task.Yield(); //??? how to avoid yield

        args.Should()
            .BeEquivalentTo(new
            {
                ChangeType = WatcherChangeTypes.Renamed,
                OldFullPath = scope.Directory + "\\file.txt",
                FullPath = scope.Directory + "\\new file.txt",
            });
    }

    [Fact]
    public async Task FileSystemWatcher_FileRenamed_Observable()
    {
        string name = null;
        string fullPath = null;
        using var scope = _fixture.CreateTestScope("IOTests");
        await using (var _ = scope.CreateFile("file.txt")) { }
        scope.Watcher.EnableRaisingEvents = true;

        var renamed = Observable.FromEventPattern<RenamedEventHandler, RenamedEventArgs>(
                x => scope.Watcher.Renamed += x,
                x => scope.Watcher.Renamed -= x)
            .Where(x => x.EventArgs.OldName == "file.txt");

        using var a = renamed.Select(x => x.EventArgs.FullPath).Subscribe(x => fullPath = x);
        using var b = renamed.Select(x => x.EventArgs.Name).Subscribe(x => name = x);

        scope.MoveFile("file.txt", "new file.txt");
        await Task.Yield(); //??? how to avoid yield
    }

    [Fact]
    public async Task FileSystemWatcher_FileChanged()
    {
        FileSystemEventArgs args = null;
        using var scope = _fixture.CreateTestScope("IOTests");
        await using (var _ = scope.CreateFile("file.txt")) { }

        scope.Watcher.Changed += (_, e) => args = e;
        scope.Watcher.EnableRaisingEvents = true;

        await using (var stream = _fixture.FileSystem.File.OpenWrite("file.txt"))
        {
            await stream.WriteAsync(new byte[10]);
        }
        await Task.Yield(); //??? how to avoid yield

        args.Should()
            .BeEquivalentTo(new
            {
                ChangeType = WatcherChangeTypes.Changed,
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
        string fullPath = null;
        using var scope = _fixture.CreateTestScope("IOTests");
        await using (var _ = scope.CreateFile("file.txt")) { }

        var file = scope.Directory.EnumerateFiles().First().ToObservable();
        using (var a = file.Name.Subscribe(x => name = x))
        using (var b = file.FullPath.Subscribe(x => fullPath = x))
        {
            scope.MoveFile("file.txt", "new file.txt");
            await Task.Delay(150); //??? how to avoid delay
        }

        name.Should()
            .Be("new file.txt");
        fullPath.Should()
            .Be(scope.Directory.FullName + "\\new file.txt");
    }

    [Fact]
    public async Task FileContentChanged_PropertiesChanged()
    {
        long length = 0;
        DateTime lastWriteTime = DateTime.MinValue;
        using var scope = _fixture.CreateTestScope("IOTests");
        await using (var _ = scope.CreateFile("file.txt")) { }

        var file = scope.Directory.EnumerateFiles().First().ToObservable();
        using (var a = file.Length.Subscribe(x => length = x))
        using (var b = file.LastWriteTime.Subscribe(x => lastWriteTime = x))
        {
            await using var stream = _fixture.FileSystem.File.OpenWrite("file.txt");
            await stream.WriteAsync(new byte[10]);
            await Task.Delay(150); //??? how to avoid delay
        }

        length.Should()
            .Be(10);
        lastWriteTime.Should()
            .BeWithin(TimeSpan.FromSeconds(1))
            .Before(DateTime.Now);
    }
}