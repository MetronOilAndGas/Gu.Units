namespace Gu.Units
{
    internal static class OperatorParser
    {
        private const char MultiplyDot = '⋅';
        private const char MultiplyStar = '*';
        private const char MultiplyX = 'x';
        private const char Divide = '/';

        internal static MultiplyOrDivide Parse(string s, ref int pos)
        {
            if (pos == s.Length)
            {
                return MultiplyOrDivide.None;
            }

            if (s[pos] == MultiplyDot || s[pos] == MultiplyStar || s[pos] == MultiplyX)
            {
                pos++;
                return MultiplyOrDivide.Multiply;
            }

            if (s[pos] == Divide)
            {
                pos++;
                return MultiplyOrDivide.Division;
            }

            return MultiplyOrDivide.None;
        }

        internal static Sign ReadSign(string s, ref int pos)
        {
            var sign = Sign.None;
            if (s[pos] == '+')
            {
                sign = Sign.Positive;
            }

            if (s[pos] == '-')
            {
                sign = Sign.Negative;
            }

            if (sign != Sign.None)
            {
                pos++;
            }
            return sign;
        }
    }
}