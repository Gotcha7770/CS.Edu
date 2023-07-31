using System;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace CS.Edu.Tests;

public class DefaultInterfaceMethodTests
{
    private static readonly DateTime Stamp = DateTime.Now;

    interface IRemovable
    {
        bool IsRemoved { get; set; }
        void Remove() => IsRemoved = true;
    }

    class Item : IRemovable
    {
        public bool IsRemoved { get; set; }
    }

    class ExtendedItem : IRemovable
    {
        public bool IsRemoved { get; set; }
        public DateTime Removed { get; set; }

        void IRemovable.Remove()
        {
            IsRemoved = true;
            Removed = Stamp;
        }
    }

    [Fact]
    public void RemoveTest()
    {
        var items = new IRemovable[]
        {
            new Item(),
            new ExtendedItem()
        };

        items.ForEach(x => x.Remove());

        items[0]
            .Should()
            .BeOfType<Item>()
            .Which.IsRemoved.Should()
            .BeTrue();

        items[1]
            .Should()
            .BeOfType<ExtendedItem>()
            .Which.Should()
            .BeEquivalentTo(new ExtendedItem
            {
                IsRemoved = true,
                Removed = Stamp
            });
    }
}