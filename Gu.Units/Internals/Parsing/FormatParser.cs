namespace Gu.Units
{
    internal static class FormatParser
    {
        internal static bool TryParse<TUnit>(string format, out QuantityFormat<TUnit> result)
            where TUnit : struct, IUnit
        {
            if (string.IsNullOrWhiteSpace(format))
            {
                result = QuantityFormat<TUnit>.Default;
                return true;
            }

            int pos = 0;
            string doubleFormat;
            format.ReadWhiteSpace(ref pos);
            var prePaddingEnd = pos;
            var readDoubleFormat = DoubleFormatReader.TryReadDoubleFormat(format, ref pos, out doubleFormat);

            var spaceStart = pos;
            format.ReadWhiteSpace(ref pos);
            var spaceEnd = pos;
            string unitFormat;
            TUnit unit;
            var readUnitFormat = TryReadUnit(format, ref pos, out unitFormat, out unit);
            var unitEnd = pos;
            format.ReadWhiteSpace(ref pos);

            if (!(readDoubleFormat || readUnitFormat) || pos != format.Length)
            {
                result = QuantityFormat<TUnit>.Default;
                return false;
            }

            var compositeFormat = CreateCompositeFormat<TUnit>(format, prePaddingEnd, doubleFormat, spaceStart, spaceEnd, unitFormat, unitEnd);
            result = new QuantityFormat<TUnit>(compositeFormat, unit);
            return true;
        }

        internal static bool TryParse<TUnit>(string format, TUnit unit, out QuantityFormat<TUnit> result)
            where TUnit : struct, IUnit
        {
            int pos = 0;
            string doubleFormat;
            format.ReadWhiteSpace(ref pos);
            var prePaddingEnd = pos;
            var readDoubleFormat = DoubleFormatReader.TryReadDoubleFormat(format, ref pos, out doubleFormat);
            var spaceStart = pos;
            format.ReadWhiteSpace(ref pos);
            var spaceEnd = pos;
            string unitFormat;
            TUnit readUnit;
            var readUnitFormat = TryReadUnit(format, ref pos, out unitFormat, out readUnit);
            var unitEnd = pos;

            format.ReadWhiteSpace(ref pos);

            if (!(readDoubleFormat || readUnitFormat) || pos != format.Length)
            {
                result = QuantityFormat<TUnit>.Default;
                return false;
            }

            if (readUnitFormat && !Equals(readUnit, unit))
            {
                // choosing the format unit here
                var cf = CreateCompositeFormat<TUnit>(format, prePaddingEnd, doubleFormat, spaceStart, spaceEnd, unitFormat, unitEnd);
                result = new QuantityFormat<TUnit>(cf, readUnit);
                return false;
            }


            var compositeFormat = string.IsNullOrEmpty(unitFormat) && spaceStart == spaceEnd
                ? CreateCompositeFormat<TUnit>(format, prePaddingEnd, doubleFormat, unit)
                : CreateCompositeFormat<TUnit>(format, prePaddingEnd, doubleFormat, spaceStart, spaceEnd, unitFormat ?? unit.Symbol, unitEnd);
            result = new QuantityFormat<TUnit>(compositeFormat, unit);
            return true;
        }

        internal static QuantityFormat<TUnit> Create<TUnit>(FormatAndUnit<TUnit> fau) where TUnit : struct, IUnit
        {
            if (string.IsNullOrEmpty(fau._format))
            {
                using (var builder = StringBuilderPool.Borrow())
                {
                    builder.AppendDoubleFormat(null);
                    var unit = fau._unit ?? (TUnit)default(TUnit).SiUnit;

                    builder.AppendUnit(unit);
                    return new QuantityFormat<TUnit>(builder.ToString(), unit);
                }
            }

            QuantityFormat<TUnit> result;
            if (fau._unit.HasValue)
            {
                TryParse(fau._format, fau._unit.Value, out result);
            }
            else
            {
                TryParse(fau._format, out result);
            }
            return result;
        }

        private static bool TryReadUnit<TUnit>(string format,
            ref int pos,
            out string unitFormat,
            out TUnit unit) where TUnit : IUnit
        {
            var start = pos;
            if (pos == format.Length)
            {
                unit = (TUnit)default(TUnit).SiUnit;
                unitFormat = null;
                return false;
            }

            if (UnitParser.TryParse<TUnit>(format, ref pos, out unit))
            {
                unitFormat = format.Substring(start, pos - start);
                return true;
            }

            unit = (TUnit)default(TUnit).SiUnit;
            unitFormat = null;
            return false;
        }

        private static string CreateCompositeFormat<TUnit>(string format,
            int prePaddingEnd,
            string doubleFormat,
            int spaceStart,
            int spaceEnd,
            string unitFormat,
            int unitEnd) where TUnit : struct, IUnit
        {
            using (var builder = StringBuilderPool.Borrow())
            {
                builder.AppendWhiteSpace(format, 0, prePaddingEnd);
                builder.AppendDoubleFormat(doubleFormat);

                if (spaceStart == spaceEnd &&
                    string.IsNullOrEmpty(unitFormat))
                {
                    builder.AppendUnit(default(TUnit).SiUnit);
                    return builder.ToString();
                }

                if (spaceStart == spaceEnd &&
    string.IsNullOrEmpty(doubleFormat))
                {
                    builder.AppendUnit(unitFormat);
                    return builder.ToString();
                }

                builder.AppendWhiteSpace(format, spaceStart, spaceEnd);
                builder.Append(unitFormat ?? default(TUnit).SiUnit.Symbol);
                builder.AppendWhiteSpace(format, unitEnd, format.Length);
                var compositeFormat = builder.ToString();
                return compositeFormat;
            }
        }

        private static string CreateCompositeFormat<TUnit>(string format,
            int prePaddingEnd,
            string doubleFormat,
            TUnit unit) where TUnit : struct, IUnit
        {
            using (var builder = StringBuilderPool.Borrow())
            {
                builder.AppendWhiteSpace(format, 0, prePaddingEnd);
                builder.AppendDoubleFormat(doubleFormat);
                builder.AppendUnit(unit);
                var compositeFormat = builder.ToString();
                return compositeFormat;
            }
        }

        private static StringBuilderPool.Builder AppendWhiteSpace(this StringBuilderPool.Builder builder,
            string source,
            int from,
            int to)
        {
            if (from == to)
            {
                return builder;
            }

            for (int i = from; i < to; i++)
            {
                builder.Append(source[i]);
            }
            return builder;
        }

        private static StringBuilderPool.Builder AppendDoubleFormat(this StringBuilderPool.Builder builder, string format)
        {
            if (string.IsNullOrEmpty(format))
            {
                builder.Append("{0}");
            }

            else
            {
                builder.Append("{0:");
                builder.Append(format);
                builder.Append("}");
            }

            return builder;
        }

        private static StringBuilderPool.Builder AppendUnit(this StringBuilderPool.Builder builder, IUnit unit)
        {
            return builder.AppendUnit(unit.Symbol);
        }

        private static StringBuilderPool.Builder AppendUnit(this StringBuilderPool.Builder builder, string symbol)
        {
            if (ShouldSpace(symbol))
            {
                builder.Append('\u00A0');
            }

            builder.Append(symbol);

            return builder;
        }

        private static bool ShouldSpace(string symbol)
        {
            if (symbol.Length > 1)
            {
                return true;
            }
            return char.IsLetter(symbol[0]);
        }
    }
}
