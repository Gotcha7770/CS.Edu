using System;

namespace CS.Edu.Core.Extensions;

public static class ActionExtensions
{
    extension (Action target)
    {
        public static Action Idle => Cache.Value;

        public bool IsIdle => ReferenceEquals(target, Cache.Value);
    }

    extension<T>(Action<T> target)
    {
        public static Action<T> Idle => Cache<T>.Value;
        public bool IsIdle => ReferenceEquals(target, Cache<T>.Value);
    }

    extension<T1, T2>(Action<T1, T2> target)
    {
        public static Action<T1, T2> Idle => Cache<T1, T2>.Value;
        public bool IsIdle => ReferenceEquals(target, Cache<T1, T2>.Value);
    }

    private static class Cache
    {
        internal static readonly Action Value = Empty;

        private static void Empty() { }
    }

    private static class Cache<T>
    {
        internal static readonly Action<T> Value = Empty;

        private static void Empty(T @in) { }
    }

    private static class Cache<T1, T2>
    {
        internal static readonly Action<T1, T2> Value = Empty;

        private static void Empty(T1 first, T2 second) { }
    }
}