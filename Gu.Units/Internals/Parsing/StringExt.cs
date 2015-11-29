namespace Gu.Units
{
    internal static class StringExt
    {
        internal static void TryReadWhiteSpace(this string text, ref int pos)
        {
            if (text == null)
            {
                return;
            }

            while (text.Length > pos && char.IsWhiteSpace(text[pos]))
            {
                pos++;
            }
        }

        internal static bool IsRestWhiteSpace(this string text, int position)
        {
            if (text == null)
            {
                return true;
            }

            text.TryReadWhiteSpace(ref position);
            return position == text.Length;
        }

        internal static bool IsRestWhiteSpace(this string text, ref int position, int end)
        {
            text.TryReadWhiteSpace(ref position);
            return position == text.Length || position >= end;
        }

        internal static bool TryReadPadding(this string text,
            ref int pos,
            out string padding)
        {
            if (text == null)
            {
                padding = null;
                return false;
            }

            var start = pos;
            text.TryReadWhiteSpace(ref pos);
            if (pos == start)
            {
                padding = null;
                return false;
            }

            if (pos == start + 1)
            {
                switch (text[start])
                {
                    case ' ':
                    {
                        padding = " ";
                        return true;
                    }

                    case '\u00A0':
                    {
                        padding = "\u00A0";
                        return true;
                    }
                }
            }

            padding = text.Substring(start, pos - start);
            return true;
        }
    }
}