using System.Collections.Generic;
using System.Linq;
using CS.Edu.Core;
using CS.Edu.Core.Extensions;
using NUnit.Framework;

namespace CS.Edu.Tests.LINQTests
{
    [TestFixture]
    public class SelectTests
    {
        [Test]
        public void SelectVsSelectMany()
        {
            IEnumerable<int> items = Enumerable.Range(0, 100)
                .Paginate(25)
                .Select((x, i) => i.IsEven() ? x : x.Reverse())
                .SelectMany(x => x)
                .ToArray();

            IEnumerable<int> items2 = Enumerable.Range(0, 100)
                .Paginate(25)
                .SelectMany((x, i) => i.IsEven() ? x : x.Reverse())
                .ToArray();

            Assert.That(items, Is.EqualTo(items2));
        }
    }
}