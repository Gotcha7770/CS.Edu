using System;
using DynamicData;

namespace CS.Edu.Core.IO;

public interface IObservableDirectory : IObservableEntity, IObservable<IChangeSet<string, string>>
{

}