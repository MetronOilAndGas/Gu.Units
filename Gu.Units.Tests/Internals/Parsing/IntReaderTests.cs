namespace Gu.Units.Tests.Internals.Parsing
{
    using System;
    using System.Collections.Generic;
    using NUnit.Framework;

    public class IntReaderTests
    {
        [TestCaseSource(nameof(SuccessSource))]
        public void ParseSuccess(SuccessData<int> data)
        {
            int pos = data.Start;
            var actual = IntReader.ReadInt32(data.Text, ref pos);
            Assert.AreEqual(actual, data.Expected);
            Assert.AreEqual(pos, data.ExpectedEnd);
        }

        [TestCaseSource(nameof(ErrorSource))]
        public void ParseError(ErrorData<int> data)
        {
            int pos = data.Start;
            Assert.Throws<FormatException>(() => IntReader.ReadInt32(data.Text, ref pos));
            Assert.AreEqual(pos, data.ExpectedEnd);
        }

        [TestCaseSource(nameof(SuccessSource))]
        public void TryParseSuccess(SuccessData<int> data)
        {
            int pos = data.Start;
            int actual;
            var success = IntReader.TryReadInt32(data.Text, ref pos, out actual);
            Assert.AreEqual(true, success);
            Assert.AreEqual(actual, data.Expected);
            Assert.AreEqual(pos, data.ExpectedEnd);
        }

        [TestCaseSource(nameof(ErrorSource))]
        public void TryParseError(ErrorData<int> data)
        {
            int pos = data.Start;
            int actual;
            var success = IntReader.TryReadInt32(data.Text, ref pos, out actual);
            Assert.AreEqual(false, success);
            Assert.AreEqual(actual, data.Expected);
            Assert.AreEqual(pos, data.ExpectedEnd);
        }

        private static readonly IReadOnlyList<SuccessData<int>> SuccessSource = new[]
        {
            SuccessData.Create("1", 0, 1, 1),
            SuccessData.Create("12", 0, 12, 2),
            SuccessData.Create("-1", 0, -1, 2),
            SuccessData.Create("-12", 0, -12, 3),
        };

        private static readonly IReadOnlyList<SuccessData<int>> ErrorSource = new[]
        {
            ErrorData.Create("-2147483649", 0, 0), // less than int.min
            ErrorData.Create("2147483648", 0, 0), // greater than int.max
        };
    }
}
