using System;

namespace CS.Edu.Core.IO;

public interface IObservableEntity : IDisposable
{
    IObservable<string> FullPath { get; }
    IObservable<string> Name { get; }
}