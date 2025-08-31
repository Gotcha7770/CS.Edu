using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CS.Edu.Core.Collections;

/// <summary>
/// Упорядоченная очередь с проверкой на уникальность
/// через PriorityQueue
/// <remarks>
/// В этой реализации используется допущение, что все одинаковые элементы
/// вставляются примерно на одном этапе и не могут появиться позднее.
/// Таким образом все одинаковые элементы будут лежать в очереди подряд.
/// </remarks>
/// </summary>
public class CollapsingPriorityQueue<T> : IEnumerable<T>
{
    private readonly PriorityQueue<T, T> _queue;
    private readonly IComparer<T> _comparer;

    public CollapsingPriorityQueue(IComparer<T> comparer = null)
    {
        _comparer = comparer ?? Comparer<T>.Default;
        _queue = new PriorityQueue<T, T>();
    }

    public CollapsingPriorityQueue(IEnumerable<T> items, IComparer<T> comparer = null)
    {
        _comparer = comparer ?? Comparer<T>.Default;
        _queue = new PriorityQueue<T, T>(items.Select(x => (x, x)));
    }

    public void Enqueue(T item) => _queue.Enqueue(item, item);

    public bool TryDequeue(out T item)
    {
        if (_queue.TryDequeue(out item, out _))
        {
            while (_queue.Count > 0 && _comparer.Compare(_queue.Peek(), item) == 0)
                _queue.Dequeue();

            return true;
        }

        return false;
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
