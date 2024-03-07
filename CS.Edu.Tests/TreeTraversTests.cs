using System.Linq;
using CS.Edu.Core;
using CS.Edu.Core.Extensions;
using FluentAssertions;
using Xunit;

namespace CS.Edu.Tests;

public class TreeTraversTests
{
    [Fact]
    public void PreOrderTest()
    {
        var tree = new TreeNode<int>(1,
            new TreeNode<int>(2,
                new TreeNode<int>(4),
                new TreeNode<int>(5)),
            new TreeNode<int>(3,
                new TreeNode<int>(6)));

        tree.SequencePreOrder()
            .Should()
            .BeEquivalentTo([1, 2, 4, 5, 3, 6]);
    }

    [Fact]
    public void LevelOrderTest()
    {
        var tree = new TreeNode<int>(1,
            new TreeNode<int>(2,
                new TreeNode<int>(4),
                new TreeNode<int>(5)),
            new TreeNode<int>(3,
                new TreeNode<int>(6)));

        tree.SequenceLevelOrder()
            .Should()
            .BeEquivalentTo([1, 2, 3, 4, 5, 6]);

        // System.Interactive.Expand is level order sequence
        tree.SequenceLevelOrder()
            .Should()
            .BeEquivalentTo(EnumerableEx.Return(tree)
                .Expand(x => x.Children).Select(x => x.Value));
    }

    [Fact]
    public void PostOrderTest()
    {
        var tree = new TreeNode<int>(1,
            new TreeNode<int>(2,
                new TreeNode<int>(4),
                new TreeNode<int>(5)),
            new TreeNode<int>(3,
                new TreeNode<int>(6)));

        tree.SequencePostOrder()
            .Should()
            .BeEquivalentTo([4, 5, 2, 6, 3, 1]);
    }
}