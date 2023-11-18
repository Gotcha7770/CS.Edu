using System.IO.Abstractions;

namespace CS.Edu.Tests.Utils.IO;

public class IOTestFixture
{
    public IOTestScope CreateTestScope(string directory, IFileSystem fileSystem)
    {
        return new IOTestScope(directory, fileSystem);
    }
}