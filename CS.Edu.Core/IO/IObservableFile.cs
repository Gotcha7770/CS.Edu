using System;

namespace CS.Edu.Core.IO;

public interface IObservableFile : IObservableEntry
{
    IObservable<long> Length { get; }
}