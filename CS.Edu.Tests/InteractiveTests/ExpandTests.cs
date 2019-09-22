using System.Linq;
using System.Xml.Linq;
using NUnit.Framework;

namespace CS.Edu.Tests.ExpandTests
{
    [TestFixture]
    public class ExpandTests
    {
        XElement root = new XElement("root",
                                     new XElement("firstLevel",
                                                  new XElement("secondLevel"),
                                                  new XElement("secondLevel")),
                                     new XElement("firstLevel",
                                                  new XElement("secondLevel"),
                                                  new XElement("secondLevel")));
        [Test]
        public void TreeTest()
        {
            var flatElements = root.Elements()
                .Expand(x => x.Elements())
                .ToArray();

            Assert.That(flatElements.Length, Is.EqualTo(6));
        }
    }
}