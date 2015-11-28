namespace Gu.Units
{
    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    internal static partial class DoubleFormatReader
    {
        private static PrefixFormat eFormats = new PrefixFormat('e');
        private static PrefixFormat EFormats = new PrefixFormat('E');
        private static PrefixFormat fFormats = new PrefixFormat('f');
        private static PrefixFormat FFormats = new PrefixFormat('F');
        private static PrefixFormat gFormats = new PrefixFormat('g');
        private static PrefixFormat GFormats = new PrefixFormat('G');
        private static PrefixFormat nFormats = new PrefixFormat('n');
        private static PrefixFormat NFormats = new PrefixFormat('N');

        private static readonly Regex HashRegex = CreateDoubleFormatPattern(@"[0,#]*(\.[0,#]+)?");

        internal static bool TryReadDoubleFormat(string format, ref int pos, out string result)
        {
            if (string.IsNullOrEmpty(format) || pos == format.Length)
            {
                result = null;
                return true;
            }

            switch (format[pos])
            {
                case 'e':
                case 'E':
                case 'f':
                case 'F':
                case 'g':
                case 'G':
                case 'n':
                case 'N':
                    return TryReadPrefixNumberFormat(format, ref pos, out result);
                case 'r':
                case 'R':
                    return TryReadRFormat(format, ref pos, out result);
                case '0':
                case '#':
                    return TryReadPoundAndZeroFormat(format, ref pos, out result);
                default:
                    result = format;
                    return false;
            }

            result = null;
            return false;
        }

        private static bool TryReadPoundAndZeroFormat(string format, ref int pos, out string result)
        {
            throw new NotImplementedException();
        }

        private static bool TryReadPrefixNumberFormat(string format, ref int pos, out string result)
        {
            var start = pos;
            var c = format[pos];
            pos++;
            int intResult;
            if (IntReader.TryReadInt32(format, ref pos, out intResult))
            {
                if (IsEndOfFormat(format, pos))
                {
                    if (intResult < 0 || intResult >= 100)
                    {
                        result = $"{c}{intResult}";
                        pos = start;
                        return false;
                    }

                    if (intResult < 18)
                    {
                        switch (c)
                        {
                            case 'e':
                                result = eFormats.Formats[intResult];
                                return true;
                            case 'E':
                                result = EFormats.Formats[intResult];
                                return true;
                            case 'f':
                                result = fFormats.Formats[intResult];
                                return true;
                            case 'F':
                                result = FFormats.Formats[intResult];
                                return true;
                            case 'g':
                                result = gFormats.Formats[intResult];
                                return true;
                            case 'G':
                                result = GFormats.Formats[intResult];
                                return true;
                            case 'n':
                                result = nFormats.Formats[intResult];
                                return true;
                            case 'N':
                                result = NFormats.Formats[intResult]; ;
                                return true;
                            default:
                                result = format;
                                pos = start;
                                return false;
                        }
                    }

                    result = $"{c}{intResult}";
                    return true;
                }

                result = format;
                pos = start;
                return false;
            }

            if (IsEndOfFormat(format, pos))
            {
                switch (c)
                {
                    case 'e':
                        result = "e";
                        return true;
                    case 'E':
                        result = "E";
                        return true;
                    case 'f':
                        result = "f";
                        return true;
                    case 'F':
                        result = "F";
                        return true;
                    case 'g':
                        result = "g";
                        return true;
                    case 'G':
                        result = "G";
                        return true;
                    case 'n':
                        result = "n";
                        return true;
                    case 'N':
                        result = "N";
                        return true;
                    default:
                        result = format;
                        pos = start;
                        return false;
                }
            }

            result = format;
            pos = start;
            return false;
        }

        private static bool TryReadRFormat(string format, ref int pos, out string result)
        {
            var r = format[pos];
            pos++;
            if (IsEndOfFormat(format, pos))
            {
                switch (r)
                {
                    case 'r':
                        result = "r";
                        return true;
                    case 'R':
                        result = "R";
                        return true;
                    default:
                        result = r.ToString();
                        return false;
                }
            }

            result = format;
            return false;
        }

        private static bool IsEndOfFormat(string format, int pos)
        {
            if (format.Length == pos)
            {
                return true;
            }

            if (char.IsWhiteSpace(format[pos]))
            {
                return true;
            }

            return format[pos] == '}';
        }

        private static Regex CreateDoubleFormatPattern(string format)
        {
            var regex = new Regex($@"\G(?<format>{format})", RegexOptions.Singleline | RegexOptions.ExplicitCapture | RegexOptions.Compiled);
            return regex;
        }

        private class PrefixFormat
        {
            internal readonly char Prefix;

            internal readonly string[] Formats;

            public PrefixFormat(char prefix)
            {
                this.Prefix = prefix;
                this.Formats = new string[18];
                for (int i = 0; i < this.Formats.Length; i++)
                {
                    this.Formats[i] = $"{this.Prefix}{i}";
                }
            }
        }
    }
}
