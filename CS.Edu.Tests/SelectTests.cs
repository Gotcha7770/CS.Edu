using System.Collections.Generic;
using System.Linq;
using CS.Edu.Core.Extensions;
using CS.Edu.Tests.Temptests;
using NUnit.Framework;

namespace CS.Edu.Tests
{
    [TestFixture]
    public class SelectTests
    {
        [Test]
        public void SelectVsSelectMany()
        {
            IEnumerable<Indexed> items = Enumerable.Range(0, 100)
                .Paginate(25)
                .Select((x, i) => i.IsEven() ? x : x.Reverse())
                .SelectMany(x => x)
                .Select((x, i) => new Indexed(i, x))
                .ToArray();

            IEnumerable<Indexed> items2 = Enumerable.Range(0, 100)
                .Paginate(25)
                .SelectMany((x, i) => i.IsEven() ? x : x.Reverse())
                .Select((x, i) => new Indexed(i, x))
                .ToArray();

            Assert.That(items, Is.EqualTo(items2));
        }
    }
}