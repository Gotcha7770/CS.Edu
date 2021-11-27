using System;
using System.Linq.Expressions;
using CS.Edu.Tests.Utils;
using NUnit.Framework;

namespace CS.Edu.Tests.LINQTests
{
    [TestFixture]
    public class ExpressionsTests
    {
        [Test]
        public void BinaryOperator_ConstantParameters()
        {
            // Expression<Func<int>> sum = () => 1 + 2;
            // Expression sum = () => 1 + 2; <- C# 10

            var lambda = Expression.Lambda<Func<int>>(
                Expression.Add(
                    Expression.Constant(1),
                    Expression.Constant(2)
                )
            );
            var function = lambda.Compile();

            Assert.AreEqual(3, function());
        }

        [Test]
        public void BinaryOperator_VariableParameters()
        {
            //Expression<Func<int, int, int>> sum = (x, y) => x + y;

            var pX = Expression.Parameter(typeof(int), "x");
            var pY = Expression.Parameter(typeof(int), "y");
            var lambda = Expression.Lambda<Func<int, int, int>>(Expression.Add(pX, pY), pX, pY);

            var function = lambda.Compile();

            Assert.AreEqual(3, function(1, 2));
        }

        [Test]
        public void CombinePredicateExpressions()
        {
            //Expression<Func<int, bool>> and = x => x > 0 && x < 10;
            //Expression<Func<int, bool>> and = x => x is > 0 and < 10; pattern matching in expressions?

            var parameter = Expression.Parameter(typeof(int), "x");

            var lambda = Expression.Lambda<Func<int, bool>>(
                Expression.And(
                    Expression.GreaterThan(parameter, Expression.Constant(0)),
                    Expression.LessThan(parameter, Expression.Constant(10))),
                parameter);

            var function = lambda.Compile();

            Assert.IsFalse(function(-1));
            Assert.IsFalse(function(0));
            Assert.IsTrue(function(1));
            Assert.IsFalse(function(10));
        }

        [Test]
        public void PropertyAccessorExpressions()
        {
            // Expression<Func<Valuable<int>, bool>> e2 = x => x.Value > 0;

             var parameter = Expression.Parameter(typeof(Valuable<int>), "x");
             var lambda = Expression.Lambda<Func<Valuable<int>, bool>>(
                 Expression.GreaterThan(
                     Expression.Property(parameter, nameof(Valuable<int>.Value)),
                     Expression.Constant(0)),
                 parameter);

             var function = lambda.Compile();

             Assert.IsFalse(function(new Valuable<int>(-1)));
             Assert.IsFalse(function(new Valuable<int>(0)));
             Assert.IsTrue(function(new Valuable<int>(1)));
        }
    }
}