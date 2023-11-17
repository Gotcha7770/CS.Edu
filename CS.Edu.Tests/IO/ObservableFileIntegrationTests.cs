using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CS.Edu.Core.IO;
using CS.Edu.Tests.Utils.IO;
using FluentAssertions;
using Xunit;

namespace CS.Edu.Tests.IO;

public class ObservableFileIntegrationTests : IClassFixture<IOTestFixture>
{
    private readonly IOTestFixture _fixture;

    public ObservableFileIntegrationTests(IOTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public void FileSystemWatcher_FileCreated()
    {
        using var scope = _fixture.CreateTestScope("IOTests");
        scope.ScheduleAction(x => x.CreateFile("file.txt").Dispose(), 30);
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
        using var scope = _fixture.CreateTestScope("IOTests");
        scope.CreateFile("file.txt").Dispose();
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
        using var scope = _fixture.CreateTestScope("IOTests");
        scope.CreateFile("file.txt").Dispose();
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
        using var scope = _fixture.CreateTestScope("IOTests");
        scope.CreateFile("file.txt").Dispose();
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
}