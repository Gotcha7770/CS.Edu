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
            GenericType genericType = (GenericType)baseType;

            Assert.IsFalse(testType.IsSubclassOf(testTypeGeneric));            
            Assert.IsTrue(testType.IsSubclassOfGeneric(openBaseType));
            Assert.IsTrue(testType.IsSubclassOf(genericType));
        }

        [Test]
        public void CanConvertFrom_Null_ReturnsFalse()
        {
            bool result = GenericType.CanConvertFrom(null);

            Assert.That(result, Is.False);
        }

        [Test]
        public void CanConvertFrom_NotGenericType_ReturnsFalse()
        {
            bool result = GenericType.CanConvertFrom(typeof(int));

            Assert.That(result, Is.False);
        }

        [Test]
        public void CanConvertFrom_OpenGenericType_ReturnsTrue()
        {
            bool result = GenericType.CanConvertFrom(typeof(BaseGeneric<>));

            Assert.That(result, Is.True);
        }

        [Test]
        public void CanConvertFrom_GenericType_ReturnsTrue()
        {
            bool result = GenericType.CanConvertFrom(typeof(BaseGeneric<TBase>));

            Assert.That(result, Is.True);
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