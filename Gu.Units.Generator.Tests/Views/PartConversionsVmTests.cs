namespace Gu.Units.Generator.Tests.Views
{
    using NUnit.Framework;

    public class PartConversionsVmTests
    {
        [Test]
        public void SetUnitToKilograms()
        {
            var settings = MockSettings.Create();
            var vm = new PartConversionsVm(settings);
            vm.SetUnit(settings.Kilograms);
            CollectionAssert.IsEmpty(vm.Conversions);
        }

        [Test]
        public void SetUnitToCubicMetres()
        {
            var settings = MockSettings.Create();
            var vm = new PartConversionsVm(settings);
            vm.SetUnit(settings.CubicMetres);
            CollectionAssert.AreEqual(new PartConversionsVm[0], vm.Conversions);
        }

        [Test]
        public void SetUnitToMetresPerSecond()
        {
            var settings = MockSettings.Create();
            var vm = new PartConversionsVm(settings);
            vm.SetUnit(settings.MetresPerSecond);
            CollectionAssert.AreEqual(new PartConversionsVm[0], vm.Conversions);
        }
    }
}