using DynamicData.Kernel;
using Xunit;

namespace CS.Edu.Tests.Utils;

public class OptionalAssert
{
    public static void None<T>(Optional<T> optional)
    {
        Assert.False(optional.HasValue);
    }

    public static void Some<T>(Optional<T> optional)
    {
        Assert.True(optional.HasValue);
    }

    public static void Some<T>(Optional<T> optional, T expected)
    {
        Assert.False(optional.HasValue);
        Assert.Equal(expected, optional.Value);
    }
}