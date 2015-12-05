namespace Gu.Units.Generator
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Globalization;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Xml.Serialization;
    using Annotations;
    using WpfStuff;

    [DebuggerDisplay("{ToSi}")]
    [TypeConverter(typeof(StringToFormulaConverter))]
    public class ConversionFormula : INotifyPropertyChanged
    {
        private readonly IUnit baseUnit;
        private double conversionFactor;
        private double offset;

        private ConversionFormula()
        {
        }

        public ConversionFormula(IUnit baseUnit)
        {
            this.baseUnit = baseUnit;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public double ConversionFactor
        {
            get { return this.conversionFactor; }
            set
            {
                if (value.Equals(this.conversionFactor))
                {
                    return;
                }

                this.conversionFactor = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(RootFactor));
                OnPropertyChanged(nameof(ToSi));
                OnPropertyChanged(nameof(FromSi));
            }
        }

        public double Offset
        {
            get { return this.offset; }
            set
            {
                if (value.Equals(this.offset))
                    return;
                this.offset = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(ToSi));
                OnPropertyChanged(nameof(FromSi));
            }
        }

        [XmlIgnore]
        public IUnit RooUnit
        {
            get
            {
                IUnit root = this.baseUnit;
                var unit = (root as Conversion)?.BaseUnit;
                while (unit != null)
                {
                    root = unit;
                    unit = (root as Conversion)?.BaseUnit;
                }

                return root;
            }
        }

        [XmlIgnore]
        public double RootFactor
        {
            get
            {
                var factor = ConversionFactor;
                var conversion = this.baseUnit as Conversion;
                while (conversion != null)
                {
                    factor *= conversion.Formula.ConversionFactor;
                    conversion = conversion.BaseUnit as Conversion;
                }

                return factor;
            }
        }

        [XmlIgnore]
        public string ToSi
        {
            get
            {
                var builder = new StringBuilder();
                if (RootFactor != 1)
                {
                    builder.Append(RootFactor.ToString(CultureInfo.InvariantCulture) + "*");
                }
                builder.Append(RooUnit != null ? RooUnit.Quantity.Unit.ClassName : "x");
                if (Offset != 0)
                {
                    if (Offset > 0)
                    {
                        builder.AppendFormat(" + {0}", Offset.ToString(CultureInfo.InvariantCulture));
                    }
                    else
                    {
                        builder.AppendFormat(" - {0}", (-Offset).ToString(CultureInfo.InvariantCulture));
                    }
                }
                return builder.ToString();
            }
        }

        [XmlIgnore]
        public string FromSi
        {
            get
            {
                var builder = new StringBuilder();

                builder.Append(RooUnit != null ? this.RooUnit.Quantity.Unit.ClassName : "x");
                if (RootFactor != 1)
                {
                    builder.Append("/" + RootFactor.ToString(CultureInfo.InvariantCulture));
                }
                if (Offset != 0)
                {
                    if (Offset < 0)
                    {
                        builder.AppendFormat(" + {0}", (-1 * Offset).ToString(CultureInfo.InvariantCulture));
                    }
                    else
                    {
                        builder.AppendFormat(" - {0}", Offset.ToString(CultureInfo.InvariantCulture));
                    }
                }
                return builder.ToString();
            }
        }

        [XmlIgnore]
        public bool CanRountrip
        {
            get
            {
                throw new NotImplementedException("message");
            }
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void Update()
        {
            //OnPropertyChanged(nameof(ConversionFactor));
            //OnPropertyChanged(nameof(Offset));
            OnPropertyChanged(nameof(ToSi));
            OnPropertyChanged(nameof(FromSi));
        }
    }
}