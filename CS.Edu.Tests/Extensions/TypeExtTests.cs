using System;
using System.Collections.Generic;
using CS.Edu.Core.Extensions;
using NUnit.Framework;

namespace CS.Edu.Tests
{
    class TBase { }
    
    class FirstLevelType : TBase { }

    class TestType : FirstLevelType { }

    class BaseGeneric<T> where T : TBase { }

    class FirstLevelGeneric<T> : BaseGeneric<FirstLevelType> { }

    class TestClass : FirstLevelGeneric<TestType> { }

    [TestFixture]
    public class TypeExtTests
    {

        [Test]
        public void TestMethod()
        {
            Type openBaseType = typeof(BaseGeneric<>);
            Type baseType = typeof(BaseGeneric<TBase>);
            Type testTypeGeneric = typeof(BaseGeneric<TestType>);
            Type testType = typeof(TestClass);

            //object obj = new TestClass();

            Assert.IsTrue(testType.IsSubclassOf(testTypeGeneric));
            Assert.IsTrue(testType.IsSubclassOfExt(openBaseType));
        }

        [Test]
        public void GenericTypeConstructionTest_NotGenericType_ThrowsException()
        {
            Assert.Throws<InvalidOperationException>(() => new GenericType(typeof(int)));
        }

        [Test]
        public void GenericTypeConstructionTest()
        {
            var type = new GenericType(typeof(IEnumerable<int>));

            Assert.That(type.GenericTypeDefinition, Is.EqualTo(typeof(IEnumerable<>)));
            Assert.That(type.GenericParameterTypes, Is.EqualTo(new[] { typeof(int) }));
        }

        [Test]
        public void GenericTypeConstructionTest_2Parameters()
        {
            var type = new GenericType(typeof(IDictionary<int, Array>));

            Assert.That(type.GenericTypeDefinition, Is.EqualTo(typeof(IDictionary<,>)));
            Assert.That(type.GenericParameterTypes, Is.EqualTo(new[] { typeof(int), typeof(Array) }));
        }

        [Test]
        public void ExplicitCastTest()
        {
            var type = (GenericType)typeof(IEnumerable<int>);

            Assert.That(type.GenericTypeDefinition, Is.EqualTo(typeof(IEnumerable<>)));
            Assert.That(type.GenericParameterTypes, Is.EqualTo(new[] { typeof(int) }));
        }
    }
}