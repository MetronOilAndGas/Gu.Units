namespace Gu.Units.Generator.Tests.Views
{
    using System.Linq;
    using NUnit.Framework;

    public class PrefixConversionsVmTests
    {
        [Test]
        public void SetBaseUnitToMetres()
        {
            var settings = new MockSettings();
            var vm = new PrefixConversionsVm(settings);
            CollectionAssert.IsEmpty(vm.Prefixes);

            vm.SetBaseUnit(settings.Metres);
            var expected = new[]
            {
                new PrefixConversionVm(settings.Metres.PrefixConversions, settings.Metres, settings.Milli),
                new PrefixConversionVm(settings.Metres.PrefixConversions, settings.Metres, settings.Kilo),
            };

            CollectionAssert.AreEqual(expected, vm.Prefixes.Single(), PrefixConversionVmComparer.Default);

            vm.SetBaseUnit(null);
            CollectionAssert.IsEmpty(vm.Prefixes);
        }

        [Test]
        public void SetBaseUnitToKilograms()
        {
            var settings = new MockSettings();
            var vm = new PrefixConversionsVm(settings);
            CollectionAssert.IsEmpty(vm.Prefixes);

            vm.SetBaseUnit(settings.Kilograms);
            //var expected = new[]
            //{
            //    new PrefixConversionVm(settings.Grams., new PrefixConversion("Millimetres","mm",settings.Milli)),
            //    new PrefixConversionVm(settings.Kilo, settings.Grams),
            //};

            //CollectionAssert.AreEqual(expected, vm.Prefixes.Single(), PrefixConversionVmComparer.Default);
            Assert.Fail();
            vm.SetBaseUnit(null);
            CollectionAssert.IsEmpty(vm.Prefixes);
        }
    }
}
