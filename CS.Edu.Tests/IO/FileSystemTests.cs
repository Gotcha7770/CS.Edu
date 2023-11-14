using System;
using System.IO;
using System.Linq;
using Xunit;

namespace CS.Edu.Tests.IO;

public class FileSystemTests
{
    [Fact]
    public void GetDrives()
    {
        var drives1 = DriveInfo.GetDrives();
        var drives2 = Directory.GetLogicalDrives();
    }

    [Fact]
    public void GetSystemDriveEntries()
    {
        var cDrive = DriveInfo.GetDrives().First();
        var directory = cDrive.RootDirectory;
        var entries = Directory.EnumerateFileSystemEntries(directory.FullName);

        var path = Path.GetPathRoot(Environment.SystemDirectory);
        entries = Directory.EnumerateFileSystemEntries(path);
    }
}