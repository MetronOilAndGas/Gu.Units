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
        private LengthUnit? _unit;
        private string _stringFormat;
        private IProvideValueTarget _provideValueTarget;

        public LengthConverter()
        {
        }

        public LengthConverter(LengthUnit unit)
        {
            Unit = unit;
        }

        [ConstructorArgument("unit"), TypeConverter(typeof(LengthUnitTypeConverter))]
        public LengthUnit? Unit
        {
            get { return this._unit; }
            set
            {
                if (value == null)
                {
                    var message = $"{nameof(Unit)} cannot be null";
                    throw new ArgumentException(message, nameof(value));
                }

                this._unit = value.Value;
            }
        }

        public bool AllowSymbol { get; set; } = false;

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            // the binding does not have stringformat set at this point
            // caching IProvideValueTarget to resolve later.
            this._provideValueTarget = (IProvideValueTarget)serviceProvider.GetService(typeof(IProvideValueTarget));
            return this;
        }

        public object Convert(object value,
            Type targetType,
            object parameter,
            CultureInfo culture)
        {
            if (value == null)
            {
                return string.Empty;
            }

            if (this._unit == null && this._stringFormat == null)
            {
                GetStringFormat();
            }

            if (!(value is Length))
            {
                var message = $"{GetType().Name} only supports converting from {nameof(Length)}";
                throw new ArgumentException(message, nameof(value));
            }

            var length = (Length)value;
            if (this._stringFormat != null)
            {
                return value;
            }

            if (targetType == typeof(string) ||
                targetType == typeof(double) ||
                targetType == typeof(object))
            {
                return length.GetValue(this._unit.Value);
            }
            {
                var message = $"{GetType().Name} does not support converting to {targetType.Name}";
                throw new ArgumentException(message, nameof(targetType));
            }
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

            if (this._unit == null && this._stringFormat == null)
            {
                GetStringFormat();
            }

            if (value is double)
            {
                return new Length((double) value, Unit.Value);
            }

            var text = value as string;
            if (string.IsNullOrEmpty(text))
            {
                return null;
            }

            if (AllowSymbol)
            {
                Length result;
                if (Length.TryParse(text, NumberStyles.Float, culture, out result))
                {
                    return result;
                }
                return text;
            }

            double d;
            if (double.TryParse(text, NumberStyles.Float, culture, out d))
            {
                return new Length(d, Unit.Value);
            }

            return value; // returning raw to trigger error
        }

        private void GetStringFormat()
        {
            var target = this._provideValueTarget.TargetObject as DependencyObject;
            Binding binding = null;
            if (target != null)
            {
                var targetProperty = this._provideValueTarget.TargetProperty as DependencyProperty;
                if (targetProperty != null)
                {
                    binding = BindingOperations.GetBinding(target, targetProperty);
                }
            }
            binding = binding ?? this._provideValueTarget.TargetObject as Binding;
            _stringFormat = binding?.StringFormat;
            if (_stringFormat != null)
            {
                QuantityFormat<LengthUnit> format;
                if (FormatParser.TryParse(_stringFormat.Trim('{','}'), out format))
                {
                    this._unit = format.Unit;
                    this._stringFormat = format.Format;
                    AllowSymbol = true;
                }
            }
        }
    }
}
