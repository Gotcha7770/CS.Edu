using System;
using System.Collections.Generic;
using System.Text;

namespace CS.Edu.Core.Extensions
{
    public static class ListExt
    {
        public static IEnumerable<IEnumerable<TSource>> Split<TSource>(this List<TSource> source, int count)
        {
            for (int i = 0; i < source.Count; i += count)
            {
                yield return source.GetRange(i, Math.Min(count, source.Count - i));
            }
        }
    }
}
