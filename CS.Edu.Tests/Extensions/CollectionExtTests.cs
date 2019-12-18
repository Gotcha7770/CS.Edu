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
            return one.Range.Minimum == other.Range.Minimum;
        }

        public int GetHashCode(TestClass obj)
        {
            return obj.Range.Minimum;
        }
    }

    [TestFixture]
    public class CollectionExtTests
    {
        [Test]
        public void InvalidateCollection()
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
    }
}