namespace Gu.Units.Tests.Internals.Parsing
{
    using System.Collections;
    using System.Globalization;

    public interface ISuccessData
    {
        object Expected { get; }

        int ExpectedEnd { get; }

        int Start { get; }

        string Text { get; }

        CultureInfo CultureInfo { get; }

        object Parse(string text);

        object Parse(string text, CultureInfo cultureInfo);

        bool TryParse(string text, out object actual);

        bool TryParse(string text, CultureInfo cultureInfo, out object actual);
    }
}