using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gu.Units.Tests.Sandbox
{
    using NUnit.Framework;

    public class ArithmeticsTests
    {
        private static Metres m = LengthUnit.m;
        [Test]
        public void LengthTimesLength()
        {
            var h = 1*m;
            var w = 1*m;
            Area area = h*w;
            Assert.AreEqual(1, area.MetresSquared);
        }
    }
}
