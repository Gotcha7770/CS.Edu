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

            var standard = new[]
            {
                new TestClass(source[0].Id, new Range<int>(0, 12)),
                new TestClass(newData[1].Id, new Range<int>(12, 22)),
                new TestClass(newData[2].Id, new Range<int>(22, 30)),
                new TestClass(source[3].Id, new Range<int>(30, 40)),
                new TestClass(source[4].Id, new Range<int>(40, 50)),
            };

            foreach (TestClass item in newData)
            {
                Merge<TestClass> mergeFunc = (x, y) =>
                {
                    x.Range = y.Range;
                    return x;
                };

                source.AddOrUpdate(item, mergeFunc, new TestComparer());
            }

            var result = source.OrderBy(x => x.Range.Minimum).ToArray();

            Assert.That(result, Is.EqualTo(standard));
        }
    }
}