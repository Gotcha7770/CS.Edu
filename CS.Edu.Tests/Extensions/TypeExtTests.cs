using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CS.Edu.Core.Extensions;
using FluentAssertions;
using Xunit;
using static FluentAssertions.FluentActions;

namespace CS.Edu.Tests.Extensions;

class A { }

class B : A { }

class C : B { }

class GenericA<T> where T : A { }

class GenericB<T> : GenericA<B> { }

public class TypeExtTests
{
    private class TestClass : GenericB<C> { }

    [Fact]
    public void Inheritance()
    {
        Type openBaseType = typeof(GenericA<>);
        Type baseType = typeof(GenericA<A>);
        Type testTypeGeneric = typeof(GenericA<C>);
        Type testType = typeof(TestClass);
        GenericType genericType = (GenericType)baseType;

        testType.IsSubclassOf(testTypeGeneric).Should().BeFalse();
        testType.IsSubclassOfGeneric(openBaseType).Should().BeTrue();
        testType.IsSubclassOf(genericType).Should().BeTrue();
    }

    [Fact]
    public void CanConvertFrom_Null_ReturnsFalse()
    {
        GenericType.CanConvertFrom(null)
            .Should().BeFalse();
    }

    [Fact]
    public void CanConvertFrom_NotGenericType_ReturnsFalse()
    {
        GenericType.CanConvertFrom(typeof(int))
            .Should().BeFalse();
    }

    [Fact]
    public void CanConvertFrom_OpenGenericType_ReturnsTrue()
    {
        GenericType.CanConvertFrom(typeof(GenericA<>))
            .Should().BeTrue();
    }

    [Fact]
    public void CanConvertFrom_GenericType_ReturnsTrue()
    {
        GenericType.CanConvertFrom(typeof(GenericA<A>))
            .Should().BeTrue();
    }

    [Fact]
    public void GenericTypeConstructionTest_NotGenericType_ThrowsException()
    {
        Invoking(() => new GenericType(typeof(int))).Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void GenericTypeConstructionTest()
    {
        var type = new GenericType(typeof(IEnumerable<int>));

        type.GenericTypeDefinition.Should().Be(typeof(IEnumerable<>));
        type.GenericParameterTypes.Should().BeEquivalentTo([typeof(int)]);
    }

    [Fact]
    public void GenericTypeConstructionTest_2Parameters()
    {
        var type = new GenericType(typeof(IDictionary<int, Array>));

        type.GenericTypeDefinition.Should().Be(typeof(IDictionary<,>));
        type.GenericParameterTypes.Should().BeEquivalentTo([typeof(int), typeof(Array)]);
    }

    [Fact]
    public void ExplicitCastTest()
    {
        var type = (GenericType)typeof(IEnumerable<int>);

        type.GenericTypeDefinition.Should().Be(typeof(IEnumerable<>));
        type.GenericParameterTypes.Should().BeEquivalentTo([typeof(int)]);
    }

    [Fact]
    public void IsOfType_Generic()
    {
        var obj = new TestClass();

        obj.IsSubclassOf((GenericType)typeof(GenericB<C>)).Should().BeTrue();
        obj.IsSubclassOf((GenericType)typeof(GenericB<B>)).Should().BeTrue();
        obj.IsSubclassOf((GenericType)typeof(GenericB<A>)).Should().BeTrue();
        obj.IsSubclassOf((GenericType)typeof(GenericA<C>)).Should().BeTrue();
        obj.IsSubclassOf((GenericType)typeof(GenericA<B>)).Should().BeTrue();
        obj.IsSubclassOf((GenericType)typeof(GenericA<A>)).Should().BeTrue();
        obj.IsSubclassOf(new GenericType(typeof(GenericB<>))).Should().BeTrue();
        obj.IsSubclassOf(new GenericType(typeof(GenericA<>))).Should().BeTrue();
    }

    [Fact]
    public void IsOfType_Generic2()
    {
        var obj = new GenericB<C>();

        obj.IsSubclassOf((GenericType)typeof(GenericB<C>)).Should().BeTrue();
        obj.IsSubclassOf((GenericType)typeof(GenericB<B>)).Should().BeTrue();
        obj.IsSubclassOf((GenericType)typeof(GenericB<A>)).Should().BeTrue();
        obj.IsSubclassOf((GenericType)typeof(GenericA<C>)).Should().BeTrue();
        obj.IsSubclassOf((GenericType)typeof(GenericA<B>)).Should().BeTrue();
        obj.IsSubclassOf((GenericType)typeof(GenericA<A>)).Should().BeTrue();
        obj.IsSubclassOf(new GenericType(typeof(GenericB<>))).Should().BeTrue();
        obj.IsSubclassOf(new GenericType(typeof(GenericA<>))).Should().BeTrue();
    }

    [Fact]
    public void IsEnumerableOfType_Generic()
    {
        var source = new[]
        {
            new GenericB<C>(),
            new TestClass()
        };

        //var result = source.OfType<GenericB<C>>();
        var result = source.OfType((GenericType)typeof(GenericB<>));

        result.Should().BeEquivalentTo(source);
    }

    [Fact]
    public void IsEnumerableOfType_Generic2()
    {
        var source = new object[]
        {
            new TestClass(),
            new GenericB<C>(),
            new C()
        };

        var result = source.OfType((GenericType)typeof(GenericA<>));

        result.Should().BeEquivalentTo(new [] { source[0], source[1] });
    }

    [Fact]
    public void IsEnumerableOfType_Generic3()
    {
        var taskC = Task.FromResult(new C());
        var taskB = Task.FromResult(new B());

        var source = new []
        {
            taskC,
            taskB,
            Task.Run(Actions.Empty())
        };

        var result = source.OfType((GenericType)typeof(Task<>));

        result.Should().BeEquivalentTo(new Task[] { taskC, taskB });
    }
}