namespace Gu.Units
{
    using System;

    internal static class StringExt
    {
        internal static void ReadWhiteSpace(this string s, ref int pos)
        {
            while (s.Length > pos && char.IsWhiteSpace(s[pos]))
            {
                pos++;
            }
        }
    }
}