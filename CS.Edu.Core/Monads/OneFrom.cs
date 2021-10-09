using System.Collections.Generic;

namespace CS.Edu.Core.Monads
{
    public class OneFrom<T> : Either<IEnumerable<T>, T>
    {
        public OneFrom(IEnumerable<T> left) : base(left) { }

        public OneFrom(T right) : base(right) { }
    }
}