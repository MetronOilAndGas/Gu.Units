namespace Gu.Units.Wpf.Tests
{
    using System;
    using System.Globalization;
    using NUnit.Framework;

    //[Explicit(Reminder.ToDo)]
    public class LengthConverterTests
    {
        [TestCase(true)]
        [TestCase(false)]
        public void ThrowsInDesignModeIfMissingUnit(bool isDesignMode)
        {
            Gu.Units.Wpf.Is.DesignMode = isDesignMode;
            var converter = new LengthConverter
            {
                Symbol = SymbolOptions.NotAllowed
            };

            var length = Length.FromMetres(1.2);
            Assert.AreEqual(null, converter.Unit);
            if (isDesignMode)
            {
                Assert.Throws<ArgumentException>(() => converter.Convert(length, typeof(string), null, null));
            }
            else
            {
                var actual = converter.Convert(length, typeof(string), null, null);
                Assert.AreEqual("No unit set", actual);
            }
        }

        [TestCase(typeof(string), 1.2)]
        [TestCase(typeof(object), 1.2)]
        [TestCase(typeof(double), 1.2)]
        public void ConvertTo(Type targetType, object expected)
        {
            var converter = new LengthConverter
            {
                Unit = LengthUnit.Centimetres,
                Symbol = SymbolOptions.NotAllowed
            };

            var length = Length.FromMillimetres(12);
            var actual = converter.Convert(length, targetType, null, null);
            Assert.AreEqual(expected, actual);
            // Yes we want a double here. Reason is to be as similar to double as possible to not cause localization issues.
            // Maybe it needs to be changed.
        }

        [TestCase(typeof(string), "")]
        [TestCase(typeof(object), null)]
        [TestCase(typeof(double), null)]
        public void ConvertNull(Type targetType, object expected)
        {
            var converter = new LengthConverter
            {
                Unit = LengthUnit.Centimetres,
                Symbol = SymbolOptions.NotAllowed
            };

            var actual = converter.Convert(null, targetType, null, null);
            Assert.AreEqual(expected, actual);
        }

        [TestCase("1.2")]
        [TestCase(1.2)]
        public void ConvertBack(object value)
        {
            var converter = new LengthConverter
            {
                Unit = LengthUnit.Centimetres,
                Symbol = SymbolOptions.NotAllowed
            };

            var actual = converter.ConvertBack(value, typeof(Length), null, CultureInfo.InvariantCulture);
            var expected = Length.FromCentimetres(1.2);
            Assert.AreEqual(expected, actual);
        }

        [TestCase("1.2", SymbolOptions.NotAllowed, true)]
        [TestCase("1.2 mm", SymbolOptions.NotAllowed, false)]
        [TestCase("1.2", SymbolOptions.Allowed, true)]
        [TestCase("12 mm", SymbolOptions.Allowed, true)]
        [TestCase("1.2", SymbolOptions.Required, false)]
        [TestCase("12 mm", SymbolOptions.Required, true)]
        public void ConvertBackSymbolSettings(string value, SymbolOptions options, bool expectSuccess)
        {
            var converter = new LengthConverter
            {
                Unit = LengthUnit.Centimetres,
                Symbol = SymbolOptions.NotAllowed
            };

            var actual = converter.ConvertBack(value, typeof(Length), null, CultureInfo.InvariantCulture);
            if (expectSuccess)
            {
                var expected = Length.FromCentimetres(1.2);
                Assert.AreEqual(expected, actual);
            }
            else
            {
                Assert.AreEqual(value, actual);
            }
        }
    }
}
