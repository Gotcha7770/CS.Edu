using System;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using DynamicData;

namespace CS.Edu.Core.IO;

internal class ObservableDirectoryWrapper : IObservableDirectory
{
    private readonly IFileSystem _fileSystem;
    private readonly IFileSystemWatcher _watcher;
    private readonly BehaviorSubject<string> _fullPath;

    public ObservableDirectoryWrapper(IDirectoryInfo directoryInfo)
    {
        _fileSystem = directoryInfo.FileSystem;
        _watcher = _fileSystem.FileSystemWatcher.New(directoryInfo.FullName);

        var renamed = Observable.FromEventPattern<RenamedEventHandler, RenamedEventArgs>(
            x => _watcher.Renamed += x,
            x => _watcher.Renamed -= x);

        _fullPath = new BehaviorSubject<string>(directoryInfo.FullName);
        renamed.Where(x => x.EventArgs.OldName == _fullPath.Value)
            .Select(x => x.EventArgs.FullPath)
            .Subscribe(_fullPath);
        Name = renamed.Select(x => x.EventArgs.Name);
        _watcher.EnableRaisingEvents = true;
    }

    public IObservable<string> FullPath => _fullPath;
    public IObservable<string> Name { get; }

    public IDisposable Subscribe(IObserver<IChangeSet<string, string>> observer)
    {
        var changeSet = _fileSystem.Directory.EnumerateFileSystemEntries(_fullPath.Value)
            .Select(x => new Change<string, string>(ChangeReason.Add, x, x))
            .ToChangeSet();

        if(changeSet.Count > 0)
            observer.OnNext(changeSet);

        var created = Observable.FromEventPattern<FileSystemEventHandler, FileSystemEventArgs>(
            x => _watcher.Created += x,
            x => _watcher.Created -= x);
        var deleted = Observable.FromEventPattern<FileSystemEventHandler, FileSystemEventArgs>(
            x => _watcher.Deleted += x,
            x => _watcher.Deleted -= x);

        return created.Merge(deleted)
            .Where(x => x.EventArgs.FullPath != _fullPath.Value)
            .Select(x => x.EventArgs.ToChangeSet())
            .Subscribe(observer);
    }

    public void Dispose()
    {
        _fullPath.Dispose();
        _watcher.Dispose();
    }
}