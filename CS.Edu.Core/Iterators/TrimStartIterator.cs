using System;
using System.Collections.Generic;
using CS.Edu.Core.StateMachine;

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
        public TrimIteratorStateMachine(Func<T, bool> skipStartGuard)
        {
            SkipStartGuard = skipStartGuard;
        }

        public Func<T, bool> SkipStartGuard { get; }
        public Func<bool> SkipEndtGuard { get; }

        public void Reset()
        {
            State = TrimIteratorStates.StartTrimming;
        }
    }

    public sealed class TrimStartIterator<T> : Iterator<T, TrimIteratorStates, TrimIteratorTriggers>
    {
        private readonly IEnumerator<T> _enumerator;

        public TrimStartIterator(IEnumerable<T> source, TrimIteratorStateMachine<T> state)
            : base(source, state)
        {
            _enumerator = _source.GetEnumerator();

            state.WhenActive(TrimIteratorStates.StartTrimming)
                .CanFire(TrimIteratorTriggers.Dispose, TrimIteratorStates.Disposed)
                .CanFire(TrimIteratorTriggers.UseRegular, TrimIteratorStates.Regular, () => !state.SkipStartGuard(Current));

            state.WhenActive(TrimIteratorStates.Regular)
                .CanFire(TrimIteratorTriggers.Dispose, TrimIteratorStates.Disposed)
                .CanFire(TrimIteratorTriggers.TrimEnd, TrimIteratorStates.EndTrimming);

            //.OnEntryFrom(TrimIteratorTriggers.TrimStart, );
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

            return false;
        }

        public bool SkipEnd()
        {
            return true;
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