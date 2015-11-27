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
        private string stringFormat;
        private IProvideValueTarget provideValueTarget;

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

        public SymbolOptions Symbol { get; set; } = SymbolOptions.Default;

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

            if (this.stringFormat == null)
            {
                GetStringFormat();
            }

            if (this.unit == null)
            {
                if (Is.DesignMode)
                {
                    var message = $"{nameof(Unit)} cannot be null\r\n" +
                                  $"Must be specified Explicitly or in Binding.StringFormat";
                    throw new ArgumentException(message);
                }

                return string.Empty;
            }

            if (value == null)
            {
                return targetType == typeof (string)
                    ? string.Empty
                    : null;
            }

            var length = (Length)value;
            if (this.stringFormat != StringFormatNotSet)
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

            if (this.stringFormat == null)
            {
                GetStringFormat();
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

            switch (Symbol)
            {
                case SymbolOptions.Default:
                case SymbolOptions.NotAllowed:
                    {
                        double d;
                        if (double.TryParse(text, NumberStyles.Float, culture, out d))
                        {
                            return new Length(d, this.unit.Value);
                        }

                        return value; // returning raw to trigger error
                    }
                case SymbolOptions.Allowed:
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

                        goto case SymbolOptions.Required;
                    }
                case SymbolOptions.Required:
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

        private void GetStringFormat()
        {
            var target = this.provideValueTarget.TargetObject as DependencyObject;
            Binding binding = null;
            if (target != null)
            {
                var targetProperty = this.provideValueTarget.TargetProperty as DependencyProperty;
                if (targetProperty != null)
                {
                    binding = BindingOperations.GetBinding(target, targetProperty);
                }
            }

            binding = binding ?? this.provideValueTarget.TargetObject as Binding;
            this.stringFormat = binding?.StringFormat;
            if (this.stringFormat != null)
            {
                QuantityFormat<LengthUnit> format;
                if (StringFormatParser.TryParse(this.stringFormat, out format))
                {
                    this.unit = format.Unit;
                    this.stringFormat = format.ValueFormat;
                    if (Symbol == SymbolOptions.Default)
                    {
                        Symbol = SymbolOptions.Required;
                    }
                    return;
                }
            }

            this.stringFormat = StringFormatNotSet;
        }

        private bool IsValidConvertTargetType(Type targetType)
        {
            return targetType == typeof(string) ||
                   targetType == typeof(double) ||
                   targetType == typeof(object);
        }
    }
}
