﻿using System.Runtime.CompilerServices;
using Gu.Units;

[assembly: TypeForwardedTo(typeof(Temperature))]
[assembly: TypeForwardedTo(typeof(TemperatureUnit))]

namespace Gu.Units.Wpf
{
    using System;
    using System.ComponentModel;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Data;
    using System.Windows.Markup;

    [MarkupExtensionReturnType(typeof(IValueConverter))]
    public class TemperatureConverter : MarkupExtension, IValueConverter
    {
        private static readonly string StringFormatNotSet = "Not Set";
        private TemperatureUnit? unit;
        private IProvideValueTarget provideValueTarget;
        private string stringFormat = StringFormatNotSet;
        private QuantityFormat<TemperatureUnit> quantityFormat;
        private string bindingStringFormat = StringFormatNotSet;
        private string errorText;

        public TemperatureConverter()
        {
        }

        public TemperatureConverter([TypeConverter(typeof(TemperatureUnitTypeConverter))]TemperatureUnit unit)
        {
            Unit = unit;
        }

        [ConstructorArgument("unit"), TypeConverter(typeof(TemperatureUnitTypeConverter))]
        public TemperatureUnit? Unit
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

        public SymbolFormat? SymbolFormat { get; set; }

        public UnitInput UnitInput { get; set; } = UnitInput.Default;

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

            if (value != null && !(value is Temperature) && Is.DesignMode)
            {
                var message = $"{GetType().Name} only supports converting from {typeof(Temperature)}";
                throw new ArgumentException(message, nameof(value));
            }

            if (this.bindingStringFormat == StringFormatNotSet)
            {
                TryGetStringFormatFromBinding();
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

            if (this.errorText != null)
            {
                return this.errorText;
            }

            if (value == null)
            {
                return targetType == typeof(string)
                    ? string.Empty
                    : null;
            }

            var temperature = (Temperature)value;
            if (this.StringFormat != StringFormatNotSet &&
                (targetType == typeof(string) || targetType == typeof(object)))
            {
                return temperature.ToString(StringFormat, culture);
            }


            if (SymbolFormat != null &&
                UnitInput == UnitInput.SymbolRequired &&
               (targetType == typeof(string) || targetType == typeof(object)))
            {
                return temperature.ToString(Unit.Value, SymbolFormat.Value, culture);
            }

            if (IsValidConvertTargetType(targetType))
            {
                return temperature.GetValue(this.unit.Value);
            }

            return value;
        }

        public object ConvertBack(object value,
            Type targetType,
            object parameter,
            CultureInfo culture)
        {
            if (!(targetType == typeof(Temperature) || targetType == typeof(Temperature?)))
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
                TryGetStringFormatFromBinding();
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
                return new Temperature((double)value, this.unit.Value);
            }

            var text = value as string;
            if (string.IsNullOrEmpty(text))
            {
                return null;
            }

            switch (UnitInput)
            {
                case UnitInput.Default:
                case UnitInput.ScalarOnly:
                    {
                        double d;
                        if (double.TryParse(text, NumberStyles.Float, culture, out d))
                        {
                            return new Temperature(d, this.unit.Value);
                        }

                        return value; // returning raw to trigger error
                    }
                case UnitInput.SymbolAllowed:
                    {
                        double d;
                        int pos = 0;
                        WhiteSpaceReader.TryRead(text, ref pos);
                        if (DoubleReader.TryRead(text, ref pos, NumberStyles.Float, culture, out d))
                        {
                            WhiteSpaceReader.TryRead(text, ref pos);
                            if (pos == text.Length)
                            {
                                return new Temperature(d, this.unit.Value);
                            }
                        }

                        goto case UnitInput.SymbolRequired;
                    }
                case UnitInput.SymbolRequired:
                    {
                        Temperature result;
                        if (Temperature.TryParse(text, NumberStyles.Float, culture, out result))
                        {
                            return result;
                        }

                        return text;
                    }
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void TryGetStringFormatFromBinding()
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
            this.bindingStringFormat = binding?.StringFormat;
            if (this.bindingStringFormat != null)
            {
                OnStringFormatChanged();
            }
        }

        private void OnStringFormatChanged()
        {
            if (this.bindingStringFormat != StringFormatNotSet &&
                this.stringFormat != StringFormatNotSet &&
                this.bindingStringFormat != this.stringFormat)
            {
                this.errorText += $"Both {nameof(Binding)}.{nameof(Binding.StringFormat)} and {nameof(StringFormat)} are set.";

                if (Is.DesignMode)
                {
                    throw new InvalidOperationException(this.errorText);
                }
            }

            var format = this.stringFormat == StringFormatNotSet
                ? this.bindingStringFormat
                : this.stringFormat;
            if (StringFormatParser<TemperatureUnit>.TryParse(format, out this.quantityFormat))
            {
                if (UnitInput == UnitInput.Default && this.quantityFormat.SymbolFormat != null)
                {
                    UnitInput = UnitInput.SymbolRequired;
                }

                if (this.unit == null)
                {
                    this.unit = this.quantityFormat.Unit;
                }

                else if (this.unit != this.quantityFormat.Unit)
                {
                    this.errorText += $"Unit is set to '{Unit}' but {nameof(StringFormat)} is '{format}'";
                    if (Is.DesignMode)
                    {
                        throw new InvalidOperationException(this.errorText);
                    }
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