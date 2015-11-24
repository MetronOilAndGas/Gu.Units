namespace Gu.Units.Tests.Internals.Parsing
{
    public class SuccessData<T>
    {
        public SuccessData(string text, int start, T expected, int expectedEnd)
        {
            Text = text;
            Start = start;
            Expected = expected;
            ExpectedEnd = expectedEnd;
        }

        public string Text { get; }

        public int Start { get; }

        public T Expected { get; }

        public int ExpectedEnd { get; }

        public override string ToString()
        {
            return $"Text: {Text}, Start: {Start}, Expected ({typeof(T)}): {Expected}, ExpectedEnd: {ExpectedEnd}";
        }
    }
}