using System;
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
        public void TestMethod2()
        {
            var cast1 = new TestClass() as BaseGeneric<TestType>;
            var cast2 = new TestClass() as BaseGeneric<TBase>;
        }
    }
}