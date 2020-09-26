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
        /// и возвращает результат, завернутый в интерфейс <see cref="T:ExtLib.IFindResult`1" />,
        /// на основании которого можно выстраивать цепочку поиска.
        /// Первое же совпадение в цепочке предикатов возвращает значение,
        /// если ни одного совпадения не было возвращается значение типа по умолчанию.
        /// </summary>
        public static ISearchResult<T> Find<T>(this IEnumerable<T> source, Predicate<T> predicate)
        {
            return new SerialSearchResult<T>(source, predicate);
        }

        class SerialSearchResult<T> : ISearchResult<T>
        {
            protected IEnumerable<T> Source { get; }

            public SerialSearchResult(IEnumerable<T> source, Predicate<T> predicate)
            {
                Source = source;
                Result = source.FirstOrOptional(x => predicate(x));
                //IsFound = !EqualityComparer<T>.Default.Equals(Result, default); //???
            }

            public ISearchResult<T> ThenFind(Predicate<T> predicate)
            {
                return Result.HasValue ? this : new SerialSearchResult<T>(Source, predicate);
            }

            //public bool IsFound { get; }

            public Optional<T> Result { get; }
        }

        class ParallelSearchResult<T> : ISearchResult<T>
        {
            private readonly List<Predicate<T>> _predicates;
            private Optional<T>[] _results;

            protected IEnumerable<T> Source { get; }

            public ParallelSearchResult(IEnumerable<T> source, Predicate<T> predicate)
            {
                Source = source ?? throw new ArgumentNullException(nameof(source));

                _predicates = predicate == null
                    ? throw new ArgumentNullException(nameof(predicate))
                    : new List<Predicate<T>> {predicate};
                //Result = source.FirstOrOptional(x => predicate(x));
                //_queue.Enqueue(source.FirstOrOptional(x => predicate(x)));
            }

            public ISearchResult<T> ThenFind(Predicate<T> predicate)
            {
                //Result = source.FirstOrOptional(x => predicate(x));
                _predicates.Add(predicate);
                return this;
            }

            //public bool IsFound { get; }

            public Optional<T> Result
            {
                get
                {
                    _results = new Optional<T>[_predicates.Count - 1];

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
                            _results[i - 1] = _predicates[i](current) ? current : Optional<T>.None;
                        }
                    }

                    return _results.FirstOrDefault(x => x.HasValue);
                }
            }
        }
    }
}