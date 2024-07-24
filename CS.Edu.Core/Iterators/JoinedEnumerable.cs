using System.Collections;
using System.Collections.Generic;

namespace CS.Edu.Core.Iterators;

public class JoinedEnumerable<T> : IEnumerable<T>
{
    public readonly IEnumerable<T> Source;
    public bool IsOuter { get; }

    public JoinedEnumerable(IEnumerable<T> source, bool isOuter) { Source = source; IsOuter = isOuter; }

    IEnumerator<T> IEnumerable<T>.GetEnumerator() { return Source.GetEnumerator(); }
    IEnumerator IEnumerable.GetEnumerator() { return Source.GetEnumerator(); }
}