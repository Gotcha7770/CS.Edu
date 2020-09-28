using System;
using System.ComponentModel;
using System.Reactive.Disposables;
using System.Runtime.CompilerServices;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using CS.Edu.Core.Extensions;
using CS.Edu.Core.Helpers;
using CS.Edu.Core.Interfaces;

namespace CS.Edu.Benchmarks.Helpers
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
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    [InvocationCount(1000)]
    [Config(typeof(DefaultConfig))]
    public class PropertySynchronizationBench
    {
        IPropertySynchronizer<string> _synchronizer = new PropertySynchronizer<string>();
        string _propertyName = nameof(TestClass.Value);

        TestClass _source;
        TestClass _target;
        IDisposable _disposable;

        [GlobalSetup]
        public void RxPropertySyncSetup()
        {
            _source = new TestClass();
            _target = new TestClass { Value = "initialValue" };
        }

        [IterationCleanup]
        public void Cleanup()
        {
            _disposable.Dispose();
        }

        [Benchmark]
        public string ReflectionSync()
        {
            var sourceContext = new ReflectionSyncContext<string>(_source, _propertyName);
            var targetContext = new ReflectionSyncContext<string>(_target, _propertyName);

            _disposable = new CompositeDisposable
            {
                _synchronizer.Sync(sourceContext, targetContext),
                sourceContext,
                targetContext
            };

            _source.Value = "newValue";

            return _target.Value;
        }

        [Benchmark]
        public string DelegateSync()
        {
            var sourceContext = new DelegateSyncContext<TestClass, string>(_source, _propertyName);
            var targetContext = new DelegateSyncContext<TestClass, string>(_target, _propertyName);

            _disposable = new CompositeDisposable
            {
                _synchronizer.Sync(sourceContext, targetContext),
                sourceContext,
                targetContext
            };

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