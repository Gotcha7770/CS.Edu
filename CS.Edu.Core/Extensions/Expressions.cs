using System;
using System.Linq;
using System.Linq.Expressions;

namespace CS.Edu.Core.Extensions;

public static class Expressions
{
    public static Expression<Func<T, TConverted>> Convert<T, TResult, TConverted>(this Expression<Func<T, TResult>> expr)
    {
        return Expression.Lambda<Func<T, TConverted>>(
            Expression.Convert(expr, typeof(TConverted)), expr.Parameters);
    }

    public static Expression<Func<T, bool>> Not<T>(Expression<Func<T, bool>> expr)
    {
        return Expression.Lambda<Func<T, bool>>(Expression.Not(expr.Body), expr.Parameters.Single());
    }

    public static Expression<Func<T,bool>> And<T>(Expression<Func<T,bool>> left, Expression<Func<T, bool>> right)
    {
        var parameter = Expression.Parameter(typeof(T));

        return Expression.Lambda<Func<T, bool>>(
            Expression.AndAlso(
                ReplaceParameter(left, parameter),
                ReplaceParameter(right, parameter)),
            parameter);
    }

    public static Expression<Func<T, bool>> Or<T>(Expression<Func<T, bool>> left, Expression<Func<T, bool>> right)
    {
        var parameter = Expression.Parameter(typeof(T));

        return Expression.Lambda<Func<T, bool>>(
            Expression.OrElse(
                ReplaceParameter(left, parameter),
                ReplaceParameter(right, parameter)),
            parameter);
    }

    private static Expression ReplaceParameter<T>(Expression<Func<T, bool>> source, ParameterExpression parameter)
    {
        var visitor = new ReplaceExpressionVisitor(source.Parameters[0], parameter);
        return visitor.Visit(source.Body);
    }

    private class ReplaceExpressionVisitor : ExpressionVisitor
    {
        private readonly ParameterExpression _oldValue;
        private readonly ParameterExpression _newValue;

        public ReplaceExpressionVisitor(ParameterExpression oldValue, ParameterExpression newValue)
        {
            _oldValue = oldValue;
            _newValue = newValue;
        }

        public override Expression Visit(Expression node)
        {
            return node == _oldValue ? _newValue : base.Visit(node);
        }
    }
}