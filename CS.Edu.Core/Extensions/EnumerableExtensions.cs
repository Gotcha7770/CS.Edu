using System;
using System.Collections;
using System.Collections.Generic;

namespace CS.Edu.Core.Extensions;

public static partial class EnumerableExtensions
{
    public static IEnumerable OfType(this IEnumerable source, GenericType constraint)
    {
        foreach (var item in source)
        {
            if (item.IsSubclassOf(constraint))
            {
                yield return item;
            }
        }
    }

    /// <summary>
    /// Сокращает последовательные вхождения конкретного элемента в последовательности до одного
    /// </summary>
    public static IEnumerable<T> ShrinkDuplicates<T>(this IEnumerable<T> source, T value)
    {
        if (source == null)
            throw new ArgumentNullException(nameof(source));

        return ShrinkDuplicatesIterator(source, value);
    }

    static IEnumerable<T> ShrinkDuplicatesIterator<T>(IEnumerable<T> source, T value)
    {
        var comparer = EqualityComparer<T>.Default;
        bool skipping = false;

        foreach (T item in source)
        {
            if (!comparer.Equals(item, value))
            {
                skipping = false;
                yield return item;
            }
            else if (!skipping)
            {
                skipping = true;
                yield return item;
            }
        }
    }

    /// <summary>
    /// Сокращает последовательные вхождения элементов возвращающих одниковые значения
    /// в последовательности в соответствии с переданной функцией до одного
    /// </summary>
    public static IEnumerable<TValue> ShrinkDuplicates<TKey, TValue>(this IEnumerable<TValue> source,
        Func<TValue, TKey> keySelector,
        TKey value)
    {
        if (source == null)
            throw new ArgumentNullException(nameof(source));
        if (keySelector == null)
            throw new ArgumentNullException(nameof(keySelector));

        return ShrinkDuplicatesIterator(source, keySelector, value);
    }

    static IEnumerable<TValue> ShrinkDuplicatesIterator<TKey, TValue>(IEnumerable<TValue> source,
        Func<TValue, TKey> keySelector,
        TKey value)
    {
        var comparer = EqualityComparer<TKey>.Default;
        bool skipping = false;

        foreach (TValue item in source)
        {
            if (!Equals(keySelector(item), value))
            {
                skipping = false;
                yield return item;
            }
            else if (!skipping)
            {
                skipping = true;
                yield return item;
            }
        }
    }
}