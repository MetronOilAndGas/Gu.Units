﻿namespace Gu.Units
{
    using System;
    using System.Globalization;
    using System.Xml;
    using System.Xml.Schema;
    using System.Xml.Serialization;

    /// <summary>
    /// A type for the quantity <see cref="Gu.Units.Pressure"/>.
    /// </summary>
    // [TypeConverter(typeof(PressureTypeConverter))]
    [Serializable]
    public partial struct Pressure : IComparable<Pressure>, IEquatable<Pressure>, IFormattable, IXmlSerializable, IQuantity<MassUnit, I1, LengthUnit, INeg1, TimeUnit, INeg2>, IQuantity<PressureUnit>
    {
        public static readonly Pressure Zero = new Pressure();

        /// <summary>
        /// The quantity in <see cref="Gu.Units.PressureUnit.Pascals"/>.
        /// </summary>
        internal readonly double pascals;

        private Pressure(double pascals)
        {
            this.pascals = pascals;
        }

        /// <summary>
        /// Initializes a new instance of <see cref="Gu.Units.Pressure"/>.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="unit"><see cref="Gu.Units.PressureUnit"/>.</param>
        public Pressure(double value, PressureUnit unit)
        {
            this.pascals = unit.ToSiUnit(value);
        }

        /// <summary>
        /// The quantity in <see cref="Gu.Units.PressureUnit.Pascals"/>
        /// </summary>
        public double SiValue
        {
            get
            {
                return this.pascals;
            }
        }

        /// <summary>
        /// The <see cref="Gu.Units.PressureUnit"/> for the <see cref="SiValue"/>
        /// </summary>
        public PressureUnit SiUnit => PressureUnit.Pascals;

        /// <summary>
        /// The <see cref="Gu.Units.IUnit"/> for the <see cref="SiValue"/>
        /// </summary>
        IUnit IQuantity.SiUnit => PressureUnit.Pascals;

        /// <summary>
        /// The quantity in pascals".
        /// </summary>
        public double Pascals
        {
            get
            {
                return this.pascals;
            }
        }

        /// <summary>
        /// The quantity in nanopascals
        /// </summary>
        public double Nanopascals
        {
            get
            {
                return PressureUnit.Nanopascals.FromSiUnit(this.pascals);
            }
        }

        /// <summary>
        /// The quantity in micropascals
        /// </summary>
        public double Micropascals
        {
            get
            {
                return PressureUnit.Micropascals.FromSiUnit(this.pascals);
            }
        }

        /// <summary>
        /// The quantity in millipascals
        /// </summary>
        public double Millipascals
        {
            get
            {
                return PressureUnit.Millipascals.FromSiUnit(this.pascals);
            }
        }

        /// <summary>
        /// The quantity in kilopascals
        /// </summary>
        public double Kilopascals
        {
            get
            {
                return PressureUnit.Kilopascals.FromSiUnit(this.pascals);
            }
        }

        /// <summary>
        /// The quantity in megapascals
        /// </summary>
        public double Megapascals
        {
            get
            {
                return PressureUnit.Megapascals.FromSiUnit(this.pascals);
            }
        }

        /// <summary>
        /// The quantity in gigapascals
        /// </summary>
        public double Gigapascals
        {
            get
            {
                return PressureUnit.Gigapascals.FromSiUnit(this.pascals);
            }
        }

        /// <summary>
        /// The quantity in bars
        /// </summary>
        public double Bars
        {
            get
            {
                return PressureUnit.Bars.FromSiUnit(this.pascals);
            }
        }

        /// <summary>
        /// The quantity in millibars
        /// </summary>
        public double Millibars
        {
            get
            {
                return PressureUnit.Millibars.FromSiUnit(this.pascals);
            }
        }

        /// <summary>
        /// Creates an instance of <see cref="Gu.Units.Pressure"/> from its string representation
        /// </summary>
        /// <param name="s">The string representation of the <see cref="Gu.Units.Pressure"/></param>
        /// <returns></returns>
		public static Pressure Parse(string s)
        {
            return QuantityParser.Parse<PressureUnit, Pressure>(s, From, NumberStyles.Float, CultureInfo.CurrentCulture);
        }

        public static Pressure Parse(string s, IFormatProvider provider)
        {
            return QuantityParser.Parse<PressureUnit, Pressure>(s, From, NumberStyles.Float, provider);
        }

        public static Pressure Parse(string s, NumberStyles styles)
        {
            return QuantityParser.Parse<PressureUnit, Pressure>(s, From, styles, CultureInfo.CurrentCulture);
        }

        public static Pressure Parse(string s, NumberStyles styles, IFormatProvider provider)
        {
            return QuantityParser.Parse<PressureUnit, Pressure>(s, From, styles, provider);
        }

        public static bool TryParse(string s, out Pressure value)
        {
            return QuantityParser.TryParse<PressureUnit, Pressure>(s, From, NumberStyles.Float, CultureInfo.CurrentCulture, out value);
        }

        public static bool TryParse(string s, IFormatProvider provider, out Pressure value)
        {
            return QuantityParser.TryParse<PressureUnit, Pressure>(s, From, NumberStyles.Float, provider, out value);
        }

        public static bool TryParse(string s, NumberStyles styles, out Pressure value)
        {
            return QuantityParser.TryParse<PressureUnit, Pressure>(s, From, styles, CultureInfo.CurrentCulture, out value);
        }

        public static bool TryParse(string s, NumberStyles styles, IFormatProvider provider, out Pressure value)
        {
            return QuantityParser.TryParse<PressureUnit, Pressure>(s, From, styles, provider, out value);
        }

        /// <summary>
        /// Reads an instance of <see cref="Gu.Units.Pressure"/> from the <paramref name="reader"/>
        /// </summary>
        /// <param name="reader"></param>
        /// <returns>An instance of  <see cref="Gu.Units.Pressure"/></returns>
        public static Pressure ReadFrom(XmlReader reader)
        {
            var v = new Pressure();
            v.ReadXml(reader);
            return v;
        }

        /// <summary>
        /// Creates a new instance of <see cref="Gu.Units.Pressure"/>.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="unit"></param>
        public static Pressure From(double value, PressureUnit unit)
        {
            return new Pressure(unit.ToSiUnit(value));
        }

        /// <summary>
        /// Creates a new instance of <see cref="Gu.Units.Pressure"/>.
        /// </summary>
        /// <param name="pascals">The value in <see cref="Gu.Units.PressureUnit.Pascals"/></param>
        public static Pressure FromPascals(double pascals)
        {
            return new Pressure(pascals);
        }

        /// <summary>
        /// Creates a new instance of <see cref="Gu.Units.Pressure"/>.
        /// </summary>
        /// <param name="nanopascals">The value in nPa</param>
        public static Pressure FromNanopascals(double nanopascals)
        {
            return From(nanopascals, PressureUnit.Nanopascals);
        }

        /// <summary>
        /// Creates a new instance of <see cref="Gu.Units.Pressure"/>.
        /// </summary>
        /// <param name="micropascals">The value in µPa</param>
        public static Pressure FromMicropascals(double micropascals)
        {
            return From(micropascals, PressureUnit.Micropascals);
        }

        /// <summary>
        /// Creates a new instance of <see cref="Gu.Units.Pressure"/>.
        /// </summary>
        /// <param name="millipascals">The value in mPa</param>
        public static Pressure FromMillipascals(double millipascals)
        {
            return From(millipascals, PressureUnit.Millipascals);
        }

        /// <summary>
        /// Creates a new instance of <see cref="Gu.Units.Pressure"/>.
        /// </summary>
        /// <param name="kilopascals">The value in kPa</param>
        public static Pressure FromKilopascals(double kilopascals)
        {
            return From(kilopascals, PressureUnit.Kilopascals);
        }

        /// <summary>
        /// Creates a new instance of <see cref="Gu.Units.Pressure"/>.
        /// </summary>
        /// <param name="megapascals">The value in MPa</param>
        public static Pressure FromMegapascals(double megapascals)
        {
            return From(megapascals, PressureUnit.Megapascals);
        }

        /// <summary>
        /// Creates a new instance of <see cref="Gu.Units.Pressure"/>.
        /// </summary>
        /// <param name="gigapascals">The value in GPa</param>
        public static Pressure FromGigapascals(double gigapascals)
        {
            return From(gigapascals, PressureUnit.Gigapascals);
        }

        /// <summary>
        /// Creates a new instance of <see cref="Gu.Units.Pressure"/>.
        /// </summary>
        /// <param name="bars">The value in bar</param>
        public static Pressure FromBars(double bars)
        {
            return From(bars, PressureUnit.Bars);
        }

        /// <summary>
        /// Creates a new instance of <see cref="Gu.Units.Pressure"/>.
        /// </summary>
        /// <param name="millibars">The value in mbar</param>
        public static Pressure FromMillibars(double millibars)
        {
            return From(millibars, PressureUnit.Millibars);
        }

        public static Force operator *(Pressure left, Area right)
        {
            return Force.FromNewtons(left.pascals * right.squareMetres);
        }

        public static Energy operator *(Pressure left, Volume right)
        {
            return Energy.FromJoules(left.pascals * right.cubicMetres);
        }

        public static Stiffness operator *(Pressure left, Length right)
        {
            return Stiffness.FromNewtonsPerMetre(left.pascals * right.metres);
        }

        public static SpecificEnergy operator /(Pressure left, Density right)
        {
            return SpecificEnergy.FromJoulesPerKilogram(left.pascals / right.kilogramsPerCubicMetre);
        }

        public static double operator /(Pressure left, Pressure right)
        {
            return left.pascals / right.pascals;
        }

        /// <summary>
        /// Indicates whether two <see cref="Gu.Units.Pressure"/> instances are equal.
        /// </summary>
        /// <returns>
        /// true if the quantitys of <paramref name="left"/> and <paramref name="right"/> are equal; otherwise, false.
        /// </returns>
        /// <param name="left">An instance of <see cref="Gu.Units.Pressure"/>.</param>
        /// <param name="right">An instance of <see cref="Gu.Units.Pressure"/>.</param>
        public static bool operator ==(Pressure left, Pressure right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Indicates whether two <see cref="Gu.Units.Pressure"/> instances are not equal.
        /// </summary>
        /// <returns>
        /// true if the quantitys of <paramref name="left"/> and <paramref name="right"/> are not equal; otherwise, false.
        /// </returns>
        /// <param name="left">An instance of <see cref="Gu.Units.Pressure"/>.</param>
        /// <param name="right">An instance of <see cref="Gu.Units.Pressure"/>.</param>
        public static bool operator !=(Pressure left, Pressure right)
        {
            return !left.Equals(right);
        }

        /// <summary>
        /// Indicates whether a specified <see cref="Gu.Units.Pressure"/> is less than another specified <see cref="Gu.Units.Pressure"/>.
        /// </summary>
        /// <returns>
        /// true if the quantity of <paramref name="left"/> is less than the quantity of <paramref name="right"/>; otherwise, false. 
        /// </returns>
        /// <param name="left">An instance of <see cref="Gu.Units.Pressure"/>.</param>
        /// <param name="right">An instance of <see cref="Gu.Units.Pressure"/>.</param>
        public static bool operator <(Pressure left, Pressure right)
        {
            return left.pascals < right.pascals;
        }

        /// <summary>
        /// Indicates whether a specified <see cref="Gu.Units.Pressure"/> is greater than another specified <see cref="Gu.Units.Pressure"/>.
        /// </summary>
        /// <returns>
        /// true if the quantity of <paramref name="left"/> is greater than the quantity of <paramref name="right"/>; otherwise, false. 
        /// </returns>
        /// <param name="left">An instance of <see cref="Gu.Units.Pressure"/>.</param>
        /// <param name="right">An instance of <see cref="Gu.Units.Pressure"/>.</param>
        public static bool operator >(Pressure left, Pressure right)
        {
            return left.pascals > right.pascals;
        }

        /// <summary>
        /// Indicates whether a specified <see cref="Gu.Units.Pressure"/> is less than or equal to another specified <see cref="Gu.Units.Pressure"/>.
        /// </summary>
        /// <returns>
        /// true if the quantity of <paramref name="left"/> is less than or equal to the quantity of <paramref name="right"/>; otherwise, false.
        /// </returns>
        /// <param name="left">An instance of <see cref="Gu.Units.Pressure"/>.</param>
        /// <param name="right">An instance of <see cref="Gu.Units.Pressure"/>.</param>
        public static bool operator <=(Pressure left, Pressure right)
        {
            return left.pascals <= right.pascals;
        }

        /// <summary>
        /// Indicates whether a specified <see cref="Gu.Units.Pressure"/> is greater than or equal to another specified <see cref="Gu.Units.Pressure"/>.
        /// </summary>
        /// <returns>
        /// true if the quantity of <paramref name="left"/> is greater than or equal to the quantity of <paramref name="right"/>; otherwise, false.
        /// </returns>
        /// <param name="left">An instance of <see cref="Gu.Units.Pressure"/>.</param>
        /// <param name="right">An instance of <see cref="Gu.Units.Pressure"/>.</param>
        public static bool operator >=(Pressure left, Pressure right)
        {
            return left.pascals >= right.pascals;
        }

        /// <summary>
        /// Multiplies an instance of <see cref="Gu.Units.Pressure"/> with <paramref name="left"/> and returns the result.
        /// </summary>
        /// <param name="right">An instance of <see cref="Gu.Units.Pressure"/></param>
        /// <param name="left">An instance of <seealso cref="System.Double"/></param>
        /// <returns>Multiplies an instance of <see cref="Gu.Units.Pressure"/> with <paramref name="left"/> and returns the result.</returns>
        public static Pressure operator *(double left, Pressure right)
        {
            return new Pressure(left * right.pascals);
        }

        /// <summary>
        /// Multiplies an instance of <see cref="Gu.Units.Pressure"/> with <paramref name="right"/> and returns the result.
        /// </summary>
        /// <param name="left">An instance of <see cref="Gu.Units.Pressure"/></param>
        /// <param name="right">An instance of <seealso cref="System.Double"/></param>
        /// <returns>Multiplies an instance of <see cref="Gu.Units.Pressure"/> with <paramref name="right"/> and returns the result.</returns>
        public static Pressure operator *(Pressure left, double right)
        {
            return new Pressure(left.pascals * right);
        }

        /// <summary>
        /// Divides an instance of <see cref="Gu.Units.Pressure"/> with <paramref name="right"/> and returns the result.
        /// </summary>
        /// <param name="left">An instance of <see cref="Gu.Units.Pressure"/></param>
        /// <param name="right">An instance of <seealso cref="System.Double"/></param>
        /// <returns>Divides an instance of <see cref="Gu.Units.Pressure"/> with <paramref name="right"/> and returns the result.</returns>
        public static Pressure operator /(Pressure left, double right)
        {
            return new Pressure(left.pascals / right);
        }

        /// <summary>
        /// Adds two specified <see cref="Gu.Units.Pressure"/> instances.
        /// </summary>
        /// <returns>
        /// An <see cref="Gu.Units.Pressure"/> whose quantity is the sum of the quantitys of <paramref name="left"/> and <paramref name="right"/>.
        /// </returns>
        /// <param name="left">An instance of <see cref="Gu.Units.Pressure"/>.</param>
        /// <param name="right">An instance of <see cref="Gu.Units.Pressure"/>.</param>
        public static Pressure operator +(Pressure left, Pressure right)
        {
            return new Pressure(left.pascals + right.pascals);
        }

        /// <summary>
        /// Subtracts an Pressure from another Pressure and returns the difference.
        /// </summary>
        /// <returns>
        /// An <see cref="Gu.Units.Pressure"/> that is the difference
        /// </returns>
        /// <param name="left">An instance of <see cref="Gu.Units.Pressure"/> (the minuend).</param>
        /// <param name="right">An instance of <see cref="Gu.Units.Pressure"/> (the subtrahend).</param>
        public static Pressure operator -(Pressure left, Pressure right)
        {
            return new Pressure(left.pascals - right.pascals);
        }

        /// <summary>
        /// Returns an <see cref="Gu.Units.Pressure"/> whose quantity is the negated quantity of the specified instance.
        /// </summary>
        /// <returns>
        /// An <see cref="Gu.Units.Pressure"/> with the same numeric quantity as this instance, but the opposite sign.
        /// </returns>
        /// <param name="pressure">An instance of <see cref="Gu.Units.Pressure"/></param>
        public static Pressure operator -(Pressure pressure)
        {
            return new Pressure(-1 * pressure.pascals);
        }

        /// <summary>
        /// Returns the specified instance of <see cref="Gu.Units.Pressure"/>.
        /// </summary>
        /// <returns>
        /// Returns <paramref name="pressure"/>.
        /// </returns>
        /// <param name="pressure">An instance of <see cref="Gu.Units.Pressure"/></param>
        public static Pressure operator +(Pressure pressure)
        {
            return pressure;
        }

        /// <summary>
        /// Get the scalar value
        /// </summary>
        /// <param name="unit"></param>
        /// <returns>The scalar value of this in the specified unit</returns>
        public double GetValue(PressureUnit unit)
        {
            return unit.FromSiUnit(this.pascals);
        }

        public override string ToString()
        {
            var quantityFormat = FormatParser<PressureUnit>.GetOrCreate(string.Empty, this.SiUnit);
            return this.ToString(quantityFormat, null);
        }

        public string ToString(string format)
        {
            var quantityFormat = FormatParser<PressureUnit>.GetOrCreate(format);
            return ToString(quantityFormat, null);
        }

        public string ToString(IFormatProvider provider)
        {
            var quantityFormat = FormatParser<PressureUnit>.GetOrCreate(string.Empty, SiUnit);
            return ToString(quantityFormat, provider);
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            var quantityFormat = FormatParser<PressureUnit>.GetOrCreate(format);
            return ToString(quantityFormat, formatProvider);
        }

        public string ToString(PressureUnit unit)
        {
            var quantityFormat = FormatParser<PressureUnit>.GetOrCreate(string.Empty, unit);
            return ToString(quantityFormat, null);
        }

        public string ToString(PressureUnit unit, IFormatProvider formatProvider)
        {
            var quantityFormat = FormatParser<PressureUnit>.GetOrCreate(string.Empty, unit);
            return ToString(quantityFormat, formatProvider);
        }

        public string ToString(string valueFormat, PressureUnit unit)
        {
            var quantityFormat = FormatParser<PressureUnit>.GetOrCreate(valueFormat, unit);
            return ToString(quantityFormat, null);
        }

        public string ToString(string valueFormat, PressureUnit unit, IFormatProvider formatProvider)
        {
            var quantityFormat = FormatParser<PressureUnit>.GetOrCreate(valueFormat, unit);
            return ToString(quantityFormat, formatProvider);
        }

        private string ToString(QuantityFormat<PressureUnit> format, IFormatProvider formatProvider)
        {
            using (var builder = StringBuilderPool.Borrow())
            {
                builder.Append(this, format, formatProvider);
                return builder.ToString();
            }
        }

        /// <summary>
        /// Compares this instance to a specified <see cref="Gu.Units.Pressure"/> object and returns an integer that indicates whether this <see cref="quantity"/> is smaller than, equal to, or greater than the <see cref="Gu.Units.Pressure"/> object.
        /// </summary>
        /// <returns>
        /// A signed number indicating the relative quantitys of this instance and <paramref name="quantity"/>.
        /// 
        ///                     Value
        /// 
        ///                     Description
        /// 
        ///                     A negative integer
        /// 
        ///                     This instance is smaller than <paramref name="quantity"/>.
        /// 
        ///                     Zero
        /// 
        ///                     This instance is equal to <paramref name="quantity"/>.
        /// 
        ///                     A positive integer
        /// 
        ///                     This instance is larger than <paramref name="quantity"/>.
        /// 
        /// </returns>
        /// <param name="quantity">An instance of <see cref="Gu.Units.Pressure"/> object to compare to this instance.</param>
        public int CompareTo(Pressure quantity)
        {
            return this.pascals.CompareTo(quantity.pascals);
        }

        /// <summary>
        /// Returns a quantity indicating whether this instance is equal to a specified <see cref="Gu.Units.Pressure"/> object.
        /// </summary>
        /// <returns>
        /// true if <paramref name="other"/> represents the same Pressure as this instance; otherwise, false.
        /// </returns>
        /// <param name="other">An instance of <see cref="Gu.Units.Pressure"/> object to compare with this instance.</param>
        public bool Equals(Pressure other)
        {
            return this.pascals.Equals(other.pascals);
        }

        /// <summary>
        /// Returns a quantity indicating whether this instance is equal to a specified <see cref="Gu.Units.Pressure"/> object within the given tolerance.
        /// </summary>
        /// <returns>
        /// true if <paramref name="other"/> represents the same Pressure as this instance; otherwise, false.
        /// </returns>
        /// <param name="other">An instance of <see cref="Gu.Units.Pressure"/> object to compare with this instance.</param>
        /// <param name="tolerance">The maximum difference for being considered equal</param>
        public bool Equals(Pressure other, double tolerance)
        {
            return Math.Abs(this.pascals - other.pascals) < tolerance;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            return obj is Pressure && this.Equals((Pressure)obj);
        }

        public override int GetHashCode()
        {
            return this.pascals.GetHashCode();
        }

        /// <summary>
        /// This method is reserved and should not be used. When implementing the IXmlSerializable interface, 
        /// you should return null (Nothing in Visual Basic) from this method, and instead, 
        /// if specifying a custom schema is required, apply the <see cref="System.Xml.Serialization.XmlSchemaProviderAttribute"/> to the class.
        /// </summary>
        /// <returns>
        /// An <see cref="System.Xml.Schema.XmlSchema"/> that describes the XML representation of the object that is produced by the
        ///  <see cref="M:System.Xml.Serialization.IXmlSerializable.WriteXml(System.Xml.XmlWriter)"/> 
        /// method and consumed by the <see cref="M:System.Xml.Serialization.IXmlSerializable.ReadXml(System.Xml.XmlReader)"/> method.
        /// </returns>
        public XmlSchema GetSchema()
        {
            return null;
        }

        /// <summary>
        /// Generates an object from its XML representation.
        /// </summary>
        /// <param name="reader">The <see cref="System.Xml.XmlReader"/> stream from which the object is deserialized. </param>
        public void ReadXml(XmlReader reader)
        {
            // Hacking set readonly fields here, can't think of a cleaner workaround
            XmlExt.SetReadonlyField(ref this, "pascals", reader, "Value");
        }

        /// <summary>
        /// Converts an object into its XML representation.
        /// </summary>
        /// <param name="writer">The <see cref="System.Xml.XmlWriter"/> stream to which the object is serialized. </param>
        public void WriteXml(XmlWriter writer)
        {
            XmlExt.WriteAttribute(writer, "Value", this.pascals);
        }
    }
}