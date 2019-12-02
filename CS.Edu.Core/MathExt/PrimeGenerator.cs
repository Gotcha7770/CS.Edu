using System;
using System.Collections;
using System.Collections.Generic;
using CS.Edu.Core.Extensions;

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
        private long _current = 0;
        private int _step = 0;
        
        private List<long> _factors = new List<long>{2, 3};

        public long Current => _current;

        object IEnumerator.Current => _current;

        public void Dispose()
        {
            
        }

        public bool MoveNext()
        {
            if (_current == 0)
            {
                _current = 2;
                return true;
            }

            if(_current == 2)
            {
                _current = 3;
                return true;
            }                

            _current += GetNextStep();

            while(_current < long.MaxValue )
            {                
                if(!HasDeviders(_current))
                {
                    _factors.Add(_current);
                    return true;
                }

                _current += GetNextStep();                       
            }

            return false;
        }

        private bool HasDeviders(long current)
        {
            foreach (long factor in _factors)
            {
                if(current % factor == 0)
                    return true;
            }

            return false;
        }

        private int GetNextStep()
        {
            return (_step++) switch
            {                
                0 => 2,
                int x when x.IsEven()=> 4,
                _ => 2
            };
        }

        public void Reset()
        {
            _current = 0;
            _step = 0;
            _factors = new List<long> { 2, 3};
        }
    }
}