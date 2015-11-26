namespace Gu.Units.Tests.Internals
{
    using System.Reflection;
    using System.Text;
    using NUnit.Framework;

    public class StringBuilderPoolTests
    {
        [Test]
        public void UseTwice()
        {
            StringBuilder inner1;
            using (var builder = StringBuilderPool.Borrow())
            {
                builder.Append("a");
                inner1 = GetInner(builder);
                Assert.AreEqual("a", builder.ToString());
            }

            using (var builder = StringBuilderPool.Borrow())
            {
                builder.Append("bc");
                var inner2 = GetInner(builder);
                Assert.AreSame(inner1, inner2);
                Assert.AreEqual("bc", builder.ToString());
            }
        }

        private static StringBuilder GetInner(StringBuilderPool.Builder outer)
        {
            var fieldInfo = typeof(StringBuilderPool.Builder).GetField("_builder", BindingFlags.Instance | BindingFlags.NonPublic);
            return (StringBuilder)fieldInfo.GetValue(outer);
        }
    }
}
