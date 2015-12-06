namespace Gu.Units.Generator
{
    using System;

    public static class StringExt
    {
       public static string ToFirstCharLower(this string text)
       {
            return Char.ToLower(text[0]) + text.Substring(1);
        }
    }
}
