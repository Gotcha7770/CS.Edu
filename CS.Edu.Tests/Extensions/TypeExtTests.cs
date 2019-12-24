using System;
using CS.Edu.Core.Extensions;
using NUnit.Framework;

namespace CS.Edu.Tests
{
    class TestTypeBase { }

    class TestType : TestTypeBase { }

    class TestGenericBase<T> where T : TestTypeBase { }

    class TestClass : TestGenericBase<TestType> { }

    [TestFixture]
    public class TypeExtTests
    {

        [Test]
        public void TestMethod()
        {
            Type openBaseType = typeof(TestGenericBase<>);
            Type baseType = typeof(TestGenericBase<TestTypeBase>);
            Type testTypeGeneric = typeof(TestGenericBase<TestType>);
            Type testType = typeof(TestClass);

            //object obj = new TestClass();

            Assert.IsTrue(testType.IsSubclassOf(testTypeGeneric));
            Assert.IsTrue(testType.IsSubclassOfExt(openBaseType));
        }
    }
}