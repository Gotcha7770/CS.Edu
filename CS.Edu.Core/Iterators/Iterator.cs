using System;
using System.Collections;
using System.Collections.Generic;
using CS.Edu.Core.Interfaces;
using CS.Edu.Core.StateMachine;

namespace CS.Edu.Core.Iterators
{
    public abstract class Iterator<T, TState, TTrigger> : IIterator<T, TState, TTrigger>
        where TState : Enum
        where TTrigger : Enum
    {
        protected readonly IEnumerable<T> _source;

        protected Iterator(IEnumerable<T> source, LightStateMachine<TState, TTrigger> state)
        {
            _source = source;
            StateMachine = state;
        }

        protected Iterator(IEnumerable<T> source, Func<LightStateMachine<TState, TTrigger>> stateFactory)
            : this(source, stateFactory())
        {
        }

        public LightStateMachine<TState, TTrigger> StateMachine { get; }

        public T Current { get; protected set; }

        object IEnumerator.Current => Current;

        public abstract bool MoveNext();

        public virtual void Reset() => throw new NotImplementedException();

        public abstract void Dispose();
    }
}