using System.Collections.Generic;
using DynamicData;

namespace CS.Edu.Core.Extensions
{
    public static class Source
    {
        public static ISourceList<T> From<T>(IEnumerable<T> items)
        {
            var result = new SourceList<T>();
            result.AddRange(items);

            return result;
        }
    }
}