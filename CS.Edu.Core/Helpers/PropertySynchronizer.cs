using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Disposables;
using System.Reflection;
using CS.Edu.Core.Interfaces;

namespace CS.Edu.Core.Helpers
{
    public interface ISynchronizationContext<T> : IObservable<T>, IObserver<T>
    {
        INotifyPropertyChanged Source { get; }
    }

    public abstract class PropertySyncContextBase<T> : ISynchronizationContext<T>
    {
        protected readonly Dictionary<IObserver<T>, PropertyChangedEventHandler> Observers
            = new Dictionary<IObserver<T>, PropertyChangedEventHandler>();

        public virtual void Dispose()
        {
            foreach (var handler in Observers.Values)
            {
                Source.PropertyChanged -= handler;
            }
        }

        public virtual INotifyPropertyChanged Source { get; protected set; }

        public virtual string PropertyName { get; protected set; }

        public virtual void OnCompleted()
        {

        }

        public virtual void OnError(Exception error)
        {

        }

        public virtual void OnNext(T value)
        {
            SetValue(value);
        }

        public virtual IDisposable Subscribe(IObserver<T> observer)
        {
            if (Observers.ContainsKey(observer))
                return Disposable.Empty; //???

            PropertyChangedEventHandler handler = (s, e) =>
            {
                if (e.PropertyName == PropertyName)
                    observer.OnNext(GetValue());
            };

            Source.PropertyChanged += handler;
            Observers[observer] = handler;

            return Disposable.Create(() => Source.PropertyChanged -= handler);
        }

        protected abstract T GetValue();

        protected abstract void SetValue(T value);
    }

    public class ReflectionSyncContext<T> : PropertySyncContextBase<T>
    {
        private const BindingFlags Flags = BindingFlags.Public | BindingFlags.Instance;
        private readonly Type _type;
        private readonly PropertyInfo _propertyInfo;

        public ReflectionSyncContext(INotifyPropertyChanged source, string propertyName)
        {
            Source = source;
            PropertyName = propertyName;
            _type = source.GetType();
            _propertyInfo = _type.GetProperty(PropertyName, Flags);
        }

        protected override T GetValue()
        {
            return (T)_propertyInfo.GetValue(Source);
        }

        protected override void SetValue(T value)
        {
            _propertyInfo.SetValue(Source, value);
        }
    }

    public class DelegateSyncContext<TComponent, TProperty> : PropertySyncContextBase<TProperty>
    {
        private static readonly Type _componentType;
        private static readonly Type _propertyType;

        private readonly Func<TComponent, TProperty> _getter;
        private readonly Action<TComponent, TProperty> _setter;

        static DelegateSyncContext()
        {
            _componentType = typeof(TComponent);
            _propertyType = typeof(TProperty);
        }

        public DelegateSyncContext(INotifyPropertyChanged source, string propertyName)
        {
            Source = source;
            PropertyName = propertyName;

            PropertyInfo propInfo = _componentType.GetProperty(PropertyName);
            MethodInfo[] methods = propInfo.GetAccessors();

            _getter = CreateDelegate<Func<TComponent, TProperty>>(methods[0]);
            _setter = CreateDelegate<Action<TComponent, TProperty>>(methods[1]);
        }

        private T CreateDelegate<T>(MethodInfo methodInfo) where T : Delegate
        {
            return (T)Delegate.CreateDelegate(typeof(T), methodInfo);
        }

        protected override TProperty GetValue()
        {
            return _getter((TComponent)Source);
        }

        protected override void SetValue(TProperty value)
        {
            _setter((TComponent)Source, value);
        }
    }

    public class DirectPropertySyncContext<TComponent, TProperty>
        : PropertySyncContextBase<TProperty>
    {
        private readonly DirectPropertyDescriptor<TComponent, TProperty> _propertyDescriptor;
        private string propertyName;

        public DirectPropertySyncContext(INotifyPropertyChanged source,
                                         string propertyName,
                                         Func<TComponent, TProperty> getter,
                                         Action<TComponent, TProperty> setter)
        {
            Source = source;
            PropertyName = propertyName;
            _propertyDescriptor = new DirectPropertyDescriptor<TComponent, TProperty>(PropertyName,
                                                                                      getter,
                                                                                      setter);
        }

        protected override TProperty GetValue()
        {
            return (TProperty)_propertyDescriptor.GetValue(Source);
        }

        protected override void SetValue(TProperty value)
        {
            _propertyDescriptor.SetValue(Source, value);
        }
    }

    public class PropertySynchronizer<T> : IPropertySynchronizer<T>
    {
        public IDisposable Sync(INotifyPropertyChanged source,
                                INotifyPropertyChanged target,
                                string propertyName,
                                SyncMode syncMode = SyncMode.TwoWay)
        {
            var sourceContext = new ReflectionSyncContext<T>(source, propertyName);
            var targetContext = new ReflectionSyncContext<T>(target, propertyName);

            return Sync(sourceContext, targetContext, syncMode);
        }

        public IDisposable Sync(ISynchronizationContext<T> sourceContext,
                                ISynchronizationContext<T> targetContext,
                                SyncMode syncMode = SyncMode.TwoWay)
        {
            var disposable = new CompositeDisposable();

            switch (syncMode)
            {
                case SyncMode.OneWay:
                    disposable.Add(sourceContext.Subscribe(targetContext));
                    break;
                case SyncMode.TwoWay:
                    disposable.Add(targetContext.Subscribe(sourceContext));
                    disposable.Add(sourceContext.Subscribe(targetContext));
                    break;
                case SyncMode.OneWayToSource:
                    disposable.Add(targetContext.Subscribe(sourceContext));
                    break;
            }
            
            return disposable;
        }

        public IDisposable Sync(INotifyPropertyChanged source,
                                IEnumerable<INotifyPropertyChanged> targets,
                                string propertyName,
                                SyncMode syncMode = SyncMode.TwoWay)
        {
            var sourceContext = new ReflectionSyncContext<T>(source, propertyName);
            var targetContexts = targets.Select(x => new ReflectionSyncContext<T>(x, propertyName));         
            
            return Sync(sourceContext, targetContexts, syncMode);
        }

        public IDisposable Sync(ISynchronizationContext<T> sourceContext,
                                IEnumerable<ISynchronizationContext<T>> targetContexts,
                                SyncMode syncMode = SyncMode.TwoWay)
        {
            var disposable = new CompositeDisposable();
            foreach (var targetContext in targetContexts)
            {
                disposable.Add(Sync(sourceContext, targetContext, syncMode));
            }

            return disposable;
        }
    }
}