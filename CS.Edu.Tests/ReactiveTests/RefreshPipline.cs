using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using CS.Edu.Tests.Utils;
using DynamicData;
using NUnit.Framework;

namespace CS.Edu.Tests.ReactiveTests
{
    [TestFixture]
    public class RefreshPiplineTests
    {
        [Test]
        public void RefreshSubscriptionTest()
        {
            SourceCache<Product, Guid> products = new SourceCache<Product, Guid>(x => x.Id);
            SourceCache<Customer, Guid> customers = new SourceCache<Customer, Guid>(x => x.Id);

            var sharedProducts = products.Connect();

            var retransformer = sharedProducts.Select(x => Unit.Default);

            sharedProducts.Bind(out ReadOnlyObservableCollection<Product> cart)
                .Subscribe();

            var subscription = customers.Connect()
                .Transform(x => new CustomerViewModel(x, cart), retransformer)
                .Bind(out ReadOnlyObservableCollection<CustomerViewModel> result)
                .Subscribe();

            customers.AddOrUpdate(new Customer());
            CollectionAssert.IsNotEmpty(result);

            products.AddOrUpdate(new Product("Apple"));
            Assert.AreEqual(result[0].Cart[0].Content, "Apple");
        }
    }
}