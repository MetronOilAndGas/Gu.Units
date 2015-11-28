namespace Gu.Units.Tests
{
    using System;
    using System.Diagnostics;
    using System.Globalization;
    using Internals.Parsing;
    using NUnit.Framework;

    // run benchmarks in release build
    [Explicit(Benchmarks.LongRunning)]
    public class ParsingBenchmarks
    {
        // 2015-11-28| TryReadDoubleFormat("#0.00#") 1 000 000 times with regex      took: 355 ms
        // 2015-11-28| TryReadDoubleFormat("e5")     1 000 000 times with optimized  took: 50 ms <- don't remember what was different.
        // 2015-11-28| TryReadDoubleFormat("e5")     1 000 000 times                 took: 118 ms
        // 2015-11-28| TryReadDoubleFormat("#0.00#") 1 000 000 times                 took: 95 ms
        // 2015-11-28| TryReadDoubleFormat("e5")     1 000 000 times                 took: 45 ms
        // 2015-11-28| TryReadDoubleFormat("#0.00#") 1 000 000 times                 took: 22 ms
        [Test]
        public void TryReadDoubleFormat()
        {
            int pos = 0;
            string actual;
            DoubleFormatReader.TryReadDoubleFormat("e5", ref pos, out actual);
            pos = 0;
            DoubleFormatReader.TryReadDoubleFormat("#0.00#", ref pos, out actual);
            var sw = Stopwatch.StartNew();
            var n = 1000000;
            for (int i = 0; i < n; i++)
            {
                pos = 0;
                DoubleFormatReader.TryReadDoubleFormat("e5", ref pos, out actual);
            }

            sw.Stop();
            Console.WriteLine($"// {DateTime.Today.ToShortDateString()}| TryReadDoubleFormat(\"e5\")     {n:N0} times                 took: {sw.ElapsedMilliseconds} ms");

            sw.Restart();
            for (int i = 0; i < n; i++)
            {
                pos = 0;
                DoubleFormatReader.TryReadDoubleFormat("#0.00#", ref pos, out actual);
            }

            sw.Stop();
            Console.WriteLine($"// {DateTime.Today.ToShortDateString()}| TryReadDoubleFormat(\"#0.00#\") {n:N0} times                 took: {sw.ElapsedMilliseconds} ms");
        }

        // 2015-11-28| DoubleReader.TryRead("  1.2", 2, ...) 1 000 000 times               took: 273 ms
        // 2015-11-28| double.TryParse(substring, ...)        1 000 000 times              took: 112 ms
        [Test]
        public void TryReadDouble()
        {
            int pos = 4;
            double actual;
            const string text = "ab  1.2";
            DoubleReader.TryRead(text, ref pos, NumberStyles.Float, CultureInfo.InvariantCulture, out actual);
            var sw = Stopwatch.StartNew();
            var n = 1000000;
            for (int i = 0; i < n; i++)
            {
                pos = 4;
                DoubleReader.TryRead(text, ref pos, NumberStyles.Float, CultureInfo.InvariantCulture, out actual);
            }

            sw.Stop();
            Console.WriteLine($"// {DateTime.Today.ToShortDateString()}| DoubleReader.TryRead(\"  1.2\", 4, ...)  {n:N0} times              took: {sw.ElapsedMilliseconds} ms");

            sw.Restart();
            for (int i = 0; i < n; i++)
            {
                var substring = text.Substring(4);
                double.TryParse(substring, NumberStyles.Float, CultureInfo.InvariantCulture, out actual);
            }

            sw.Stop();
            Console.WriteLine($"// {DateTime.Today.ToShortDateString()}| double.TryParse(substring, ...)        {n:N0} times              took: {sw.ElapsedMilliseconds} ms");
        }

        // 2015-11-28| IntReader.TryReadInt32("  12", 4, ...) 1 000 000 times               took: 40 ms
        // 2015-11-28| int.TryParse(substring, ...)           1 000 000 times               took: 134 ms
        [Test]
        public void TryReadInt()
        {
            int pos = 4;
            int actual;
            const string text = "ab  12";
            IntReader.TryReadInt32(text, ref pos, out actual);
            var sw = Stopwatch.StartNew();
            var n = 1000000;
            for (int i = 0; i < n; i++)
            {
                pos = 4;
                IntReader.TryReadInt32(text, ref pos, out actual);
            }

            sw.Stop();
            Console.WriteLine($"// {DateTime.Today.ToShortDateString()}| IntReader.TryReadInt32(\"  12\", 4, ...) {n:N0} times               took: {sw.ElapsedMilliseconds} ms");

            sw.Restart();
            for (int i = 0; i < n; i++)
            {
                var substring = text.Substring(4);
                int.TryParse(substring, out actual);
            }

            sw.Stop();
            Console.WriteLine($"// {DateTime.Today.ToShortDateString()}| int.TryParse(substring, ...)           {n:N0} times               took: {sw.ElapsedMilliseconds} ms");
        }
    }
}
