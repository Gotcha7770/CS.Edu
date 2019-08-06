using Moq;
using NSubstitute;
using NUnit.Framework;

namespace CS.Edu.Tests
{
    public interface ITestService
    {
        public object Process(object main, params object[] additional);
    }

    [TestFixture]
    public class Test
    {
       [Test]
        public void TestMethod1()
        {
            var main = new object();
            var sub = Substitute.For<ITestService>();
            sub.Process(Arg.Any<object>()).Returns(x => x[0]);

            Assert.That(sub.Process(main), Is.EqualTo(main));
        }

        [Test]
        public void TestMethod2()
        {
            var main = new object();
            var sub = new Mock<ITestService>();
            sub.Setup(x => x.Process(It.IsAny<object>())).Returns<object, object>((x, y) => x);

            Assert.That(sub.Object.Process(main), Is.EqualTo(main));
        }
    }
}
