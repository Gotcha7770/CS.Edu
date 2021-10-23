using CS.Edu.Core.Monads;
using NUnit.Framework;

namespace CS.Edu.Tests.Utils
{
    public class EitherAssert
    {
        public static void Left<TL ,TR>(Either<TL, TR> either)
        {
            Assert.IsTrue(either.IsLeft);
        }

        public static void Left<TL ,TR>(Either<TL, TR> either, TL expected)
        {
            either.Match(l => Assert.AreEqual(expected, l), r => Assert.Fail("Either is not left"));
        }

        public static void Right<TL ,TR>(Either<TL, TR> either)
        {
            Assert.IsFalse(either.IsLeft);
        }

        public static void Right<TL ,TR>(Either<TL, TR> either, TR expected)
        {
            either.Match(l => Assert.Fail("Either is not right"), r => Assert.AreEqual(expected, r));
        }
    }
}