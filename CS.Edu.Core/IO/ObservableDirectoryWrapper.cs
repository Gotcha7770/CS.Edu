using System;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Reactive.Linq;
using DynamicData;

namespace CS.Edu.Core.IO;

internal class ObservableDirectoryWrapper : ObservableEntryWrapper, IObservableDirectory
{
    public ObservableDirectoryWrapper(IDirectoryInfo directoryInfo)
        : base(directoryInfo, directoryInfo.FullName)
    {
        var changeObserver = Observable.FromEventPattern<FileSystemEventHandler, FileSystemEventArgs>(
                x => Watcher.Changed += x,
                x => Watcher.Changed -= x)
            .Where(x => x.EventArgs.FullPath == EntryFullPath)
            .Select(x => FileSystem.DirectoryInfo.New(x.EventArgs.FullPath));

        LastWriteTime = changeObserver.Select(x => x.LastWriteTime).StartWith(directoryInfo.LastWriteTime);
        Watcher.EnableRaisingEvents = true;
    }

    public IDisposable Subscribe(IObserver<IChangeSet<string, string>> observer)
    {
        var changeSet = FileSystem.DirectoryInfo.New(EntryFullPath)
            .EnumerateFileSystemInfos()
            .Select(x => new Change<string, string>(ChangeReason.Add, x.FullName, x.Name))
            .ToChangeSet();

        if(changeSet.Count > 0)
            observer.OnNext(changeSet);

        var created = Observable.FromEventPattern<FileSystemEventHandler, FileSystemEventArgs>(
            x => Watcher.Created += x,
            x => Watcher.Created -= x);
        var deleted = Observable.FromEventPattern<FileSystemEventHandler, FileSystemEventArgs>(
            x => Watcher.Deleted += x,
            x => Watcher.Deleted -= x);

        return created.Merge(deleted)
            .Where(x => x.EventArgs.FullPath != EntryFullPath)
            .Select(x => x.EventArgs.ToChangeSet())
            .Subscribe(observer);
    }
}