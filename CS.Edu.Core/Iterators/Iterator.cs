using System;
using System.Collections;
using System.Collections.Generic;

namespace CS.Edu.Core.Iterators
{
    public abstract class Iterator<T> : IIterator<T>
    {
        protected readonly IEnumerable<T> _source;

        protected int _state = 1;

        protected Iterator(IEnumerable<T> source)
        {
            _source = source;
        }

        public T Current { get; protected set; }

        public abstract bool MoveNext();

        public virtual void Reset() => throw new NotImplementedException();

        public abstract void Dispose();

        object IEnumerator.Current => Current;
    }
}