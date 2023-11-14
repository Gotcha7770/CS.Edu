using System;
using System.IO;
using System.IO.Abstractions;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace CS.Edu.Core.IO;

internal class ObservableFileWrapper : IObservableFile
{
    private readonly IFileSystemWatcher _watcher;
    private readonly BehaviorSubject<string> _fullPath;

    public ObservableFileWrapper(IFileInfo fileInfo)
    {
        // optimize
        // Notifications are only raised for entries inside the directory you are watching.
        _watcher = fileInfo.FileSystem.FileSystemWatcher.New(fileInfo.DirectoryName);

        // var changeObserver = Observable.FromEventPattern<FileSystemEventHandler, FileSystemEventArgs>(
        //     x => _watcher.Changed += x,
        //     x => _watcher.Changed -= x);

        var renamed = Observable.FromEventPattern<RenamedEventHandler, RenamedEventArgs>(
            x => _watcher.Renamed += x,
            x => _watcher.Renamed -= x);

        _fullPath = new BehaviorSubject<string>(fileInfo.FullName);
        renamed.Where(x => x.EventArgs.OldName == _fullPath.Value)
            .Select(x => x.EventArgs.FullPath)
            .Subscribe(_fullPath);
        Name = renamed.Select(x => x.EventArgs.Name);
        _watcher.EnableRaisingEvents = true;
    }

    public IObservable<string> FullPath => _fullPath;
    public IObservable<string> Name { get; }

    public void Dispose()
    {
        _fullPath.Dispose();
        _watcher.Dispose();
    }
}