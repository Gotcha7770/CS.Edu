using System.Diagnostics;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using NUnit.Framework;

namespace CS.Edu.Tests.ReactiveTests
{
    [TestFixture]
    public class LockTests
    {
        [Test]
        public void Test1()
        {
            var sequence = new Subject<int>();
            Debug.WriteLine("Next line should lock the system.");
            var value = sequence.First();
            sequence.OnNext(1);
            Debug.WriteLine("I can never execute...");
        }
    }
}