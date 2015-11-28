﻿namespace Gu.Units.Tests
{
    using System.Globalization;
    using System.Threading;
    using Internals.Parsing;
    using NUnit.Framework;

    public class FormatTests
    {
        private const string Superscripts = "⁺⁻⁰¹²³⁴⁵⁶⁷⁸⁹";
        private const char MultiplyDot = '⋅';

        [Test]
        public void FormatAngle()
        {
            var angle = Angle.FromDegrees(1.2);
            using (Thread.CurrentThread.UsingTempCulture(CultureInfo.InvariantCulture))
            {
                Assert.AreEqual("0.020943951023932\u00A0rad", angle.ToString());
                Assert.AreEqual("1.200°", angle.ToString("F3°"));
                Assert.AreEqual("1.2°", angle.ToString(AngleUnit.Degrees));
                Assert.AreEqual(" 1.2 ° ", angle.ToString(" F1 ° "));
                Assert.AreEqual(" 0.02 rad", angle.ToString(" F2 "));
                Assert.AreEqual("1.200°", angle.ToString("F3", AngleUnit.Degrees));
                Assert.AreEqual("0.02\u00A0rad", angle.ToString("F2", AngleUnit.Radians));
            }

            var sv = CultureInfo.GetCultureInfo("sv-SE");
            Assert.AreEqual("0,020943951023932\u00A0rad", angle.ToString(sv));
            Assert.AreEqual("1,200°", angle.ToString("F3°", sv));
            Assert.AreEqual("1,2°", angle.ToString(AngleUnit.Degrees, sv));
            Assert.AreEqual(" 1,2 ° ", angle.ToString(" F1 ° ", sv));
            Assert.AreEqual(" 0,02 rad", angle.ToString(" F2 ", sv));
            Assert.AreEqual("1,200°", angle.ToString("F3", AngleUnit.Degrees, sv));
            Assert.AreEqual("0,02\u00A0rad", angle.ToString("F2", AngleUnit.Radians, sv));
        }

        [Test]
        public void FormatSpeed()
        {
            var speed = Speed.FromMetresPerSecond(1.2);
            const string UnknownFormat = "unknown format";
            using (Thread.CurrentThread.UsingTempCulture(CultureInfo.InvariantCulture))
            {
                Assert.AreEqual("1.20\u00A0m/s", speed.ToString("F2"));
                Assert.AreEqual(UnknownFormat, 1.2.ToString(UnknownFormat)); // for comparison
                Assert.AreEqual(UnknownFormat, speed.ToString(UnknownFormat));
                Assert.AreEqual("1.20 m⋅s⁻¹", speed.ToString("F2 m⋅s⁻¹"));
                Assert.AreEqual("1.2\u00A0m/s", speed.ToString());
                Assert.AreEqual("1.2\u00A0m⋅s⁻¹", speed.ToString("m⋅s⁻¹"));
                Assert.AreEqual("1200\u00A0mm⋅s⁻¹", speed.ToString("mm⋅s⁻¹"));
                Assert.AreEqual("1200\u00A0s⁻¹⋅mm", speed.ToString("s⁻¹⋅mm"));
                Assert.AreEqual("1200\u00A0s⁻¹⋅mm¹", speed.ToString("s⁻¹⋅mm¹"));
                Assert.AreEqual("1.2\u00A0m*s^-1", speed.ToString("m*s^-1"));
                Assert.AreEqual("1.2\u00A0s^-1*m", speed.ToString("s^-1*m"));
                Assert.AreEqual("1.2\u00A0s^-1*m^1", speed.ToString("s^-1*m^1"));
                Assert.AreEqual("4.32\u00A0km/h", speed.ToString(SpeedUnit.KilometresPerHour));
                Assert.AreEqual("4.3\u00A0km/h", speed.ToString("F1", SpeedUnit.KilometresPerHour));
                Assert.AreEqual("1,200.00 mm⋅s⁻¹", speed.ToString("N mm⋅s⁻¹"));
            }

            var sv = CultureInfo.GetCultureInfo("sv-SE");
            Assert.AreEqual("1,20\u00A0m/s", speed.ToString("F2", sv));
            Assert.AreEqual(UnknownFormat, 1.2.ToString(UnknownFormat, sv)); // for comparison
            Assert.AreEqual(UnknownFormat, speed.ToString(UnknownFormat, sv));
            Assert.AreEqual("1,20 m⋅s⁻¹", speed.ToString("F2 m⋅s⁻¹", sv));
            Assert.AreEqual("1,2\u00A0m/s", speed.ToString(sv));
            Assert.AreEqual("1,2\u00A0m⋅s⁻¹", speed.ToString("m⋅s⁻¹", sv));
            Assert.AreEqual("1200\u00A0mm⋅s⁻¹", speed.ToString("mm⋅s⁻¹", sv));
            Assert.AreEqual("1200\u00A0s⁻¹⋅mm", speed.ToString("s⁻¹⋅mm", sv));
            Assert.AreEqual("1200\u00A0s⁻¹⋅mm¹", speed.ToString("s⁻¹⋅mm¹", sv));
            Assert.AreEqual("1,2\u00A0m*s^-1", speed.ToString("m*s^-1", sv));
            Assert.AreEqual("4,32\u00A0km/h", speed.ToString(SpeedUnit.KilometresPerHour, sv));
            Assert.AreEqual("4,3\u00A0km/h", speed.ToString("F1", SpeedUnit.KilometresPerHour, sv));
            Assert.AreEqual("1\u00A0200,00 mm⋅s⁻¹", speed.ToString("N mm⋅s⁻¹", sv));
        }


        [Test]
        public void Reminders()
        {
            Assert.Fail("Add enum SymbolOptions{ HatPowers, SuperScripts, Fractions, Names }");
            Assert.Fail("Add ToString(\"F2\",\"mm/s\")");
            Assert.Fail("Composite formats must die force.ToString(\"N\") is ambiguous");
        }

        [Explicit(Reminder.ToDo)]
        [Test]
        public void FormatPressure()
        {
            var pressure = Pressure.FromMegapascals(1.2);
            Assert.AreEqual("1.20 N/m^2", pressure.ToString("F2 N/m^2"));
            Assert.AreEqual("1.20 N/m^2", pressure.ToString("F2 N⋅m⁻²"));
            Assert.AreEqual("1.20 N/m^2", pressure.ToString("F2 N⋅mm⁻²"));
            Assert.AreEqual("1.20 MPa", pressure.ToString("F2 MPa"));
            Assert.AreEqual("1.20E6 Pa", pressure.ToString("E Pa"));
        }
    }
}