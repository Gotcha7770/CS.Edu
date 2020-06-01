using System;
using System.Collections.Generic;

namespace CS.Edu.Tests.Utils
{
    class CustomerViewModel
    {
        private readonly Customer _customer;
        private readonly List<Product> _cart =  new List<Product>();

        public CustomerViewModel(Customer customer, IEnumerable<Product> cart)
        {
            _customer = customer;
            _cart.AddRange(cart);
        }

        public Guid Id => _customer.Id;

        public IReadOnlyList<Product> Cart => _cart;
    }
}