namespace Gu.Units.Wpf.Tests
{
    using System;
    using System.Globalization;
    using NUnit.Framework;

    public partial class LengthConverterTests
    {
        public class Setup
        {
            [TestCase("F1 mm")]
            public void SettingStringFormatHappyPath(string stringFormat)
            {
                var converter = new LengthConverter
                {
                    StringFormat = stringFormat
                };

                Assert.AreEqual(LengthUnit.Millimetres, converter.Unit);
                Assert.AreEqual(UnitInput.SymbolRequired, converter.UnitInput);
            }

            [Test]
            public void SettingStringFormatWithUnitWhenUnitIsSetThrowsInDesignMode()
            {
                Gu.Units.Wpf.Is.DesignMode = true;
                var converter = new LengthConverter
                {
                    Unit = LengthUnit.Centimetres
                };

                Assert.Throws<InvalidOperationException>(() => converter.StringFormat = "F1 mm");
            }

            [Test]
            public void SettingBindingStringFormatWithUnitWhenUnitIsSetThrowsInDesignMode()
            {
                Gu.Units.Wpf.Is.DesignMode = true;
                var converter = new LengthConverter
                {
                    Unit = LengthUnit.Centimetres
                };

                Assert.Throws<InvalidOperationException>(() => converter.StringFormat = "F1 mm");
            }

            [Test]
            public void SettingStringFormatWithUnitWhenUnitIsSetShowsErrorTextInRuntime()
            {
                Gu.Units.Wpf.Is.DesignMode = false;
                var converter = new LengthConverter
                {
                    Unit = LengthUnit.Centimetres
                };

                converter.StringFormat = "F1 mm";
                var actual = converter.Convert(Length.FromMetres(1.2), typeof(string), null, CultureInfo.InvariantCulture);

                Assert.AreEqual("Unit is set to cm but StringFormat is 'F1 mm'", actual);
            }

            [Test]
            public void SettingBindingStringFormatWithUnitWhenUnitIsSetShowsErrorTextInRuntime()
            {
                Gu.Units.Wpf.Is.DesignMode = false;
                var converter = new LengthConverter
                {
                    Unit = LengthUnit.Centimetres
                };

                converter.StringFormat = "F1 mm";
                var actual = converter.Convert(Length.FromMetres(1.2), typeof(string), null, CultureInfo.InvariantCulture);

                Assert.AreEqual("Unit is set to cm but Binding.StringFormat is 'F1 mm'", actual);
            }

            [TestCase(true)]
            [TestCase(false)]
            public void ThrowsInDesignModeIfMissingUnit(bool isDesignMode)
            {
                Gu.Units.Wpf.Is.DesignMode = isDesignMode;
                var converter = new LengthConverter
                {
                    UnitInput = UnitInput.ScalarOnly
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
        }
    }
}
