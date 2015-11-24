namespace Gu.Units.Tests.Internals.Parsing
{
    using System.Collections;

    public interface ISuccessData
    {
        object Expected { get; }

        int ExpectedEnd { get; }

        int Start { get; }

        string Text { get; }
    }
}