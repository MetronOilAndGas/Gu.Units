namespace Gu.Units
{
    internal static class StringExt
    {
        internal static void TryReadWhiteSpace(this string text, ref int pos)
        {
            while (text.Length > pos && char.IsWhiteSpace(text[pos]))
            {
                pos++;
            }
        }

        internal static bool IsRestWhiteSpace(this string text, int position)
        {
            text.TryReadWhiteSpace(ref position);
            return position == text.Length;
        }

        internal static bool IsRestWhiteSpace(this string text, ref int position, int end)
        {
            text.TryReadWhiteSpace(ref position);
            return position == text.Length || position >= end;
        }
    }
}