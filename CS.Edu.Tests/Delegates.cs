using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace CS.Edu.Tests;

public record Product(string Name, decimal Price, int Quantity);

public record Customer(string Name, int Age);

public class Delegates
{
    private readonly Product[] _products =
    [
        new("Apple", 1.5m, 2),
        new("Banana", 1.2m, 7),
        new("BubbleGum", 0.5m, 0),
    ];

    private readonly Customer[] _customer =
    [
        new("Bob", 35),
        new("Alice", 27),
    ];

    [Fact]
    public void Filter_UsingLoop()
    {
        List<Product> result = [];
        for (int i = 0; i < _products.Length; i++)
        {
            Product product = _products[i];
            if (product.Quantity > 0)
            {
                result.Add(product);
            }
        }
    }

    [Fact]
    public void Filter_UsingLINQ()
    {
        // x => f x
        Func<Product, bool> function1 = IsInStock;
        Func<Product, bool> function2 = delegate(Product product)
        {
            return product.Quantity > 0;
        };

        List<Product> result2 = _products.Where(product => product.Quantity > 0).ToList();
    }

    private IEnumerable<T> Where<T>(IEnumerable<T> items, IFilter<T> filter)
    {
        foreach (T item in items)
        {
            if (filter.SatisfyFilter(item))
                yield return item;
        }
    }

    private interface IFilter<T>
    {
        bool SatisfyFilter(T product);
    }

    private class ProductQuantityFilter : IFilter<Product>
    {
        public bool SatisfyFilter(Product product)
        {
            return product.Quantity > 0;
        }
    }

    private static bool IsInStock(Product product)
    {
        return product.Quantity > 0;
    }

    private static bool PriceMoreThan100(Product product)
    {
        return product.Price > 100;
    }

    private delegate bool Predicate(Product product);
}