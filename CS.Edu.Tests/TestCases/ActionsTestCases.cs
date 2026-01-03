using System;
using CS.Edu.Core.Extensions;
using Xunit;

namespace CS.Edu.Tests.TestCases;

public class ActionsTestCases : TheoryData<Action, bool>
{
    public ActionsTestCases()
    {
        Add(Action.Idle, true);
        Add(() => { }, false);
    }
}