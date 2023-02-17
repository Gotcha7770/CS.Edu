using System.Collections.Generic;
using System.Linq;

namespace CS.Edu.Core.Monads;

public class Group<TKey, T> : Either<Group<TKey, T>[], T[]>
{
    public Group(TKey key, IEnumerable<Group<TKey, T>> children) : base(children.ToArray())
    {
        Key = key;
    }

    public Group(TKey key, IEnumerable<T> values) : base(values.ToArray())
    {
        Key = key;
    }

    public TKey Key { get; }
}