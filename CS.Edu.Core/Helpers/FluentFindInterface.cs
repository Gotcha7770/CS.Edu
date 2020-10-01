using System;
using System.Collections.Generic;
using System.Linq;
using CS.Edu.Core.Interfaces;
using DynamicData.Kernel;

namespace CS.Edu.Core.Helpers
{
    /// <summary>Текучий интерфейс для поиска по перечислению</summary>
    public static class FluentFindInterface
    {
        /// <summary>
        /// Ищет значение в перечислении, удовлетворяющее переданному предикату
        /// и возвращает результат, завернутый в интерфейс <see cref="ISearchResult{T}"/>,
        /// на основании которого можно выстраивать цепочку поиска.
        /// Первое же совпадение в цепочке предикатов возвращает значение,
        /// если ни одного совпадения не было возвращается значение типа по умолчанию.
        /// </summary>
        public static ISearchResult<T> Find<T>(this IEnumerable<T> source, Predicate<T> predicate, bool useParallel = false)
        {
            if (useParallel)
                return new ParallelSearchResult<T>(source, predicate);

            return new SerialSearchResult<T>(source, predicate);
        }

        class SerialSearchResult<T> : ISearchResult<T>
        {
            protected IEnumerable<T> Source { get; }

            public SerialSearchResult(IEnumerable<T> source, Predicate<T> predicate)
            {
                Source = source;
                Result = Optional<T>.None;

                using var enumerator = Source.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    if (predicate(enumerator.Current))
                    {
                        Result = enumerator.Current;
                        break;
                    }
                }
            }

            public ISearchResult<T> ThenFind(Predicate<T> predicate)
            {
                return Result.HasValue ? this : new SerialSearchResult<T>(Source, predicate);
            }

            public Optional<T> Result { get; }
        }

        class ParallelSearchResult<T> : ISearchResult<T>
        {
            private readonly List<Predicate<T>> _predicates;

            protected IEnumerable<T> Source { get; }

            public ParallelSearchResult(IEnumerable<T> source, Predicate<T> predicate)
            {
                Source = source ?? throw new ArgumentNullException(nameof(source));

                _predicates = predicate == null
                    ? throw new ArgumentNullException(nameof(predicate))
                    : new List<Predicate<T>> {predicate};
            }

            public ISearchResult<T> ThenFind(Predicate<T> predicate)
            {
                _predicates.Add(predicate);
                return this;
            }

            public Optional<T> Result
            {
                get
                {
                    var results = new Optional<T>[_predicates.Count];
                    using var enumerator = Source.GetEnumerator();
                    while (enumerator.MoveNext())
                    {
                        var current = enumerator.Current;
                        if (_predicates[0](current))
                        {
                            return current;
                        }

                        for (int i = 1; i < _predicates.Count; i++)
                        {
                            if (!results[i - 1].HasValue && _predicates[i](current))
                                results[i - 1] = current;
                        }
                    }

                    return results.FirstOrDefault(x => x.HasValue);
                }
            }
        }
    }
}