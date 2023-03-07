using System;
using System.Reactive.Linq;
using FluentAssertions;
using Xunit;

namespace CS.Edu.Tests.ReactiveTests;

public class ComplexReactiveQueries
{
    [Fact]
    public void OneToOne()
    {
        string result = null;
        IObservable<int> source = Observable.Return(1);
        IObservable<string> selector = source.Select(x => x.ToString());

        using (_ = selector.Subscribe(x => result = x))
        {
            result.Should().Be("1");
        }
    }

    [Fact]
    public void OneToMany()
    {
        IObservable<string> result = null;
        IObservable<int> source = Observable.Return(1);
        IObservable<IObservable<string>> selector = source.Select(x => Observable.Return(x.ToString()));

        using (_ = selector.Subscribe(x => result = x))
        {
            //Assert.AreEqual("1", result.First());
        }
    }

    [Fact]
    public void ManyToOne()
    {
        string result = null;
        IObservable<IObservable<string>> source = Observable.Return(1).Select(x => Observable.Return(x.ToString()));
        IObservable<string> selector = source.Merge(); //SelectMany, Switch

        using (_ = selector.Subscribe(x => result = x))
        {
            result.Should().Be("1");
        }
    }

    [Fact]
    public void ManyToMany()
    {

    }
}