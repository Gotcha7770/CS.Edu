using System;
using System.Collections.Generic;
using System.Linq;
using CS.Edu.Core.Extensions;
using NUnit.Framework;

namespace CS.Edu.Tests.Extensions
{
    [TestFixture]
    public class PredicatesTests
    {
        [TestCase(2, ExpectedResult = false)]
        [TestCase(3, ExpectedResult = false)]
        [TestCase(6, ExpectedResult = true)]
        [TestCase(10, ExpectedResult = false)]
        [TestCase(12, ExpectedResult = true)]
        public bool PredicatesAndTest(int value)
        {
            Predicate<int> predicate1 = x => x % 2 == 0;
            Predicate<int> predicate2 = x => x % 3 == 0;
            Predicate<int> predicate3 = predicate1
                .And(predicate2);

            return predicate3(value);
        }

        [TestCase(2, ExpectedResult = true)]
        [TestCase(3, ExpectedResult = true)]
        [TestCase(4, ExpectedResult = true)]
        [TestCase(5, ExpectedResult = false)]
        [TestCase(6, ExpectedResult = true)]
        public bool PredicatesOrTest(int value)
        {
            Predicate<int> predicate1 = x => x % 2 == 0;
            Predicate<int> predicate2 = x => x % 3 == 0;
            Predicate<int> predicate3 = predicate1
                .Or(predicate2);

            return predicate3(value);
        }
    }
}