﻿namespace Gu.Units
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;

    /// <summary>
    /// A type for the unit <see cref="Gu.Units.KinematicViscosity"/>.
	/// Contains logic for conversion and formatting.
    /// </summary>
    [Serializable]
    [TypeConverter(typeof(KinematicViscosityUnitTypeConverter))]
    public struct KinematicViscosityUnit : IUnit, IUnit<KinematicViscosity>, IEquatable<KinematicViscosityUnit>
    {
        /// <summary>
        /// The KinematicViscosityUnit unit
        /// Contains logic for conversion and formatting.
        /// </summary>
        public static readonly KinematicViscosityUnit SquareMetresPerSecond = new KinematicViscosityUnit(squareMetresPerSecond => squareMetresPerSecond, squareMetresPerSecond => squareMetresPerSecond, "m²/s");

        private readonly Func<double, double> toSquareMetresPerSecond;
        private readonly Func<double, double> fromSquareMetresPerSecond;
        internal readonly string symbol;

        public KinematicViscosityUnit(Func<double, double> toSquareMetresPerSecond, Func<double, double> fromSquareMetresPerSecond, string symbol)
        {
            this.toSquareMetresPerSecond = toSquareMetresPerSecond;
            this.fromSquareMetresPerSecond = fromSquareMetresPerSecond;
            this.symbol = symbol;
        }

        /// <summary>
        /// The symbol for the <see cref="Gu.Units.KinematicViscosityUnit"/>.
        /// </summary>
        public string Symbol => this.symbol;

        /// <summary>
        /// The default unit for <see cref="Gu.Units.KinematicViscosityUnit"/>
        /// </summary>
        public KinematicViscosityUnit SiUnit => SquareMetresPerSecond;

        /// <summary>
        /// The default <see cref="Gu.Units.IUnit"/> for <see cref="Gu.Units.KinematicViscosityUnit"/>
        /// </summary>
        IUnit IUnit.SiUnit => SquareMetresPerSecond;

        public static KinematicViscosity operator *(double left, KinematicViscosityUnit right)
        {
            return KinematicViscosity.From(left, right);
        }

        public static bool operator ==(KinematicViscosityUnit left, KinematicViscosityUnit right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(KinematicViscosityUnit left, KinematicViscosityUnit right)
        {
            return !left.Equals(right);
        }

        /// <summary>
        /// Constructs a <see cref="KinematicViscosityUnit"/> from a string.
        /// Leading and trailing whitespace characters are allowed.
        /// </summary>
        /// <param name="text"></param>
        /// <returns>An instance of <see cref="KinematicViscosityUnit"/></returns>
        public static KinematicViscosityUnit Parse(string text)
        {
            return UnitParser<KinematicViscosityUnit>.Parse(text);
        }

        public static bool TryParse(string text, out KinematicViscosityUnit value)
        {
            return UnitParser<KinematicViscosityUnit>.TryParse(text, out value);
        }

        /// <summary>
        /// Converts <paramref name="value"/> to SquareMetresPerSecond.
        /// </summary>
        /// <param name="value"></param>
        /// <returns>The converted value</returns>
        public double ToSiUnit(double value)
        {
            return this.toSquareMetresPerSecond(value);
        }

        /// <summary>
        /// Converts a value from squareMetresPerSecond.
        /// </summary>
        /// <param name="SquareMetresPerSecond">The value in SquareMetresPerSecond</param>
        /// <returns>The converted value</returns>
        public double FromSiUnit(double squareMetresPerSecond)
        {
            return this.fromSquareMetresPerSecond(squareMetresPerSecond);
        }

        /// <summary>
        /// Creates a quantity with this unit
        /// </summary>
        /// <param name="value">The scalar value"</param>
        /// <returns>new KinematicViscosity(<paramref name="value"/>, this)</returns>
        public KinematicViscosity CreateQuantity(double value)
        {
            return new KinematicViscosity(value, this);
        }

        /// <summary>
        /// Gets the scalar value of <paramref name="quantity"/> in SquareMetresPerSecond
        /// </summary>
        /// <param name="quantity"></param>
        /// <returns></returns>
        public double GetScalarValue(KinematicViscosity quantity)
        {
            return FromSiUnit(quantity.squareMetresPerSecond);
        }

        public override string ToString()
        {
            return this.symbol;
        }

        public string ToString(string format)
        {
            KinematicViscosityUnit unit;
            var paddedFormat = UnitFormatCache<KinematicViscosityUnit>.GetOrCreate(format, out unit);
            if (unit != this)
            {
                return format;
            }

            using (var builder = StringBuilderPool.Borrow())
            {
                builder.Append(paddedFormat.PrePadding);
                builder.Append(paddedFormat.Format);
                builder.Append(paddedFormat.PostPadding);
                return builder.ToString();
            }
        }

        public string ToString(SymbolFormat format)
        {
            var paddedFormat = UnitFormatCache<KinematicViscosityUnit>.GetOrCreate(this, format);
            using (var builder = StringBuilderPool.Borrow())
            {
                builder.Append(paddedFormat.PrePadding);
                builder.Append(paddedFormat.Format);
                builder.Append(paddedFormat.PostPadding);
                return builder.ToString();
            }
        }

        public bool Equals(KinematicViscosityUnit other)
        {
            return this.symbol == other.symbol;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            return obj is KinematicViscosityUnit && Equals((KinematicViscosityUnit)obj);
        }

        /// <summary>
        /// Returns the hashcode for this <see cref="LengthUnit"/>
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            if (this.symbol == null)
            {
                return 0; // Needed due to default ctor
            }

            return this.symbol.GetHashCode();
        }
    }
}