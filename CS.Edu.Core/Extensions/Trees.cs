using System;
using System.Collections.Generic;

namespace CS.Edu.Core.Extensions;

public static class Trees
{
    /// <summary>
    /// Pre-order Traversal:
    /// - Visit the root node.
    /// - Recursively traverse each subtree.
    /// </summary>
    /// <remarks>
    ///     1
    ///    /\
    ///   2  3
    ///  /\   \
    /// 4 5    6
    /// turns into [1, 2, 4, 5, 3, 6]
    /// </remarks>
    /// <param name="root"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns>flat sequence of T values</returns>
    public static IEnumerable<T> SequencePreOrder<T>(this TreeNode<T> root)
    {
        var stack = new Stack<TreeNode<T>>();
        stack.Push(root);

        while (stack.Count > 0)
        {
            var node = stack.Pop();
            yield return node.Value;

            // Push children in reverse order to maintain correct order during traversal
            for (int i = node.Children.Count - 1; i >= 0; i--)
            {
                stack.Push(node.Children[i]);
            }
        }
    }

    public static IEnumerable<TResult> TraversePreOrder<T, TResult>(this TreeNode<T> root, Func<T, TResult> selector)
    {
        var stack = new Stack<TreeNode<T>>();
        stack.Push(root);

        while (stack.Count > 0)
        {
            var node = stack.Pop();
            yield return selector(node.Value);

            // Push children in reverse order to maintain correct order during traversal
            for (int i = node.Children.Count - 1; i >= 0; i--)
            {
                stack.Push(node.Children[i]);
            }
        }
    }

    /// <summary>
    /// Post-order Traversal:
    /// - Recursively traverse each subtree.
    /// - Visit the root node.
    /// </summary>
    /// <remarks>
    ///     1
    ///    /\
    ///   2  3
    ///  /\   \
    /// 4 5    6
    /// turns into [4, 5, 2, 6, 3, 1]
    /// </remarks>
    /// <param name="root"></param>
    /// <returns>flat sequence of T values</returns>
    public static IEnumerable<T> SequencePostOrder<T>(this TreeNode<T> root)
    {
        var stack = new Stack<TreeNode<T>>();
        var resultStack = new Stack<T>();
        stack.Push(root);

        while (stack.Count > 0)
        {
            var node = stack.Pop();
            resultStack.Push(node.Value);

            foreach (var child in node.Children)
            {
                stack.Push(child);
            }
        }

        while (resultStack.Count > 0)
        {
            yield return resultStack.Pop();
        }
    }

    /// <summary>
    /// Level-order Traversal (Breadth-first Traversal):
    /// - Visit nodes level by level, from left to right.
    /// </summary>
    /// <remarks>
    ///     1
    ///    /\
    ///   2  3
    ///  /\   \
    /// 4 5    6
    /// turns into [1, 2, 3, 4, 5, 6]
    /// </remarks>
    /// <param name="root"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns>flat sequence of T values</returns>
    public static IEnumerable<T> SequenceLevelOrder<T>(this TreeNode<T> root)
    {
        var queue = new Queue<TreeNode<T>>();
        queue.Enqueue(root);

        while (queue.Count > 0)
        {
            var node = queue.Dequeue();
            yield return node.Value;

            foreach (var child in node.Children)
            {
                queue.Enqueue(child);
            }
        }
    }

    public static IEnumerable<TResult> TraverseLevelOrder<T, TResult>(this TreeNode<T> root, Func<T, TResult> selector)
    {
        var queue = new Queue<TreeNode<T>>();
        queue.Enqueue(root);

        while (queue.Count > 0)
        {
            var node = queue.Dequeue();
            yield return selector(node.Value);

            foreach (var child in node.Children)
            {
                queue.Enqueue(child);
            }
        }
    }
}