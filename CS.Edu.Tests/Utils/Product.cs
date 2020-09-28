using System;

namespace CS.Edu.Tests.Utils
{
    class Product
    {
        public Product(string content)
        {
            Content = content;
        }

        public Guid Id { get; } = Guid.NewGuid();

        public string Content {get; }
    }
}