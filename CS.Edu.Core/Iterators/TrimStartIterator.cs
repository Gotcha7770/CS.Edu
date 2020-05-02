using System.Collections.Generic;

namespace CS.Edu.Core.Iterators
{
    public sealed class TrimStartIterator<T> : Iterator<T>
        {
            private readonly T _valueToSkip;
            private IEnumerator<T> _enumerator;

            public TrimStartIterator(IEnumerable<T> source, T valueToSkip)
                : base(source)
            {
                _valueToSkip = valueToSkip;
            }

            public bool SkipStart()
            {
                var comparer = EqualityComparer<T>.Default;

                switch (_state)
                {
                    case 1:
                        _enumerator = _source.GetEnumerator();
                        _state = 2;
                        goto case 2;
                    case 2:
                        if (_enumerator.MoveNext())
                        {
                            Current = _enumerator.Current;
                            if(comparer.Equals(Current, _valueToSkip))
                            {
                                return true;
                            }

                            _state = 3;
                            return false;
                        }

                        Dispose();
                        break;
                }

                return false;
            }

            public override bool MoveNext()
            {
                switch (_state)
                {
                    case 1:
                    case 2:
                        while (SkipStart()) { }
                        if (_state != 3)
                            break;
                        goto case 3;
                    case 3:
                        _state = 4;
                        return true;
                    case 4:
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

            public override void Dispose()
            {
                _state = -1;
                Current = default;
                _enumerator.Dispose();
            }
        }
}