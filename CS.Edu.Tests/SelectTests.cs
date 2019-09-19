using System.Collections.Generic;
using System.Linq;
using CS.Edu.Core;
using CS.Edu.Core.Extensions;
using NUnit.Framework;

namespace CS.Edu.Tests
{
    [TestFixture]
    public class SelectTests
    {
        [Test]
        public void SelectVsSelectMany()
        {
            IEnumerable<Indexed<int>> items = Enumerable.Range(0, 100)
                .Paginate(25)
                .Select((x, i) => i.IsEven() ? x : x.Reverse())
                .SelectMany(x => x)
                .Select((x, i) => new Indexed<int>(i, x))
                .ToArray();

            IEnumerable<Indexed<int>> items2 = Enumerable.Range(0, 100)
                .Paginate(25)
                .SelectMany((x, i) => i.IsEven() ? x : x.Reverse())
                .Select((x, i) => new Indexed<int>(i, x))
                .ToArray();

            Assert.That(items, Is.EqualTo(items2));
        }
    }
}