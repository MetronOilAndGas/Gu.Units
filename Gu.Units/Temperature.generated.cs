﻿namespace Gu.Units
{
    using System;
    using System.Globalization;
    using System.Xml;
    using System.Xml.Schema;

    /// <summary>
    /// A type for the quantity <see cref="Gu.Units.Temperature"/>.
    /// </summary>
    // [TypeConverter(typeof(TemperatureTypeConverter))]
    [Serializable]
    public partial struct Temperature : IQuantity<TemperatureUnit>, IComparable<Temperature>, IEquatable<Temperature>
    {
        public static readonly Temperature Zero = new Temperature();

        /// <summary>
        /// The quantity in <see cref="Gu.Units.TemperatureUnit.Kelvin"/>.
        /// </summary>
        internal readonly double kelvin;

        private Temperature(double kelvin)
        {
            this.kelvin = kelvin;
        }

        /// <summary>
        /// Initializes a new instance of <see cref="Gu.Units.Temperature"/>.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="unit"><see cref="Gu.Units.TemperatureUnit"/>.</param>
        public Temperature(double value, TemperatureUnit unit)
        {
            this.kelvin = unit.ToSiUnit(value);
        }

        /// <summary>
        /// The quantity in <see cref="Gu.Units.TemperatureUnit.Kelvin"/>
        /// </summary>
        public double SiValue
        {
            get
            {
                return this.kelvin;
            }
        }

        /// <summary>
        /// The <see cref="Gu.Units.TemperatureUnit"/> for the <see cref="SiValue"/>
        /// </summary>
        public TemperatureUnit SiUnit => TemperatureUnit.Kelvin;

        /// <summary>
        /// The <see cref="Gu.Units.IUnit"/> for the <see cref="SiValue"/>
        /// </summary>
        IUnit IQuantity.SiUnit => TemperatureUnit.Kelvin;

        /// <summary>
        /// The quantity in kelvin".
        /// </summary>
        public double Kelvin
        {
            get
            {
                return this.kelvin;
            }
        }

        /// <summary>
        /// The quantity in celsius
        /// </summary>
        public double Celsius
        {
            get
            {
                return TemperatureUnit.Celsius.FromSiUnit(this.kelvin);
            }
        }

        /// <summary>
        /// The quantity in fahrenheit
        /// </summary>
        public double Fahrenheit
        {
            get
            {
                return TemperatureUnit.Fahrenheit.FromSiUnit(this.kelvin);
            }
        }

        /// <summary>
        /// Creates an instance of <see cref="Gu.Units.Temperature"/> from its string representation
        /// </summary>
        /// <param name="s">The string representation of the <see cref="Gu.Units.Temperature"/></param>
        /// <returns></returns>
		public static Temperature Parse(string s)
        {
            return QuantityParser.Parse<TemperatureUnit, Temperature>(s, From, NumberStyles.Float, CultureInfo.CurrentCulture);
        }

        public static Temperature Parse(string s, IFormatProvider provider)
        {
            return QuantityParser.Parse<TemperatureUnit, Temperature>(s, From, NumberStyles.Float, provider);
        }

        public static Temperature Parse(string s, NumberStyles styles)
        {
            return QuantityParser.Parse<TemperatureUnit, Temperature>(s, From, styles, CultureInfo.CurrentCulture);
        }

        public static Temperature Parse(string s, NumberStyles styles, IFormatProvider provider)
        {
            return QuantityParser.Parse<TemperatureUnit, Temperature>(s, From, styles, provider);
        }

        public static bool TryParse(string s, out Temperature value)
        {
            return QuantityParser.TryParse<TemperatureUnit, Temperature>(s, From, NumberStyles.Float, CultureInfo.CurrentCulture, out value);
        }

        public static bool TryParse(string s, IFormatProvider provider, out Temperature value)
        {
            return QuantityParser.TryParse<TemperatureUnit, Temperature>(s, From, NumberStyles.Float, provider, out value);
        }

        public static bool TryParse(string s, NumberStyles styles, out Temperature value)
        {
            return QuantityParser.TryParse<TemperatureUnit, Temperature>(s, From, styles, CultureInfo.CurrentCulture, out value);
        }

        public static bool TryParse(string s, NumberStyles styles, IFormatProvider provider, out Temperature value)
        {
            return QuantityParser.TryParse<TemperatureUnit, Temperature>(s, From, styles, provider, out value);
        }

        /// <summary>
        /// Reads an instance of <see cref="Gu.Units.Temperature"/> from the <paramref name="reader"/>
        /// </summary>
        /// <param name="reader"></param>
        /// <returns>An instance of  <see cref="Gu.Units.Temperature"/></returns>
        public static Temperature ReadFrom(XmlReader reader)
        {
            var v = new Temperature();
            v.ReadXml(reader);
            return v;
        }

        /// <summary>
        /// Creates a new instance of <see cref="Gu.Units.Temperature"/>.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="unit"></param>
        public static Temperature From(double value, TemperatureUnit unit)
        {
            return new Temperature(unit.ToSiUnit(value));
        }

        /// <summary>
        /// Creates a new instance of <see cref="Gu.Units.Temperature"/>.
        /// </summary>
        /// <param name="kelvin">The value in <see cref="Gu.Units.TemperatureUnit.Kelvin"/></param>
        public static Temperature FromKelvin(double kelvin)
        {
            return new Temperature(kelvin);
        }

        /// <summary>
        /// Creates a new instance of <see cref="Gu.Units.Temperature"/>.
        /// </summary>
        /// <param name="celsius">The value in °C</param>
        public static Temperature FromCelsius(double celsius)
        {
            return From(celsius, TemperatureUnit.Celsius);
        }

        /// <summary>
        /// Creates a new instance of <see cref="Gu.Units.Temperature"/>.
        /// </summary>
        /// <param name="fahrenheit">The value in °F</param>
        public static Temperature FromFahrenheit(double fahrenheit)
        {
            return From(fahrenheit, TemperatureUnit.Fahrenheit);
        }

        public static double operator /(Temperature left, Temperature right)
        {
            return left.kelvin / right.kelvin;
        }

        /// <summary>
        /// Indicates whether two <see cref="Gu.Units.Temperature"/> instances are equal.
        /// </summary>
        /// <returns>
        /// true if the quantitys of <paramref name="left"/> and <paramref name="right"/> are equal; otherwise, false.
        /// </returns>
        /// <param name="left">An instance of <see cref="Gu.Units.Temperature"/>.</param>
        /// <param name="right">An instance of <see cref="Gu.Units.Temperature"/>.</param>
        public static bool operator ==(Temperature left, Temperature right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Indicates whether two <see cref="Gu.Units.Temperature"/> instances are not equal.
        /// </summary>
        /// <returns>
        /// true if the quantitys of <paramref name="left"/> and <paramref name="right"/> are not equal; otherwise, false.
        /// </returns>
        /// <param name="left">An instance of <see cref="Gu.Units.Temperature"/>.</param>
        /// <param name="right">An instance of <see cref="Gu.Units.Temperature"/>.</param>
        public static bool operator !=(Temperature left, Temperature right)
        {
            return !left.Equals(right);
        }

        /// <summary>
        /// Indicates whether a specified <see cref="Gu.Units.Temperature"/> is less than another specified <see cref="Gu.Units.Temperature"/>.
        /// </summary>
        /// <returns>
        /// true if the quantity of <paramref name="left"/> is less than the quantity of <paramref name="right"/>; otherwise, false. 
        /// </returns>
        /// <param name="left">An instance of <see cref="Gu.Units.Temperature"/>.</param>
        /// <param name="right">An instance of <see cref="Gu.Units.Temperature"/>.</param>
        public static bool operator <(Temperature left, Temperature right)
        {
            return left.kelvin < right.kelvin;
        }

        /// <summary>
        /// Indicates whether a specified <see cref="Gu.Units.Temperature"/> is greater than another specified <see cref="Gu.Units.Temperature"/>.
        /// </summary>
        /// <returns>
        /// true if the quantity of <paramref name="left"/> is greater than the quantity of <paramref name="right"/>; otherwise, false. 
        /// </returns>
        /// <param name="left">An instance of <see cref="Gu.Units.Temperature"/>.</param>
        /// <param name="right">An instance of <see cref="Gu.Units.Temperature"/>.</param>
        public static bool operator >(Temperature left, Temperature right)
        {
            return left.kelvin > right.kelvin;
        }

        /// <summary>
        /// Indicates whether a specified <see cref="Gu.Units.Temperature"/> is less than or equal to another specified <see cref="Gu.Units.Temperature"/>.
        /// </summary>
        /// <returns>
        /// true if the quantity of <paramref name="left"/> is less than or equal to the quantity of <paramref name="right"/>; otherwise, false.
        /// </returns>
        /// <param name="left">An instance of <see cref="Gu.Units.Temperature"/>.</param>
        /// <param name="right">An instance of <see cref="Gu.Units.Temperature"/>.</param>
        public static bool operator <=(Temperature left, Temperature right)
        {
            return left.kelvin <= right.kelvin;
        }

        /// <summary>
        /// Indicates whether a specified <see cref="Gu.Units.Temperature"/> is greater than or equal to another specified <see cref="Gu.Units.Temperature"/>.
        /// </summary>
        /// <returns>
        /// true if the quantity of <paramref name="left"/> is greater than or equal to the quantity of <paramref name="right"/>; otherwise, false.
        /// </returns>
        /// <param name="left">An instance of <see cref="Gu.Units.Temperature"/>.</param>
        /// <param name="right">An instance of <see cref="Gu.Units.Temperature"/>.</param>
        public static bool operator >=(Temperature left, Temperature right)
        {
            return left.kelvin >= right.kelvin;
        }

        /// <summary>
        /// Multiplies an instance of <see cref="Gu.Units.Temperature"/> with <paramref name="left"/> and returns the result.
        /// </summary>
        /// <param name="right">An instance of <see cref="Gu.Units.Temperature"/></param>
        /// <param name="left">An instance of <seealso cref="System.Double"/></param>
        /// <returns>Multiplies an instance of <see cref="Gu.Units.Temperature"/> with <paramref name="left"/> and returns the result.</returns>
        public static Temperature operator *(double left, Temperature right)
        {
            return new Temperature(left * right.kelvin);
        }

        /// <summary>
        /// Multiplies an instance of <see cref="Gu.Units.Temperature"/> with <paramref name="right"/> and returns the result.
        /// </summary>
        /// <param name="left">An instance of <see cref="Gu.Units.Temperature"/></param>
        /// <param name="right">An instance of <seealso cref="System.Double"/></param>
        /// <returns>Multiplies an instance of <see cref="Gu.Units.Temperature"/> with <paramref name="right"/> and returns the result.</returns>
        public static Temperature operator *(Temperature left, double right)
        {
            return new Temperature(left.kelvin * right);
        }

        /// <summary>
        /// Divides an instance of <see cref="Gu.Units.Temperature"/> with <paramref name="right"/> and returns the result.
        /// </summary>
        /// <param name="left">An instance of <see cref="Gu.Units.Temperature"/></param>
        /// <param name="right">An instance of <seealso cref="System.Double"/></param>
        /// <returns>Divides an instance of <see cref="Gu.Units.Temperature"/> with <paramref name="right"/> and returns the result.</returns>
        public static Temperature operator /(Temperature left, double right)
        {
            return new Temperature(left.kelvin / right);
        }

        /// <summary>
        /// Adds two specified <see cref="Gu.Units.Temperature"/> instances.
        /// </summary>
        /// <returns>
        /// An <see cref="Gu.Units.Temperature"/> whose quantity is the sum of the quantitys of <paramref name="left"/> and <paramref name="right"/>.
        /// </returns>
        /// <param name="left">An instance of <see cref="Gu.Units.Temperature"/>.</param>
        /// <param name="right">An instance of <see cref="Gu.Units.Temperature"/>.</param>
        public static Temperature operator +(Temperature left, Temperature right)
        {
            return new Temperature(left.kelvin + right.kelvin);
        }

        /// <summary>
        /// Subtracts an Temperature from another Temperature and returns the difference.
        /// </summary>
        /// <returns>
        /// An <see cref="Gu.Units.Temperature"/> that is the difference
        /// </returns>
        /// <param name="left">An instance of <see cref="Gu.Units.Temperature"/> (the minuend).</param>
        /// <param name="right">An instance of <see cref="Gu.Units.Temperature"/> (the subtrahend).</param>
        public static Temperature operator -(Temperature left, Temperature right)
        {
            return new Temperature(left.kelvin - right.kelvin);
        }

        /// <summary>
        /// Returns an <see cref="Gu.Units.Temperature"/> whose quantity is the negated quantity of the specified instance.
        /// </summary>
        /// <returns>
        /// An <see cref="Gu.Units.Temperature"/> with the same numeric quantity as this instance, but the opposite sign.
        /// </returns>
        /// <param name="temperature">An instance of <see cref="Gu.Units.Temperature"/></param>
        public static Temperature operator -(Temperature temperature)
        {
            return new Temperature(-1 * temperature.kelvin);
        }

        /// <summary>
        /// Returns the specified instance of <see cref="Gu.Units.Temperature"/>.
        /// </summary>
        /// <returns>
        /// Returns <paramref name="temperature"/>.
        /// </returns>
        /// <param name="temperature">An instance of <see cref="Gu.Units.Temperature"/></param>
        public static Temperature operator +(Temperature temperature)
        {
            return temperature;
        }

        /// <summary>
        /// Get the scalar value
        /// </summary>
        /// <param name="unit"></param>
        /// <returns>The scalar value of this in the specified unit</returns>
        public double GetValue(TemperatureUnit unit)
        {
            return unit.FromSiUnit(this.kelvin);
        }

        public override string ToString()
        {
            var quantityFormat = FormatParser<TemperatureUnit>.GetOrCreate(string.Empty, this.SiUnit);
            return this.ToString(quantityFormat, null);
        }

        public string ToString(string format)
        {
            var quantityFormat = FormatParser<TemperatureUnit>.GetOrCreate(format);
            return ToString(quantityFormat, null);
        }

        public string ToString(IFormatProvider provider)
        {
            var quantityFormat = FormatParser<TemperatureUnit>.GetOrCreate(string.Empty, SiUnit);
            return ToString(quantityFormat, provider);
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            var quantityFormat = FormatParser<TemperatureUnit>.GetOrCreate(format);
            return ToString(quantityFormat, formatProvider);
        }

        public string ToString(TemperatureUnit unit)
        {
            var quantityFormat = FormatParser<TemperatureUnit>.GetOrCreate(string.Empty, unit);
            return ToString(quantityFormat, null);
        }

        public string ToString(TemperatureUnit unit, IFormatProvider formatProvider)
        {
            var quantityFormat = FormatParser<TemperatureUnit>.GetOrCreate(string.Empty, unit);
            return ToString(quantityFormat, formatProvider);
        }

        public string ToString(string valueFormat, TemperatureUnit unit)
        {
            var quantityFormat = FormatParser<TemperatureUnit>.GetOrCreate(valueFormat, unit);
            return ToString(quantityFormat, null);
        }

        public string ToString(string valueFormat, TemperatureUnit unit, IFormatProvider formatProvider)
        {
            var quantityFormat = FormatParser<TemperatureUnit>.GetOrCreate(valueFormat, unit);
            return ToString(quantityFormat, formatProvider);
        }

        private string ToString(QuantityFormat<TemperatureUnit> format, IFormatProvider formatProvider)
        {
            using (var builder = StringBuilderPool.Borrow())
            {
                builder.Append(this, format, formatProvider);
                return builder.ToString();
            }
        }

        /// <summary>
        /// Compares this instance to a specified <see cref="Gu.Units.Temperature"/> object and returns an integer that indicates whether this <see cref="quantity"/> is smaller than, equal to, or greater than the <see cref="Gu.Units.Temperature"/> object.
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
        /// <param name="quantity">An instance of <see cref="Gu.Units.Temperature"/> object to compare to this instance.</param>
        public int CompareTo(Temperature quantity)
        {
            return this.kelvin.CompareTo(quantity.kelvin);
        }

        /// <summary>
        /// Returns a quantity indicating whether this instance is equal to a specified <see cref="Gu.Units.Temperature"/> object.
        /// </summary>
        /// <returns>
        /// true if <paramref name="other"/> represents the same Temperature as this instance; otherwise, false.
        /// </returns>
        /// <param name="other">An instance of <see cref="Gu.Units.Temperature"/> object to compare with this instance.</param>
        public bool Equals(Temperature other)
        {
            return this.kelvin.Equals(other.kelvin);
        }

        /// <summary>
        /// Returns a quantity indicating whether this instance is equal to a specified <see cref="Gu.Units.Temperature"/> object within the given tolerance.
        /// </summary>
        /// <returns>
        /// true if <paramref name="other"/> represents the same Temperature as this instance; otherwise, false.
        /// </returns>
        /// <param name="other">An instance of <see cref="Gu.Units.Temperature"/> object to compare with this instance.</param>
        /// <param name="tolerance">The maximum difference for being considered equal</param>
        public bool Equals(Temperature other, double tolerance)
        {
            return Math.Abs(this.kelvin - other.kelvin) < tolerance;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            return obj is Temperature && this.Equals((Temperature)obj);
        }

        public override int GetHashCode()
        {
            return this.kelvin.GetHashCode();
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
            XmlExt.SetReadonlyField(ref this, "kelvin", reader, "Value");
        }

        /// <summary>
        /// Converts an object into its XML representation.
        /// </summary>
        /// <param name="writer">The <see cref="System.Xml.XmlWriter"/> stream to which the object is serialized. </param>
        public void WriteXml(XmlWriter writer)
        {
            XmlExt.WriteAttribute(writer, "Value", this.kelvin);
        }
    }
}