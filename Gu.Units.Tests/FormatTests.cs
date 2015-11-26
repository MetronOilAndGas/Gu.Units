namespace Gu.Units.Tests
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
            Assert.AreEqual("1.20°", angle.ToString("F2°"));
            using (Thread.CurrentThread.UsingTempCulture(CultureInfo.InvariantCulture))
            {
                Assert.AreEqual("1.2\u00A0rad", angle.ToString());
                Assert.AreEqual("120°", angle.ToString(AngleUnit.Degrees));
                Assert.AreEqual(" 1200.0 ° ", angle.ToString(" F1 ° "));
                Assert.AreEqual(" 1.2 rad", angle.ToString(" F1 "));
                Assert.AreEqual("12.0°", angle.ToString("F1", AngleUnit.Degrees));
                Assert.AreEqual("12.0\u00A0dm", angle.ToString("F1", AngleUnit.Radians));
            }

            var sv = CultureInfo.GetCultureInfo("sv-SE");
            Assert.AreEqual("1,2\u00A0 radians", angle.ToString(sv));
            Assert.AreEqual(" 1200,0° ", angle.ToString(" F1° ", sv));
            Assert.AreEqual("1200,0°", angle.ToString("F1", sv, AngleUnit.Degrees));
        }

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