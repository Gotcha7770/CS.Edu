using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS.Edu.Tests.LINQTests
{
    [TestFixture]
    public class AggregateBoolTest
    {
        [Test]
        public void EnumerableOfBoolsTest()
        {
            var func = new Func<IEnumerable<bool>, bool?>((source) =>
            {
                return source.Aggregate<bool, bool?>(source.First(), (acc, cur) =>
                {
                    if (acc.HasValue && acc.Value == cur)
                        return cur;

                    return null;
                });
            });

            var items = new bool[] { true };
            Assert.That(func(items), Is.True);

            items = new bool[] { false };
            Assert.That(func(items), Is.False);

            items = new bool[] { true, false, true };
            Assert.That(func(items), Is.Null);

            items = new bool[] { false, false, false };
            Assert.That(func(items), Is.False);

            items = new bool[] { true, true, true };
            Assert.That(func(items), Is.True);
        }

        [Test]
        public void EnumerableOfNullableBoolsTest()
        {
            var func = new Func<IEnumerable<bool?>, bool?>((source) =>
            {
                return source.Aggregate((acc, cur) =>
                {
                    return acc.HasValue && acc.Value == cur ? cur : null;
                });
            });

            var items = new bool?[] { true };
            Assert.That(func(items), Is.True);

            items = new bool?[] { false };
            Assert.That(func(items), Is.False);

            items = new bool?[] { true, false, true };
            Assert.That(func(items), Is.Null);

            items = new bool?[] { false, false, false };
            Assert.That(func(items), Is.False);

            items = new bool?[] { true, true, true };
            Assert.That(func(items), Is.True);

            items = new bool?[] { true, true, null };
            Assert.That(func(items), Is.Null);
        }
    }
}
