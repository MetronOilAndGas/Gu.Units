namespace Gu.Units.Wpf.Tests
{
    using System;
    using System.Globalization;
    using NUnit.Framework;

    [Explicit(Reminder.ToDo)]
    public class LengthConverterTests
    {
        [Test]
        public void Reminders()
        {
            Assert.Fail("Return errors in string in runtime");
            Assert.Fail("Throw in designtime");
        }

        [TestCase(true)]
        [TestCase(false)]
        public void ThrowsInDesignModeIfMissingUnit(bool isDesignMode)
        {
            Gu.Units.Wpf.Is.DesignMode = isDesignMode;
            var converter = new LengthConverter
            {
                UnitInput = UnitInputOptions.ScalarOnly
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
                UnitInput = UnitInputOptions.ScalarOnly
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
                UnitInput = UnitInputOptions.ScalarOnly
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
                UnitInput = UnitInputOptions.ScalarOnly
            };

            var actual = converter.ConvertBack(value, typeof(Length), null, CultureInfo.InvariantCulture);
            var expected = Length.FromCentimetres(1.2);
            Assert.AreEqual(expected, actual);
        }

        [TestCase("1.2", UnitInputOptions.ScalarOnly, true)]
        [TestCase("1.2 mm", UnitInputOptions.ScalarOnly, false)]
        [TestCase("1.2", UnitInputOptions.SymbolAllowed, true)]
        [TestCase("12 mm", UnitInputOptions.SymbolAllowed, true)]
        [TestCase("1.2", UnitInputOptions.SymbolRequired, false)]
        [TestCase("12 mm", UnitInputOptions.SymbolRequired, true)]
        public void ConvertBackSymbolSettings(string value, UnitInputOptions options, bool expectSuccess)
        {
            var converter = new LengthConverter
            {
                Unit = LengthUnit.Centimetres,
                UnitInput = UnitInputOptions.ScalarOnly
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
