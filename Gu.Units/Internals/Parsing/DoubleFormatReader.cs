namespace Gu.Units
{
    internal static class DoubleFormatReader
    {
        private static readonly PrefixFormat eFormats = new PrefixFormat('e');
        private static readonly PrefixFormat EFormats = new PrefixFormat('E');
        private static readonly PrefixFormat fFormats = new PrefixFormat('f');
        private static readonly PrefixFormat FFormats = new PrefixFormat('F');
        private static readonly PrefixFormat gFormats = new PrefixFormat('g');
        private static readonly PrefixFormat GFormats = new PrefixFormat('G');
        private static readonly PrefixFormat nFormats = new PrefixFormat('n');
        private static readonly PrefixFormat NFormats = new PrefixFormat('N');

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
        }

        private static bool TryReadPoundAndZeroFormat(string format,
            ref int pos,
            out string result)
        {
            var start = pos;
            while (!IsEndOfPoundAndZeroFormat(format, pos))
            {
                switch (format[pos])
                {
                    case '#':
                    case '0':
                    case '.':
                    case ',':
                        pos++;
                        continue;
                    case ' ':
                    case '\u00A0':
                        {
                            if (format.Length == pos + 1)
                            {
                                break;
                            }
                            switch (format[pos + 1])
                            {
                                case '#':
                                case '0':
                                    {
                                        pos++;
                                        continue;
                                    }
                            }
                            goto default;
                        }
                    default:
                        {
                            result = format;
                            pos = start;
                            return false;
                        }
                }
            }

            result = format.Substring(start, pos - start);
            return true;
        }

        private static bool TryReadPrefixNumberFormat(string format, ref int pos, out string result)
        {
            var start = pos;
            var c = format[pos];
            pos++;
            int intResult;
            if (IntReader.TryReadInt32(format, ref pos, out intResult))
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

        private static bool TryReadRFormat(string format, ref int pos, out string result)
        {
            switch (format[pos])
            {
                case 'r':
                    result = "r";
                    pos++;
                    return true;
                case 'R':
                    result = "R";
                    pos++;
                    return true;
                default:
                    result = format;
                    return false;
            }
        }

        private static bool IsEndOfPoundAndZeroFormat(string format, int pos)
        {
            if (format.Length == pos)
            {
                return true;
            }

            switch (format[pos])
            {
                case '}':
                    return true;
                case '#':
                case '0':
                    return false;
                case ',':
                case '.':
                    if (format.Length == pos + 1)
                    {
                        return true;
                    }

                    switch (format[pos + 1])
                    {
                        case '#':
                        case '0':
                            return false;
                        default:
                            return true;
                    }
            }

            if (char.IsWhiteSpace(format[pos]))
            {
                if (format.Length == pos + 1)
                {
                    return true;
                }

                switch (format[pos+1])
                {
                    case '#':
                    case '0':
                        return false;
                    default:
                        return true;
                }
            }

            return true;
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
