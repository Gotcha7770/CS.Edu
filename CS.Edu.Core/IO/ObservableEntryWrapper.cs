using System;
using System.IO;
using System.IO.Abstractions;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace CS.Edu.Core.IO;

internal record FileNames(string Name, string FullPath);

internal abstract class ObservableEntryWrapper : IObservableEntry
{
    private readonly BehaviorSubject<FileNames> _fileNames;
    protected readonly IFileSystemWatcher Watcher;

    protected ObservableEntryWrapper(IFileSystemInfo fileSystemInfo, string path)
    {
        // optimize
        // Notifications are only raised for entries inside the directory you are watching.
        Watcher = fileSystemInfo.FileSystem.FileSystemWatcher.New(path);
        _fileNames = new BehaviorSubject<FileNames>(new FileNames(fileSystemInfo.Name, fileSystemInfo.FullName));

        Observable.FromEventPattern<RenamedEventHandler, RenamedEventArgs>(
                x => Watcher.Renamed += x,
                x => Watcher.Renamed -= x)
            .Where(x => x.EventArgs.OldFullPath == _fileNames.Value.FullPath)
            .Select(x => new FileNames(x.EventArgs.Name, x.EventArgs.FullPath))
            .Subscribe(_fileNames);

        Name = _fileNames.Select(x => x.Name);
        FullPath = _fileNames.Select(x => x.FullPath);
    }

    protected IFileSystem FileSystem => Watcher.FileSystem;
    protected string EntryFullPath => _fileNames.Value.FullPath;

    public IObservable<string> FullPath { get; }
    public IObservable<string> Name { get; }
    public IObservable<DateTime> LastWriteTime { get; protected init; }

    public virtual void Dispose()
    {
        Watcher.EnableRaisingEvents = false;
        _fileNames.Dispose();
        Watcher.Dispose();
    }
}