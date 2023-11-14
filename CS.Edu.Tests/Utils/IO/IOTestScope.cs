using System;
using System.IO;
using System.IO.Abstractions;

namespace CS.Edu.Tests.Utils.IO;

public class IOTestScope : IDisposable
{
    private readonly IFileSystem _fileSystem;

    public IOTestScope(IFileSystem fileSystem, string directory)
    {
        _fileSystem = fileSystem;
        var tmpFolder = _fileSystem.Path.GetTempPath();
        var path = _fileSystem.Path.Combine(tmpFolder, directory);
        if (_fileSystem.Directory.Exists(path))
            _fileSystem.Directory.Delete(path, true);

        _fileSystem.Directory.CreateDirectory(path);
        _fileSystem.Directory.SetCurrentDirectory(path);
        Directory = _fileSystem.DirectoryInfo.New(path);
        Watcher = _fileSystem.FileSystemWatcher.New(Directory.FullName);
    }

    public IDirectoryInfo Directory { get; }
    public IFileSystemWatcher Watcher { get; }

    public Stream CreateFile(string file)
    {
        DeleteFile(file);

        return _fileSystem.File.Create(file);
    }

    public void MoveFile(string oldName, string newName)
    {
        _fileSystem.File.Move(oldName, newName);
    }

    public void DeleteFile(string file)
    {
        if (_fileSystem.File.Exists(file))
            _fileSystem.File.Delete(file);
    }

    public void CreateDirectory(string directory)
    {
        if (_fileSystem.Directory.Exists(directory))
            _fileSystem.Directory.Delete(directory, true);

        _fileSystem.Directory.CreateDirectory(directory);
    }

    public void Dispose()
    {
        Watcher.EnableRaisingEvents = false;
        Watcher.Dispose();

        _fileSystem.Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
        _fileSystem.Directory.Delete(Directory.FullName, true);
    }
}