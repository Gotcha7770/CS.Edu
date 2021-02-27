using System.Collections.Generic;

namespace CS.Edu.Core.Monads
{
    public class FindResult<T> : Either<IEnumerable<T>, T>
    {
        public FindResult(IEnumerable<T> left) : base(left) { }

        public FindResult(T right) : base(right) { }
    }
}