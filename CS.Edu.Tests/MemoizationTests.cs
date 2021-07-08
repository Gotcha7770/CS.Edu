using CS.Edu.Core.Extensions;
using NUnit.Framework;

namespace CS.Edu.Tests
{
    [TestFixture]
    public class MemoizationTests
    {
        class TestClass
        {
            public int Counter { get; private set; }

            public int Add(int one, int other)
            {
                Counter++;
                return one + other;
            }
        }

        [Test]
        public void OneInvocation()
        {
            var test = new TestClass();
            var memoized = Functions.Memoize<int, int, int>((x,y) => test.Add(x, y));
            //var memoized = Functions.Memoize<(int, int), int>(x => test.Add(x.Item1, x.Item2));
            //var memoized = Functions.Memoize<(int, int), int>(((int a, int b) x) => test.Add(x.a, x.b));
            //var memoized = Functions.Memoize<int, Func<int, int>>(x => y => test.Add(x, y));
            var result = memoized(2, 3);

            Assert.AreEqual(5, result);
            Assert.AreEqual(1, test.Counter);
        }

        [Test]
        public void TwoInvocations_SameValue()
        {
            var test = new TestClass();
            var memoized = Functions.Memoize<int, int, int>(test.Add);
            var first = memoized(2, 3);
            var second = memoized(2, 3);

            Assert.AreEqual(5, first);
            Assert.AreEqual(5, second);
            Assert.AreEqual(1, test.Counter);
        }

        [Test]
        public void TwoInvocations_DifferentValues()
        {
            var test = new TestClass();
            var memoized = Functions.Memoize<int, int, int>(test.Add);
            var first = memoized(3, 2); //тут вопрос как считать, параметры то одинаковые
            var second = memoized(2, 3);

            Assert.AreEqual(5, first);
            Assert.AreEqual(5, second);
            Assert.AreEqual(2, test.Counter);
        }
    }
}