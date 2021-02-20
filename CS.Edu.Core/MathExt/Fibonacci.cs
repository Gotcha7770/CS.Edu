using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace CS.Edu.Core.MathExt
{
    using RX = System.Reactive.Linq.Observable;

    public static class Fibonacci
    {
        public static int Recursive(int n)
        {
            return GetNthFibonacci(0, 1, n);
        }

        static int GetNthFibonacci(int a, int b, int n)
        {
            return n > 1 ? GetNthFibonacci(b, b + a, --n) : a;
        }

        public static IEnumerable<int> Iterator()
        {
            for (int x = 0, y = 1 ;; y = x + y, x = y - x)
            {
                yield return x;
            }
        }

        public static IObservable<int> Observable(int count)
        {
            return RX.Create<int>(observer =>
            {
                var storage = new BehaviorSubject<int>(1);
                var output = RX.Return(0).Concat(storage).Take(count);
                var transfer = output.Zip(storage, (x, y) => x + y);

                return new CompositeDisposable
                {
                    output.Subscribe(observer),
                    transfer.Subscribe(storage),
                    storage
                };
            });
        }
    }
}