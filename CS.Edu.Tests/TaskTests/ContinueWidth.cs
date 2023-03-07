using System.Threading;
using System.Threading.Tasks;
using CS.Edu.Tests.Utils;
using DynamicData.Kernel;
using NUnit.Framework;

namespace CS.Edu.Tests.TaskTests;

public class ContinueWidth
{
    [Test]
    public void ContinueWidth_CanceledTask()
    {
        Optional<int> result = default;
        Task<Optional<int>> current = Task.FromCanceled<Optional<int>>(new CancellationToken(true));

        Assert.DoesNotThrowAsync(async () =>
        {
            result = await current.ContinueWith(t => !t.IsCanceled ? 42 : Optional<int>.None);
        });

        OptionalAssert.None(result);
    }

    [Test]
    public async Task Await_CanceledTask()
    {
        //Prefer await over continuation???

        Optional<int> result = default;
        Task<Optional<int>> current = Task.FromCanceled<Optional<int>>(new CancellationToken(true));

        Assert.ThrowsAsync<TaskCanceledException>(async () => result = await current);

        OptionalAssert.None(result);
    }
}