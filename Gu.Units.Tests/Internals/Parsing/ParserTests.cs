namespace Gu.Units.Tests.Internals.Parsing
{
    using System;
    using System.Globalization;
    using NUnit.Framework;

    public class ParserTests
    {
        [TestCase("1.0cm", "sv-se")]
        [TestCase("1,0cm", "en-us")]
        public void Exceptions(string s, string culture)
        {
            var cultureInfo = CultureInfo.GetCultureInfo(culture);
            var ex = Assert.Throws<FormatException>(() => QuantityParser.Parse<LengthUnit, Length>(s, Length.From, NumberStyles.Float, cultureInfo));
            Console.Write(ex.Message);
        }
    }
}
