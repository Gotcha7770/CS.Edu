using System;
using System.Threading;
using System.Threading.Tasks;
using CS.Edu.Core.Extensions;
using CS.Edu.Core.Monads;
using CS.Edu.Tests.Utils;
using NUnit.Framework;

namespace CS.Edu.Tests.Extensions
{
    [TestFixture]
    public class TasksTests
    {
        [Test]
        public async Task TestWithTimout_Task()
        {
            bool isFaulted = false;
            var task = Task.FromResult(1);

            var result = await task.WithTimeout(10, _ => isFaulted = true);

            Assert.AreEqual(1, result);
            Assert.IsFalse(isFaulted);
        }

        [Test]
        public async Task TestWithTimout_Timout()
        {
            var task = Task.Run(() =>
            {
                Thread.Sleep(100);
                return 1;
            });

            var result = await task.WithTimeout(10, Actions.Empty<Task<int>>())
                //.ContinueWith(x => x.IsFaulted ? 0 : x.Result);
                //.ContinueWith(x => x.IsFaulted ? Optional<int>.None : Optional.Some(x.Result));
                .ContinueWith(x => x.IsFaulted ? Either<Exception, int>.Left(task.Exception) : x.Result);

            // Assert.AreNotEqual(1, result);
            EitherAssert.Left(result);
        }

        [Test]
        public async Task TestWithTimout_TimoutTwice()
        {
            var task = Task.Run(() =>
            {
                Thread.Sleep(1000);
                return 1;
            });

            await task.WithTimeout(10, x => x.WithTimeout(10, _ => Assert.Pass()));
        }
    }
}