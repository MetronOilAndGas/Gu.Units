namespace Gu.Units.Tests.Internals.Parsing
{
    using System.Collections.Generic;
    using NUnit.Framework;

    public class IntReaderTests
    {
        [TestCaseSource(nameof(SuccessSource))]
        public void ParseSuccess(SuccessData<int> data)
        {
            int pos = data.Start;
            var actual = IntReader.Read(data.Text, ref pos);
            Assert.AreEqual(actual, data.Expected);
            Assert.AreEqual(pos, data.ExpectedEnd);
        }

        [TestCaseSource(nameof(SuccessSource))]
        public void TryParseSuccess(SuccessData<int> data)
        {
            int pos = data.Start;
            int actual;
            var success = IntReader.TryRead(data.Text, ref pos, out actual);
            Assert.AreEqual(true, success);
            Assert.AreEqual(actual, data.Expected);
            Assert.AreEqual(pos, data.ExpectedEnd);
        }

        private static readonly IReadOnlyList<SuccessData<int>> SuccessSource = new[]
        {
            SuccessData.Create("1", 0, 1, 1),
            SuccessData.Create("12", 0, 12, 12),
            SuccessData.Create("-1", 0, 12, 12),
        };
    }
}
