using Xunit;

namespace CS.Edu.Tests.TestCases;

public class ThenFindTestCases : TheoryData<int[], int>
{
    public ThenFindTestCases()
    {
        Add([1, 2, 3, 5, 10, 11], 10);
        Add([1, 2, 3, 5, 11], 5);
        Add([1, 2, 3, 6, 11], 3);
        Add([1, 2, 4, 11], 2);
    }
}