namespace Gu.Units.Wpf.Tests
{
    using System;
    using System.Globalization;
    using NUnit.Framework;

    [Explicit(Reminder.ToDo)]
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
                Assert.AreEqual(length, actual);
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

        [Test]
        public void ConvertNullToStringReturnsEmpty()
        {
            var converter = new LengthConverter
            {
                Unit = LengthUnit.Centimetres,
                Symbol = SymbolOptions.NotAllowed
            };

            var actual = converter.Convert(null, typeof(string), null, null);
            Assert.AreEqual(string.Empty, actual);
        }

        [Test]
        public void ConvertNullToObjectReturnsEmpty()
        {
            var converter = new LengthConverter
            {
                Unit = LengthUnit.Centimetres,
                Symbol = SymbolOptions.NotAllowed
            };

            var actual = converter.Convert(null, typeof(object), null, null);
            Assert.AreEqual(null, actual);
        }

        [Test]
        public void ConvertFromString()
        {
            var converter = new LengthConverter
            {
                Unit = LengthUnit.Centimetres,
                Symbol = SymbolOptions.NotAllowed
            };

            var actual = converter.Convert("1.2", typeof(string), null, CultureInfo.InvariantCulture);
            var expected = Length.FromCentimetres(1.2);
            Assert.AreEqual(expected, actual);
        }
    }
}
