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

        [Test]
        public void ConvertToString()
        {
            var converter = new LengthConverter
            {
                Unit = LengthUnit.Centimetres,
                Symbol = SymbolOptions.NotAllowed
            };

            var length = Length.FromMillimetres(12);
            var actual = converter.Convert(length, typeof(string), null, null);
            Assert.AreEqual(1.2, actual);
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
        public void ConvertBackFromString(object value)
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
    }
}
