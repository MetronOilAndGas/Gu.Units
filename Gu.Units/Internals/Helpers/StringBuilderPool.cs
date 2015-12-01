namespace Gu.Units
{
    using System;
    using System.Collections.Concurrent;
    using System.Text;

    internal static class StringBuilderPool
    {
        private static readonly ConcurrentQueue<StringBuilder> Builders = new ConcurrentQueue<StringBuilder>();

        internal static Builder Borrow()
        {
            return new Builder();
        }

        internal sealed class Builder : IDisposable
        {
            private readonly StringBuilder builder;
            private bool disposed;

            internal Builder()
            {
                if (!Builders.TryDequeue(out this.builder))
                {
                    this.builder = new StringBuilder(12);
                }
            }

            public void Dispose()
            {
                if (this.disposed)
                {
                    return;
                }

                this.disposed = true;
                this.builder.Clear();
                Builders.Enqueue(this.builder);
            }

            public void Append(char c)
            {
                this.builder.Append(c);
            }

            public void Append(string s)
            {
                this.builder.Append(s);
            }

            public override string ToString()
            {
                return this.builder.ToString();
            }

            internal void Append<TQuantity, TUnit>(
                TQuantity quantity,
                QuantityFormat<TUnit> format,
                IFormatProvider formatProvider)
                where TQuantity : IQuantity<TUnit>
                where TUnit : struct, IUnit, IEquatable<TUnit>
            {
                if (format.ErrorFormat != null)
                {
                    Append(format.ErrorFormat);
                    return;
                }
                Append(format.PrePadding);
                var scalarValue = quantity.GetValue(format.Unit);
                Append(scalarValue.ToString(format.ValueFormat, formatProvider));
                Append(format.Padding);
                Append(format.SymbolFormat ?? format.Unit.Symbol);
                Append(format.PostPadding);
            }

            internal void Append(SymbolAndPower symbolAndPower, SymbolFormat symbolFormat)
            {
                if (symbolAndPower.Power == 1)
                {
                    Append(symbolAndPower.Symbol);
                    return;
                }

                switch (symbolFormat)
                {
                    case SymbolFormat.SignedHatPowers:
                        Append('^');
                        Append(symbolAndPower.Power.ToString());
                        break;
                    case SymbolFormat.FractionHatPowers:
                        Append('^');
                        Append(Math.Abs(symbolAndPower.Power).ToString());
                        break;
                    case SymbolFormat.SignedSuperScript:
                        if (symbolAndPower.Power < 0)
                        {
                            Append(SuperScript.Minus);
                        }
                        Append(SuperScript.GetChar(Math.Abs(symbolAndPower.Power)));
                        break;
                    case SymbolFormat.Default:
                    case SymbolFormat.FractionSuperScript:
                        Append(SuperScript.GetChar(Math.Abs(symbolAndPower.Power)));
                        break;

                    default:
                        throw new ArgumentOutOfRangeException(nameof(symbolFormat), symbolFormat, null);
                }
            }
        }
    }
}
