using System;
using System.Linq;
using CS.Edu.Core.MathExt;
using FluentAssertions;
using Xunit;

namespace CS.Edu.Tests.MathExtTests;

public class MatrixTests
{
    private readonly double[][] _matrix = Enumerable.Range(0, 4)
        .Select(c => Enumerable.Range(0, 1000).Select(r => r * Math.Pow(10, c)).ToArray())
        .ToArray();

    [Fact]
    public void ConvertTo2D_Jagged_returns2D()
    {
        double[,] result = _matrix.To2D();

        result.Rank.Should().Be(2);
        result.GetLength(0).Should().Be(4);
        result.GetLength(1).Should().Be(1000);
    }
}