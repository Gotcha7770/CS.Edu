using System.Globalization;
using NUnit.Framework;

namespace CS.Edu.Tests
{
    [TestFixture]
    public class CustomNumericFormatStrings
    {
        [Test]
        public void DoubeToStringFormat()
        {
            int a = 123;
            double b = 123.0;
            double c = 123.120;
            double d = 123.12345;

            string format = "{0:0.####}";
            //string format = "{0:F4}";
            var culture = CultureInfo.InvariantCulture;

            var _1 = string.Format(culture, format, a);
            var _2 = string.Format(culture, format, b);
            var _3 = string.Format(culture, format, c);
            var _4 = string.Format(culture, format, d);
        }
    }
}