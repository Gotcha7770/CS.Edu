using System;
using System.Collections;
using System.Collections.Generic;

namespace CS.Edu.Core.MathExt
{
    public static class PrimesGenerator
    {
        public static IEnumerable<long> GetPrimes()
        {            
            return new PrimesIterator();
        }
    }

    class PrimesIterator : IEnumerable<long>
    {
        public IEnumerator<long> GetEnumerator()
        {
            return new PrimesEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new PrimesEnumerator();
        }
    }

    class PrimesEnumerator : IEnumerator<long>
    {
        private long _current = 2;
        private HashSet<long> _factors = new HashSet<long>();

        public long Current => _current;

        object IEnumerator.Current => _current;

        public void Dispose()
        {
            
        }

        public bool MoveNext()
        {
            throw new NotImplementedException();
        }

        public void Reset()
        {
            _current = 2;
            _factors.Clear();
        }
    }
}