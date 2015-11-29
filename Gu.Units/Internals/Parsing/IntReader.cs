namespace Gu.Units
{
    using System;
    using System.Runtime.CompilerServices;

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

            long temp = GetDigitOrMinusOne(text[pos]);
            if (temp < 0)
            {
                result = 0;
                pos = start;
                return false;
            }
            int i;
            pos++;
            while (pos < text.Length &&
                   (i = GetDigitOrMinusOne(text[pos])) != -1)
            {
                temp *= 10;
                temp += i;
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static bool IsDigit(char c)
        {
            switch (c)
            {
                case '0':
                case '1':
                case '2':
                case '3':
                case '4':
                case '5':
                case '6':
                case '7':
                case '8':
                case '9':
                    return true;
                default:
                    return false;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static int GetDigit(char c)
        {
            switch (c)
            {
                case '0':
                    return 0;
                case '1':
                    return 1;
                case '2':
                    return 2;
                case '3':
                    return 3;
                case '4':
                    return 4;
                case '5':
                    return 5;
                case '6':
                    return 6;
                case '7':
                    return 7;
                case '8':
                    return 8;
                case '9':
                    return 9;
                default:
                    throw new ArgumentOutOfRangeException(nameof(c), $"{c} is not a digit, check before calling");
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static int GetDigitOrMinusOne(char c)
        {
            var i = c - '0';
            return i < 0 || i > 9
                ? -1
                : i;
        }
    }
}
