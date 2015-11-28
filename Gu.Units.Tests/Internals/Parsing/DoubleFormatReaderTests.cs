namespace Gu.Units.Tests.Internals.Parsing
{
    using System;
    using System.Diagnostics;
    using NUnit.Framework;

    public class DoubleFormatReaderTests
    {
        [TestCase(null, 0, null, 0)]
        [TestCase("", 0, null, 0)]
        [TestCase("E", 0, "E", 1)]
        [TestCase("e", 0, "e", 1)]
        [TestCase("E5", 0, "E5", 2)]
        [TestCase("E20", 0, "E20", 3)] // double
        [TestCase("e5", 0, "e5", 2)]
        [TestCase("F", 0, "F", 1)]
        [TestCase("f", 0, "f", 1)]
        [TestCase("F5", 0, "F5", 2)]
        [TestCase("f5", 0, "f5", 2)]
        [TestCase("G", 0, "G", 1)]
        [TestCase("g", 0, "g", 1)]
        [TestCase("G5", 0, "G5", 2)]
        [TestCase("g5", 0, "g5", 2)]
        [TestCase("N", 0, "N", 1)]
        [TestCase("n", 0, "n", 1)]
        [TestCase("N5", 0, "N5", 2)]
        [TestCase("n5", 0, "n5", 2)]
        [TestCase("R", 0, "R", 1)]
        [TestCase("r", 0, "r", 1)]
        [TestCase("0", 0, "0", 1)]
        [TestCase("0.00", 0, "0.00", 4)]
        [TestCase("#", 0, "#", 1)]
        [TestCase("#.#", 0, "#.#", 3)]
        [TestCase("#.0#", 0, "#.0#", 4)]
        [TestCase("#0.00#", 0, "#0.00#", 6)]
        public void TryRead(string text, int pos, string expected, int expectedPos)
        {
            string actual;
            var success = DoubleFormatReader.TryReadDoubleFormat(text, ref pos, out actual);
            Assert.AreEqual(true, success);
            Assert.AreEqual(expected, actual);
            Assert.AreEqual(expectedPos, pos);
        }

        [TestCase("J", 0, null)]
        [TestCase("J5", 0, null)] 
        [TestCase("E100", 0, "E101")] 
        [TestCase("E101", 0, "E111")] 
        [TestCase("E102", 0, "E112")] 
        [TestCase("E-1", 0, "E-1")] 
        [TestCase("abc", 0, "abc")]
        [TestCase("efg", 0, "efg")] 
        public void TryReadError(string text, int pos, string expectedFormatted)
        {
            string actual;
            var success = DoubleFormatReader.TryReadDoubleFormat(text, ref pos, out actual);
            Assert.AreEqual(false, success);
            Assert.AreEqual(text, actual);
            Assert.AreEqual(0, pos);
            string formatted = null;
            try
            {
                formatted = 1.2.ToString(text);
            }
            catch
            {
            }

            Assert.AreEqual(expectedFormatted, formatted);
        }

        // 2015-11-28| 10 000 000 times with optimized took: 497 ms
        // 2015-11-28| 10 000 000 times with regex     took: 3548 ms
        [Test, Explicit(Benchmarks.LongRunning)]
        public void Benchmark()
        {
            int pos = 0;
            string actual;
            DoubleFormatReader.TryReadDoubleFormat("e5", ref pos, out actual);
            pos = 0;
            DoubleFormatReader.TryReadDoubleFormat("#0.00#", ref pos, out actual);
            var sw = Stopwatch.StartNew();
            var n = 10000000;
            for (int i = 0; i < n; i++)
            {
                pos = 0;
                DoubleFormatReader.TryReadDoubleFormat("e5", ref pos, out actual);
            }

            sw.Stop();
            Console.WriteLine($"{DateTime.Today.ToShortDateString()}| {n:N0} times with optimized took: {sw.ElapsedMilliseconds} ms");

            sw.Restart();
            for (int i = 0; i < n; i++)
            {
                pos = 0;
                DoubleFormatReader.TryReadDoubleFormat("#0.00#", ref pos, out actual);
            }

            sw.Stop();
            Console.WriteLine($"{DateTime.Today.ToShortDateString()}| {n:N0} times with regex     took: {sw.ElapsedMilliseconds} ms");
        }

        [Test]
        public void Reminders()
        {
            Assert.Fail("Remove regexes :)");
        }
    }
}