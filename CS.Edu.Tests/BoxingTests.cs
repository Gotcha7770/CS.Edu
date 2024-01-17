using Xunit;

namespace CS.Edu.Tests;

public class BoxingTests
{
    [Fact]
    public void Test()
    {
        int value = 6;
        // object method call cause boxing
        // IL_0004: box
        var type = value.GetType();

        // it`s like
        var obj = (object)value;
        type = obj.GetType();
    }
}