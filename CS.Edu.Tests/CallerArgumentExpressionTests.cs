using System;
using System.Runtime.CompilerServices;
using CS.Edu.Tests.Utils;
using NUnit.Framework;

namespace CS.Edu.Tests;

[TestFixture]
public class CallerArgumentExpressionTests
{
    public static void Check(bool condition, [CallerArgumentExpression("condition")] string message = default)
    {
        if (!condition)
            throw new Exception($"Condition failed: {message}");
    }

    [Test]
    public void METHOD()
    {
        //var item = new Identity<int>(3);
        Identity<int> identity = null;
        Assert.Throws<Exception>(() => Check(identity is { Key: 3 }));
    }
}