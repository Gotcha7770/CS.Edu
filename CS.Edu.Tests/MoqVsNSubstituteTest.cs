using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using NSubstitute;
using NUnit.Framework;

namespace CS.Edu.Tests
{
    public interface ITestService
    {
        object Process(object main);
        object ProcessParams(object main, params object[] additional);
    }

    [TestFixture]
    public class MoqVsNSubstituteTest
    {
       [Test]
        public void NSubstitute_SetupTest()
        {
            var main = new object();
            var sub = Substitute.For<ITestService>();

            sub.Process(Arg.Any<object>()).Returns(x => x[0]);
            Assert.AreEqual(main, sub.Process(main));

            sub.ProcessParams(Arg.Any<object>()).Returns(x => x[0]);
            Assert.AreEqual(main, sub.Process(main));
        }

        [Test]
        public void MoqSetupTest()
        {
            var main = new object();
            var sub = new Mock<ITestService>();

            //Требует явного указания типа в Returns, в отличии от NSubstitute
            // + обращение через Object
            sub.Setup(x => x.Process(It.IsAny<object>())).Returns<object>(x => x);
            Assert.AreEqual(main, sub.Object.Process(main));

            //Во-первых, задаем возвращаемое значение через предикат (sic!)
            //во-вторых, приходится возвращать глобальный объект, а не аргумент
            var moq = Mock.Of<ITestService>(x => x.Process(It.IsAny<object>()) == main);
            Assert.AreEqual(main, moq.Process(main));

            //Требует явного указания параметров, в отличии от NSubstitute
            //sub.Setup(x => x.ProcessParams(It.IsAny<object>())).Returns<object, object>((x, _) => x);
            sub.Setup(x => x.ProcessParams(It.IsAny<object>())).Returns(new object());
            Assert.AreEqual(main, sub.Object.Process(main));
        }

        [Test]
        public void NSubstitute_LINQTest()
        {
            var input = Substitute.For<IEnumerable<int>>();

            Assert.IsFalse(input.Any());

            input.Any(x => x % 2 != 0).Returns(true);

            Assert.IsFalse(input.Any());
            Assert.IsTrue(input.Any(x => x % 2 != 0));
        }

        [Test]
        public void Moq_LINQTest()
        {
            var input = Mock.Of<IEnumerable<int>>();

            //Падает с NRE
            Assert.Throws<NullReferenceException>(() => input.Any());

            var moq = Mock.Get(input);
            moq.Setup(x => x.Any(x => x % 2 != 0)).Returns(true);

            Assert.IsFalse(input.Any());
            Assert.IsTrue(input.Any(x => x % 2 != 0));
        }
    }
}
