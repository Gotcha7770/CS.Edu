using NUnit.Framework;
using System.Collections.Generic;
using CS.Edu.Core.Extensions;
using CS.Edu.Core;
using System.Linq;

namespace CS.Edu.Tests.Extensions
{
    [TestFixture]
    public class CollectionExtTests
    {
        [Test]
        public void InvalidateCollection()
        {
            var source = new List<(int, Range<int>)>
            {
                (0, new Range<int>(0, 10)),
                (1, new Range<int>(10, 20)),
                (2, new Range<int>(20, 30)),
                (3, new Range<int>(30, 40)),
                (4, new Range<int>(40, 50))
            };

            var patch = new[]
            {
                (5, new Range<int>(0, 12)),
                (6, new Range<int>(12, 22)),
                (7, new Range<int>(22, 30)),
                (8, new Range<int>(30, 40))
            };

            var standard = new[]
            {
                (source[0].Item1, new Range<int>(0, 12)),
                patch[1],
                patch[2],
                source[3]
            };

            Merge<(int, Range<int>)> mergeFunc = (x, y) =>
            {
                x.Item2 = y.Item2;
                return x;
            };

            source.Invalidate(patch, mergeFunc, x => x.Item2.Min);
            var result = source.OrderBy(x => x.Item2.Min).ToArray();

            Assert.That(result, Is.EqualTo(standard));
        }
    }
}