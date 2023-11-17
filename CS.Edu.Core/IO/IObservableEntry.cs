using System;

namespace CS.Edu.Core.IO;

public interface IObservableEntry : IDisposable
{
    IObservable<string> FullPath { get; }
    IObservable<string> Name { get; }
    IObservable<DateTime> LastWriteTime { get; }
}