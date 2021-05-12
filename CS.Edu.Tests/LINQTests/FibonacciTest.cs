using CS.Edu.Core.MathExt;
using NUnit.Framework;
using System.Linq;
using System.Reactive.Linq;
using CS.Edu.Core.Extensions;

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
            int tenth3 = Enumerables.Generate((X: 0, Y: 1), t => (t.Y, t.X + t.Y)).ElementAt(9).X;
            int tenth4 = Fibonacci.Observable(10).Last();

            //Expand
            //Zip
            //Concat -> func(obj, IEnumerable<obj>) =>
            //Publish???
            //Observable

            Assert.That(tenth1, Is.EqualTo(34));
            Assert.That(tenth2, Is.EqualTo(34));
            Assert.That(tenth3, Is.EqualTo(34));
            Assert.That(tenth4, Is.EqualTo(34));
        }
    }
}