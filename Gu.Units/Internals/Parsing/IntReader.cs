namespace Gu.Units
{
    using System;

    internal static class IntReader
    {
        internal static int ReadInt32(string text, ref int pos)
        {
            int result;
            if (TryReadInt32(text, ref pos, out result))
            {
                return result;
            }

            throw new FormatException($"Expected int starting at pos: {pos} in {text} was {text[pos]}");
        }

        internal static bool TryReadInt32(string text, ref int pos, out int result)
        {
            if (pos == text.Length)
            {
                result = 0;
                return false;
            }

            var start = pos;
            var sign = OperatorReader.TryReadSign(text, ref pos);

            if (!char.IsDigit(text[pos]))
            {
                result = 0;
                pos = start;
                return false;
            }

            long temp = (int)char.GetNumericValue(text[pos]);
            pos++;
            while (pos < text.Length &&
                   char.IsDigit(text[pos]))
            {
                temp *= 10;
                temp += (int)char.GetNumericValue(text[pos]);
                pos++;
                if (temp > int.MaxValue)
                {
                    break;
                }
            }

            if (sign == Sign.Negative)
            {
                temp *= -1;
                if (temp < int.MinValue)
                {
                    result = 0;
                    pos = start;
                    return false;
                }
            }
            else
            {
                if (temp > int.MaxValue)
                {
                    result = 0;
                    pos = start;
                    return false;
                }
            }

            result = (int)temp;
            return true;
        }
    }
}
