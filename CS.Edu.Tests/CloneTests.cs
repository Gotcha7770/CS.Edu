using NUnit.Framework;
using System;
using System.Linq;
using CS.Edu.Core.Extensions;
using CS.Edu.Core;
using System.Collections.Generic;

namespace CS.Edu.Tests
{
    public class IdInfo
    {
        public int IdNumber;

        public IdInfo(int IdNumber)
        {
            this.IdNumber = IdNumber;
        }

        public override bool Equals(object obj)
        {
            return obj is IdInfo idInfo && (ReferenceEquals(this, idInfo) || idInfo.IdNumber == IdNumber);
        }
    }

    public class Person
    {
        public int Age;
        public string Name;
        public IdInfo IdInfo;

        public Person ShallowCopy()
        {
            return (Person)this.MemberwiseClone();
        }

        public Person DeepCopy()
        {
            Person other = (Person)this.MemberwiseClone();
            other.IdInfo = new IdInfo(IdInfo.IdNumber);
            other.Name = String.Copy(Name);
            return other;
        }

        public bool IsEqual(Person other)
        {
            return Age == other.Age
                && Name == other.Name
                && IdInfo.Equals(other.IdInfo);
        }

        public override bool Equals(object obj)
        {
            return obj is Person other && (ReferenceEquals(this, other) || IsEqual(other));
        }
    }

    [TestFixture]
    public class CloneTests
    {
        [Test]
        public void ShallowCopy()
        {
            Person p1 = new Person
            {
                Age = 42,
                Name = "Sam",
                IdInfo = new IdInfo(6565)
            };

            Person p2 = p1.ShallowCopy();
            
            Assert.False(ReferenceEquals(p1, p2));
            Assert.That(p1, Is.EqualTo(p2));
            Assert.True(ReferenceEquals(p1.IdInfo, p2.IdInfo));
        }

        [Test]
        public void DeepCopy()
        {
            Person p1 = new Person
            {
                Age = 42,
                Name = "Sam",
                IdInfo = new IdInfo(6565)
            };

            Person p2 = p1.DeepCopy();
            
            Assert.False(ReferenceEquals(p1, p2));
            Assert.That(p1, Is.EqualTo(p2));
            Assert.False(ReferenceEquals(p1.IdInfo, p2.IdInfo));
        }
    }
}