namespace Gu.Units.Tests
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.Linq;
    using Internals.Parsing;
    using NUnit.Framework;
    using Units.Internals;

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
            //DoubleReader.TryRead(text, ref pos, NumberStyles.Float, CultureInfo.InvariantCulture, out actual);
            //var sw = Stopwatch.StartNew();
            var n = 1000000;
            for (int i = 0; i < n; i++)
            {
                pos = 4;
                DoubleReader.TryRead(text, ref pos, NumberStyles.Float, CultureInfo.InvariantCulture, out actual);
            }

            //sw.Stop();
            //Console.WriteLine($"// {DateTime.Today.ToShortDateString()}| DoubleReader.TryRead(\"  1.2\", 4, ...)  {n:N0} times              took: {sw.ElapsedMilliseconds} ms");

            //sw.Restart();
            //for (int i = 0; i < n; i++)
            //{
            //    var substring = text.Substring(4);
            //    double.TryParse(substring, NumberStyles.Float, CultureInfo.InvariantCulture, out actual);
            //}

            //sw.Stop();
            //Console.WriteLine($"// {DateTime.Today.ToShortDateString()}| double.TryParse(substring, ...)        {n:N0} times              took: {sw.ElapsedMilliseconds} ms");
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

        // 2015-11-28| IntReader.TryReadInt32("  12", 4, ...) 1 000 000 times               took: 40 ms
        // 2015-11-28| int.TryParse(substring, ...)           1 000 000 times               took: 134 ms
        [Test]
        public void TryGetValue()
        {
            string actual;
            var dict = new Dictionary<int, string>();
            for (int i = 0; i < 10; i++)
            {
                dict[i] = i.ToString();
            }
            dict.TryGetValue(2, out actual);
            var sw = Stopwatch.StartNew();
            var n = 1000000;
            for (int i = 0; i < n; i++)
            {
                dict.TryGetValue(i % 10, out actual);
            }

            sw.Stop();
            Console.WriteLine($"// {DateTime.Today.ToShortDateString()}| dict.TryGetValue(i % 10, out actual)       {n:N0} times {sw.ElapsedMilliseconds} ms");

            var cdict = new ConcurrentDictionary<int, string>();
            for (int i = 0; i < 10; i++)
            {
                cdict[i] = i.ToString();
            }

            cdict.TryGetValue(2, out actual);
            sw.Restart();
            for (int i = 0; i < n; i++)
            {
                cdict.TryGetValue(i % 10, out actual);
            }

            sw.Stop();
            Console.WriteLine($"// {DateTime.Today.ToShortDateString()}| cdict.TryGetValue(i % 10, out actual);     {n:N0} times {sw.ElapsedMilliseconds} ms");

            var slist = new SortedList<int, string>();
            for (int i = 0; i < 10; i++)
            {
                slist[i] = i.ToString();
            }

            slist.TryGetValue(2, out actual);
            sw.Restart();
            for (int i = 0; i < n; i++)
            {
                slist.TryGetValue(i % 10, out actual);
            }

            sw.Stop();
            Console.WriteLine($"// {DateTime.Today.ToShortDateString()}| slist.TryGetValue(i % 10, out actual);     {n:N0} times {sw.ElapsedMilliseconds} ms");

            var array = new KeyValuePair<int, string>[10];
            for (int i = 0; i < 10; i++)
            {
                array[i] = new KeyValuePair<int, string>(i, i.ToString());
            }

            var kvp = Array.Find(array, x => x.Key == 2);
            sw.Restart();
            for (int i = 0; i < n; i++)
            {
                kvp = Array.Find(array, x => x.Key == i % 10);
            }

            sw.Stop();
            Console.WriteLine($"// {DateTime.Today.ToShortDateString()}| Array.Find(array, x => x.Key == i % 10)    {n:N0} times {sw.ElapsedMilliseconds} ms");

            sw.Restart();
            var kvpComparer = new KvpComparer();

            for (int i = 0; i < n; i++)
            {
                var j = Array.BinarySearch(array, array[i % 10], kvpComparer);
            }

            sw.Stop();
            Console.WriteLine($"// {DateTime.Today.ToShortDateString()}| Array.BinarySearch(array, array[i % 10])   {n:N0} times {sw.ElapsedMilliseconds} ms");

            sw.Restart();

            for (int i = 0; i < n; i++)
            {
                var j = array.BinarySearchBy(x => x.Key, i % 10);
            }

            sw.Stop();
            Console.WriteLine($"// {DateTime.Today.ToShortDateString()}| array.BinarySearchBy(x => x.Key, i % 10    {n:N0} times {sw.ElapsedMilliseconds} ms");

            sw.Restart();

            for (int i = 0; i < n; i++)
            {
                kvp = array.First(x => x.Key == i % 10);
            }

            sw.Stop();
            Console.WriteLine($"// {DateTime.Today.ToShortDateString()}| array.First(x => x.Key == i % 10)          {n:N0} times {sw.ElapsedMilliseconds} ms");
        }

        private class KvpComparer : IComparer<KeyValuePair<int, string>>
        {
            public int Compare(KeyValuePair<int, string> x, KeyValuePair<int, string> y)
            {
                return x.Key.CompareTo(y.Key);
            }
        }
    }
}
