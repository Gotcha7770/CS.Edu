using System;
using System.Collections.ObjectModel;
using System.Linq;
using CS.Edu.Tests.Utils;
using DynamicData;
using DynamicData.Binding;
using NUnit.Framework;

namespace CS.Edu.Tests.ReactiveTests
{
    [TestFixture]
    public class SotrTests
    {
        private Orderable<int>[] _items = Enumerable.Range(1, 10)
            .Select(i => new Orderable<int>(i, i))
            .ToArray();

        [Test]
        public void ComparerTests()
        {
            var items = new[] {0, 5, 2, 7, 1, 3, 4, 9, 6, 8};
            var comparer = SortExpressionComparer<int>.Ascending(x => x);

            Array.Sort(items, comparer);

            CollectionAssert.IsOrdered(items);
        }

        [Test]
        public void SomeOperatorBetweenSortAndBind()
        {
            var comparer = SortExpressionComparer<Orderable<int>>.Ascending(x => x.Order);

            using (var cache = new SourceCache<Orderable<int>, Guid>(x => x.Key))
            using (var subscription = cache.Connect()
                .Sort(comparer)
                .Transform(x => new Valuable<int>(x.Key, x.Value))
                .Bind(out ReadOnlyObservableCollection<Valuable<int>> target)
                .Subscribe())
            {
                cache.AddOrUpdate(_items);

                _items[0].Order = 11;
                cache.Refresh(_items[0]);

                CollectionAssert.AreEqual(_items.OrderBy(x => x.Order), target.Select(x => x.Value));
            }
        }

        [Test]
        public void BindAfterSort()
        {
            var comparer = SortExpressionComparer<Orderable<int>>.Ascending(x => x.Order);

            using (var cache = new SourceCache<Orderable<int>, Guid>(x => x.Key))
            using (var subscription = cache.Connect()
                .Sort(comparer)
                .Bind(out ReadOnlyObservableCollection<Orderable<int>> target)
                .Subscribe())
            {
                cache.AddOrUpdate(_items);

                _items[0].Order = 11;
                cache.Refresh(_items[0]);

                CollectionAssert.AreEqual(_items.OrderBy(x => x.Order), target.Select(x => x.Value));
            }
        }
    }
}