using System;
using System.Collections.Generic;

namespace CS.Edu.Tests.Utils
{
    class CustomerViewModel
    {
        private readonly Customer _customer;

        public CustomerViewModel(Customer customer, IEnumerable<Product> cart)
        {
            _customer = customer;
            _customer.Cart.AddRange(cart);
        }

        public Guid Id => _customer.Id;

        public IReadOnlyList<Product> Cart => _customer.Cart;
    }
}