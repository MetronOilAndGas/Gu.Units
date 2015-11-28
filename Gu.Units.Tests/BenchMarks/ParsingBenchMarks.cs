namespace Gu.Units.Tests
{
    using System;
    using System.Diagnostics;
    using Internals.Parsing;
    using NUnit.Framework;

    // run benchmarks in release build
    [Explicit(Benchmarks.LongRunning)]
    public class ParsingBenchmarks
    {
        // 2015-11-28| TryReadDoubleFormat("e5") 10 000 000 times with optimized took: 497 ms
        // 2015-11-28| TryReadDoubleFormat("#0.00#") 10 000 000 times with regex     took: 3548 ms
        // 2015-11-28| TryReadDoubleFormat("#0.00#") 10 000 000 times with regex     took: 232 ms
        [Test]
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
            Console.WriteLine($"// {DateTime.Today.ToShortDateString()}| TryReadDoubleFormat(\"e5\") {n:N0} times with optimized took: {sw.ElapsedMilliseconds} ms");

            sw.Restart();
            for (int i = 0; i < n; i++)
            {
                pos = 0;
                DoubleFormatReader.TryReadDoubleFormat("#0.00#", ref pos, out actual);
            }

            sw.Stop();
            Console.WriteLine($"// {DateTime.Today.ToShortDateString()}| TryReadDoubleFormat(\"#0.00#\") {n:N0} times with regex     took: {sw.ElapsedMilliseconds} ms");
        }
    }
}
