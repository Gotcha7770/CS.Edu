using System;

namespace CS.Edu.Core.Monads
{
    public class Either<TL, TR>
    {
        private readonly TL _left;
        private readonly TR _right;

        public Either(TL left)
        {
            _left = left;
            IsLeft = true;
        }

        public Either(TR right) => _right = right;

        public T Match<T>(Func<TL, T> leftFunc, Func<TR, T> rightFunc) => IsLeft
            ? leftFunc(_left)
            : rightFunc(_right);

        public void Match(Action<TL> leftFunc, Action<TR> rightFunc)
        {
            if (IsLeft)
                leftFunc(_left);
            else
                rightFunc(_right);
        }

        public bool IsLeft { get; }

        public static Either<TL, TR> Left(TL left) => new Either<TL, TR>(left);

        public static Either<TL, TR> Right(TR right) => new Either<TL, TR>(right);

        public static implicit operator Either<TL, TR>(TL left) => new Either<TL, TR>(left);

        public static implicit operator Either<TL, TR>(TR right) => new Either<TL, TR>(right);
    }
}