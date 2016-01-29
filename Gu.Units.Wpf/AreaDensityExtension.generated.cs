﻿namespace Gu.Units.Wpf
{
    using System;
    using System.Globalization;
    using System.Windows.Markup;

    [MarkupExtensionReturnType(typeof(AreaDensity))]
    public class AreaDensityExtension : MarkupExtension
    {
        public AreaDensityExtension(AreaDensity value)
        {
            this.Value = value;
        }

        public AreaDensity Value { get; private set; }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this.Value;
        }
    }
}