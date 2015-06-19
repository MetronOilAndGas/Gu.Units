﻿namespace Gu.Units
{
    using System;
    using System.Diagnostics;

    /// <summary>
    /// A type for the unit <see cref="T:Gu.Units.JerkUnit"/>.
    /// Contains conversion logic.
    /// </summary>
    [Serializable, DebuggerDisplay("1{symbol} == {ToSiUnit(1)}{MetresPerSecondCubed.symbol}")]
    public struct JerkUnit : IUnit, IUnit<Jerk>, IEquatable<JerkUnit>
    {
        /// <summary>
        /// The <see cref="T:Gu.Units.MetresPerSecondCubed"/> unit
        /// Contains conversion logic to from and formatting.
        /// </summary>
        public static readonly JerkUnit MetresPerSecondCubed = new JerkUnit(1.0, "m/s³");

        /// <summary>
        /// The <see cref="T:Gu.Units.MillimetresPerSecondCubed"/> unit
        /// Contains conversion logic to from and formatting.
        /// </summary>
        public static readonly JerkUnit MillimetresPerSecondCubed = new JerkUnit(0.001, "mm⋅s⁻³");

        /// <summary>
        /// The <see cref="T:Gu.Units.MillimetresPerHourCubed"/> unit
        /// Contains conversion logic to from and formatting.
        /// </summary>
        public static readonly JerkUnit MillimetresPerHourCubed = new JerkUnit(2.1433470507544584E-14, "mm⋅h⁻³");

        /// <summary>
        /// The <see cref="T:Gu.Units.MillimetresPerMinuteCubed"/> unit
        /// Contains conversion logic to from and formatting.
        /// </summary>
        public static readonly JerkUnit MillimetresPerMinuteCubed = new JerkUnit(4.6296296296296295E-09, "mm⋅min⁻³");

        /// <summary>
        /// The <see cref="T:Gu.Units.MetresPerHourCubed"/> unit
        /// Contains conversion logic to from and formatting.
        /// </summary>
        public static readonly JerkUnit MetresPerHourCubed = new JerkUnit(2.1433470507544583E-11, "m⋅h⁻³");

        /// <summary>
        /// The <see cref="T:Gu.Units.MetresPerMinuteCubed"/> unit
        /// Contains conversion logic to from and formatting.
        /// </summary>
        public static readonly JerkUnit MetresPerMinuteCubed = new JerkUnit(4.6296296296296296E-06, "m⋅min⁻³");

        /// <summary>
        /// The <see cref="T:Gu.Units.NanometresPerHourCubed"/> unit
        /// Contains conversion logic to from and formatting.
        /// </summary>
        public static readonly JerkUnit NanometresPerHourCubed = new JerkUnit(2.1433470507544585E-20, "nm⋅h⁻³");

        /// <summary>
        /// The <see cref="T:Gu.Units.NanometresPerMinuteCubed"/> unit
        /// Contains conversion logic to from and formatting.
        /// </summary>
        public static readonly JerkUnit NanometresPerMinuteCubed = new JerkUnit(4.62962962962963E-15, "nm⋅min⁻³");

        /// <summary>
        /// The <see cref="T:Gu.Units.CentimetresPerSecondCubed"/> unit
        /// Contains conversion logic to from and formatting.
        /// </summary>
        public static readonly JerkUnit CentimetresPerSecondCubed = new JerkUnit(0.01, "cm⋅s⁻³");

        /// <summary>
        /// The <see cref="T:Gu.Units.CentimetresPerHourCubed"/> unit
        /// Contains conversion logic to from and formatting.
        /// </summary>
        public static readonly JerkUnit CentimetresPerHourCubed = new JerkUnit(2.1433470507544584E-13, "cm⋅h⁻³");

        /// <summary>
        /// The <see cref="T:Gu.Units.CentimetresPerMinuteCubed"/> unit
        /// Contains conversion logic to from and formatting.
        /// </summary>
        public static readonly JerkUnit CentimetresPerMinuteCubed = new JerkUnit(4.6296296296296295E-08, "cm⋅min⁻³");

        private readonly double conversionFactor;
        private readonly string symbol;

        public JerkUnit(double conversionFactor, string symbol)
        {
            this.conversionFactor = conversionFactor;
            this.symbol = symbol;
        }

        /// <summary>
        /// The symbol for <see cref="T:Gu.Units.MetresPerSecondCubed"/>.
        /// </summary>
        public string Symbol
        {
            get
            {
                return this.symbol;
            }
        }

        public static Jerk operator *(double left, JerkUnit right)
        {
            return Jerk.From(left, right);
        }

        public static bool operator ==(JerkUnit left, JerkUnit right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(JerkUnit left, JerkUnit right)
        {
            return !left.Equals(right);
        }

        public static JerkUnit Parse(string text)
        {
            return Parser.ParseUnit<JerkUnit>(text);
        }

        public static bool TryParse(string text, out JerkUnit value)
        {
            return Parser.TryParseUnit<JerkUnit>(text, out value);
        }

        /// <summary>
        /// Converts a value to <see cref="T:Gu.Units.MetresPerSecondCubed"/>.
        /// </summary>
        /// <param name="value"></param>
        /// <returns>The converted value</returns>
        public double ToSiUnit(double value)
        {
            return this.conversionFactor * value;
        }

        /// <summary>
        /// Converts a value from MetresPerSecondCubed.
        /// </summary>
        /// <param name="value">The value in MetresPerSecondCubed</param>
        /// <returns>The converted value</returns>
        public double FromSiUnit(double value)
        {
            return value / this.conversionFactor;
        }

        /// <summary>
        /// Creates a quantity with this unit
        /// </summary>
        /// <param name="value"></param>
        /// <returns>new TTQuantity(value, this)</returns>
        public Jerk CreateQuantity(double value)
        {
            return new Jerk(value, this);
        }

        /// <summary>
        /// Gets the scalar value
        /// </summary>
        /// <param name="quantity"></param>
        /// <returns></returns>
        public double GetScalarValue(Jerk quantity)
        {
            return FromSiUnit(quantity.metresPerSecondCubed);
        }

        public override string ToString()
        {
            return this.symbol;
        }

        public bool Equals(JerkUnit other)
        {
            return this.symbol == other.symbol;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            return obj is JerkUnit && Equals((JerkUnit)obj);
        }

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