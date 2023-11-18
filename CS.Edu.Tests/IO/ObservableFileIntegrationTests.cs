using System;
using System.IO;
using System.IO.Abstractions;
using System.Reactive;
using System.Threading.Tasks;
using CS.Edu.Core.IO;
using CS.Edu.Tests.Utils.IO;
using FluentAssertions;
using Microsoft.Reactive.Testing;
using Xunit;

namespace CS.Edu.Tests.IO;

//https://learn.microsoft.com/en-us/dotnet/api/system.io.filesystemwatcher.changed?view=net-7.0#remarks
//https://www.codeproject.com/Articles/1220093/A-Robust-Solution-for-FileSystemWatcher-Firing-Eve

public class ObservableFileTests : IClassFixture<IOTestFixture>
{
    private readonly TestScheduler _scheduler = new TestScheduler();
    private readonly IFileSystem _fileSystem = new FileSystem();
    private readonly IOTestFixture _fixture;

    public ObservableFileTests(IOTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public void FileSystemWatcher_FileCreated()
    {
        using var scope = _fixture.CreateTestScope("IOTests", _fileSystem);
        scope.ScheduleAction(x => x.CreateFile("file.txt", out _).Dispose(), 30);
        var result = scope.Watcher.WaitForChanged(WatcherChangeTypes.Created, 150);

        result.Should()
            .BeEquivalentTo(new
            {
                ChangeType = WatcherChangeTypes.Created,
                Name = "file.txt"
            });
    }

    [Fact]
    public void FileSystemWatcher_FileRenamed()
    {
        using var scope = _fixture.CreateTestScope("IOTests", _fileSystem);
        scope.CreateFile("file.txt", out _).Dispose();
        scope.ScheduleAction(x => x.MoveFile("file.txt", "new file.txt"), 30);
        var result = scope.Watcher.WaitForChanged(WatcherChangeTypes.Renamed, 150);

        result.Should()
            .BeEquivalentTo(new
            {
                ChangeType = WatcherChangeTypes.Renamed,
                OldName = "file.txt",
                Name = "new file.txt"
            });
    }

    [Fact]
    public void FileSystemWatcher_FileChanged()
    {
        using var scope = _fixture.CreateTestScope("IOTests", _fileSystem);
        scope.CreateFile("file.txt", out _).Dispose();
        scope.ScheduleAction(x => x.Write("file.txt", new byte[10]), 30);
        var result = scope.Watcher.WaitForChanged(WatcherChangeTypes.Changed, 150);

        result.Should()
            .BeEquivalentTo(new
            {
                ChangeType = WatcherChangeTypes.Changed,
                Name = "file.txt"
            });
    }

    [Fact]
    public void FileSystemWatcher_FileDeleted()
    {
        using var scope = _fixture.CreateTestScope("IOTests", _fileSystem);
        scope.CreateFile("file.txt", out _).Dispose();
        scope.ScheduleAction(x => x.DeleteFile("file.txt"), 30);
        var result = scope.Watcher.WaitForChanged(WatcherChangeTypes.Deleted, 150);

        result.Should()
            .BeEquivalentTo(new
            {
                ChangeType = WatcherChangeTypes.Deleted,
                Name = "file.txt"
            });
    }

    [Fact]
    public async Task ObservableFile_Name()
    {
        using var scope = _fixture.CreateTestScope("IOTests", _fileSystem);
        await scope.CreateFile("file.txt", out var file).DisposeAsync();

        var testObserver = _scheduler.CreateObserver<string>();
        using var subscription = file.ToObservable().Name.Subscribe(testObserver);
        scope.MoveFile("file.txt", "new file.txt");
        await Task.Delay(150); //??? how to avoid delay

        testObserver.Messages.Should()
            .BeEquivalentTo(new[]
            {
                new { Value = Notification.CreateOnNext("file.txt") },
                new { Value = Notification.CreateOnNext("new file.txt") }
            });
    }

    [Fact]
    public async Task ObservableFile_FullPath()
    {
        using var scope = _fixture.CreateTestScope("IOTests", _fileSystem);
        await scope.CreateFile("file.txt", out var file).DisposeAsync();

        var testObserver = _scheduler.CreateObserver<string>();
        using var subscription = file.ToObservable().FullPath.Subscribe(testObserver);
        scope.MoveFile("file.txt", "new file.txt");
        await Task.Delay(150); //??? how to avoid delay

        testObserver.Messages.Should()
            .BeEquivalentTo(new[]
            {
                new { Value = Notification.CreateOnNext(scope.Directory.FullName + "\\file.txt") },
                new { Value = Notification.CreateOnNext(scope.Directory.FullName + "\\new file.txt") }
            });
    }

    [Fact]
    public async Task ObservableFile_Length()
    {
        using var scope = _fixture.CreateTestScope("IOTests", _fileSystem);
        await scope.CreateFile("file.txt", out var file).DisposeAsync();

        //var testObserver = _scheduler.Start(() => file.ToObservable().Length, 0, 0, ReactiveTest.Disposed);
        var testObserver = _scheduler.CreateObserver<long>();
        using var subscription = file.ToObservable().Length.Subscribe(testObserver);
        scope.Write("file.txt", new byte[10]);
        await Task.Delay(150); //??? how to avoid delay

        testObserver.Messages.Should()
            .BeEquivalentTo(new[]
            {
                new { Value = Notification.CreateOnNext(0L) },
                new { Value = Notification.CreateOnNext(10L) }
            });
    }

    [Fact]
    public async Task ObservableFile_LastWriteTime()
    {
        using var scope = _fixture.CreateTestScope("IOTests", _fileSystem);
        await scope.CreateFile("file.txt", out var file).DisposeAsync();

        //var testObserver = _scheduler.Start(() => file.ToObservable().LastWriteTime, 0, 0, ReactiveTest.Disposed);
        var testObserver = _scheduler.CreateObserver<DateTime>();
        using var subscription = file.ToObservable().LastWriteTime.Subscribe(testObserver);
        scope.Write("file.txt", new byte[10]);
        await Task.Delay(150); //??? how to avoid delay

        testObserver.Messages.Should()
            .BeEquivalentTo(new[]
            {
                new { Value = Notification.CreateOnNext(file.CreationTime) },
                new { Value = Notification.CreateOnNext(file.LastWriteTime) }
            });
    }
}