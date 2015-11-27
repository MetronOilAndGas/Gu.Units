namespace Gu.Units
{
    internal static class StringExt
    {
        internal static void ReadWhiteSpace(this string text, ref int pos)
        {
            while (text.Length > pos && char.IsWhiteSpace(text[pos]))
            {
                pos++;
            }
        }

        internal static bool IsRestWhiteSpace(this string text, int position)
        {
           text.ReadWhiteSpace(ref position);
            return position == text.Length;
        }
    }
}