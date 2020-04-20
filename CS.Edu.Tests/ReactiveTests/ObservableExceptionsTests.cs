using System;
using System.Reactive.Linq;
using NUnit.Framework;

namespace CS.Edu.Tests.ReactiveTests
{
    [TestFixture]
    public class ObservableExceptionsTests
    {
        [Test]
        public void ThrowExceptionTest_WithoutOnError()
        {
            Assert.Throws<Exception>(() =>
            {
                Observable.Throw<Exception>(new Exception())
                    .Subscribe();
            });
        }

        [Test]
        public void ThrowExceptionTest_WithOnError()
        {
            object result = null;
            Exception exception = null;

            Assert.DoesNotThrow(() =>
            {
                Observable.Throw<Exception>(new Exception())
                    .Subscribe(x => result = x, 
                               ex => exception = ex);
            });

            Assert.IsNull(result);
            Assert.IsNotNull(exception);
        }
    }
}