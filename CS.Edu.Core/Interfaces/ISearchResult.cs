using System;
using DynamicData.Kernel;

namespace CS.Edu.Core.Interfaces
{
    /// <summary>Интерфейс описывающий результат поиска</summary>
    public interface ISearchResult<T>
    {
        /// <summary>
        /// Выполняет поиск по новому условию,
        /// если ни один из предыдущих этапов не дал результата.
        /// </summary>
        /// <param name="predicate">условие</param>
        ISearchResult<T> ThenFind(Predicate<T> predicate);

        /// <summary>Значение, определяющее, был ли найден искомый объект</summary>
        //bool IsFound { get; }

        /// <summary>
        /// Результат поиска,
        /// искомый объект или значение типа по умолчанию
        /// </summary>
        Optional<T> Result { get; }
    }
}