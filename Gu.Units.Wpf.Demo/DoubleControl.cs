namespace Gu.Units.Wpf.Demo
{
    using System.Windows;
    using System.Windows.Controls;

    public class DoubleControl : Control
    {
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
            "Value",
            typeof(double),
            typeof(DoubleControl),
            new PropertyMetadata(default(double)));

        public double Value
        {
            get { return (double)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }
    }
}
