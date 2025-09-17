using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace CS.Edu.Tests;

public class ExclusiveAccessToken<T> : IDisposable
{
    private readonly Func<Task<T>> _operation;
    private readonly Func<T> _defaultFactory;
    private int _isOperationRunning;

    public ExclusiveAccessToken(Func<Task<T>> operation, Func<T> defaultFactory)
    {
        _operation = operation;
        _defaultFactory = defaultFactory;
    }

    public async Task<T> Execute()
    {
        if (Interlocked.CompareExchange(ref _isOperationRunning, 1, 0) is 0)
        {
            return await _operation();
        }

        return _defaultFactory();
    }

    public void Dispose() => _isOperationRunning = 0;
}

public class ExclusiveAccessTests
{
    [Fact]
    public async Task ExclusiveOperationReturnsValue()
    {
        using var token = new ExclusiveAccessToken<string>(async () =>
        {
            await Task.Delay(100).ConfigureAwait(false);
            return "Test";
        }, () => null);

        var first = token.Execute();
        var second = token.Execute();

        var value = await first;
    }

    [Fact]
    public async Task BlockedOperationReturnsDefault()
    {
        using var token = new ExclusiveAccessToken<string>(async () =>
        {
            await Task.Delay(100).ConfigureAwait(false);
            return "Test";
        }, () => null);

        var first = token.Execute();
        var second = token.Execute();

        var value = await second;
    }

    [Fact]
    public async Task TokenReleased()
    {
        using (var token = new ExclusiveAccessToken<string>(async () =>
               {
                   await Task.Delay(100).ConfigureAwait(false);
                   return "Test";
               }, () => null))
        {
            var first = token.Execute();
        }
    }
}