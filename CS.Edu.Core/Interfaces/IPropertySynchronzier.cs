using System;
using System.Collections.Generic;
using System.ComponentModel;
using CS.Edu.Core.Helpers;

namespace CS.Edu.Core.Interfaces
{
    public enum SyncMode
    {
        OneWay,
        TwoWay,
        OneWayToSource
    }

    public interface IPropertySynchronizer<T>
    {
        IDisposable Sync(INotifyPropertyChanged source,
                         INotifyPropertyChanged target,
                         string propertyName,
                         SyncMode syncMode = SyncMode.TwoWay);

        IDisposable Sync(INotifyPropertyChanged source,
                         IEnumerable<INotifyPropertyChanged> targets,
                         string propertyName,
                         SyncMode syncMode = SyncMode.TwoWay);

        IDisposable Sync(ISynchronizationContext<T> sourceContext,
                         ISynchronizationContext<T> targetContext,
                         SyncMode syncMode = SyncMode.TwoWay);

        IDisposable Sync(ISynchronizationContext<T> sourceContext,
                         IEnumerable<ISynchronizationContext<T>> targetContexts,
                         SyncMode syncMode = SyncMode.TwoWay);
    }
}