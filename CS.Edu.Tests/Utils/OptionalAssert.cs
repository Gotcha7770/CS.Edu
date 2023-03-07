using DynamicData.Kernel;
using NUnit.Framework;

namespace CS.Edu.Tests.Utils;

public class OptionalAssert
{
    public static void None<T>(Optional<T> optional)
    {
        Assert.IsFalse(optional.HasValue);
    }

    public static void Some<T>(Optional<T> optional)
    {
        Assert.IsTrue(optional.HasValue);
    }

    public static void Some<T>(Optional<T> optional, T expected)
    {
        Assert.IsTrue(optional.HasValue);
        Assert.AreEqual(expected, optional.Value);
    }
}