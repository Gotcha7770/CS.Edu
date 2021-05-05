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

            Assert.AreEqual(predicate1, predicate2);
        }

        [Test]
        public void TwoIdentityFunc_NotEquals()
        {
            var func1 = new Func<int, int>(x => x);
            var func2 = new Func<int, int>(x => x);

            Assert.AreNotEqual(func1, func2);
        }

        [Test]
        public void TwoIdentityFunc_Equals()
        {
            var func1 = Functions.Identity<int>();
            var func2 = Functions.Identity<int>();

            Assert.AreEqual(func1, func2);
        }

        [Test]
        public void TwoEmptyActions_NotEquals()
        {
            var act1 = new Action<int>(x => {});
            var act2 = new Action<int>(x => {});

            Assert.AreNotEqual(act1, act2);
        }

        [Test]
        public void TwoEmptyActions_Equals()
        {
            var act1 = Actions.Empty<int>();
            var act2 = Actions.Empty<int>();

            Assert.AreEqual(act1, act2);
        }
    }
}