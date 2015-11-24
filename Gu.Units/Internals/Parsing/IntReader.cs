namespace Gu.Units
{
    using System;

    internal static class IntReader
    {
        internal static int Read(string text, ref int pos)
        {
            int result;
            if (TryRead(text, ref pos, out result))
            {
                return result;
            }

            throw new FormatException($"Expected int starting at pos: {pos} in {text} was {text[pos]}");
        }

        internal static bool TryRead(string text, ref int pos, out int result)
        {
            if (!char.IsDigit(text[pos]))
            {
                result = 0;
                return false;
            }

            result = (int)char.GetNumericValue(text[pos]);
            pos++;
            while (char.IsDigit(text[pos]) &&
                   pos < text.Length)
            {
                result *= 10;
                result += (int)char.GetNumericValue(text[pos]);
                pos++;
            }

            return true;
        }
    }
}
