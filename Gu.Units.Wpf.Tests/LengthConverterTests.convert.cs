namespace Gu.Units.Wpf.Tests
{
    using System;
    using System.Globalization;
    using NUnit.Framework;

    [RequiresSTA]
    public partial class LengthConverterTests
    {
        [RequiresSTA]
        public class Convert
        {
            [TestCase(typeof(string), 1.2)]
            [TestCase(typeof(object), 1.2)]
            [TestCase(typeof(double), 1.2)]
            public void WithExplicitUnit(Type targetType, object expected)
            {
                var converter = new LengthConverter
                {
                    Unit = LengthUnit.Centimetres,
                };

                var length = Length.FromMillimetres(12);
                var actual = converter.Convert(length, targetType, null, null);
                Assert.AreEqual(expected, actual);
            }

            [TestCase(typeof(string), SymbolFormat.SignedHatPowers, "1.2 m*s^-1")]
            [TestCase(typeof(object), SymbolFormat.FractionHatPowers, "1.2 m/s")]
            public void WithUnitAndSymbolFormat(Type targetType, SymbolFormat format, object expected)
            {
                var converter = new LengthConverter
                {
                    Unit = LengthUnit.Centimetres,
                    SymbolFormat = format
                };

                var length = Length.FromMillimetres(12);
                var actual = converter.Convert(length, targetType, null, CultureInfo.InvariantCulture);
                Assert.AreEqual(expected, actual);
            }

            [TestCase(typeof(string), "F1 m", "en-us", "1.2 m")]
            [TestCase(typeof(object), "F1 m", "en-us", "1.2 m")]
            [TestCase(typeof(double), "F1 m", "en-us", 1.234)]
            public void WithStringFormat(Type targetType, string stringFormat, string culture, object expected)
            {
                var converter = new LengthConverter
                {
                    StringFormat = stringFormat
                };

                var length = Length.FromMillimetres(1234);
                var actual = converter.Convert(length, targetType, null, CultureInfo.GetCultureInfo(culture));
                Assert.AreEqual(expected, actual);
            }

            [TestCase(typeof(string), "F1 m", "en-us", 1.234)]
            [TestCase(typeof(object), "F1 m", "en-us", 1.234)]
            [TestCase(typeof(double), "F1 m", "en-us", 1.234)]
            public void WithBindingStringFormat(Type targetType, string stringFormat, string culture, object expected)
            {
                var converter = new LengthConverter();
                var providerMock = new ServiceProviderMock
                {
                    BindingStringFormat = stringFormat
                };

                converter.ProvideValue(providerMock.Object);
                var length = Length.FromMillimetres(1234);
                var actual = converter.Convert(length, targetType, null, CultureInfo.GetCultureInfo(culture));
                Assert.AreEqual(expected, actual);
            }

            [Test]
            public void WithBindingStringFormatAndExplicitStringFormat()
            {
                var converter = new LengthConverter
                {
                    StringFormat = "F1 mm"
                };

                var providerMock = new ServiceProviderMock
                {
                    BindingStringFormat = "F2 cm"
                };

                converter.ProvideValue(providerMock.Object);
                var length = Length.FromMillimetres(1234);
                Gu.Units.Wpf.Is.DesignMode = true;
                var ex = Assert.Throws<InvalidOperationException>(()=> converter.Convert(length, typeof(string), null, CultureInfo.GetCultureInfo("en-US")));
                Assert.AreEqual("Both Binding.StringFormat and StringFormat are set", ex.Message);

                Gu.Units.Wpf.Is.DesignMode = false;
                var convert = converter.Convert(length, typeof (string), null, CultureInfo.GetCultureInfo("en-US"));
                Assert.AreEqual("Both Binding.StringFormat and StringFormat are set", convert);
            }

            [TestCase(typeof(string), "")]
            [TestCase(typeof(object), null)]
            [TestCase(typeof(double), null)]
            public void Null(Type targetType, object expected)
            {
                var converter = new LengthConverter
                {
                    Unit = LengthUnit.Centimetres,
                    UnitInput = UnitInput.ScalarOnly
                };

                var actual = converter.Convert(null, targetType, null, null);
                Assert.AreEqual(expected, actual);
            }
        }
    }
}
