using System;
using System.Collections.Generic;

namespace CS.Edu.Tests.Utils
{
    class Customer
    {
        public Guid Id { get; } = Guid.NewGuid();

        public List<Product> Cart {get;} = new List<Product>();
    }
}