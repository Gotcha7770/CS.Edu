using System;
using System.Diagnostics.CodeAnalysis;

namespace CS.Edu.Core.Extensions;

public static class Actions
{
    public static Action Empty() => EmptyAction.Value;

    public static Action<T> Empty<T>() => EmptyAction<T>.Value;

    [SuppressMessage("ReSharper", "MemberHidesStaticFromOuterClass")]
    private static class EmptyAction
    {
        internal static readonly Action Value = Empty;

        private static void Empty() { }
    }

    private static class EmptyAction<T>
    {
        internal static readonly Action<T> Value = Empty;

        private static void Empty(T @in) { }
    }
}