using System.Linq;
using Xunit;

namespace CS.Edu.Tests;

public class InversionOfControl
{
    [Fact]
    public void InversionOfControl_Loop()
    {
        foreach (var a in Enumerable.Range(1, 10))
        {
            foreach (var b in Enumerable.Range(1, 10))
            {
                // func(a, b)
            }
        }
    }
}