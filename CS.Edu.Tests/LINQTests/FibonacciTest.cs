using CS.Edu.Core.MathExt;
using NUnit.Framework;
using System.Linq;

namespace CS.Edu.Tests.LINQTests
{
    [TestFixture]
    public class FibonacciTest
    {
        [Test]
        public void Test()
        {
            int tenth1 = Fibonacci.Recursive(10);
            int tenth2 = Fibonacci.Iterator().ElementAt(9);
            int tenth3 = Enumerable.Range(0, int.MaxValue)
                                   .Scan((X: 0, Y: 1), (acc, curr) => (acc.Y, acc.X + acc.Y))
                                   .ElementAt(9).X;

            //Expand
            //Zip
            //Concat -> func(obj, IEnumerable<obj>) =>
            //Publish???
            //Create
            //Defer
            //Observable

            Assert.That(tenth1, Is.EqualTo(55));
            Assert.That(tenth2, Is.EqualTo(55));
            Assert.That(tenth3, Is.EqualTo(55));
        }
    }
}