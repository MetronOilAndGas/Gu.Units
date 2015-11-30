namespace Gu.Units
{
    internal static class ULongReader
    {
        internal static bool TryRead(string text,
            ref int pos,
            out ulong result)
        {
            if (pos == text.Length)
            {
                result = 0;
                return false;
            }
            var start = pos;
            result = 0;

            while (pos < text.Length)
            {
                var i = text[pos] - '0';
                if (i < 0 || 9 < i)
                {
                    break;
                }
                try
                {
                    pos++;
                    result *= 10;
                    result += (ulong)i;
                }
                catch
                {
                    // expecting this to never happen so try catching overflow is an optimization
                    result = 0;
                    pos = start;
                    return false;
                }
            }

            return true;
        }
    }
}