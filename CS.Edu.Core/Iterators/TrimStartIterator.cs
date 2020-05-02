using System;
using System.Collections.Generic;

namespace CS.Edu.Core.Iterators
{
    public enum TrimIteratorStates
    {
        StartTrimming,
        Regular,
        EndTrimming,
        Disposed
    }

    public enum TrimIteratorTriggers
    {
        TrimStart,
        UseRegular,
        TrimEnd,
        Dispose,
    }

    public class TrimIteratorStateMachine<T> : LightStateMachine<TrimIteratorStates, TrimIteratorTriggers>
    {
        private readonly Predicate<T> _match;

        public TrimIteratorStateMachine(Predicate<T> match)
        {
            _match = match;
        }

        public TrimIteratorStateMachine(T valueToSkip)
        {
            var comparer = EqualityComparer<T>.Default;
            _match = x => comparer.Equals(x, valueToSkip);
        }

        public bool IsMatch(T value) => _match(value);
    }

    public sealed class TrimStartIterator<T> : Iterator<T, TrimIteratorStates, TrimIteratorTriggers>
    {
        //private readonly T _valueToSkip;
        private readonly IEnumerator<T> _enumerator;

        public TrimStartIterator(IEnumerable<T> source, TrimIteratorStateMachine<T> state)
            : base(source, state)
        {
            _enumerator = _source.GetEnumerator();

            state.WhenActive(TrimIteratorStates.StartTrimming)
                .CanFire(TrimIteratorTriggers.Dispose, TrimIteratorStates.Disposed)
                .CanFire(TrimIteratorTriggers.UseRegular, TrimIteratorStates.Regular, () => !state.IsMatch(Current));

            state.WhenActive(TrimIteratorStates.Regular)
                .CanFire(TrimIteratorTriggers.Dispose, TrimIteratorStates.Disposed);

            //.OnEntryFrom(TrimIteratorTriggers.TrimStart, );

        }

        public TrimStartIterator(IEnumerable<T> source, T valueToTrim)
            : this(source, new TrimIteratorStateMachine<T>(valueToTrim))
        {
        }

        public bool SkipStart()
        {
            if (StateMachine.State == TrimIteratorStates.StartTrimming)
            {
                if (_enumerator.MoveNext())
                {
                    Current = _enumerator.Current;
                    StateMachine.Fire(TrimIteratorTriggers.UseRegular);
                    return StateMachine.State == TrimIteratorStates.StartTrimming;
                }

                Dispose();
                return false;
            }

            // switch (_state)
            // {
            //     case 1:
            //         _enumerator = _source.GetEnumerator();
            //         _state = 2;
            //         goto case 2;
            //     case 2:
            //         if (_enumerator.MoveNext())
            //         {
            //             Current = _enumerator.Current;
            //             if (comparer.Equals(Current, _valueToSkip))
            //             {
            //                 return true;
            //             }
            //
            //             _state = 3;
            //             return false;
            //         }
            //
            //         Dispose();
            //         break;
            // }

            return false;
        }

        public override bool MoveNext()
        {
            switch (StateMachine.State)
            {
                case TrimIteratorStates.StartTrimming:
                     while (SkipStart()) { }
                     return StateMachine.State == TrimIteratorStates.Regular;
                case TrimIteratorStates.Regular:
                    if (_enumerator.MoveNext())
                    {
                        Current = _enumerator.Current;
                        return true;
                    }

                    Dispose();
                    break;
            }

            // switch (_state)
            // {
            //     case 1:
            //     case 2:
            //         while (SkipStart()) { }
            //
            //         if (_state != 3)
            //             break;
            //         goto case 3;
            //     case 3:
            //         _state = 4;
            //         return true;
            //     case 4:
            //         if (_enumerator.MoveNext())
            //         {
            //             Current = _enumerator.Current;
            //             return true;
            //         }
            //
            //         Dispose();
            //         break;
            // }

            return false;
        }

        public override void Dispose()
        {
            if(StateMachine.State == TrimIteratorStates.Disposed)
                return;

            StateMachine.Fire(TrimIteratorTriggers.Dispose);
            Current = default;
            _enumerator.Dispose();
        }
    }
}