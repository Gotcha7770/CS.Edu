using System;

namespace CS.Edu.Core.Monads
{
    public static class BindOverLINQ
    {
        class Bindable<T>
        {
            public readonly T Value;
            public Bindable(T value) => Value = value;
        }

        // logically, Select is just a special way of calling SelectMany;
        private static Bindable<R> Select<T, R>(this Bindable<T> item, Func<T, R> selector)
        {
            var unwrapped = item.Value;
            var result = selector(unwrapped);

            return new Bindable<R>(result);
        }

        // SelectMany is what enables you to bind any operation
        // to the end of a monadic workflow and produce a new workflow
        static Bindable<R> SelectMany<T, R>(this Bindable<T> item,
            Func<T, Bindable<R>> selector)
        {
            return selector(item.Value);
        }

        // with intermediate operation
        // to build computation pipeline
        static Bindable<R> SelectMany<T, I, R>(this Bindable<T> item,
            Func<T, Bindable<I>> selector,
            Func<T, I, R> resultSelector)
        {
            var intermediate = selector(item.Value);
            var result = resultSelector(item.Value, intermediate.Value);

            return new Bindable<R>(result);
        }

        public static void Example()
        {
            var bindable = new Bindable<int>(42);

            Func<int, string> toString = x => x.ToString();
            Bindable<string> result = bindable.Select(toString);

            Func<int, Bindable<string>> toWrappedString = x => new Bindable<string>(x.ToString());
            Bindable<Bindable<string>> twoLayers = bindable.Select(toWrappedString); // but we want Bindable<T>
            result = bindable.SelectMany(toWrappedString); //right way

            // same with LINQ syntax
            result = from inner in bindable select inner.ToString();

            result = bindable.SelectMany(x => bindable, (x, y) => x.ToString() + y);

            // same with LINQ syntax
            result = from x in bindable
                     from y in bindable
                     select x.ToString() + y;

        }
    }
}