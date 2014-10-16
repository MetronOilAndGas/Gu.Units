﻿namespace Gu.Units.Generator
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;
    using System.Xml.Serialization;

    using Gu.Units.Generator.WpfStuff;

    [Serializable, TypeConverter(typeof(QuantityConverter))]
    public class Quantity : TypeMetaData
    {
        private IUnit _unit;
        private string _unitName;

        public Quantity()
        {
        }

        public Quantity(string @namespace, string className, IUnit unit)
            : base(@namespace, className)
        {
            _unit = unit;
        }

        public static Quantity Empty
        {
            get
            {
                return new Quantity("", "", null);
            }
        }

        /// <summary>
        /// DummyData for the template
        /// </summary>
        public static Quantity DummySiUnit
        {
            get
            {
                var siUnit = new SiUnit("Gu.Units", "Metres", "m");
                var quantity = new Quantity("", "Metres", siUnit);
                return quantity;
            }
        }

        public string UnitName
        {
            get
            {
                if (Unit != null)
                    return this.Unit.ClassName;
                return _unitName;
            }
            set
            {
                if (Unit != null)
                {
                    throw new InvalidOperationException("Trying to set unit name");
                }
                _unitName = value;
            }
        }

        [XmlIgnore]
        public IUnit Unit
        {
            get
            {
                return this._unit;
            }
            set
            {
                if (Equals(value, this._unit))
                {
                    return;
                }
                this._unit = value;
                this.OnPropertyChanged();
                this.OnPropertyChanged("UnitName");
                this.OnPropertyChanged("Interface");
            }
        }

        [XmlIgnore]
        public string Interface
        {
            get
            {
                var siUnit = Unit as SiUnit;
                if (siUnit != null)
                {
                    if (string.IsNullOrEmpty(Unit.Symbol))
                    {
                        return "ERROR: string.IsNullOrEmpty(Unit.Symbol)";
                    }
                    return string.Format("IQuantity<{0}, I1>", siUnit.ClassName);
                }
                var derivedUnit = Unit as DerivedUnit;
                if (derivedUnit == null)
                {
                    return "Unit == null";
                }
                var flattened = derivedUnit.Parts.Flattened.ToArray();
                if (!flattened.Any())
                {
                    return "ERROR No Units";
                }
                var args = string.Join(", ",
                    flattened.Select(u => string.Format("{0}, I{1}{2}",
                        u.Unit.ClassName,
                        u.Power < 0 ? "Neg" : "",
                        u.Power < 0 ? -1 * u.Power : u.Power)));
                return string.Format("IQuantity<{0}>",  args);
            }
        }

        public override string ToString()
        {
            return string.Format("{0} ({1})", ClassName, Unit.ToString());
        }
    }
}
