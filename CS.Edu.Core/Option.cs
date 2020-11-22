using System;
using System.Collections.Generic;

namespace CS.Edu.Core
{
    public static class Option
    {
        // wrap the given value into a Some
        public static Option<T> Some<T>(T value) => new Some<T>(value);

        // the None value
        public static None None => default;

        public static Option<R> Map<T, R>(this None _, Func<T, R> f) => None;

        public static Option<R> Map<T, R>(this Some<T> some, Func<T, R> f) => Some(f(some.Value));

        public static Option<R> Map<T, R>(this Option<T> optT, Func<T, R> f)
        {
            return optT.Match(() => None, x => Some(f(x)));
        }

        public static Option<R> Select<T, R>(this Option<T> one, Func<T, R> func) => one.Map(func);
    }

    public struct Option<T> : IEquatable<None>, IEquatable<Option<T>>
    {
        private readonly bool _isSome;

        private Option(T value)
        {
            Value = value;
            _isSome = true;
        }

        public T Value { get; }

        public static implicit operator Option<T>(None _) => new Option<T>();

        public static implicit operator Option<T>(Some<T> some) => new Option<T>(some.Value);

        public static implicit operator Option<T>(T value) => value == null ? Option.None : Option.Some(value);

        public static explicit operator T(Option<T> value) => value.Value;

        public R Match<R>(Func<R> None, Func<T, R> Some) => _isSome ? Some(Value) : None();

        public IEnumerable<T> AsEnumerable()
        {
            if (_isSome)
                yield return Value;
        }

        public bool Equals(None other) => !_isSome;


        public bool Equals(Option<T> other)
        {
            return _isSome ? Value.Equals(other.Value) : !other._isSome;
        }

        public static bool operator ==(Option<T> one, Option<T> other) => one.Equals(other);

        public static bool operator !=(Option<T> one, Option<T> other) => !(one == other);

        public override string ToString() => _isSome ? $"Some({Value})" : "None";
    }

    public struct None { }

    public struct Some<T>
    {
        internal T Value { get; }

        internal Some(T value)
        {
            Value = value;
        }
    }
}