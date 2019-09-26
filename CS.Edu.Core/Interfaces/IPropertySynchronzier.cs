using System;
using System.ComponentModel;
using CS.Edu.Core.Helpers;

namespace CS.Edu.Core.Interfaces
{
    public interface IPropertySynchronizer<T>
    {
        IDisposable Sync(INotifyPropertyChanged source, INotifyPropertyChanged target, string propertyName);

        IDisposable Sync(INotifyPropertyChanged source, INotifyPropertyChanged[] targets, string propertyName);

        IDisposable Sync(ISynchronizationContext<T> sourceContext, ISynchronizationContext<T> targetContext);

        IDisposable Sync(ISynchronizationContext<T> sourceContext, ISynchronizationContext<T>[] targetContexts);
    }
}