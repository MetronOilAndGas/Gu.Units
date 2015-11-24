namespace Gu.Units
{
    internal static class OperatorParser
    {
        private const char MultiplyDot = '⋅';
        private const char MultiplyStar = '*';
        private const char MultiplyX = 'x';
        private const char Divide = '/';


        internal static Operator Parse(string s, ref int pos)
        {
            if (pos == s.Length)
            {
                return Operator.None;
            }

            if (s[pos] == MultiplyDot || s[pos] == MultiplyStar || s[pos] == MultiplyX)
            {
                pos++;
                return Operator.Multiply;
            }

            if (s[pos] == Divide)
            {
                pos++;
                return Operator.Division;
            }

            return Operator.None;
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