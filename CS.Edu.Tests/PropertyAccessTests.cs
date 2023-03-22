using System;
using FastMember;
using FluentAssertions;
using Xunit;

namespace CS.Edu.Tests;

public class PropertyAccessTests
{
    private readonly DateTime _now = DateTime.Now;

    [Fact]
    public void MemberAccessWithReflection()
    {
        var obj = new PropsOnClass { A = 123, B = "abc", C = _now, D = null };
        DateTime value = (DateTime)typeof(PropsOnClass).GetProperty("C").GetValue(obj);

        value.Should().Be(_now);
    }

    [Fact]
    public void MemberAccessWithFastMember_TypeAccessor()
    {
        var obj = new PropsOnClass { A = 123, B = "abc", C = _now, D = null };
        var accessor = TypeAccessor.Create(typeof(PropsOnClass));
        DateTime value = (DateTime)accessor[obj, "C"];

        value.Should().Be(_now);
    }

    [Fact]
    public void MemberAccessWithFastMember_ObjectAccessor()
    {
        var obj = new PropsOnClass { A = 123, B = "abc", C = _now, D = null };
        var accessor = ObjectAccessor.Create(obj);
        var value = (DateTime)accessor["C"];

        value.Should().Be(_now);
    }

    [Fact]
    public void StaticMemberAccessWithFastMember_ObjectAccessor()
    {
        var obj = new PropsOnClass { A = 123, B = "abc", C = _now, D = null };
        var accessor = TypeAccessor.Create(typeof(PropsOnClass));
        int value = (int)accessor[obj, "Count"];

        value.Should().Be(1);
    }
}

public class PropsOnClass
{
    public PropsOnClass()
    {
        Count++;
    }

    public static int Count { get; private set; }

    public int A { get; set; }
    public string B { get; set; }
    public DateTime C { get; set; }
    public object D { get; set; }
}