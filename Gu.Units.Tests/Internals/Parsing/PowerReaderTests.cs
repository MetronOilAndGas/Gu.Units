namespace Gu.Units.Tests.Internals.Parsing
{
    using System.Collections.Generic;
    using NUnit.Framework;

    public class PowerReaderTests
    {
        [TestCaseSource(nameof(SuccessSource))]
        public void ReadSuccess(SuccessData<int> data)
        {
            var end =data.Start;
            var actual = PowerReader.Read(data.Text, ref end);
            Assert.AreEqual(data.Expected, actual);
            Assert.AreEqual(data.ExpectedEnd, end);
        }

        [TestCaseSource(nameof(SuccessSource))]
        public void TryReadSuccess(SuccessData<int> data)
        {
            var end = data.Start;
            int actual;
            var success = PowerReader.TryRead(data.Text, ref end, out actual);
            Assert.AreEqual(true, success);
            Assert.AreEqual(data.Expected, actual);
            Assert.AreEqual(data.ExpectedEnd, end);
        }

        private const string Superscripts = "⋅⁺⁻⁰¹²³⁴⁵⁶⁷⁸⁹"; // keeping this here for copy pasting
        private static readonly IReadOnlyList<SuccessData<int>> SuccessSource = new[]
        {
            SuccessData.Create("mm^2", 2, 2, 4),
            SuccessData.Create("mm⁰", 2, 2, 4),
            SuccessData.Create("mm²", 2, 2, 4),
            SuccessData.Create("mm⁺²", 2, 2, 4),
            SuccessData.Create("mm⁻¹²", 2, 2, 4),
            SuccessData.Create("mm", 2, 1, 2),
        };
    }
}
