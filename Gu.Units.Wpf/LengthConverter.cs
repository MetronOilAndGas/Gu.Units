namespace Gu.Units.Wpf
{
    using System;
    using System.ComponentModel;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Data;
    using System.Windows.Markup;

    [MarkupExtensionReturnType(typeof(IValueConverter))]
    public class LengthConverter : MarkupExtension, IValueConverter
    {
        private static readonly string StringFormatNotSet = "Not Set";
        private LengthUnit? unit;
        private IProvideValueTarget provideValueTarget;
        private string stringFormat;
        private QuantityFormat<LengthUnit> format;

        public LengthConverter()
        {
        }

        public LengthConverter([TypeConverter(typeof(LengthUnitTypeConverter))]LengthUnit unit)
        {
            Unit = unit;
        }

        [ConstructorArgument("unit"), TypeConverter(typeof(LengthUnitTypeConverter))]
        public LengthUnit? Unit
        {
            get { return this.unit; }
            set
            {
                if (value == null)
                {
                    var message = $"{nameof(Unit)} cannot be null";
                    throw new ArgumentException(message, nameof(value));
                }

                this.unit = value.Value;
            }
        }

        public UnitInputOptions UnitInput { get; set; } = UnitInputOptions.Default;

        public string StringFormat
        {
            get { return this.stringFormat; }
            set
            {
                this.stringFormat = value;
                OnStringFormatChanged();
            }
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            // the binding does not have stringformat set at this point
            // caching IProvideValueTarget to resolve later.
            this.provideValueTarget = (IProvideValueTarget)serviceProvider.GetService(typeof(IProvideValueTarget));
            return this;
        }

        public object Convert(object value,
            Type targetType,
            object parameter,
            CultureInfo culture)
        {
            if (!IsValidConvertTargetType(targetType) && Is.DesignMode)
            {
                var message = $"{GetType().Name} does not support converting to {targetType.Name}";
                throw new ArgumentException(message, nameof(targetType));
            }

            if (value != null && !(value is Length) && Is.DesignMode)
            {
                var message = $"{GetType().Name} only supports converting from {nameof(Length)}";
                throw new ArgumentException(message, nameof(value));
            }

            if (this.StringFormat == null)
            {
                TryGetStringFormatFromTarget();
            }

            if (this.unit == null)
            {
                if (Is.DesignMode)
                {
                    var message = $"{nameof(Unit)} cannot be null\r\n" +
                                  $"Must be specified Explicitly or in Binding.StringFormat";
                    throw new ArgumentException(message);
                }

                return "No unit set";
            }

            if (value == null)
            {
                return targetType == typeof (string)
                    ? string.Empty
                    : null;
            }

            var length = (Length)value;
            if (this.StringFormat != StringFormatNotSet)
            {
                return value;
            }

            if (IsValidConvertTargetType(targetType))
            {
                return length.GetValue(this.unit.Value);
            }

            return value;
        }

        public object ConvertBack(object value,
            Type targetType,
            object parameter,
            CultureInfo culture)
        {
            if (!(targetType == typeof(Length) || targetType == typeof(Length?)))
            {
                var message = $"{GetType().Name} does not support converting to {targetType.Name}";
                throw new NotSupportedException(message);
            }

            if (value == null)
            {
                return null;
            }

            if (this.StringFormat == null)
            {
                TryGetStringFormatFromTarget();
            }

            if (this.unit == null)
            {
                if (Is.DesignMode)
                {
                    var message = $"{nameof(Unit)} cannot be null\r\n" +
                                  $"Must be specified Explicitly or in Binding.StringFormat";
                    throw new ArgumentException(message);
                }

                return value;
            }

            if (value is double)
            {
                return new Length((double)value, this.unit.Value);
            }

            var text = value as string;
            if (string.IsNullOrEmpty(text))
            {
                return null;
            }

            switch (UnitInput)
            {
                case UnitInputOptions.Default:
                case UnitInputOptions.ScalarOnly:
                    {
                        double d;
                        if (double.TryParse(text, NumberStyles.Float, culture, out d))
                        {
                            return new Length(d, this.unit.Value);
                        }

                        return value; // returning raw to trigger error
                    }
                case UnitInputOptions.SymbolAllowed:
                    {
                        double d;
                        int pos = 0;
                        text.ReadWhiteSpace(ref pos);
                        if (DoubleReader.TryRead(text, pos, NumberStyles.Float, culture, out d, out pos))
                        {
                            text.ReadWhiteSpace(ref pos);
                            if (pos == text.Length)
                            {
                                return new Length(d, this.unit.Value);
                            }
                        }

                        goto case UnitInputOptions.SymbolRequired;
                    }
                case UnitInputOptions.SymbolRequired:
                    {
                        Length result;
                        if (Length.TryParse(text, NumberStyles.Float, culture, out result))
                        {
                            return result;
                        }

                        return text;
                    }
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void TryGetStringFormatFromTarget()
        {
            var target = this.provideValueTarget?.TargetObject as DependencyObject;
            Binding binding = null;
            if (target != null)
            {
                var targetProperty = this.provideValueTarget.TargetProperty as DependencyProperty;
                if (targetProperty != null)
                {
                    binding = BindingOperations.GetBinding(target, targetProperty);
                }
            }

            binding = binding ?? this.provideValueTarget?.TargetObject as Binding;
            this.StringFormat = binding?.StringFormat;
        }

        private void OnStringFormatChanged()
        {
            if (StringFormatParser.TryParse(this.stringFormat, out this.format))
            {
                if (UnitInput == UnitInputOptions.Default)
                {
                    UnitInput = UnitInputOptions.SymbolRequired;
                }

                if (Unit == null)
                {
                    this.unit = this.format.Unit;
                }

                else if(this.unit != this.format.Unit)
                {
                    if (Is.DesignMode)
                    {
                        throw new InvalidOperationException($"The Unit is set to {Unit} but the stringformat has unit: {this.format.Unit}");
                    }

                    this.unit = this.format.Unit;
                }
                return;
            }

            this.stringFormat = null;
        }

        private bool IsValidConvertTargetType(Type targetType)
        {
            return targetType == typeof(string) ||
                   targetType == typeof(double) ||
                   targetType == typeof(object);
        }
    }
}
