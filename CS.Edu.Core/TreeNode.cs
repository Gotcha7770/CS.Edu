using System.Collections.Generic;

namespace CS.Edu.Core;

public class TreeNode<T>
{
    public TreeNode(T value, params TreeNode<T>[] children)
    {
        Value = value;
        Children = children;

        foreach (var child in Children)
        {
            child.Parent = this;
        }
    }

    public T Value { get; }
    public TreeNode<T> Parent { get; private set; }
    public IReadOnlyList<TreeNode<T>> Children { get; }
}