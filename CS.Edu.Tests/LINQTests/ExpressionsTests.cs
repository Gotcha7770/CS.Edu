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
        public void CombinePredicateExpression()
        {
            //Expression<Func<int, bool>> and = x => x > 0 && x < 10;
            //Expression<Func<int, bool>> and = x => x is > 0 and < 10; pattern matching in expressions?

            var parameter = Expression.Parameter(typeof(int), "x");

            var lambda = Expression.Lambda<Func<int, bool>>(
                Expression.AndAlso(
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
                            Expression.Convert(parameter, typeof(Identity<int>)), nameof(Identity<int>.Key)),
                        Expression.Constant(15))),
                parameter);

            var function = lambda.Compile();
            Assert.IsFalse(function(new object()));
            Assert.IsFalse(function(new Identity<int>(42)));
            Assert.IsTrue(function(new Identity<int>(15)));
        }

        [Test]
        public void ReplaceLambda()
        {
            Func<Identity<int>, bool> func = x => x.Key is > 15 and < 35;
            Expression<Func<Identity<int>, bool>> expression1 = x => func(x);
            Expression<Func<object, bool>> expression2 = x => x.GetType() == typeof(Identity<int>) && func((Identity<int>)x);
        }

        [Test]
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

             Assert.IsFalse(function(new Valuable<int>(-1)));
             Assert.IsFalse(function(new Valuable<int>(0)));
             Assert.IsTrue(function(new Valuable<int>(1)));
        }
    }
}