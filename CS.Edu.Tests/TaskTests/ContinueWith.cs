using System.Threading;
using System.Threading.Tasks;
using CS.Edu.Tests.Utils;
using DynamicData.Kernel;
using FluentAssertions;
using Xunit;
using static FluentAssertions.FluentActions;

namespace CS.Edu.Tests.TaskTests;

public class ContinueWith
{
    [Fact]
    public async Task ContinueWidth_CanceledTask()
    {
        Optional<int> result = default;
        Task<Optional<int>> current = Task.FromCanceled<Optional<int>>(new CancellationToken(true));

        await Invoking(() => current.ContinueWith(t => !t.IsCanceled ? 42 : default))
            .Should().NotThrowAsync();

        OptionalAssert.None(result);
    }

    [Fact]
    public async Task Await_CanceledTask()
    {
        //Prefer await over continuation???

        Optional<int> result = default;
        Task<Optional<int>> current = Task.FromCanceled<Optional<int>>(new CancellationToken(true));

        await Invoking(async () => result = await current)
            .Should().ThrowAsync<TaskCanceledException>();

        OptionalAssert.None(result);
    }
}