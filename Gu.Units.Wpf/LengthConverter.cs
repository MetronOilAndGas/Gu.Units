namespace Gu.Units.Wpf
{
    using System;
    using System.ComponentModel;
    using System.Globalization;
    using System.Windows.Data;
    using System.Windows.Markup;

    [MarkupExtensionReturnType(typeof(IValueConverter))]
    public class LengthConverter : MarkupExtension, IValueConverter
    {
        private LengthUnit? _unit;

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
            get { return this._unit; }
            set
            {
                if (value == null)
                {
                    var message = $"{nameof(Unit)} cannot be null";
                    throw new ArgumentException(message, nameof(value));
                }

                this._unit = value;
            }
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (Unit == null)
            {
                var message = $"{nameof(Unit)} cannot be null";
                throw new InvalidOperationException(message);
            }

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

            if (!(value is Length))
            {
                var message = $"{GetType().Name} only supports converting from {nameof(Length)}";
                throw new InvalidOperationException(message);
            }

            var length = (Length)value;

            if (targetType == typeof(string))
            {
                throw new NotImplementedException("Use string format from binding");
                return Unit.Value.GetScalarValue(length).ToString(culture);
            }

            if (targetType == typeof(double))
            {
                return Unit.Value.GetScalarValue(length);
            }

            {
                var message = $"{GetType().Name} does not support vonverting to {targetType.Name}";
                throw new NotSupportedException(message);
            }
        }

        public object ConvertBack(object value,
            Type targetType,
            object parameter,
            CultureInfo culture)
        {
            if (!(targetType == typeof(Length) || targetType == typeof(Length?)))
            {
                var message = $"{GetType().Name} does not support vonverting to {targetType.Name}";
                throw new NotSupportedException(message);
            }

            if (value == null)
            {
                return null;
            }

            var text = value as string;
            if (string.IsNullOrEmpty(text))
            {
                return null;
            }

            double d;
            if (double.TryParse(text, NumberStyles.Float, culture, out d))
            {
                return new Length(d, Unit.Value);
            }

            Length result;
            if (Length.TryParse(text, NumberStyles.Float, culture, out result))
            {
                return result;
            }

            return Binding.DoNothing;
        }
    }
}
