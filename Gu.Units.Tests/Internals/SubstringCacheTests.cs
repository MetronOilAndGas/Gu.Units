namespace Gu.Units.Tests.Internals
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using NUnit.Framework;

    public class SubstringCacheTests
    {
        [Test]
        public void AddSameTwice()
        {
            var cache = new SubstringCache<string>();
            cache.Add(new SubstringCache<string>.CachedItem("abc", "d"));
            cache.Add(new SubstringCache<string>.CachedItem("abc", "d"));
            var cachedItems = GetInnerCache(cache);
            Assert.AreEqual(1, cachedItems.Count);
        }

        [Test]
        public void AddDifferentWithSameKeyTwice()
        {
            var cache = new SubstringCache<string>();
            cache.Add(new SubstringCache<string>.CachedItem("abc", "d"));
            Assert.Throws<InvalidOperationException>(() => cache.Add(new SubstringCache<string>.CachedItem("abc", "e")));
        }

        [TestCase("abc", 0)]
        [TestCase("abcdef", 0)]
        [TestCase(" abc", 1)]
        public void AddThenGetSuccess(string key, int pos)
        {
            var cache = new SubstringCache<string>();
            cache.Add(new SubstringCache<string>.CachedItem("abc", "d"));
            cache.Add(new SubstringCache<string>.CachedItem("foo", "e"));
            cache.Add(new SubstringCache<string>.CachedItem("bar", "f"));
            SubstringCache<string>.CachedItem actual;
            var success = cache.TryFind(key, pos, out actual);
            Assert.AreEqual(true, success);
            Assert.AreEqual("abc", actual.Key);
            Assert.AreEqual("d", actual.Value);
        }

        [TestCase(null, 0)]
        [TestCase("", 0)]
        [TestCase(" ggg", 1)]
        [TestCase("ggg", 1)]
        [TestCase("g", 1)]
        public void AddThenGetFail(string key, int pos)
        {
            var cache = new SubstringCache<string>();
            cache.Add(new SubstringCache<string>.CachedItem("abc", "d"));
            cache.Add(new SubstringCache<string>.CachedItem("foo", "e"));
            cache.Add(new SubstringCache<string>.CachedItem("bar", "f"));
            SubstringCache<string>.CachedItem actual;
            var success = cache.TryFind(key, pos, out actual);
            Assert.AreEqual(false, success);
            Assert.AreEqual(null, actual.Key);
            Assert.AreEqual(null, actual.Value);
        }

        private static List<SubstringCache<string>.CachedItem> GetInnerCache(SubstringCache<string> cache)
        {
            var fieldInfo = typeof(SubstringCache<string>).GetField("cache", BindingFlags.NonPublic | BindingFlags.Instance);
            return (List<SubstringCache<string>.CachedItem>)fieldInfo.GetValue(cache);
        }
    }
}
