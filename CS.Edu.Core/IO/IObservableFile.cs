using System;

namespace CS.Edu.Core.IO;

public interface IObservableFile : IObservableEntity
{
    IObservable<long> Length { get; }
}