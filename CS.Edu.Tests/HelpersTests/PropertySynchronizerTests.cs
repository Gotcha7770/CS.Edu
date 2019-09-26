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

        [Test]
        public void ReflectionSyncTest_TargetValueBecomesEqualToSourceValue()
        {
            var source = new TestClass();
            var target = new TestClass { Value = "initialValue" };
            IPropertySynchronizer<string> synchronizer = new PropertySynchronizer<string>();

            synchronizer.Sync(source, target, _propertyName);
            source.Value = "newValue";

            Assert.That(target.Value, Is.EqualTo(source.Value));
        }

        [Test]
        public void DelegateSyncTest_TargetValueBecomesEqualToSourceValue()
        {
            var source = new TestClass();
            var sourceContext = new DelegateSyncContext<TestClass, string>(source, _propertyName);
            var target = new TestClass { Value = "initialValue" };
            var targetContext = new DelegateSyncContext<TestClass, string>(target, _propertyName);
            IPropertySynchronizer<string> synchronizer = new PropertySynchronizer<string>();

            synchronizer.Sync(sourceContext, targetContext);
            source.Value = "newValue";

            Assert.That(target.Value, Is.EqualTo(source.Value));
        }

        [Test]
        public void DirectPropertySyncTest_TargetValueBecomesEqualToSourceValue()
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
            IPropertySynchronizer<string> synchronizer = new PropertySynchronizer<string>();

            synchronizer.Sync(sourceContext, targetContext);
            source.Value = "newValue";

            Assert.That(target.Value, Is.EqualTo(source.Value));
        }

        [Test]
        public void SyncDisposingTest_NoValueSynchronizationAfterDisposing()
        {
            var source = new TestClass();
            var target = new TestClass { Value = "initialValue" };
            IPropertySynchronizer<string> synchronizer = new PropertySynchronizer<string>();

            using (synchronizer.Sync(source, target, nameof(TestClass.Value)))
            {
                source.Value = "syncValue";
            }

            source.Value = "desyncValue";

            Assert.That(target.Value, Is.EqualTo("syncValue"));
        }
    }
}