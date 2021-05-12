using System.Collections.Generic;
using System.Linq;
using CS.Edu.Core.Extensions;
using NUnit.Framework;

namespace CS.Edu.Tests.Extensions.EnumerableExtensions
{
    [TestFixture]
    public class ThenGroupByTests
    {
        private readonly IEnumerable<(string FirstName, string LastName, string Department)> _empty = Enumerable.Empty<(string FirstName, string LastName, string Department)>();

        private readonly (string FirstName, string LastName, string Department)[] _source =
        {
            ("Bob", "Wilkins", "DevOps"),
            ("Bob", "Farnsworth", "DevOps"),
            ("Alice", "Wilkins", "HR"),
            ("Frank", "Zummer", "IT"),
            ("John", "Snow", "Dev"),
            ("Jack", "Daniels", "IT"),
            ("Bill", "Murrey", "Dev"),
            ("Alice", "Milano", "Dev"),
            ("Bred", "Pit", "HR"),
            ("Anny", "Hall", "DevOps"),
            ("Alice", "White", "Dev"),
        };

        [Test]
        public void EmptySource_ReturnsEmptyEnumerable()
        {
            var result = _empty.GroupBy(x => x.FirstName).ThenBy(x => x.LastName);
            Assert.IsEmpty(result);
        }

        [Test]
        public void GroupOnSecondLayer()
        {
            var result = _source.GroupBy(x => x.LastName[0]).ThenBy(x => x.FirstName[0]);
        }

        [Test]
        public void GroupOnThirdLayer()
        {
            var result = _source.GroupBy(x => x.Department)
                .ThenBy(x => x.FirstName)
                .ThenBy(x => x.LastName);
        }
    }
}