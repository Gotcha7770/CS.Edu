using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Moq;
using NSubstitute;
using Xunit;
using static FluentAssertions.FluentActions;

namespace CS.Edu.Tests;

public interface ITestService
{
    object Process(object main);
    object ProcessParams(object main, params object[] additional);
}

public class MoqVsNSubstituteTest
{
    [Fact]
    public void NSubstitute_SetupTest()
    {
        var main = new object();
        var sub = Substitute.For<ITestService>();

        sub.Process(Arg.Any<object>()).Returns(x => x[0]);
        sub.Process(main).Should().Be(main);

        sub.ProcessParams(Arg.Any<object>()).Returns(x => x[0]);
        sub.Process(main).Should().Be(main);
    }

    [Fact]
    public void MoqSetupTest()
    {
        var main = new object();
        var sub = new Mock<ITestService>();

        //Требует явного указания типа в Returns, в отличии от NSubstitute
        // + обращение через Object
        sub.Setup(x => x.Process(It.IsAny<object>())).Returns<object>(x => x);
        sub.Object.Process(main).Should().Be(main);

        //Во-первых, задаем возвращаемое значение через предикат (sic!)
        //во-вторых, приходится возвращать глобальный объект, а не аргумент
        var moq = Mock.Of<ITestService>(x => x.Process(It.IsAny<object>()) == main);
        sub.Object.Process(main).Should().Be(main);

        //Требует явного указания параметров, в отличии от NSubstitute
        //sub.Setup(x => x.ProcessParams(It.IsAny<object>())).Returns<object, object>((x, _) => x);
        sub.Setup(x => x.ProcessParams(It.IsAny<object>())).Returns(new object());
        sub.Object.Process(main).Should().Be(main);
    }

    [Fact]
    public void NSubstitute_LINQTest()
    {
        var input = Substitute.For<IEnumerable<int>>();

        input.Any().Should().BeFalse();

        input.Any(x => x % 2 != 0).Returns(true);

        input.Any().Should().BeFalse();
        input.Any(x => x % 2 != 0).Should().BeTrue();
    }

    [Fact]
    public void Moq_LINQTest()
    {
        var input = Mock.Of<IEnumerable<int>>();

        //Падает с NRE
        Invoking(() => input.Any()).Should().Throw<NullReferenceException>();

        var moq = Mock.Get(input);
        moq.Setup(x => x.Any(x => x % 2 != 0)).Returns(true);

        input.Any().Should().BeFalse();
        input.Any(x => x % 2 != 0).Should().BeTrue();
    }
}