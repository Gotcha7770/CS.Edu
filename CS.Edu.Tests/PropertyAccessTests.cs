using System;
using FastMember;
using NUnit.Framework;

namespace CS.Edu.Tests;

[TestFixture]
public class PropertyAccessTests
{
    private readonly DateTime _now = DateTime.Now;

    [Test]
    public void MemberAccessWithReflection()
    {
        var obj = new PropsOnClass { A = 123, B = "abc", C = _now, D = null };
        DateTime value = (DateTime)typeof(PropsOnClass).GetProperty("C").GetValue(obj);

        Assert.AreEqual(_now, value);
    }

    [Test]
    public void MemberAccessWithFastMember_TypeAccessor()
    {
        var obj = new PropsOnClass { A = 123, B = "abc", C = _now, D = null };
        var accessor = TypeAccessor.Create(typeof(PropsOnClass));
        DateTime value = (DateTime)accessor[obj, "C"];

        Assert.AreEqual(_now, value);
    }

    [Test]
    public void MemberAccessWithFastMember_ObjectAccessor()
    {
        var obj = new PropsOnClass { A = 123, B = "abc", C = _now, D = null };
        var accessor = ObjectAccessor.Create(obj);
        var value = (DateTime)accessor["C"];

        Assert.AreEqual(_now, value);
    }

    [Test]
    public void StaticMemberAccessWithFastMember_ObjectAccessor()
    {
        var obj = new PropsOnClass { A = 123, B = "abc", C = _now, D = null };
        var accessor = TypeAccessor.Create(typeof(PropsOnClass));
        int value = (int)accessor[obj, "Count"];

        Assert.AreEqual(1, value);
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