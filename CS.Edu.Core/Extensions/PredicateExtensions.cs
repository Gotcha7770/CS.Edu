using System;

namespace CS.Edu.Core.Extensions;

public static class PredicateExtensions
{
    extension<T>(Predicate<T> target)
    {
        public static Predicate<T> True => Cache<T>.True;

        public static Predicate<T> False => Cache<T>.False;

        public Predicate<T> And(Predicate<T> other)
        {
            return x => target(x) && other(x);
        }

        public Predicate<T> Or(Predicate<T> other)
        {
            return x => target(x) || other(x);
        }
    }

    private static class Cache<T>
    {
        internal static readonly Predicate<T> True = GetTrue;
        internal static readonly Predicate<T> False = GetFalse;

        private static bool GetTrue(T value) => true;
        private static bool GetFalse(T value) => false;
    }
}