
using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace CS.Edu.Tests.Utils
{
    public class EnumerableAssert
    {
        /// <summary>
        /// Asserts that collection contains any item that matches predicate
        /// </summary>
        /// <param name="collection">IEnumerable of objects to be considered</param>
        /// <param name="predicate">predicate for checking item</param>
        public static void Any<T>(IEnumerable<T> collection, Predicate<T> predicate)
        {
            Assert.That(collection, Has.Some.Matches(predicate));
        }
        
        /// <summary>
        /// Asserts that collection doesn't contains any item that matches predicate
        /// </summary>
        /// <param name="collection">IEnumerable of objects to be considered</param>
        /// <param name="predicate">predicate for checking item</param>
        public static void None<T>(IEnumerable<T> collection, Predicate<T> predicate)
        {
            Assert.That(collection, Has.None.Matches(predicate));
        }

        /// <summary>
        /// Asserts that all items in collection matches predicate
        /// </summary>
        /// <param name="collection">IEnumerable of objects to be considered</param>
        /// <param name="predicate">predicate for checking item</param>
        public static void All<T>(IEnumerable<T> collection, Predicate<T> predicate)
        {
            Assert.That(collection, Has.All.Matches(predicate));
        }
    }
}