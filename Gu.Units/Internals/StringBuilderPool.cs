namespace Gu.Units
{
    using System;
    using System.Collections.Concurrent;
    using System.Text;

    internal static class StringBuilderPool
    {
        private static readonly ConcurrentQueue<StringBuilder> Builders = new ConcurrentQueue<StringBuilder>();

        internal static Builder Borrow()
        {
            return new Builder();
        }

        internal sealed class Builder : IDisposable
        {
            private readonly StringBuilder _builder;
            private bool _disposed;

            internal Builder()
            {
                if (!Builders.TryDequeue(out _builder))
                {
                    _builder = new StringBuilder(12);
                }
            }

            public void Dispose()
            {
                if (_disposed)
                {
                    return;
                }

                _disposed = true;
                _builder.Clear();
                Builders.Enqueue(_builder);
            }

            public void Append(char c)
            {
                _builder.Append(c);
            }

            public void Append(string s)
            {
                _builder.Append(s);
            }

            public override string ToString()
            {
                return _builder.ToString();
            }
        }
    }
}
