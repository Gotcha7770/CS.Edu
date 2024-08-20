using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Text.RegularExpressions;
using DynamicData;

namespace CS.Edu.Core.IO;

public static class FileSystemExtensions
{
    public static int GetNextDirectoryIndex(this IFileSystem fileSystem, string directory)
    {
        //var tmp = fileSystem.Path.DirectorySeparatorChar;
        //var regex = new Regex(@"(\\|/)Новая папка( \(\d+\))?$");
        var regex = new Regex(@"^Новая папка( \(\d+\))?$");
        return fileSystem.DirectoryInfo.New(directory)
            .EnumerateDirectories()
            .Count(x => regex.IsMatch(x.Name));
    }

    public static IObservableFile ToObservable(this IFileInfo file)
    {
        return new ObservableFile(file);
    }

    public static IObservableDirectory ToObservable(this IDirectoryInfo directory)
    {
        return new ObservableDirectory(directory);
    }

    internal static ChangeReason ToChangeReason(this WatcherChangeTypes changeType)
    {
        return changeType switch
        {
            WatcherChangeTypes.Created => ChangeReason.Add,
            WatcherChangeTypes.Changed => ChangeReason.Update,
            //WatcherChangeTypes.Renamed => ChangeReason.Moved,
            WatcherChangeTypes.Deleted => ChangeReason.Remove,
            _ => throw new ArgumentOutOfRangeException(nameof(changeType), changeType, null)
        };
    }

    internal static Change<string, string> ToChange(this FileSystemEventArgs eventArgs)
    {
        return new Change<string, string>(
            eventArgs.ChangeType.ToChangeReason(),
            eventArgs.FullPath,
            eventArgs.Name);
    }

    internal static ChangeSet<string, string> ToChangeSet(this FileSystemEventArgs eventArgs)
    {
        return [eventArgs.ToChange()];
    }

    internal static ChangeSet<TValue, TKey> ToChangeSet<TValue, TKey>(this IEnumerable<Change<TValue, TKey>> source)
    {
        return new ChangeSet<TValue, TKey>(source);
    }
}