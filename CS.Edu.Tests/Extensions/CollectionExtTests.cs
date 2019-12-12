using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CS.Edu.Core.Extensions;
using CS.Edu.Core;

namespace CS.Edu.Tests.Extensions
{
    [TestFixture]
    public class CollectionExtTests
    {
        [Test]
        public void InvalidateCollection()
        {
            var list = new List<Range<int>>()
            {
                new Range<int>(0, 10),
                new Range<int>(10, 20),
                new Range<int>(20, 30),
                new Range<int>(30, 40)
            };

            
        }
    }
}