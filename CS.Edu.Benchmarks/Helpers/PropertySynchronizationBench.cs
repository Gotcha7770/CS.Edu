using System;
using System.ComponentModel;
using System.Reactive.Linq;
using System.Runtime.CompilerServices;
using BenchmarkDotNet.Attributes;
using CS.Edu.Core.Extensions;
using CS.Edu.Core.Helpers;
using CS.Edu.Core.Interfaces;

namespace CS.Edu.Benchmarks.PropertySynchronization
{
    class TestClass : INotifyPropertyChanged
    {
        private string _value;

        public string Value
        {
            get => _value;
            set
            {
                if (_value != value)
                {
                    _value = value;
                    RaisePropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    [MemoryDiagnoser]
    [Config(typeof(DefaultConfig))]
    public class PropertySynchronizationBench
    {
        IPropertySynchronizer<string> _synchronizer = new PropertySynchronizer<string>();
        string _propertyName = nameof(TestClass.Value);

        TestClass _source;
        TestClass _target;
        ISynchronizationContext<string> _sourceContext;
        ISynchronizationContext<string> _targetContext;
        IDisposable _disposable;

        [GlobalSetup(Target = nameof(ReflectionSync))]
        public void ReflectionSyncSetup()
        {
            _source = new TestClass();
            _sourceContext = new ReflectionSyncContext<string>(_source, _propertyName);
            _target = new TestClass { Value = "initialValue" };
            _targetContext = new ReflectionSyncContext<string>(_target, _propertyName);
        }

        [GlobalSetup(Target = nameof(DelegateSync))]
        public void DelegateSyncSetup()
        {
            _source = new TestClass();
            _sourceContext = new DelegateSyncContext<TestClass, string>(_source, _propertyName);
            _target = new TestClass { Value = "initialValue" };
            _targetContext = new DelegateSyncContext<TestClass, string>(_target, _propertyName);
        }

        [GlobalSetup(Target = nameof(DirectPropertySync))]
        public void DirectPropertySyncSetup()
        {
            _source = new TestClass();
            _sourceContext = new DirectPropertySyncContext<TestClass, string>(_source,
                                                                              _propertyName,
                                                                              (c) => c.Value,
                                                                              (c, v) => c.Value = v);
            _target = new TestClass { Value = "initialValue" };
            _targetContext = new DirectPropertySyncContext<TestClass, string>(_target,
                                                                              _propertyName,
                                                                              (c) => c.Value,
                                                                              (c, v) => c.Value = v);
        }

        [GlobalSetup(Target = nameof(RxPropertySync))]
        public void RxPropertySyncSetup()
        {
            _source = new TestClass();
            _target = new TestClass { Value = "initialValue" };
        }

        [GlobalCleanup(Target = nameof(RxPropertySync))]
        public void GlobalCleanup()
        {
            _disposable.Dispose();
        }

        [Benchmark]
        public string ReflectionSync()
        {
            _synchronizer.Sync(_sourceContext, _targetContext);
            _source.Value = "newValue";

            return _target.Value;
        }

        [Benchmark]
        public string DelegateSync()
        {
            _synchronizer.Sync(_sourceContext, _targetContext);
            _source.Value = "newValue";

            return _target.Value;
        }

        [Benchmark]
        public string DirectPropertySync()
        {
            _synchronizer.Sync(_sourceContext, _targetContext);
            _source.Value = "newValue";

            return _target.Value;
        }

        [Benchmark]
        public string RxPropertySync()
        {
            _disposable = ObservableExt.CreateFromProperty(_source, x => x.Value)
                .Subscribe(x => _target.Value = x);

            _source.Value = "newValue";

            return _target.Value;
        }
    }
}