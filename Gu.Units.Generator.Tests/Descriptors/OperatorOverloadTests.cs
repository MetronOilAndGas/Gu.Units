namespace Gu.Units.Generator.Tests.Descriptors
{
    using NUnit.Framework;

    public class OperatorOverloadTests
    {
        private MockSettings settings;
        private Quantity length;
        private Quantity speed;
        private Quantity time;
        private Quantity area;
        private Quantity volume;

        [SetUp]
        public void SetUp()
        {
            Settings.Instance = null;
            this.settings = new MockSettings();
            this.length = this.settings.Length;
            this.speed = this.settings.Speed;
            this.time = this.settings.Time;
            this.area = this.settings.Area;
            this.volume = this.settings.Volume;
        }

        [Test]
        public void LengthSpeed()
        {
            OperatorOverload overload;
            Assert.AreEqual(true, OperatorOverload.TryCreateDivision(this.length, this.speed, this.settings.AllUnits, out overload));
            Assert.AreEqual(OperatorOverload.Divide, overload.Operator);
            Assert.AreEqual(this.length, overload.Left);
            Assert.AreEqual(this.speed, overload.Right);
            Assert.AreEqual(this.speed, overload.Result);
        }

        [Test]
        public void LengthDividedByTime()
        {
            OperatorOverload overload;
            Assert.AreEqual(true, OperatorOverload.TryCreateDivision(this.length, this.time, this.settings.AllUnits, out overload));
            Assert.AreEqual(OperatorOverload.Divide, overload.Operator);
            Assert.AreEqual(this.length, overload.Left);
            Assert.AreEqual(this.time, overload.Right);
            Assert.AreEqual(this.speed, overload.Result);
        }

        [Test]
        public void SpeedLength()
        {
            Assert.Fail();
            //var overload = new OperatorOverload(this.speed, this.length, this.settings.AllUnits);
            //Assert.AreEqual(OperatorOverload.Multiply, overload.Operator);
            //Assert.AreEqual(this.speed, overload.Left);
            //Assert.AreEqual(this.time, overload.Right);
            //Assert.AreEqual(this.length, overload.Result);
        }

        [Test]
        public void SpeedTime()
        {
            Assert.Fail();
            //var overload = new OperatorOverload(this.speed, this.time, this.settings.AllUnits);
            //Assert.AreEqual(OperatorOverload.Multiply, overload.Operator);
            //Assert.AreEqual(this.speed, overload.Left);
            //Assert.AreEqual(this.length, overload.Right);
            //Assert.AreEqual(this.time, overload.Result);
        }

        [Test]
        public void LengthArea()
        {
            Assert.Fail();
            //var overload = new OperatorOverload(this.length, this.area, this.settings.AllUnits);
            //Assert.AreEqual(OperatorOverload.Multiply, overload.Operator);
            //Assert.AreEqual(this.length, overload.Left);
            //Assert.AreEqual(this.length, overload.Right);
            //Assert.AreEqual(this.area, overload.Result);
        }

        [Test]
        public void AreaLength()
        {
            Assert.Fail();
            //var overload = new OperatorOverload(this.area, this.length, this.settings.AllUnits);
            //Assert.AreEqual(OperatorOverload.Divide, overload.Operator);
            //Assert.AreEqual(this.area, overload.Left);
            //Assert.AreEqual(this.length, overload.Right);
            //Assert.AreEqual(this.length, overload.Result);
        }

        [Test]
        public void LengthVolume()
        {
            Assert.Fail();
            //var overload = new OperatorOverload(this.length, this.volume, this.settings.AllUnits);
            //Assert.AreEqual(OperatorOverload.Multiply, overload.Operator);
            //Assert.AreEqual(this.length, overload.Left);
            //Assert.AreEqual(this.area, overload.Right);
            //Assert.AreEqual(this.volume, overload.Result);
        }
    }
}
