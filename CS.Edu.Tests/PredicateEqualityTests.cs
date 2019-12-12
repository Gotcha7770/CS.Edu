using System;
using CS.Edu.Core.Extensions;
using NUnit.Framework;

namespace CS.Edu.Tests
{
     [TestFixture]
    class PredicateEqualityTests
    {
        [Test]
        public void TwoPredicatesWithAnonymousMethods_NotEquals()
        {
            var predicate1 = new Predicate<long>(x => x.IsPrime());
            var predicate2 = new Predicate<long>(x => x.IsPrime());

            Assert.That(predicate1, Is.Not.EqualTo(predicate2));
        }

        [Test]
        public void TwoPredicatesWithMethods_Equals()
        {
            var predicate1 = new Predicate<long>(IntExt.IsPrime);
            var predicate2 = new Predicate<long>(IntExt.IsPrime);

            Assert.That(predicate1, Is.EqualTo(predicate2));
        }
    }
}