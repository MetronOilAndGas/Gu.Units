﻿namespace Gu.Units
{
    using System;
    using System.ComponentModel;
    using System.Globalization;
    using System.Xml;
    using System.Xml.Schema;
    using System.Xml.Serialization;

    /// <summary>
    /// A type for the quantity <see cref="Gu.Units.Jerk"/>.
    /// </summary>
    [Serializable, TypeConverter(typeof(JerkTypeConverter))]
    public partial struct Jerk : IComparable<Jerk>, IEquatable<Jerk>, IFormattable, IXmlSerializable, IQuantity<LengthUnit, I1, TimeUnit, INeg3>, IQuantity<JerkUnit>
    {
        public static readonly Jerk Zero = new Jerk();

        /// <summary>
        /// The quantity in <see cref="Gu.Units.JerkUnit.MetresPerSecondCubed"/>.
        /// </summary>
        internal readonly double metresPerSecondCubed;

        private Jerk(double metresPerSecondCubed)
        {
            this.metresPerSecondCubed = metresPerSecondCubed;
        }

        /// <summary>
        /// Initializes a new instance of <see cref="Gu.Units.Jerk"/>.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="unit"><see cref="Gu.Units.JerkUnit"/>.</param>
        public Jerk(double value, JerkUnit unit)
        {
            this.metresPerSecondCubed = unit.ToSiUnit(value);
        }

        /// <summary>
        /// The quantity in <see cref="Gu.Units.JerkUnit.MetresPerSecondCubed"/>
        /// </summary>
        public double SiValue
        {
            get
            {
                return this.metresPerSecondCubed;
            }
        }

        /// <summary>
        /// The <see cref="Gu.Units.JerkUnit"/> for the <see cref="SiValue"/>
        /// </summary>
        public JerkUnit SiUnit => JerkUnit.MetresPerSecondCubed;

        /// <summary>
        /// The <see cref="Gu.Units.IUnit"/> for the <see cref="SiValue"/>
        /// </summary>
        IUnit IQuantity.SiUnit => JerkUnit.MetresPerSecondCubed;

        /// <summary>
        /// The quantity in metresPerSecondCubed".
        /// </summary>
        public double MetresPerSecondCubed
        {
            get
            {
                return this.metresPerSecondCubed;
            }
        }

        /// <summary>
        /// The quantity in millimetresPerSecondCubed
        /// </summary>
        public double MillimetresPerSecondCubed
        {
            get
            {
                return JerkUnit.MillimetresPerSecondCubed.FromSiUnit(this.metresPerSecondCubed);
            }
        }

        /// <summary>
        /// The quantity in millimetresPerHourCubed
        /// </summary>
        public double MillimetresPerHourCubed
        {
            get
            {
                return JerkUnit.MillimetresPerHourCubed.FromSiUnit(this.metresPerSecondCubed);
            }
        }

        /// <summary>
        /// The quantity in millimetresPerMinuteCubed
        /// </summary>
        public double MillimetresPerMinuteCubed
        {
            get
            {
                return JerkUnit.MillimetresPerMinuteCubed.FromSiUnit(this.metresPerSecondCubed);
            }
        }

        /// <summary>
        /// The quantity in metresPerHourCubed
        /// </summary>
        public double MetresPerHourCubed
        {
            get
            {
                return JerkUnit.MetresPerHourCubed.FromSiUnit(this.metresPerSecondCubed);
            }
        }

        /// <summary>
        /// The quantity in metresPerMinuteCubed
        /// </summary>
        public double MetresPerMinuteCubed
        {
            get
            {
                return JerkUnit.MetresPerMinuteCubed.FromSiUnit(this.metresPerSecondCubed);
            }
        }

        /// <summary>
        /// The quantity in nanometresPerHourCubed
        /// </summary>
        public double NanometresPerHourCubed
        {
            get
            {
                return JerkUnit.NanometresPerHourCubed.FromSiUnit(this.metresPerSecondCubed);
            }
        }

        /// <summary>
        /// The quantity in nanometresPerMinuteCubed
        /// </summary>
        public double NanometresPerMinuteCubed
        {
            get
            {
                return JerkUnit.NanometresPerMinuteCubed.FromSiUnit(this.metresPerSecondCubed);
            }
        }

        /// <summary>
        /// The quantity in centimetresPerSecondCubed
        /// </summary>
        public double CentimetresPerSecondCubed
        {
            get
            {
                return JerkUnit.CentimetresPerSecondCubed.FromSiUnit(this.metresPerSecondCubed);
            }
        }

        /// <summary>
        /// The quantity in centimetresPerHourCubed
        /// </summary>
        public double CentimetresPerHourCubed
        {
            get
            {
                return JerkUnit.CentimetresPerHourCubed.FromSiUnit(this.metresPerSecondCubed);
            }
        }

        /// <summary>
        /// The quantity in centimetresPerMinuteCubed
        /// </summary>
        public double CentimetresPerMinuteCubed
        {
            get
            {
                return JerkUnit.CentimetresPerMinuteCubed.FromSiUnit(this.metresPerSecondCubed);
            }
        }

        /// <summary>
        /// Creates an instance of <see cref="Gu.Units.Jerk"/> from its string representation
        /// </summary>
        /// <param name="s">The string representation of the <see cref="Gu.Units.Jerk"/></param>
        /// <returns></returns>
		public static Jerk Parse(string s)
        {
            return QuantityParser.Parse<JerkUnit, Jerk>(s, From, NumberStyles.Float, CultureInfo.CurrentCulture);
        }

        public static Jerk Parse(string s, IFormatProvider provider)
        {
            return QuantityParser.Parse<JerkUnit, Jerk>(s, From, NumberStyles.Float, provider);
        }

        public static Jerk Parse(string s, NumberStyles styles)
        {
            return QuantityParser.Parse<JerkUnit, Jerk>(s, From, styles, CultureInfo.CurrentCulture);
        }

        public static Jerk Parse(string s, NumberStyles styles, IFormatProvider provider)
        {
            return QuantityParser.Parse<JerkUnit, Jerk>(s, From, styles, provider);
        }

        public static bool TryParse(string s, out Jerk value)
        {
            return QuantityParser.TryParse<JerkUnit, Jerk>(s, From, NumberStyles.Float, CultureInfo.CurrentCulture, out value);
        }

        public static bool TryParse(string s, IFormatProvider provider, out Jerk value)
        {
            return QuantityParser.TryParse<JerkUnit, Jerk>(s, From, NumberStyles.Float, provider, out value);
        }

        public static bool TryParse(string s, NumberStyles styles, out Jerk value)
        {
            return QuantityParser.TryParse<JerkUnit, Jerk>(s, From, styles, CultureInfo.CurrentCulture, out value);
        }

        public static bool TryParse(string s, NumberStyles styles, IFormatProvider provider, out Jerk value)
        {
            return QuantityParser.TryParse<JerkUnit, Jerk>(s, From, styles, provider, out value);
        }

        /// <summary>
        /// Reads an instance of <see cref="Gu.Units.Jerk"/> from the <paramref name="reader"/>
        /// </summary>
        /// <param name="reader"></param>
        /// <returns>An instance of  <see cref="Gu.Units.Jerk"/></returns>
        public static Jerk ReadFrom(XmlReader reader)
        {
            var v = new Jerk();
            v.ReadXml(reader);
            return v;
        }

        /// <summary>
        /// Creates a new instance of <see cref="Gu.Units.Jerk"/>.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="unit"></param>
        public static Jerk From(double value, JerkUnit unit)
        {
            return new Jerk(unit.ToSiUnit(value));
        }

        /// <summary>
        /// Creates a new instance of <see cref="Gu.Units.Jerk"/>.
        /// </summary>
        /// <param name="metresPerSecondCubed">The value in <see cref="Gu.Units.JerkUnit.MetresPerSecondCubed"/></param>
        public static Jerk FromMetresPerSecondCubed(double metresPerSecondCubed)
        {
            return new Jerk(metresPerSecondCubed);
        }

        /// <summary>
        /// Creates a new instance of <see cref="Gu.Units.Jerk"/>.
        /// </summary>
        /// <param name="millimetresPerSecondCubed">The value in mm⋅s⁻³</param>
        public static Jerk FromMillimetresPerSecondCubed(double millimetresPerSecondCubed)
        {
            return From(millimetresPerSecondCubed, JerkUnit.MillimetresPerSecondCubed);
        }

        /// <summary>
        /// Creates a new instance of <see cref="Gu.Units.Jerk"/>.
        /// </summary>
        /// <param name="millimetresPerHourCubed">The value in mm⋅h⁻³</param>
        public static Jerk FromMillimetresPerHourCubed(double millimetresPerHourCubed)
        {
            return From(millimetresPerHourCubed, JerkUnit.MillimetresPerHourCubed);
        }

        /// <summary>
        /// Creates a new instance of <see cref="Gu.Units.Jerk"/>.
        /// </summary>
        /// <param name="millimetresPerMinuteCubed">The value in mm⋅min⁻³</param>
        public static Jerk FromMillimetresPerMinuteCubed(double millimetresPerMinuteCubed)
        {
            return From(millimetresPerMinuteCubed, JerkUnit.MillimetresPerMinuteCubed);
        }

        /// <summary>
        /// Creates a new instance of <see cref="Gu.Units.Jerk"/>.
        /// </summary>
        /// <param name="metresPerHourCubed">The value in m⋅h⁻³</param>
        public static Jerk FromMetresPerHourCubed(double metresPerHourCubed)
        {
            return From(metresPerHourCubed, JerkUnit.MetresPerHourCubed);
        }

        /// <summary>
        /// Creates a new instance of <see cref="Gu.Units.Jerk"/>.
        /// </summary>
        /// <param name="metresPerMinuteCubed">The value in m⋅min⁻³</param>
        public static Jerk FromMetresPerMinuteCubed(double metresPerMinuteCubed)
        {
            return From(metresPerMinuteCubed, JerkUnit.MetresPerMinuteCubed);
        }

        /// <summary>
        /// Creates a new instance of <see cref="Gu.Units.Jerk"/>.
        /// </summary>
        /// <param name="nanometresPerHourCubed">The value in nm⋅h⁻³</param>
        public static Jerk FromNanometresPerHourCubed(double nanometresPerHourCubed)
        {
            return From(nanometresPerHourCubed, JerkUnit.NanometresPerHourCubed);
        }

        /// <summary>
        /// Creates a new instance of <see cref="Gu.Units.Jerk"/>.
        /// </summary>
        /// <param name="nanometresPerMinuteCubed">The value in nm⋅min⁻³</param>
        public static Jerk FromNanometresPerMinuteCubed(double nanometresPerMinuteCubed)
        {
            return From(nanometresPerMinuteCubed, JerkUnit.NanometresPerMinuteCubed);
        }

        /// <summary>
        /// Creates a new instance of <see cref="Gu.Units.Jerk"/>.
        /// </summary>
        /// <param name="centimetresPerSecondCubed">The value in cm⋅s⁻³</param>
        public static Jerk FromCentimetresPerSecondCubed(double centimetresPerSecondCubed)
        {
            return From(centimetresPerSecondCubed, JerkUnit.CentimetresPerSecondCubed);
        }

        /// <summary>
        /// Creates a new instance of <see cref="Gu.Units.Jerk"/>.
        /// </summary>
        /// <param name="centimetresPerHourCubed">The value in cm⋅h⁻³</param>
        public static Jerk FromCentimetresPerHourCubed(double centimetresPerHourCubed)
        {
            return From(centimetresPerHourCubed, JerkUnit.CentimetresPerHourCubed);
        }

        /// <summary>
        /// Creates a new instance of <see cref="Gu.Units.Jerk"/>.
        /// </summary>
        /// <param name="centimetresPerMinuteCubed">The value in cm⋅min⁻³</param>
        public static Jerk FromCentimetresPerMinuteCubed(double centimetresPerMinuteCubed)
        {
            return From(centimetresPerMinuteCubed, JerkUnit.CentimetresPerMinuteCubed);
        }

        public static Frequency operator /(Jerk left, Acceleration right)
        {
            return Frequency.FromHertz(left.metresPerSecondCubed / right.metresPerSecondSquared);
        }

        public static Acceleration operator *(Jerk left, Time right)
        {
            return Acceleration.FromMetresPerSecondSquared(left.metresPerSecondCubed * right.seconds);
        }

        public static double operator /(Jerk left, Jerk right)
        {
            return left.metresPerSecondCubed / right.metresPerSecondCubed;
        }

        /// <summary>
        /// Indicates whether two <see cref="Gu.Units.Jerk"/> instances are equal.
        /// </summary>
        /// <returns>
        /// true if the quantitys of <paramref name="left"/> and <paramref name="right"/> are equal; otherwise, false.
        /// </returns>
        /// <param name="left">An instance of <see cref="Gu.Units.Jerk"/>.</param>
        /// <param name="right">An instance of <see cref="Gu.Units.Jerk"/>.</param>
        public static bool operator ==(Jerk left, Jerk right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Indicates whether two <see cref="Gu.Units.Jerk"/> instances are not equal.
        /// </summary>
        /// <returns>
        /// true if the quantitys of <paramref name="left"/> and <paramref name="right"/> are not equal; otherwise, false.
        /// </returns>
        /// <param name="left">An instance of <see cref="Gu.Units.Jerk"/>.</param>
        /// <param name="right">An instance of <see cref="Gu.Units.Jerk"/>.</param>
        public static bool operator !=(Jerk left, Jerk right)
        {
            return !left.Equals(right);
        }

        /// <summary>
        /// Indicates whether a specified <see cref="Gu.Units.Jerk"/> is less than another specified <see cref="Gu.Units.Jerk"/>.
        /// </summary>
        /// <returns>
        /// true if the quantity of <paramref name="left"/> is less than the quantity of <paramref name="right"/>; otherwise, false. 
        /// </returns>
        /// <param name="left">An instance of <see cref="Gu.Units.Jerk"/>.</param>
        /// <param name="right">An instance of <see cref="Gu.Units.Jerk"/>.</param>
        public static bool operator <(Jerk left, Jerk right)
        {
            return left.metresPerSecondCubed < right.metresPerSecondCubed;
        }

        /// <summary>
        /// Indicates whether a specified <see cref="Gu.Units.Jerk"/> is greater than another specified <see cref="Gu.Units.Jerk"/>.
        /// </summary>
        /// <returns>
        /// true if the quantity of <paramref name="left"/> is greater than the quantity of <paramref name="right"/>; otherwise, false. 
        /// </returns>
        /// <param name="left">An instance of <see cref="Gu.Units.Jerk"/>.</param>
        /// <param name="right">An instance of <see cref="Gu.Units.Jerk"/>.</param>
        public static bool operator >(Jerk left, Jerk right)
        {
            return left.metresPerSecondCubed > right.metresPerSecondCubed;
        }

        /// <summary>
        /// Indicates whether a specified <see cref="Gu.Units.Jerk"/> is less than or equal to another specified <see cref="Gu.Units.Jerk"/>.
        /// </summary>
        /// <returns>
        /// true if the quantity of <paramref name="left"/> is less than or equal to the quantity of <paramref name="right"/>; otherwise, false.
        /// </returns>
        /// <param name="left">An instance of <see cref="Gu.Units.Jerk"/>.</param>
        /// <param name="right">An instance of <see cref="Gu.Units.Jerk"/>.</param>
        public static bool operator <=(Jerk left, Jerk right)
        {
            return left.metresPerSecondCubed <= right.metresPerSecondCubed;
        }

        /// <summary>
        /// Indicates whether a specified <see cref="Gu.Units.Jerk"/> is greater than or equal to another specified <see cref="Gu.Units.Jerk"/>.
        /// </summary>
        /// <returns>
        /// true if the quantity of <paramref name="left"/> is greater than or equal to the quantity of <paramref name="right"/>; otherwise, false.
        /// </returns>
        /// <param name="left">An instance of <see cref="Gu.Units.Jerk"/>.</param>
        /// <param name="right">An instance of <see cref="Gu.Units.Jerk"/>.</param>
        public static bool operator >=(Jerk left, Jerk right)
        {
            return left.metresPerSecondCubed >= right.metresPerSecondCubed;
        }

        /// <summary>
        /// Multiplies an instance of <see cref="Gu.Units.Jerk"/> with <paramref name="left"/> and returns the result.
        /// </summary>
        /// <param name="right">An instance of <see cref="Gu.Units.Jerk"/></param>
        /// <param name="left">An instance of <seealso cref="System.Double"/></param>
        /// <returns>Multiplies an instance of <see cref="Gu.Units.Jerk"/> with <paramref name="left"/> and returns the result.</returns>
        public static Jerk operator *(double left, Jerk right)
        {
            return new Jerk(left * right.metresPerSecondCubed);
        }

        /// <summary>
        /// Multiplies an instance of <see cref="Gu.Units.Jerk"/> with <paramref name="right"/> and returns the result.
        /// </summary>
        /// <param name="left">An instance of <see cref="Gu.Units.Jerk"/></param>
        /// <param name="right">An instance of <seealso cref="System.Double"/></param>
        /// <returns>Multiplies an instance of <see cref="Gu.Units.Jerk"/> with <paramref name="right"/> and returns the result.</returns>
        public static Jerk operator *(Jerk left, double right)
        {
            return new Jerk(left.metresPerSecondCubed * right);
        }

        /// <summary>
        /// Divides an instance of <see cref="Gu.Units.Jerk"/> with <paramref name="right"/> and returns the result.
        /// </summary>
        /// <param name="left">An instance of <see cref="Gu.Units.Jerk"/></param>
        /// <param name="right">An instance of <seealso cref="System.Double"/></param>
        /// <returns>Divides an instance of <see cref="Gu.Units.Jerk"/> with <paramref name="right"/> and returns the result.</returns>
        public static Jerk operator /(Jerk left, double right)
        {
            return new Jerk(left.metresPerSecondCubed / right);
        }

        /// <summary>
        /// Adds two specified <see cref="Gu.Units.Jerk"/> instances.
        /// </summary>
        /// <returns>
        /// An <see cref="Gu.Units.Jerk"/> whose quantity is the sum of the quantitys of <paramref name="left"/> and <paramref name="right"/>.
        /// </returns>
        /// <param name="left">An instance of <see cref="Gu.Units.Jerk"/>.</param>
        /// <param name="right">An instance of <see cref="Gu.Units.Jerk"/>.</param>
        public static Jerk operator +(Jerk left, Jerk right)
        {
            return new Jerk(left.metresPerSecondCubed + right.metresPerSecondCubed);
        }

        /// <summary>
        /// Subtracts an Jerk from another Jerk and returns the difference.
        /// </summary>
        /// <returns>
        /// An <see cref="Gu.Units.Jerk"/> that is the difference
        /// </returns>
        /// <param name="left">An instance of <see cref="Gu.Units.Jerk"/> (the minuend).</param>
        /// <param name="right">An instance of <see cref="Gu.Units.Jerk"/> (the subtrahend).</param>
        public static Jerk operator -(Jerk left, Jerk right)
        {
            return new Jerk(left.metresPerSecondCubed - right.metresPerSecondCubed);
        }

        /// <summary>
        /// Returns an <see cref="Gu.Units.Jerk"/> whose quantity is the negated quantity of the specified instance.
        /// </summary>
        /// <returns>
        /// An <see cref="Gu.Units.Jerk"/> with the same numeric quantity as this instance, but the opposite sign.
        /// </returns>
        /// <param name="jerk">An instance of <see cref="Gu.Units.Jerk"/></param>
        public static Jerk operator -(Jerk jerk)
        {
            return new Jerk(-1 * jerk.metresPerSecondCubed);
        }

        /// <summary>
        /// Returns the specified instance of <see cref="Gu.Units.Jerk"/>.
        /// </summary>
        /// <returns>
        /// Returns <paramref name="jerk"/>.
        /// </returns>
        /// <param name="jerk">An instance of <see cref="Gu.Units.Jerk"/></param>
        public static Jerk operator +(Jerk jerk)
        {
            return jerk;
        }

        /// <summary>
        /// Get the scalar value
        /// </summary>
        /// <param name="unit"></param>
        /// <returns>The scalar value of this in the specified unit</returns>
        public double GetValue(JerkUnit unit)
        {
            return unit.FromSiUnit(this.metresPerSecondCubed);
        }

        public override string ToString()
        {
            return this.ToString((string)null, (IFormatProvider)NumberFormatInfo.CurrentInfo);
        }

        public string ToString(string format)
        {
            return this.ToString(format, (IFormatProvider)NumberFormatInfo.CurrentInfo);
        }

        public string ToString(IFormatProvider provider)
        {
            return this.ToString((string)null, (IFormatProvider)NumberFormatInfo.GetInstance(provider));
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            return this.ToString(format, formatProvider, JerkUnit.MetresPerSecondCubed);
        }

        public string ToString(JerkUnit unit)
        {
            return this.ToString((string)null, (IFormatProvider)NumberFormatInfo.CurrentInfo, unit);
        }

        public string ToString(string valueFormat, JerkUnit unit)
        {
            return this.ToString(valueFormat, (IFormatProvider)NumberFormatInfo.CurrentInfo, unit);
        }

        public string ToString(string valueFormat, IFormatProvider formatProvider, JerkUnit unit)
        {
            var quantity = unit.FromSiUnit(this.metresPerSecondCubed);
            return string.Format("{0}{1}", quantity.ToString(valueFormat, formatProvider), unit.Symbol);
        }

        /// <summary>
        /// Compares this instance to a specified <see cref="Gu.Units.Jerk"/> object and returns an integer that indicates whether this <see cref="quantity"/> is smaller than, equal to, or greater than the <see cref="Gu.Units.Jerk"/> object.
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
        /// <param name="quantity">An instance of <see cref="Gu.Units.Jerk"/> object to compare to this instance.</param>
        public int CompareTo(Jerk quantity)
        {
            return this.metresPerSecondCubed.CompareTo(quantity.metresPerSecondCubed);
        }

        /// <summary>
        /// Returns a quantity indicating whether this instance is equal to a specified <see cref="Gu.Units.Jerk"/> object.
        /// </summary>
        /// <returns>
        /// true if <paramref name="other"/> represents the same Jerk as this instance; otherwise, false.
        /// </returns>
        /// <param name="other">An instance of <see cref="Gu.Units.Jerk"/> object to compare with this instance.</param>
        public bool Equals(Jerk other)
        {
            return this.metresPerSecondCubed.Equals(other.metresPerSecondCubed);
        }

        /// <summary>
        /// Returns a quantity indicating whether this instance is equal to a specified <see cref="Gu.Units.Jerk"/> object within the given tolerance.
        /// </summary>
        /// <returns>
        /// true if <paramref name="other"/> represents the same Jerk as this instance; otherwise, false.
        /// </returns>
        /// <param name="other">An instance of <see cref="Gu.Units.Jerk"/> object to compare with this instance.</param>
        /// <param name="tolerance">The maximum difference for being considered equal</param>
        public bool Equals(Jerk other, double tolerance)
        {
            return Math.Abs(this.metresPerSecondCubed - other.metresPerSecondCubed) < tolerance;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            return obj is Jerk && this.Equals((Jerk)obj);
        }

        public override int GetHashCode()
        {
            return this.metresPerSecondCubed.GetHashCode();
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
            XmlExt.SetReadonlyField(ref this, "metresPerSecondCubed", reader, "Value");
        }

        /// <summary>
        /// Converts an object into its XML representation.
        /// </summary>
        /// <param name="writer">The <see cref="System.Xml.XmlWriter"/> stream to which the object is serialized. </param>
        public void WriteXml(XmlWriter writer)
        {
            XmlExt.WriteAttribute(writer, "Value", this.metresPerSecondCubed);
        }
    }
}