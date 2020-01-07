using System.ComponentModel;
using System.Runtime.CompilerServices;
using CS.Edu.Core.Helpers;
using CS.Edu.Core.Interfaces;
using NUnit.Framework;

namespace CS.Edu.Tests.HelpersTests
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

    [TestFixture]
    public class PropertySynchronizerTests
    {
        string _propertyName = nameof(TestClass.Value);
        IPropertySynchronizer<string> _synchronizer = new PropertySynchronizer<string>();

        [Test]
        public void ReflectionSyncTest_TargetValueBecomesEqualToSource()
        {
            var source = new TestClass();
            var target = new TestClass { Value = "initialValue" };

            _synchronizer.Sync(source, target, _propertyName);
            source.Value = "newValue";

            Assert.That(target.Value, Is.EqualTo(source.Value));
        }

        [Test]
        public void DelegateSyncTest_TargetValueBecomesEqualToSource()
        {
            var source = new TestClass();
            var sourceContext = new DelegateSyncContext<TestClass, string>(source, _propertyName);
            var target = new TestClass { Value = "initialValue" };
            var targetContext = new DelegateSyncContext<TestClass, string>(target, _propertyName);

            _synchronizer.Sync(sourceContext, targetContext);
            source.Value = "newValue";

            Assert.That(target.Value, Is.EqualTo(source.Value));
        }

        [Test]
        public void DirectPropertySyncTest_TargetValueBecomesEqualToSource()
        {
            var source = new TestClass();
            var sourceContext = new DirectPropertySyncContext<TestClass, string>(source,
                                                                                 _propertyName,
                                                                                 (c) => c.Value,
                                                                                 (c, v) => c.Value = v);
            var target = new TestClass { Value = "initialValue" };
            var targetContext = new DirectPropertySyncContext<TestClass, string>(target,
                                                                                 _propertyName,
                                                                                 (c) => c.Value,
                                                                                 (c, v) => c.Value = v);

            _synchronizer.Sync(sourceContext, targetContext);
            source.Value = "newValue";

            Assert.That(target.Value, Is.EqualTo(source.Value));
        }

        [Test]
        public void SyncDisposingTest_NoValueSynchronizationAfterDisposing()
        {
            var source = new TestClass();
            var target = new TestClass { Value = "initialValue" };

            using (_synchronizer.Sync(source, target, nameof(TestClass.Value)))
            {
                source.Value = "syncValue";
            }

            source.Value = "desyncValue";

            Assert.That(target.Value, Is.EqualTo("syncValue"));
        }

        [Test]
        public void OneWaySyncTest_TargetValueBecomesEqualToSourceButNotViceVersa()
        {
            var source = new TestClass();
            var target = new TestClass { Value = "initialValue" };

            _synchronizer.Sync(source, target, _propertyName, SyncMode.OneWay);

            source.Value = "syncValue";
            Assert.That(target.Value, Is.EqualTo("syncValue"));

            target.Value = "newTargetValue";
            Assert.That(source.Value, Is.EqualTo("syncValue"));
        }

        [Test]
        public void OneWayToSourceSyncTest_SourceValueBecomesEqualToTargetButNotViceVersa()
        {
            var source = new TestClass();
            var target = new TestClass { Value = "initialValue" };

            _synchronizer.Sync(source, target, _propertyName, SyncMode.OneWayToSource);

            source.Value = "syncValue";
            Assert.That(target.Value, Is.EqualTo("initialValue"));

            target.Value = "newTargetValue";
            Assert.That(source.Value, Is.EqualTo("newTargetValue"));
        }
    }
}