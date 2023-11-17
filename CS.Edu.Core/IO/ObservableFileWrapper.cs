using System;
using System.IO;
using System.IO.Abstractions;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace CS.Edu.Core.IO;

internal class ObservableFileWrapper : IObservableFile
{
    private record FileNames(string Name, string FullPath);

    private readonly IFileSystemWatcher _watcher;
    private readonly BehaviorSubject<FileNames> _fileNames;
    private IFileSystem FileSystem => _watcher.FileSystem;

    public ObservableFileWrapper(IFileInfo fileInfo)
    {
        // optimize
        // Notifications are only raised for entries inside the directory you are watching.
        _watcher = fileInfo.FileSystem.FileSystemWatcher.New(fileInfo.DirectoryName);
        _fileNames = new BehaviorSubject<FileNames>(new FileNames(fileInfo.Name, fileInfo.FullName));

        var changeObserver = Observable.FromEventPattern<FileSystemEventHandler, FileSystemEventArgs>(
            x => _watcher.Changed += x,
            x => _watcher.Changed -= x)
            .Where(x => x.EventArgs.FullPath == _fileNames.Value.FullPath)
            .Select(x => FileSystem.FileInfo.New(x.EventArgs.FullPath));

        Observable.FromEventPattern<RenamedEventHandler, RenamedEventArgs>(
                x => _watcher.Renamed += x,
                x => _watcher.Renamed -= x)
            .Where(x => x.EventArgs.OldFullPath == _fileNames.Value.FullPath)
            .Select(x => new FileNames(x.EventArgs.Name, x.EventArgs.FullPath))
            .Subscribe(_fileNames);

        Name = _fileNames.Select(x => x.Name);
        FullPath = _fileNames.Select(x => x.FullPath);

        Length = changeObserver.Select(x => x.Length);
        LastWriteTime = changeObserver.Select(x => x.LastWriteTime);
        _watcher.EnableRaisingEvents = true;
    }

    public IObservable<string> FullPath { get; }
    public IObservable<string> Name { get; }
    public IObservable<long> Length { get; }
    public IObservable<DateTime> LastWriteTime { get; }

    public void Dispose()
    {
        _watcher.EnableRaisingEvents = false;
        _fileNames.Dispose();
        _watcher.Dispose();
    }
}