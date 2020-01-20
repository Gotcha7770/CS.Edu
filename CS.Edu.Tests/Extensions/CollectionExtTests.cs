using NUnit.Framework;
using System;
using System.Collections.Generic;
using CS.Edu.Core.Extensions;
using CS.Edu.Core;
using System.Linq;
using System.Diagnostics.CodeAnalysis;

namespace CS.Edu.Tests.Extensions
{
    class TestClass : IEquatable<TestClass>
    {
        public TestClass(Range<int> range)
            : this(Guid.NewGuid(), range)
        { }

        public TestClass(Guid id, Range<int> range)
        {
            Id = id;
            Range = range;
        }

        public Guid Id { get; set; }
        public Range<int> Range { get; set; }

        public override bool Equals(object obj)
        {
            return Equals(obj as TestClass);
        }

        public bool Equals([AllowNull] TestClass other)
        {
            return other != null && ReferenceEquals(this, other) || IsEqual(other);
        }

        private bool IsEqual(TestClass other)
        {
            return Id == other.Id
                && Range == other.Range;
        }
    }

    class TestComparer : IEqualityComparer<TestClass>
    {
        public bool Equals(TestClass one, TestClass other)
        {
            return GetHashCode(one) == GetHashCode(other);
        }

        public int GetHashCode(TestClass obj)
        {
            return obj.Range.Min;
        }
    }

    [TestFixture]
    public class CollectionExtTests
    {
        [Test]
        public void InvalidateCollection()
        {
            var source = new List<TestClass>()
            {
                new TestClass(new Range<int>(0, 10)),
                new TestClass(new Range<int>(10, 20)),
                new TestClass(new Range<int>(20, 30)),
                new TestClass(new Range<int>(30, 40)),
                new TestClass(new Range<int>(40, 50))
            };

            var newData = new []
            {
                new TestClass(new Range<int>(0, 12)),
                new TestClass(new Range<int>(12, 22)),
                new TestClass(new Range<int>(22, 30)),
                new TestClass(new Range<int>(30, 40))
            };

            var standard = new []
            {
                new TestClass(source[0].Id, new Range<int>(0, 12)),
                newData[1],
                newData[2],
                source[3]
            };

            Merge<TestClass> mergeFunc = (x, y) =>
            {
                x.Range = y.Range;
                return x;
            };

            source.Invalidate(newData, mergeFunc, new TestComparer());

            var result = source.OrderBy(x => x.Range.Min).ToArray();

            Assert.That(result, Is.EqualTo(standard));
        }

        [Test]
        public void AddOrUpdate_SourceIsNull()
        {
            List<TestClass> source = null;
            var newData = new TestClass(new Range<int>(0, 12));

            Assert.Throws<ArgumentNullException>(() => source.AddOrUpdate(newData, (x,y) => x));
        }

        [Test]
        public void AddOrUpdate_SourceIsArray()
        {
            TestClass[] source = Array.Empty<TestClass>();
            var newData = new TestClass(new Range<int>(0, 12));

            Assert.Throws<InvalidOperationException>(() => source.AddOrUpdate(newData, (x,y) => x));
        }

        [Test]
        public void AddOrUpdate_SourceContainsItem_ItemUpdated()
        {
            var targetId = Guid.NewGuid();
            var source = new List<TestClass>()
            {
                new TestClass(targetId, new Range<int>(0, 10)),
                new TestClass(new Range<int>(10, 20)),
                new TestClass(new Range<int>(20, 30))
            };

            var newData = new TestClass(new Range<int>(0, 12));

            Merge<TestClass> mergeFunc = (x, y) =>
            {
                x.Range = y.Range;
                return x;
            };

            source.AddOrUpdate(newData, mergeFunc, new TestComparer());

            Assert.That(source[0].Id, Is.EqualTo(targetId));
            Assert.That(source[0].Range, Is.EqualTo(newData.Range));
        }

        [Test]
        public void AddOrUpdate_SourceDoesNotContainsItem_ItemAdded()
        {
            var source = new List<TestClass>()
            {
                new TestClass(new Range<int>(0, 10)),
            };

            var newData = new TestClass(new Range<int>(1, 12));

            source.AddOrUpdate(newData, (x,y) => x, new TestComparer());

            Assert.That(source[1], Is.EqualTo(newData));
        }
    }
}