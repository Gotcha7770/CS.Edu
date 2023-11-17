using System;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CS.Edu.Core.IO;
using CS.Edu.Tests.Utils.IO;
using FluentAssertions;
using Xunit;

namespace CS.Edu.Tests.IO;

public class ObservableFileTests : IClassFixture<IOTestFixture>
{
    private readonly IOTestFixture _fixture;

    public ObservableFileTests(IOTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task FileSystemWatcher_FileCreated()
    {
        using var scope = _fixture.CreateTestScope("IOTests");
        await using var _ = ScheduleAction(() => { using var _ = scope.CreateFile("file.txt"); }, 30);
        var result = scope.Watcher.WaitForChanged(WatcherChangeTypes.Created, 150);

        result.Should()
            .BeEquivalentTo(new
            {
                ChangeType = WatcherChangeTypes.Created,
                Name = "file.txt"
            });
    }

    [Fact]
    public async Task FileSystemWatcher_FileRenamed()
    {
        using var scope = _fixture.CreateTestScope("IOTests");
        using var monitor = scope.Watcher.Monitor();
        await using (var _ = scope.CreateFile("file.txt")) { }
        scope.Watcher.EnableRaisingEvents = true;

        scope.MoveFile("file.txt", "new file.txt");
        await Task.Yield(); //??? how to avoid yield

        monitor.Should()
            .Raise(nameof(IFileSystemWatcher.Renamed))
            .WithArgs<RenamedEventArgs>(args =>
                args.ChangeType == WatcherChangeTypes.Renamed
                && args.OldFullPath == scope.Directory + "\\file.txt"
                && args.FullPath == scope.Directory + "\\new file.txt");
    }

    [Fact]
    public async Task FileSystemWatcher_FileChanged()
    {
        using var scope = _fixture.CreateTestScope("IOTests");
        using var monitor = scope.Watcher.Monitor();
        await using (var _ = scope.CreateFile("file.txt")) { }
        scope.Watcher.EnableRaisingEvents = true;

        await using (var stream = _fixture.FileSystem.File.OpenWrite("file.txt"))
        {
            await stream.WriteAsync(new byte[10]);
        }

        monitor.Should()
            .Raise(nameof(IFileSystemWatcher.Changed))
            .WithArgs<FileSystemEventArgs>(args =>
                args.ChangeType == WatcherChangeTypes.Changed
                && args.FullPath == scope.Directory + "\\file.txt"
                && args.Name == "file.txt");
    }

    [Fact]
    public async Task FileSystemWatcher_FileDeleted()
    {
        using var scope = _fixture.CreateTestScope("IOTests");
        using var monitor = scope.Watcher.Monitor();
        await using (var _ = scope.CreateFile("file.txt")) { }
        scope.Watcher.EnableRaisingEvents = true;

        scope.DeleteFile("file.txt");
        await Task.Yield(); //??? how to avoid yield

        monitor.Should()
            .Raise(nameof(IFileSystemWatcher.Deleted))
            .WithArgs<FileSystemEventArgs>(args =>
                args.ChangeType == WatcherChangeTypes.Deleted
                && args.FullPath == scope.Directory + "\\file.txt"
                && args.Name == "file.txt");
    }

    [Fact]
    public async Task ObservableFile_InitialNames()
    {
        string name = null;
        string fullPath = null;
        using var scope = _fixture.CreateTestScope("IOTests");
        await using (var _ = scope.CreateFile("file.txt")) { }

        var file = scope.Directory.EnumerateFiles().First().ToObservable();
        using (var a = file.Name.Subscribe(x => name = x))
        using (var b = file.FullPath.Subscribe(x => fullPath = x)) { }

        name.Should()
            .Be("file.txt");
        fullPath.Should()
            .Be(scope.Directory.FullName + "\\file.txt");
    }

    [Fact]
    public async Task FileRenamed_ObservableFileNameChanged()
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
    public async Task ObservableFile_InitialPropertyValues()
    {
        long length = -1;
        DateTime lastWriteTime = DateTime.MinValue;
        using var scope = _fixture.CreateTestScope("IOTests");
        await using (var _ = scope.CreateFile("file.txt")) { }

        var file = scope.Directory.EnumerateFiles().First().ToObservable();
        using (var a = file.Length.Subscribe(x => length = x))
        using (var b = file.LastWriteTime.Subscribe(x => lastWriteTime = x)) { }

        length.Should()
            .Be(0);
        lastWriteTime.Should()
            .BeWithin(TimeSpan.FromSeconds(1))
            .Before(DateTime.Now);
    }

    [Fact]
    public async Task FileContentChanged_ObservableFilePropertiesChanged()
    {
        long length = -1;
        DateTime lastWriteTime = DateTime.MinValue;
        using var scope = _fixture.CreateTestScope("IOTests");
        await using (var _ = scope.CreateFile("file.txt")) { }

        var file = scope.Directory.EnumerateFiles().First().ToObservable();
        using (var a = file.Length.Subscribe(x => length = x))
        using (var b = file.LastWriteTime.Subscribe(x => lastWriteTime = x))
        {
            await using (var stream = _fixture.FileSystem.File.OpenWrite("file.txt"))
            {
                await stream.WriteAsync(new byte[10]);
            }
            await Task.Delay(150); //??? how to avoid delay
        }

        length.Should()
            .Be(10);
        lastWriteTime.Should()
            .BeWithin(TimeSpan.FromSeconds(1))
            .Before(DateTime.Now);
    }

    private static IAsyncDisposable ScheduleAction(Action action, int dueTime)
    {
        return new Timer(_ => action(), null, dueTime, Timeout.Infinite);
    }
}