﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using Microsoft.Reactive.Testing;
using Xunit;

namespace CS.Edu.Tests.ReactiveTests;

public class BufferTests
{
    [Fact]
    public void BufferCountTest()
    {
        var scheduler = new TestScheduler();
        var source = scheduler.CreateHotObservable(
            ReactiveTest.OnNext(10, 1),
            ReactiveTest.OnNext(20, 2),
            ReactiveTest.OnNext(30, 3),
            ReactiveTest.OnNext(40, 4),
            ReactiveTest.OnNext(50, 5),
            ReactiveTest.OnNext(60, 6),
            ReactiveTest.OnNext(70, 7),
            ReactiveTest.OnNext(80, 8),
            ReactiveTest.OnNext(90, 9),
            ReactiveTest.OnCompleted<int>(100));

        var result = scheduler.Start(() => source.Buffer(2), 0, 0, ReactiveTest.Disposed);

        result.Messages.AssertEqual(
            ReactiveTest.OnNext<IList<int>>(20, x => x.SequenceEqual(new []{1, 2})),
            ReactiveTest.OnNext<IList<int>>(40, x => x.SequenceEqual(new []{3, 4})),
            ReactiveTest.OnNext<IList<int>>(60, x => x.SequenceEqual(new []{5, 6})),
            ReactiveTest.OnNext<IList<int>>(80, x => x.SequenceEqual(new []{7, 8})),
            ReactiveTest.OnNext<IList<int>>(100, x => x.SequenceEqual(new []{9})),
            ReactiveTest.OnCompleted<IList<int>>(100));
    }

    [Fact]
    public void BuffetTimeoutTest()
    {
        var scheduler = new TestScheduler();
        var source = scheduler.CreateHotObservable(
            ReactiveTest.OnNext(10, 1),
            ReactiveTest.OnNext(20, 2),
            ReactiveTest.OnNext(30, 3),
            ReactiveTest.OnNext(40, 4),
            ReactiveTest.OnNext(50, 5),
            ReactiveTest.OnNext(60, 6),
            ReactiveTest.OnNext(70, 7),
            ReactiveTest.OnNext(80, 8),
            ReactiveTest.OnNext(90, 9),
            ReactiveTest.OnCompleted<int>(100));

        var result = scheduler.Start(() => source.Buffer(TimeSpan.FromTicks(20), scheduler), 0, 0, ReactiveTest.Disposed);

        result.Messages.AssertEqual(
            ReactiveTest.OnNext<IList<int>>(21, x => x.SequenceEqual(new []{1, 2})),
            ReactiveTest.OnNext<IList<int>>(41, x => x.SequenceEqual(new []{3, 4})),
            ReactiveTest.OnNext<IList<int>>(61, x => x.SequenceEqual(new []{5, 6})),
            ReactiveTest.OnNext<IList<int>>(81, x => x.SequenceEqual(new []{7, 8})),
            ReactiveTest.OnNext<IList<int>>(100, x => x.SequenceEqual(new []{9})),
            ReactiveTest.OnCompleted<IList<int>>(100));
    }

    [Fact]
    public void BufferOnObservable()
    {
        var scheduler = new TestScheduler();
        var source = scheduler.CreateHotObservable(
            ReactiveTest.OnNext(10, 1),
            ReactiveTest.OnNext(20, 2),
            ReactiveTest.OnNext(30, 3),
            ReactiveTest.OnNext(40, 4),
            ReactiveTest.OnNext(50, 5),
            ReactiveTest.OnNext(60, 6),
            ReactiveTest.OnNext(70, 7),
            ReactiveTest.OnNext(80, 8),
            ReactiveTest.OnNext(90, 9),
            ReactiveTest.OnCompleted<int>(100));

        var boundary = scheduler.CreateHotObservable(
            ReactiveTest.OnNext(35, 0),
            ReactiveTest.OnNext(65, 0));

        var result = scheduler.Start(() => source.Buffer(boundary), 0, 0, ReactiveTest.Disposed);

        result.Messages.AssertEqual(
            ReactiveTest.OnNext<IList<int>>(35, x => x.SequenceEqual(new []{1, 2, 3})),
            ReactiveTest.OnNext<IList<int>>(65, x => x.SequenceEqual(new []{4, 5, 6})),
            ReactiveTest.OnNext<IList<int>>(100, x => x.SequenceEqual(new []{7, 8, 9})),
            ReactiveTest.OnCompleted<IList<int>>(100));
    }
}