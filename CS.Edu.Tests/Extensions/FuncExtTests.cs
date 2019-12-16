using NUnit.Framework;
using System;
using CS.Edu.Core.Extensions;

namespace CS.Edu.Tests.Extensions
{
    [TestFixture]
    public class FuncExtTests
    {
        [Test]
        public void ApplyPartial()
        {
            Func<int, int, int> func = (a, b) => a + b;
            var partial = func.ApplyPartial(12);
        }

        [Test]
        public void Curry()
        {
            Func<int, int, int, int[]> func = (a, b, c) => new[] { a, b, c };
            var curried = func.Curry();

            int[] result = curried(12)(13)(14);
        }
    }
}
