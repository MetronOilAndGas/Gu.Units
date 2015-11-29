﻿namespace Gu.Units
{
    using System;
    using System.Globalization;
    using System.Xml;
    using System.Xml.Schema;

    /// <summary>
    /// A type for the quantity <see cref="Gu.Units.Voltage"/>.
    /// </summary>
    // [TypeConverter(typeof(VoltageTypeConverter))]
    [Serializable]
    public partial struct Voltage : IQuantity<VoltageUnit>, IComparable<Voltage>, IEquatable<Voltage>
    {
        public static readonly Voltage Zero = new Voltage();

        /// <summary>
        /// The quantity in <see cref="Gu.Units.VoltageUnit.Volts"/>.
        /// </summary>
        internal readonly double volts;

        private Voltage(double volts)
        {
            this.volts = volts;
        }

        /// <summary>
        /// Initializes a new instance of <see cref="Gu.Units.Voltage"/>.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="unit"><see cref="Gu.Units.VoltageUnit"/>.</param>
        public Voltage(double value, VoltageUnit unit)
        {
            this.volts = unit.ToSiUnit(value);
        }

        /// <summary>
        /// The quantity in <see cref="Gu.Units.VoltageUnit.Volts"/>
        /// </summary>
        public double SiValue
        {
            get
            {
                return this.volts;
            }
        }

        /// <summary>
        /// The <see cref="Gu.Units.VoltageUnit"/> for the <see cref="SiValue"/>
        /// </summary>
        public VoltageUnit SiUnit => VoltageUnit.Volts;

        /// <summary>
        /// The <see cref="Gu.Units.IUnit"/> for the <see cref="SiValue"/>
        /// </summary>
        IUnit IQuantity.SiUnit => VoltageUnit.Volts;

        /// <summary>
        /// The quantity in volts".
        /// </summary>
        public double Volts
        {
            get
            {
                return this.volts;
            }
        }

        /// <summary>
        /// The quantity in millivolts
        /// </summary>
        public double Millivolts
        {
            get
            {
                return VoltageUnit.Millivolts.FromSiUnit(this.volts);
            }
        }

        /// <summary>
        /// The quantity in kilovolts
        /// </summary>
        public double Kilovolts
        {
            get
            {
                return VoltageUnit.Kilovolts.FromSiUnit(this.volts);
            }
        }

        /// <summary>
        /// The quantity in megavolts
        /// </summary>
        public double Megavolts
        {
            get
            {
                return VoltageUnit.Megavolts.FromSiUnit(this.volts);
            }
        }

        /// <summary>
        /// The quantity in microvolts
        /// </summary>
        public double Microvolts
        {
            get
            {
                return VoltageUnit.Microvolts.FromSiUnit(this.volts);
            }
        }

        /// <summary>
        /// Creates an instance of <see cref="Gu.Units.Voltage"/> from its string representation
        /// </summary>
        /// <param name="s">The string representation of the <see cref="Gu.Units.Voltage"/></param>
        /// <returns></returns>
		public static Voltage Parse(string s)
        {
            return QuantityParser.Parse<VoltageUnit, Voltage>(s, From, NumberStyles.Float, CultureInfo.CurrentCulture);
        }

        public static Voltage Parse(string s, IFormatProvider provider)
        {
            return QuantityParser.Parse<VoltageUnit, Voltage>(s, From, NumberStyles.Float, provider);
        }

        public static Voltage Parse(string s, NumberStyles styles)
        {
            return QuantityParser.Parse<VoltageUnit, Voltage>(s, From, styles, CultureInfo.CurrentCulture);
        }

        public static Voltage Parse(string s, NumberStyles styles, IFormatProvider provider)
        {
            return QuantityParser.Parse<VoltageUnit, Voltage>(s, From, styles, provider);
        }

        public static bool TryParse(string s, out Voltage value)
        {
            return QuantityParser.TryParse<VoltageUnit, Voltage>(s, From, NumberStyles.Float, CultureInfo.CurrentCulture, out value);
        }

        public static bool TryParse(string s, IFormatProvider provider, out Voltage value)
        {
            return QuantityParser.TryParse<VoltageUnit, Voltage>(s, From, NumberStyles.Float, provider, out value);
        }

        public static bool TryParse(string s, NumberStyles styles, out Voltage value)
        {
            return QuantityParser.TryParse<VoltageUnit, Voltage>(s, From, styles, CultureInfo.CurrentCulture, out value);
        }

        public static bool TryParse(string s, NumberStyles styles, IFormatProvider provider, out Voltage value)
        {
            return QuantityParser.TryParse<VoltageUnit, Voltage>(s, From, styles, provider, out value);
        }

        /// <summary>
        /// Reads an instance of <see cref="Gu.Units.Voltage"/> from the <paramref name="reader"/>
        /// </summary>
        /// <param name="reader"></param>
        /// <returns>An instance of  <see cref="Gu.Units.Voltage"/></returns>
        public static Voltage ReadFrom(XmlReader reader)
        {
            var v = new Voltage();
            v.ReadXml(reader);
            return v;
        }

        /// <summary>
        /// Creates a new instance of <see cref="Gu.Units.Voltage"/>.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="unit"></param>
        public static Voltage From(double value, VoltageUnit unit)
        {
            return new Voltage(unit.ToSiUnit(value));
        }

        /// <summary>
        /// Creates a new instance of <see cref="Gu.Units.Voltage"/>.
        /// </summary>
        /// <param name="volts">The value in <see cref="Gu.Units.VoltageUnit.Volts"/></param>
        public static Voltage FromVolts(double volts)
        {
            return new Voltage(volts);
        }

        /// <summary>
        /// Creates a new instance of <see cref="Gu.Units.Voltage"/>.
        /// </summary>
        /// <param name="millivolts">The value in mV</param>
        public static Voltage FromMillivolts(double millivolts)
        {
            return From(millivolts, VoltageUnit.Millivolts);
        }

        /// <summary>
        /// Creates a new instance of <see cref="Gu.Units.Voltage"/>.
        /// </summary>
        /// <param name="kilovolts">The value in kV</param>
        public static Voltage FromKilovolts(double kilovolts)
        {
            return From(kilovolts, VoltageUnit.Kilovolts);
        }

        /// <summary>
        /// Creates a new instance of <see cref="Gu.Units.Voltage"/>.
        /// </summary>
        /// <param name="megavolts">The value in MV</param>
        public static Voltage FromMegavolts(double megavolts)
        {
            return From(megavolts, VoltageUnit.Megavolts);
        }

        /// <summary>
        /// Creates a new instance of <see cref="Gu.Units.Voltage"/>.
        /// </summary>
        /// <param name="microvolts">The value in µV</param>
        public static Voltage FromMicrovolts(double microvolts)
        {
            return From(microvolts, VoltageUnit.Microvolts);
        }

        public static Energy operator *(Voltage left, ElectricCharge right)
        {
            return Energy.FromJoules(left.volts * right.coulombs);
        }

        public static Power operator *(Voltage left, Current right)
        {
            return Power.FromWatts(left.volts * right.amperes);
        }

        public static Resistance operator /(Voltage left, Current right)
        {
            return Resistance.FromOhm(left.volts / right.amperes);
        }

        public static double operator /(Voltage left, Voltage right)
        {
            return left.volts / right.volts;
        }

        /// <summary>
        /// Indicates whether two <see cref="Gu.Units.Voltage"/> instances are equal.
        /// </summary>
        /// <returns>
        /// true if the quantitys of <paramref name="left"/> and <paramref name="right"/> are equal; otherwise, false.
        /// </returns>
        /// <param name="left">An instance of <see cref="Gu.Units.Voltage"/>.</param>
        /// <param name="right">An instance of <see cref="Gu.Units.Voltage"/>.</param>
        public static bool operator ==(Voltage left, Voltage right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Indicates whether two <see cref="Gu.Units.Voltage"/> instances are not equal.
        /// </summary>
        /// <returns>
        /// true if the quantitys of <paramref name="left"/> and <paramref name="right"/> are not equal; otherwise, false.
        /// </returns>
        /// <param name="left">An instance of <see cref="Gu.Units.Voltage"/>.</param>
        /// <param name="right">An instance of <see cref="Gu.Units.Voltage"/>.</param>
        public static bool operator !=(Voltage left, Voltage right)
        {
            return !left.Equals(right);
        }

        /// <summary>
        /// Indicates whether a specified <see cref="Gu.Units.Voltage"/> is less than another specified <see cref="Gu.Units.Voltage"/>.
        /// </summary>
        /// <returns>
        /// true if the quantity of <paramref name="left"/> is less than the quantity of <paramref name="right"/>; otherwise, false. 
        /// </returns>
        /// <param name="left">An instance of <see cref="Gu.Units.Voltage"/>.</param>
        /// <param name="right">An instance of <see cref="Gu.Units.Voltage"/>.</param>
        public static bool operator <(Voltage left, Voltage right)
        {
            return left.volts < right.volts;
        }

        /// <summary>
        /// Indicates whether a specified <see cref="Gu.Units.Voltage"/> is greater than another specified <see cref="Gu.Units.Voltage"/>.
        /// </summary>
        /// <returns>
        /// true if the quantity of <paramref name="left"/> is greater than the quantity of <paramref name="right"/>; otherwise, false. 
        /// </returns>
        /// <param name="left">An instance of <see cref="Gu.Units.Voltage"/>.</param>
        /// <param name="right">An instance of <see cref="Gu.Units.Voltage"/>.</param>
        public static bool operator >(Voltage left, Voltage right)
        {
            return left.volts > right.volts;
        }

        /// <summary>
        /// Indicates whether a specified <see cref="Gu.Units.Voltage"/> is less than or equal to another specified <see cref="Gu.Units.Voltage"/>.
        /// </summary>
        /// <returns>
        /// true if the quantity of <paramref name="left"/> is less than or equal to the quantity of <paramref name="right"/>; otherwise, false.
        /// </returns>
        /// <param name="left">An instance of <see cref="Gu.Units.Voltage"/>.</param>
        /// <param name="right">An instance of <see cref="Gu.Units.Voltage"/>.</param>
        public static bool operator <=(Voltage left, Voltage right)
        {
            return left.volts <= right.volts;
        }

        /// <summary>
        /// Indicates whether a specified <see cref="Gu.Units.Voltage"/> is greater than or equal to another specified <see cref="Gu.Units.Voltage"/>.
        /// </summary>
        /// <returns>
        /// true if the quantity of <paramref name="left"/> is greater than or equal to the quantity of <paramref name="right"/>; otherwise, false.
        /// </returns>
        /// <param name="left">An instance of <see cref="Gu.Units.Voltage"/>.</param>
        /// <param name="right">An instance of <see cref="Gu.Units.Voltage"/>.</param>
        public static bool operator >=(Voltage left, Voltage right)
        {
            return left.volts >= right.volts;
        }

        /// <summary>
        /// Multiplies an instance of <see cref="Gu.Units.Voltage"/> with <paramref name="left"/> and returns the result.
        /// </summary>
        /// <param name="right">An instance of <see cref="Gu.Units.Voltage"/></param>
        /// <param name="left">An instance of <seealso cref="System.Double"/></param>
        /// <returns>Multiplies an instance of <see cref="Gu.Units.Voltage"/> with <paramref name="left"/> and returns the result.</returns>
        public static Voltage operator *(double left, Voltage right)
        {
            return new Voltage(left * right.volts);
        }

        /// <summary>
        /// Multiplies an instance of <see cref="Gu.Units.Voltage"/> with <paramref name="right"/> and returns the result.
        /// </summary>
        /// <param name="left">An instance of <see cref="Gu.Units.Voltage"/></param>
        /// <param name="right">An instance of <seealso cref="System.Double"/></param>
        /// <returns>Multiplies an instance of <see cref="Gu.Units.Voltage"/> with <paramref name="right"/> and returns the result.</returns>
        public static Voltage operator *(Voltage left, double right)
        {
            return new Voltage(left.volts * right);
        }

        /// <summary>
        /// Divides an instance of <see cref="Gu.Units.Voltage"/> with <paramref name="right"/> and returns the result.
        /// </summary>
        /// <param name="left">An instance of <see cref="Gu.Units.Voltage"/></param>
        /// <param name="right">An instance of <seealso cref="System.Double"/></param>
        /// <returns>Divides an instance of <see cref="Gu.Units.Voltage"/> with <paramref name="right"/> and returns the result.</returns>
        public static Voltage operator /(Voltage left, double right)
        {
            return new Voltage(left.volts / right);
        }

        /// <summary>
        /// Adds two specified <see cref="Gu.Units.Voltage"/> instances.
        /// </summary>
        /// <returns>
        /// An <see cref="Gu.Units.Voltage"/> whose quantity is the sum of the quantitys of <paramref name="left"/> and <paramref name="right"/>.
        /// </returns>
        /// <param name="left">An instance of <see cref="Gu.Units.Voltage"/>.</param>
        /// <param name="right">An instance of <see cref="Gu.Units.Voltage"/>.</param>
        public static Voltage operator +(Voltage left, Voltage right)
        {
            return new Voltage(left.volts + right.volts);
        }

        /// <summary>
        /// Subtracts an Voltage from another Voltage and returns the difference.
        /// </summary>
        /// <returns>
        /// An <see cref="Gu.Units.Voltage"/> that is the difference
        /// </returns>
        /// <param name="left">An instance of <see cref="Gu.Units.Voltage"/> (the minuend).</param>
        /// <param name="right">An instance of <see cref="Gu.Units.Voltage"/> (the subtrahend).</param>
        public static Voltage operator -(Voltage left, Voltage right)
        {
            return new Voltage(left.volts - right.volts);
        }

        /// <summary>
        /// Returns an <see cref="Gu.Units.Voltage"/> whose quantity is the negated quantity of the specified instance.
        /// </summary>
        /// <returns>
        /// An <see cref="Gu.Units.Voltage"/> with the same numeric quantity as this instance, but the opposite sign.
        /// </returns>
        /// <param name="voltage">An instance of <see cref="Gu.Units.Voltage"/></param>
        public static Voltage operator -(Voltage voltage)
        {
            return new Voltage(-1 * voltage.volts);
        }

        /// <summary>
        /// Returns the specified instance of <see cref="Gu.Units.Voltage"/>.
        /// </summary>
        /// <returns>
        /// Returns <paramref name="voltage"/>.
        /// </returns>
        /// <param name="voltage">An instance of <see cref="Gu.Units.Voltage"/></param>
        public static Voltage operator +(Voltage voltage)
        {
            return voltage;
        }

        /// <summary>
        /// Get the scalar value
        /// </summary>
        /// <param name="unit"></param>
        /// <returns>The scalar value of this in the specified unit</returns>
        public double GetValue(VoltageUnit unit)
        {
            return unit.FromSiUnit(this.volts);
        }

        public override string ToString()
        {
            var quantityFormat = FormatParser<VoltageUnit>.GetOrCreate(string.Empty, this.SiUnit);
            return this.ToString(quantityFormat, null);
        }

        public string ToString(string format)
        {
            var quantityFormat = FormatParser<VoltageUnit>.GetOrCreate(format);
            return ToString(quantityFormat, null);
        }

        public string ToString(IFormatProvider provider)
        {
            var quantityFormat = FormatParser<VoltageUnit>.GetOrCreate(string.Empty, SiUnit);
            return ToString(quantityFormat, provider);
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            var quantityFormat = FormatParser<VoltageUnit>.GetOrCreate(format);
            return ToString(quantityFormat, formatProvider);
        }

        public string ToString(VoltageUnit unit)
        {
            var quantityFormat = FormatParser<VoltageUnit>.GetOrCreate(string.Empty, unit);
            return ToString(quantityFormat, null);
        }

        public string ToString(VoltageUnit unit, IFormatProvider formatProvider)
        {
            var quantityFormat = FormatParser<VoltageUnit>.GetOrCreate(string.Empty, unit);
            return ToString(quantityFormat, formatProvider);
        }

        public string ToString(string valueFormat, VoltageUnit unit)
        {
            var quantityFormat = FormatParser<VoltageUnit>.GetOrCreate(valueFormat, unit);
            return ToString(quantityFormat, null);
        }

        public string ToString(string valueFormat, VoltageUnit unit, IFormatProvider formatProvider)
        {
            var quantityFormat = FormatParser<VoltageUnit>.GetOrCreate(valueFormat, unit);
            return ToString(quantityFormat, formatProvider);
        }

        private string ToString(QuantityFormat<VoltageUnit> format, IFormatProvider formatProvider)
        {
            using (var builder = StringBuilderPool.Borrow())
            {
                builder.Append(this, format, formatProvider);
                return builder.ToString();
            }
        }

        /// <summary>
        /// Compares this instance to a specified <see cref="Gu.Units.Voltage"/> object and returns an integer that indicates whether this <see cref="quantity"/> is smaller than, equal to, or greater than the <see cref="Gu.Units.Voltage"/> object.
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
        /// <param name="quantity">An instance of <see cref="Gu.Units.Voltage"/> object to compare to this instance.</param>
        public int CompareTo(Voltage quantity)
        {
            return this.volts.CompareTo(quantity.volts);
        }

        /// <summary>
        /// Returns a quantity indicating whether this instance is equal to a specified <see cref="Gu.Units.Voltage"/> object.
        /// </summary>
        /// <returns>
        /// true if <paramref name="other"/> represents the same Voltage as this instance; otherwise, false.
        /// </returns>
        /// <param name="other">An instance of <see cref="Gu.Units.Voltage"/> object to compare with this instance.</param>
        public bool Equals(Voltage other)
        {
            return this.volts.Equals(other.volts);
        }

        /// <summary>
        /// Returns a quantity indicating whether this instance is equal to a specified <see cref="Gu.Units.Voltage"/> object within the given tolerance.
        /// </summary>
        /// <returns>
        /// true if <paramref name="other"/> represents the same Voltage as this instance; otherwise, false.
        /// </returns>
        /// <param name="other">An instance of <see cref="Gu.Units.Voltage"/> object to compare with this instance.</param>
        /// <param name="tolerance">The maximum difference for being considered equal</param>
        public bool Equals(Voltage other, double tolerance)
        {
            return Math.Abs(this.volts - other.volts) < tolerance;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            return obj is Voltage && this.Equals((Voltage)obj);
        }

        public override int GetHashCode()
        {
            return this.volts.GetHashCode();
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
            XmlExt.SetReadonlyField(ref this, "volts", reader, "Value");
        }

        /// <summary>
        /// Converts an object into its XML representation.
        /// </summary>
        /// <param name="writer">The <see cref="System.Xml.XmlWriter"/> stream to which the object is serialized. </param>
        public void WriteXml(XmlWriter writer)
        {
            XmlExt.WriteAttribute(writer, "Value", this.volts);
        }
    }
}