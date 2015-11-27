namespace Gu.Units
{
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    internal static class DoubleFormatReader
    {
        internal static readonly IReadOnlyList<Regex> DoubleFormatRegexes = new[]
{
            CreateDoubleFormatPattern(@"(E|e)\d*"),
            CreateDoubleFormatPattern(@"(F|f)\d*"),
            CreateDoubleFormatPattern(@"(G|g)\d*"),
            CreateDoubleFormatPattern(@"(N|n)\d*"),
            CreateDoubleFormatPattern(@"(R|r)"),
            CreateDoubleFormatPattern(@"[0,#]*(\.[0,#]+)?"),
        };

        internal static bool TryReadDoubleFormat(string format, ref int pos, out string doubleFormat)
        {
            foreach (var regex in DoubleFormatRegexes)
            {
                var match = regex.Match(format, pos);
                if (!string.IsNullOrEmpty(match.Value))
                {
                    pos += match.Length;
                    doubleFormat = match.Value;
                    return true;
                }
            }

            doubleFormat = null;
            return false;
        }


        private static Regex CreateDoubleFormatPattern(string format)
        {
            var regex = new Regex($@"\G(?<format>{format})", RegexOptions.Singleline | RegexOptions.ExplicitCapture | RegexOptions.Compiled);
            return regex;
        }
    }
}
