using System;
using NUnit.Framework;
using CS.Edu.Tests.Utils;
using CS.Edu.Core.Extensions;

namespace CS.Edu.Tests.Extensions
{
    [TestFixture]
    public class ObservableExtTests
    {
        [Test]
        public void ObservableFromPropertyTest()
        {
            var source = new TestClass();
            var target = new TestClass { Value = "initialValue" };

            ObservableExt.CreateFromProperty(source, x => x.Value)
                .Subscribe(x => target.Value = x);

            source.Value = "newValue";

            Assert.That(target.Value, Is.EqualTo(source.Value));
        }
    }
}