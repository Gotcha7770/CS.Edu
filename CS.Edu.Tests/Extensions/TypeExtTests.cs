using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CS.Edu.Core.Extensions;
using NUnit.Framework;

using static CS.Edu.Core.Extensions.DelegateExtensions;

namespace CS.Edu.Tests
{
    class A { }

    class B : A { }

    class C : B { }

    class GenericA<T> where T : A { }

    class GenericB<T> : GenericA<B> { } //???

    class TestClass : GenericB<C> { }

    [TestFixture]
    public class TypeExtTests
    {

        [Test]
        public void TestMethod()
        {
            Type openBaseType = typeof(GenericA<>);
            Type baseType = typeof(GenericA<A>);
            Type testTypeGeneric = typeof(GenericA<C>);
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
            bool result = GenericType.CanConvertFrom(typeof(GenericA<>));

            Assert.That(result, Is.True);
        }

        [Test]
        public void CanConvertFrom_GenericType_ReturnsTrue()
        {
            bool result = GenericType.CanConvertFrom(typeof(GenericA<A>));

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

        [Test]
        public void IsOfType_Generic()
        {
            var obj = new TestClass();

            Assert.IsTrue(obj.IsSubclassOf((GenericType)typeof(GenericB<C>)));
            Assert.IsTrue(obj.IsSubclassOf((GenericType)typeof(GenericB<B>)));
            Assert.IsTrue(obj.IsSubclassOf((GenericType)typeof(GenericB<A>)));
            Assert.IsTrue(obj.IsSubclassOf((GenericType)typeof(GenericA<C>)));
            Assert.IsTrue(obj.IsSubclassOf((GenericType)typeof(GenericA<B>)));
            Assert.IsTrue(obj.IsSubclassOf((GenericType)typeof(GenericA<A>)));
            Assert.IsTrue(obj.IsSubclassOf(new GenericType(typeof(GenericB<>))));
            Assert.IsTrue(obj.IsSubclassOf(new GenericType(typeof(GenericA<>))));
        }

        [Test]
        public void IsOfType_Generic2()
        {
            var obj = new GenericB<C>();

            Assert.IsTrue(obj.IsSubclassOf((GenericType)typeof(GenericB<C>)));
            Assert.IsTrue(obj.IsSubclassOf((GenericType)typeof(GenericB<B>)));
            Assert.IsTrue(obj.IsSubclassOf((GenericType)typeof(GenericB<A>)));
            Assert.IsTrue(obj.IsSubclassOf((GenericType)typeof(GenericA<C>)));
            Assert.IsTrue(obj.IsSubclassOf((GenericType)typeof(GenericA<B>)));
            Assert.IsTrue(obj.IsSubclassOf((GenericType)typeof(GenericA<A>)));
            Assert.IsTrue(obj.IsSubclassOf(new GenericType(typeof(GenericB<>))));
            Assert.IsTrue(obj.IsSubclassOf(new GenericType(typeof(GenericA<>))));
        }

        [Test]
        public void IsIEnumerableOfType_Generic()
        {
            var source = new GenericB<C>[]
            {
                new GenericB<C>(),
                new TestClass()
            };

            //var result = source.OfType<GenericB<C>>();
            var result = source.OfType((GenericType)typeof(GenericB<>));

            Assert.That(result, Is.EqualTo(source));
        }

        [Test]
        public void IsIEnumerableOfType_Generic2()
        {
            var source = new object[]
            {
               new TestClass(),
               new GenericB<C>(),
               new C()
            };

            var result = source.OfType((GenericType)typeof(GenericA<>));

            Assert.That(result, Is.EqualTo(new [] { source[0], source[1] }));
        }

        [Test]
        public void IsIEnumerableOfType_Generic3()
        {
            var taskC = Task<C>.Run(() => new C());
            var taskB = Task<B>.Run(() => new B());

            var source = new Task[]
            {
               taskC,
               taskB,
               Task.Run(Empty)
            };

            var result = source.OfType((GenericType)typeof(Task<>));

            Assert.That(result, Is.EqualTo(new Task[] { taskC, taskB }));
        }
    }
}