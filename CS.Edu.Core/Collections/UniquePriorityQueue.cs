using System.Collections;
using System.Collections.Generic;

namespace CS.Edu.Core.Collections;

/// <summary>
/// Упорядоченная очередь с проверкой на уникальность
/// через SortedSet
/// </summary>
public class UniquePriorityQueue<T> : IEnumerable<T>
{
    private readonly SortedSet<T> _set;

    public UniquePriorityQueue(IComparer<T> comparer = null)
    {
        _set = new SortedSet<T>(comparer);
    }

    public UniquePriorityQueue(IEnumerable<T> items, IComparer<T> comparer = null)
    {
        _set = new SortedSet<T>(items, comparer);
    }

    public void Enqueue(T item) => _set.Add(item);

    public bool TryDequeue(out T item)
    {
        if (_set.Count == 0)
        {
            item = default;
            return false;
        }

        item = _set.Min;
        _set.Remove(item);
        return true;
    }

    public IEnumerator<T> GetEnumerator()
    {
        while (TryDequeue(out T item))
        {
            yield return item;
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}