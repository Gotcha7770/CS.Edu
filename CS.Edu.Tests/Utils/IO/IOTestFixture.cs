using System.IO.Abstractions;

namespace CS.Edu.Tests.Utils.IO;

public class IOTestFixture
{
    public IFileSystem FileSystem { get; } = new FileSystem();

    public IOTestScope CreateTestScope(string directory)
    {
        return new IOTestScope(FileSystem, directory);
    }
}