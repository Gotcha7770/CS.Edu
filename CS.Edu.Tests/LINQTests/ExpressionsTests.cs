using System;
using System.Linq.Expressions;
using CS.Edu.Core.Extensions;
using CS.Edu.Tests.Utils.Models;
using FluentAssertions;
using Xunit;

namespace CS.Edu.Tests.LINQTests;

public class ExpressionsTests
{
    [Fact]
    public void BinaryOperator_ConstantParameters()
    {
        // Expression<Func<int>> sum = () => 1 + 2;
        // Expression sum = () => 1 + 2; <- C# 10

        var lambda = Expression.Lambda<Func<int>>(
            Expression.Add(
                Expression.Constant(1),
                Expression.Constant(2)));
        var function = lambda.Compile();

        function()
            .Should()
            .Be(3);
    }

    [Theory]
    [InlineData(1, 2)]
    public void BinaryOperator_VariableParameters(int x, int y)
    {
        //Expression<Func<int>> sum = () => x + y;

        var lambda = Expression.Lambda<Func<int>>(
            Expression.Add(
                Expression.Constant(x),
                Expression.Constant(y)));
        var function = lambda.Compile();

        function()
            .Should()
            .Be(3);
    }

    [Fact]
    public void BinaryOperator_ArgumentParameters()
    {
        //Expression<Func<int, int, int>> sum = (x, y) => x + y;

        var pX = Expression.Parameter(typeof(int), "x");
        var pY = Expression.Parameter(typeof(int), "y");
        var lambda = Expression.Lambda<Func<int, int, int>>(Expression.Add(pX, pY), pX, pY);

        var function = lambda.Compile();

        function(1, 2)
            .Should()
            .Be(3);
    }

    [Fact]
    public void CombinePredicateExpression()
    {
        //Expression<Func<int, bool>> and = x => x > 0 && x < 10;
        //Expression<Func<int, bool>> and = x => x is > 0 and < 10; pattern matching in expressions?

        var parameter = Expression.Parameter(typeof(int), "x");

        var lambda = Expression.Lambda<Func<int, bool>>(
            Expression.AndAlso(
                Expression.GreaterThan(parameter, Expression.Constant(0)),
                Expression.LessThan(parameter, Expression.Constant(10))), parameter);

        var function = lambda.Compile();

        function(-1)
            .Should()
            .BeFalse();
        function(0)
            .Should()
            .BeFalse();
        function(1)
            .Should()
            .BeTrue();
        function(10)
            .Should()
            .BeFalse();
    }

    [Fact]
    public void IsOfTypeExpression()
    {
        // Func<object, bool> function = x => x.GetType() == typeof(Identity<int>) && ((Identity<int>)x).Key == 15;
        //Expression<Func<object, bool>> ofType = x => x.GetType() == typeof(Identity<int>) && ((Identity<int>)x).Key == 15;

        Expression<Func<Identity<int>, bool>> expression = x => x.Key == 15;

        var parameter = Expression.Parameter(typeof(object), "x");

        var lambda = Expression.Lambda<Func<object, bool>>(
            Expression.AndAlso(
                Expression.TypeIs(parameter, typeof(Identity<int>)),
                Expression.Equal(
                    Expression.Property(
                        Expression.Convert(parameter, typeof(Identity<int>)),
                        nameof(Identity<int>.Key)),
                    Expression.Constant(15))), parameter);

        var function = lambda.Compile();
        function(new object())
            .Should()
            .BeFalse();
        function(new Identity<int>(42))
            .Should()
            .BeFalse();
        function(new Identity<int>(15))
            .Should()
            .BeTrue();
    }

    [Fact]
    public void ReplaceLambda()
    {
        Func<Item, bool> func = x => x.Key is > 15 and < 35;
        Expression<Func<Item, bool>> expression1 = x => func(x);
        Expression<Func<object, bool>> expression2 = x => x.GetType() == typeof(Item) && func((Item)x);
    }

    [Fact]
    public void PropertyAccessorExpression()
    {
        // Expression<Func<Valuable<int>, bool>> e2 = x => x.Value > 0;

        var parameter = Expression.Parameter(typeof(Valuable<int>), "x");
        var lambda = Expression.Lambda<Func<Valuable<int>, bool>>(
            Expression.GreaterThan(
                Expression.Property(parameter, nameof(Valuable<int>.Value)),
                Expression.Constant(0)),
            parameter);

        var function = lambda.Compile();

        function(new Valuable<int>(-1))
            .Should()
            .BeFalse();
        function(new Valuable<int>(0))
            .Should()
            .BeFalse();
        function(new Valuable<int>(1))
            .Should()
            .BeTrue();
    }

    [Fact]
    public void PropertyAccessor_Multiple_Expression()
    {
        // Expression<Func<Range, int>> e = x => x.Start.Value;

        var parameter = Expression.Parameter(typeof(Range), "x");
        var lambda = Expression.Lambda<Func<Range, int>>(
            Expression.Property(
                Expression.Property(parameter, nameof(Range.Start)),
                nameof(Index.Value)),
            parameter);

        var function = lambda.Compile();
        function(new Range(new Index(2), new Index(5))).Should().Be(2);
    }

    [Fact]
    public void Coalesce()
    {
        //Expression<Func<int?, int>> e = x => x ?? 0;

        var parameter = Expression.Parameter(typeof(int?), "x");
        var lambda = Expression.Lambda<Func<int?, int>>(
            Expression.Coalesce(parameter, Expression.Constant(0)),
            parameter);

        var function = lambda.Compile();

        function(null).Should().Be(0);
        function(42).Should().Be(42);
    }

    [Theory]
    [InlineData(2, false)]
    [InlineData(3, true)]
    public void NotExpression(int value, bool expected)
    {
        Expression<Func<int, bool>> expr = x => x % 2 == 0;
        Func<int, bool> predicate = Expressions.Not(expr)
            .Compile();

        predicate(value)
            .Should()
            .Be(expected);
    }

    [Theory]
    [InlineData(2, false)]
    [InlineData(3, false)]
    [InlineData(6, true)]
    [InlineData(10, false)]
    [InlineData(12, true)]
    public void AndExpressions(int value, bool expected)
    {
        Expression<Func<int, bool>> expr1 = x => x % 3 == 0;
        Expression<Func<int, bool>> expr2 = x => x % 2 == 0;
        Func<int, bool> predicate = Expressions.And(expr1, expr2)
            .Compile();

        predicate(value)
            .Should()
            .Be(expected);
    }

    [Theory]
    [InlineData(2, true)]
    [InlineData(3, true)]
    [InlineData(4, true)]
    [InlineData(5, false)]
    [InlineData(6, true)]
    public void OrExpressions(int value, bool expected)
    {
        Expression<Func<int, bool>> expr1 = x => x % 2 == 0;
        Expression<Func<int, bool>> expr2 = x => x % 3 == 0;
        Func<int, bool> predicate = Expressions.Or(expr1, expr2)
            .Compile();

        predicate(value)
            .Should()
            .Be(expected);
    }

    [Fact]
    public void ReplaceReturnType()
    {
        // Expression<Func<Valuable<int>, object>> e2 = x => (object)x.Value;

        var parameter = Expression.Parameter(typeof(Valuable<int>), "x");
        var expression1 = Expression.Lambda<Func<Valuable<int>, int>>(
            Expression.Property(parameter, nameof(Valuable<int>.Value)), parameter);

        Func<Valuable<int>, int> func1 = expression1.Compile();
        Func<Valuable<int>, object> func2 = expression1.Convert<Valuable<int>, int, object>().Compile();
    }
}