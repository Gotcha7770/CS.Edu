using System;
using System.Collections.Generic;
using System.Threading;

// ReSharper disable once CheckNamespace
namespace CS.Edu.Core.Extensions;

public static partial class AsyncEnumerableEx
{
    public static IAsyncEnumerable<T> Create<T>(Func<CancellationToken, IAsyncEnumerator<T>> factory)
    {
        return new AnonymousAsyncEnumerable<T>(factory);
    }

    private sealed class AnonymousAsyncEnumerable<T> : IAsyncEnumerable<T>
    {
        private readonly Func<CancellationToken, IAsyncEnumerator<T>> _factory;

        public AnonymousAsyncEnumerable(Func<CancellationToken, IAsyncEnumerator<T>> factory) => _factory = factory;

        public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken)
        {
            cancellationToken
                .ThrowIfCancellationRequested(); // NB: [LDM-2018-11-28] Equivalent to async iterator behavior.

            return _factory(cancellationToken);
        }
    }
}