namespace Gu.Units.Generator.Tests.Descriptors
{
    using System.Linq;
    using NUnit.Framework;

    public class QuantityTests
    {
        private MockSettings settings;
        [SetUp]
        public void SetUp()
        {
            Settings.Instance = null;
            this.settings = new MockSettings();
        }

        [Test]
        public void LengthOverloads()
        {
            var actual = this.settings.Length.OperatorOverloads.ToArray();
            var expected = new[]
                               {
                                   new OperatorOverload(this.settings.Length, this.settings.Time, this.settings.AllUnits),
                                   new OperatorOverload(this.settings.Length, this.settings.Speed, this.settings.AllUnits),
                                   new OperatorOverload(this.settings.Length, this.settings.Energy, this.settings.AllUnits),
                                   new OperatorOverload(this.settings.Length, this.settings.Area, this.settings.AllUnits),
                                   new OperatorOverload(this.settings.Length, this.settings.Volume, this.settings.AllUnits)
                               };
            CollectionAssert.AreEqual(expected.Select(x => x.ToString()), actual.Select(x => x.ToString()));
        }

        [Test]
        public void TimeOverloads()
        {
            var actual = this.settings.Time.OperatorOverloads.ToArray();
            var expected = new[]
                               {
                                   new OperatorOverload(this.settings.Time, this.settings.Length, this.settings.AllUnits),
                                   new OperatorOverload(this.settings.Time, this.settings.ElectricCharge, this.settings.AllUnits),
                               };
            CollectionAssert.AreEqual(expected.Select(x => x.ToString()), actual.Select(x => x.ToString()));
        }

        [Test]
        public void Inversions()
        {
            var actual = this.settings.Time.Inverse;
            Assert.AreEqual("1 / Time = Frequency", actual.ToString());
            Assert.IsNull(this.settings.Length.Inverse);
        }
    }
}
