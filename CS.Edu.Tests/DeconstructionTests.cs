using System.Collections.Generic;
using NUnit.Framework;

namespace CS.Edu.Core
{
    class Container 
    {
        public int Id { get; set; }

        public string Value { get; set; }

        public string Value1 { get; set; }

        public void Deconstruct(out int id, out string value, out string value1)
        {
            id = Id;
            value = Value;
            value1 = Value1;
        }
    }

    [TestFixture]
    public class DeconstructionTests
    {
        [Test]
        public void IgnoreTest()
        {
            var container = new Container();

            var (id, value, _) = container;
        }
    }
}