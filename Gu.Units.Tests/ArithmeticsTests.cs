namespace Gu.Units.Tests
{
    using NUnit.Framework;

    public class ArithmeticsTests
    {
        private static Metres m = LengthUnit.m;
        private static Seconds s = TimeUnit.s;

        [Test]
        public void LengthMulLength()
        {
            var h = 1*m;
            var w = 1*m;
            var area = h*w;
            Assert.AreEqual(1, area.MetresSquared);
        }

        [Test]
        public void LengthDivTime()
        {
            var l = 1 * m;
            var t = 1 * s;
            var area = l / t;
            Assert.AreEqual(1, area.MetresPerSecond);
        }
    }
} 
