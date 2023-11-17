using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using Xunit;

namespace CS.Edu.Tests.ReactiveTests;

public class FromEventPatternTests
{
    private class TestEventArgs : EventArgs
    {
        public int Value { get; }

        public TestEventArgs(int value)
        {
            Value = value;
        }
    }

    private event EventHandler<TestEventArgs> ValueChanged;

    private BehaviorSubject<int> Subject { get; } = new BehaviorSubject<int>(-1);
    private IObservable<int> ObservableProperty { get; set; }

    [Fact]
    public async Task FromEventPattern_MultipleSubscribers()
    {
        int first = 0;
        int second = 0;
        var observable = Observable.FromEventPattern<EventHandler<TestEventArgs>, TestEventArgs>(
                x => ValueChanged += x,
                x => ValueChanged -= x)
            .Where(x => x.EventArgs.Value % 3 == 0 || x.EventArgs.Value % 5 == 0);

        //using var a = observable.Subscribe(x => first = x.EventArgs.Value);
        ObservableProperty = observable.Select(x => x.EventArgs.Value);
        using var b = ObservableProperty.Subscribe(x => second = x);
        using var a = observable.Select(x => x.EventArgs.Value).Subscribe(Subject);

        ValueChanged.Invoke(this, new TestEventArgs(2));
        ValueChanged.Invoke(this, new TestEventArgs(3));
        ValueChanged.Invoke(this, new TestEventArgs(4));
        //await Task.Delay(150);
    }
}