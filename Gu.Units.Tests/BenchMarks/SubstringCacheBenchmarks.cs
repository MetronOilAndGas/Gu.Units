namespace Gu.Units.Tests
{
    using System;
    using System.Diagnostics;
    using Internals.Parsing;
    using NUnit.Framework;

    // run benchmarks in release build
    [Explicit(Benchmarks.LongRunning)]
    public class SubstringCacheBenchmarks
    {
        // 2015-11-28| cache.TryFind(text, 3, out cached) 10 000 000 times with cache took: 595 ms using ReaderWriterLoclSlim
        // 2015-11-28| cache.TryFind(text, 3, out cached) 10 000 000 times with cache took: 408 ms
        // 2015-11-28| text.Substring(3, 3)               10 000 000 times                  took: 205 ms
        [Test]
        public void BenchmarkWorstCase()
        {
            // this is not a really meaningful comparison as Find does much more work
            // finding the end while substring gets the end for free.
            var cache = new SubstringCache<string>();
            cache.Add(new SubstringCache<string>.CachedItem("abc0", "d"));
            cache.Add(new SubstringCache<string>.CachedItem("abc01", "e"));
            cache.Add(new SubstringCache<string>.CachedItem("abc1", "e"));
            cache.Add(new SubstringCache<string>.CachedItem("abc11", "e"));
            cache.Add(new SubstringCache<string>.CachedItem("abc2", "f"));
            cache.Add(new SubstringCache<string>.CachedItem("abc21", "f"));

            const string text0 = "   abc0   ";
            const string text1 = "   abc1   ";
            const string text2 = "   abc2   ";
            SubstringCache<string>.CachedItem cached;
            cache.TryFind(text0, 4, out cached);

            var subString = text0.Substring(3, 4);

            var sw = Stopwatch.StartNew();
            var n = 10000000;
            for (int i = 0; i < n; i++)
            {
                switch (i % 3)
                {
                    case 0:
                        cache.TryFind(text0, 4, out cached);
                        continue;
                    case 1:
                        cache.TryFind(text1, 4, out cached);
                        continue;
                    case 2:
                        cache.TryFind(text2, 4, out cached);
                        continue;
                    default:
                        throw new Exception();
                }
            }

            sw.Stop();
            Console.WriteLine($"// {DateTime.Today.ToShortDateString()}| cache.TryFind(text, 4, out cached) {n:N0} times with cache took: {sw.ElapsedMilliseconds} ms");

            sw.Restart();
            for (int i = 0; i < n; i++)
            {
                switch (i % 3)
                {
                    case 0:
                        subString = text0.Substring(3, 4);
                        continue;
                    case 1:
                        subString = text1.Substring(3, 4);
                        continue;
                    case 2:
                        subString = text2.Substring(3, 4);
                        continue;
                    default:
                        throw new Exception();
                }
            }

            sw.Stop();
            Console.WriteLine($"// {DateTime.Today.ToShortDateString()}| text.Substring(3, 4)               {n:N0} times            took: {sw.ElapsedMilliseconds} ms");
        }
    }
}
