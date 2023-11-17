using System;
using DynamicData;

namespace CS.Edu.Core.IO;

public interface IObservableDirectory : IObservableEntry, IObservable<IChangeSet<string, string>>
{

}