using System;
using System.IO;
using System.IO.Abstractions;
using System.Reactive.Linq;

namespace CS.Edu.Core.IO;

internal class ObservableFile : ObservableEntry, IObservableFile
{
    public ObservableFile(IFileInfo fileInfo)
        : base(fileInfo, fileInfo.DirectoryName)
    {
        var changeObserver = Observable.FromEventPattern<FileSystemEventHandler, FileSystemEventArgs>(
            x => Watcher.Changed += x,
            x => Watcher.Changed -= x)
            .Where(x => x.EventArgs.FullPath == EntryFullPath)
            .Select(x => FileSystem.FileInfo.New(x.EventArgs.FullPath));

        Length = changeObserver.Select(x => x.Length)
            .DistinctUntilChanged()
            .StartWith(fileInfo.Length);
        LastWriteTime = changeObserver.Select(x => x.LastWriteTime)
            .DistinctUntilChanged()
            .StartWith(fileInfo.LastWriteTime);
        Watcher.EnableRaisingEvents = true;
    }

    public IObservable<long> Length { get; }
}